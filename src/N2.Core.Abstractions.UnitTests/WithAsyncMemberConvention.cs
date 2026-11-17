using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core;

/// <summary>
/// Guards the 2.0.0 convention: every asynchronous member takes a required CancellationToken as
/// its last parameter, and no member name ends in Async.
/// </summary>
[TestClass]
public sealed class WithAsyncMemberConvention
{
    private const string AsyncSuffix = "Async";

    private const BindingFlags DeclaredPublicMembers =
        BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

    private static readonly Type[] AsyncReturnTypes =
    [
        typeof(Task),
        typeof(Task<>),
        typeof(ValueTask),
        typeof(ValueTask<>),
        typeof(IAsyncEnumerable<>),
    ];

    [TestMethod]
    public void EveryAsyncMemberTakesARequiredCancellationTokenAsLastParameter()
    {
        var violations = PublicMethods()
            .Where(m => IsAsyncReturnType(m.ReturnType))
            .Where(m => !EndsWithRequiredCancellationToken(m))
            .Select(Describe)
            .ToList();

        Assert.AreEqual(string.Empty, string.Join(Environment.NewLine, violations));
    }

    [TestMethod]
    public void NoPublicMemberNameEndsInAsync()
    {
        var violations = PublicTypes()
            .SelectMany(t => t.GetMembers(DeclaredPublicMembers))
            .Where(m => m.Name.EndsWith(AsyncSuffix, StringComparison.Ordinal))
            .Select(m => $"{m.DeclaringType?.FullName}.{m.Name}")
            .ToList();

        Assert.AreEqual(string.Empty, string.Join(Environment.NewLine, violations));
    }

    private static IEnumerable<Type> PublicTypes()
        => typeof(EditMode).Assembly.GetExportedTypes();

    private static IEnumerable<MethodInfo> PublicMethods()
        => PublicTypes()
            .SelectMany(t => t.GetMethods(DeclaredPublicMembers))
            .Where(m => !m.IsSpecialName);

    private static bool IsAsyncReturnType(Type returnType)
    {
        var candidate = returnType.IsGenericType ? returnType.GetGenericTypeDefinition() : returnType;
        return AsyncReturnTypes.Contains(candidate);
    }

    private static bool EndsWithRequiredCancellationToken(MethodInfo method)
    {
        var parameters = method.GetParameters();
        if (parameters.Length == 0)
        {
            return false;
        }

        var last = parameters[^1];
        return last.ParameterType == typeof(CancellationToken) && !last.IsOptional;
    }

    private static string Describe(MethodInfo method)
        => $"{method.DeclaringType?.FullName}.{method.Name}";
}
