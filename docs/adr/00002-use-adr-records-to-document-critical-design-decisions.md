# 00002. Use ADR records to document critical design decisions

2026-03-11

## Status

__Accepted__

## Context

Architectural and design decisions are made continuously during a project's lifetime. Without a deliberate record, the reasoning behind those decisions fades quickly: contributors who were not present cannot understand why the codebase is shaped the way it is, and the team risks revisiting settled debates or repeating past mistakes. Comments in code capture the "what", but not the "why" or the trade-offs that were considered and rejected.

A lightweight, text-based format stored alongside the code ensures that decisions travel with the repository and are subject to the same review process as code changes.

## Decision

We will use Architecture Decision Records (ADRs) to document every decision that is architecturally significant — meaning decisions that affect structure, cross-cutting concerns, external dependencies, interfaces, or construction techniques that would be costly to reverse.

Each ADR is a short Markdown file stored in `docs/adr/` and managed with the `adr-cli` tool. Records are numbered sequentially and never deleted; superseded decisions are marked as such and linked to the record that replaces them.

An ADR is warranted when:
- a decision is difficult or expensive to reverse
- multiple reasonable options existed and the choice needs justification
- future contributors are likely to question or challenge the decision without context

ADRs are **not** required for implementation details, routine refactors, or decisions that are trivially reversible.

## Consequences

- The reasoning behind significant decisions is preserved and discoverable by anyone with access to the repository.
- New contributors can understand the architectural intent without relying on tribal knowledge.
- ADRs must be kept up to date: when a decision is revised or reversed, the affected record must be updated and a new record written. Stale or incomplete ADRs are worse than none.
- Writing an ADR takes deliberate effort at decision time, which is a small, acceptable cost compared to the long-term benefit of having the record.

## References

- [Documenting Architecture Decisions — Cognitect (2011)](https://cognitect.com/blog/2011/11/15/documenting-architecture-decisions)
- ADR tooling: [adr-cli](https://github.com/gjkaal/adr-cli)
