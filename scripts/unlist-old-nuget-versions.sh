#!/usr/bin/env bash
# unlist-old-nuget-versions.sh
#
# Unlists old versions of a NuGet package, keeping the KEEP_VERSIONS most recent
# versions listed. NuGet.org does not support deletion; unlisted packages are
# hidden from search but remain downloadable for existing consumers.
#
# The NuGet API key must have the "Unlist package" scope for the target package.
# Using the same key as publish is fine if that key was created with both scopes.
#
# Environment variables:
#   NUGET_PACKAGE_ID    NuGet package identifier, e.g. "N2.Core.Abstractions" (required)
#   PACKAGE_N2          NuGet API key with unlist permission (required)
#   KEEP_VERSIONS       Number of most-recent versions to leave listed (optional, default: 5)
#
# Exit codes:
#   0   Completed (including "nothing to do")
#   1   Missing required variable or unrecoverable API error

set -euo pipefail

NUGET_PACKAGE_ID="${NUGET_PACKAGE_ID:?Environment variable NUGET_PACKAGE_ID is required}"
PACKAGE_N2="${PACKAGE_N2:?Environment variable PACKAGE_N2 is required}"
KEEP_VERSIONS="${KEEP_VERSIONS:-5}"

PACKAGE_ID_LOWER=$(echo "$NUGET_PACKAGE_ID" | tr '[:upper:]' '[:lower:]')

echo "Managing listed versions for package: $NUGET_PACKAGE_ID"
echo "  Keeping $KEEP_VERSIONS most recent versions listed"
echo ""

# Fetch all published versions (ascending order as returned by NuGet)
RESPONSE=$(curl -sf "https://api.nuget.org/v3-flatcontainer/${PACKAGE_ID_LOWER}/index.json" || true)

if [ -z "$RESPONSE" ]; then
  echo "Package '$NUGET_PACKAGE_ID' not found on NuGet.org — nothing to do."
  exit 0
fi

# Parse version list (one per line, ascending)
ALL_VERSIONS=$(echo "$RESPONSE" | python3 -c "
import json, sys
data = json.load(sys.stdin)
for v in data.get('versions', []):
    print(v)
")

TOTAL_COUNT=$(echo "$ALL_VERSIONS" | grep -c . || true)
echo "Total published versions: $TOTAL_COUNT"

if [ "$TOTAL_COUNT" -le "$KEEP_VERSIONS" ]; then
  echo "Nothing to unlist — $TOTAL_COUNT version(s) published, keeping $KEEP_VERSIONS."
  exit 0
fi

# The versions to unlist are the oldest ones (everything except the last KEEP_VERSIONS)
TO_UNLIST=$(echo "$ALL_VERSIONS" | head -n "$((TOTAL_COUNT - KEEP_VERSIONS))")
UNLIST_COUNT=$(echo "$TO_UNLIST" | grep -c . || true)

echo "Unlisting $UNLIST_COUNT version(s):"
echo "$TO_UNLIST"
echo ""

ERRORS=0

while IFS= read -r VERSION; do
  [ -z "$VERSION" ] && continue

  HTTP_STATUS=$(curl -s -o /dev/null -w "%{http_code}" \
    -X DELETE \
    -H "X-NuGet-ApiKey: $PACKAGE_N2" \
    "https://www.nuget.org/api/v2/package/$NUGET_PACKAGE_ID/$VERSION")

  case "$HTTP_STATUS" in
    204) echo "  [unlisted]     $VERSION" ;;
    404) echo "  [not found]    $VERSION — already unlisted or never published" ;;
    401) echo "  [unauthorized] $VERSION — API key missing or lacks unlist scope"; ERRORS=$((ERRORS + 1)) ;;
    403) echo "  [forbidden]    $VERSION — API key does not have unlist permission for this package"; ERRORS=$((ERRORS + 1)) ;;
    *)   echo "  [error $HTTP_STATUS]  $VERSION — unexpected response"; ERRORS=$((ERRORS + 1)) ;;
  esac
done <<< "$TO_UNLIST"

echo ""
if [ "$ERRORS" -gt 0 ]; then
  echo "$ERRORS error(s) occurred. Check the API key scopes under nuget.org → Account → API Keys."
  exit 1
fi

echo "Done. $UNLIST_COUNT version(s) processed."
