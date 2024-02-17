using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis;

internal static class SymbolExtensions
{
    public static readonly SymbolDisplayFormat GlobalTypeDisplayFormat = new SymbolDisplayFormat(SymbolDisplayGlobalNamespaceStyle.Included, SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces)
                .WithGenericsOptions(SymbolDisplayGenericsOptions.IncludeTypeParameters)
                .AddMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.UseSpecialTypes/* | SymbolDisplayMiscellaneousOptions.ExpandNullable*/);

    public static readonly SymbolDisplayFormat TypeDisplayFormat = new SymbolDisplayFormat(SymbolDisplayGlobalNamespaceStyle.Omitted, SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces)
                .WithGenericsOptions(SymbolDisplayGenericsOptions.IncludeTypeParameters)
                .AddMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.UseSpecialTypes/* | SymbolDisplayMiscellaneousOptions.ExpandNullable*/);

    public static string GetFullName(this ITypeSymbol type, bool global = true) => global ? type.ToDisplayString(GlobalTypeDisplayFormat) : type.ToDisplayString(TypeDisplayFormat);

    public static bool IsEnum(this ITypeSymbol type) => type.BaseType != null && type.BaseType.ToDisplayString() == "System.Enum";

    public static bool IsNullable(this ITypeSymbol type) => type is INamedTypeSymbol namedTypeSymbol
               && namedTypeSymbol.IsGenericType
               && namedTypeSymbol.ConstructedFrom.ToDisplayString() == "System.Nullable<T>";

    public static bool HasInitializer(this IFieldSymbol field)
    {
        if (field.DeclaringSyntaxReferences.Length == 0)
            return false;

        if (field.DeclaringSyntaxReferences[0].GetSyntax() is not VariableDeclaratorSyntax syntax)
            return false;

        return syntax.Initializer != null;

    }

    public static string? GetNamespace(this ITypeSymbol type)
    {
        if (type is IArrayTypeSymbol arrayTypeSymbol)
        {
            return arrayTypeSymbol.ElementType.GetNamespace();
        }
        else
        {
            return type.ContainingNamespace.IsGlobalNamespace ? null :
                type.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.OmittedAsContaining));
        }
    }

    public static AttributeData? GetAttribute(this ISymbol symbol, string attributeTypeName)
    {
        foreach (var attribute in symbol.GetAttributes())
        {
            if (attribute.AttributeClass?.ToDisplayString() == attributeTypeName)
                return attribute;
        }

        return null;
    }

    public static IEnumerable<AttributeData> GetAttributes(this ISymbol symbol, string attributeTypeName)
    {
        foreach (var attribute in symbol.GetAttributes())
        {
            if (attribute.AttributeClass?.ToDisplayString() == attributeTypeName)
                yield return attribute;
        }
    }

    public static bool HasAttribute(this ISymbol symbol, string attributeTypeName)
    {
        foreach (var attribute in symbol.GetAttributes())
        {
            if (attribute.AttributeClass?.ToDisplayString() == attributeTypeName)
                return true;
        }
        return false;
    }

    public static IEnumerable<ITypeSymbol> EnumerableTypes(this IAssemblySymbol assembly)
    {
        Queue<INamespaceSymbol> queue = new([assembly.GlobalNamespace]);
        while (queue.Count > 0)
        {
            var ns = queue.Dequeue();
            foreach (var nsOrType in ns.GetMembers())
            {
                if (nsOrType is ITypeSymbol typeSymbol)
                    yield return typeSymbol;
                else
                    queue.Enqueue((INamespaceSymbol)nsOrType);
            }
        }
    }
}
