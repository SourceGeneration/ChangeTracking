using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace Microsoft.CodeAnalysis;

internal static class SyntaxExtensions
{
    public static bool IsPartial(this MemberDeclarationSyntax typeDeclaration) => typeDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));

    public static bool IsPublic(this MemberDeclarationSyntax typeDeclaration) => typeDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword));

    public static bool IsPublicOrInternal(this MemberDeclarationSyntax typeDeclaration) => typeDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword) || m.IsKind(SyntaxKind.InternalKeyword));

    public static bool IsNested(this MemberDeclarationSyntax typeDeclaration) => typeDeclaration.Parent is TypeDeclarationSyntax;

    public static bool IsAbstract(this MemberDeclarationSyntax typeDeclaration) => typeDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.AbstractKeyword));

    public static bool IsStatic(this MemberDeclarationSyntax typeDeclaration) => typeDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword));

    public static string GetTypeName(this TypeSyntax type)
    {
        if (type is QualifiedNameSyntax qualified)
            return qualified.Right.Identifier.Text;
        else if (type is IdentifierNameSyntax identifier)
            return identifier.Identifier.Text;
        else if (type is PredefinedTypeSyntax predefined)
            return predefined.Keyword.Text;

        return type.ToFullString();
    }

    public static bool IsDerivedFrom(this TypeDeclarationSyntax typeDeclaration, params string[] baseTypeNames)
    {
        if (typeDeclaration.BaseList == null)
            return false;

        if (baseTypeNames == null)
            return false;

        foreach (var baseType in typeDeclaration.BaseList.Types)
        {
            string baseName;
            if (baseType.Type is QualifiedNameSyntax qualifiedName)
            {
                baseName = qualifiedName.Right.Identifier.ValueText;
            }
            else if (baseType.Type is IdentifierNameSyntax identifierName)
            {
                baseName = identifierName.Identifier.ValueText;
            }
            else if (baseType.Type is GenericNameSyntax genericName)
            {
                baseName = genericName.Identifier.ValueText + "`" + genericName.TypeArgumentList.Arguments.Count;
            }
            else
            {
                continue;
            }

            if (baseTypeNames.Contains(baseName))
            {
                return true;
            }

            if (baseTypeNames.Any(x => x.EndsWith('.' + baseName)))
            {
                return true;
            }

        }

        return false;
    }

    public static string GetNamespace(this BaseTypeDeclarationSyntax syntax)
    {
        // If we don't have a namespace at all we'll return an empty string
        // This accounts for the "default namespace" case
        string nameSpace = string.Empty;

        // Get the containing syntax node for the type declaration
        // (could be a nested type, for example)
        SyntaxNode? potentialNamespaceParent = syntax.Parent;

        // Keep moving "out" of nested classes etc until we get to a namespace
        // or until we run out of parents
        while (potentialNamespaceParent != null &&
                potentialNamespaceParent is not NamespaceDeclarationSyntax
                && potentialNamespaceParent is not FileScopedNamespaceDeclarationSyntax)
        {
            potentialNamespaceParent = potentialNamespaceParent.Parent;
        }

        // Build up the final namespace by looping until we no longer have a namespace declaration
        if (potentialNamespaceParent is BaseNamespaceDeclarationSyntax namespaceParent)
        {
            // We have a namespace. Use that as the type
            nameSpace = namespaceParent.Name.ToString();

            // Keep moving "out" of the namespace declarations until we 
            // run out of nested namespace declarations
            while (true)
            {
                if (namespaceParent.Parent is not NamespaceDeclarationSyntax parent)
                {
                    break;
                }

                // Add the outer namespace as a prefix to the final namespace
                nameSpace = $"{namespaceParent.Name}.{nameSpace}";
                namespaceParent = parent;
            }
        }

        // return the final namespace
        return nameSpace;
    }

    public static bool HasMethod(this TypeDeclarationSyntax typeDeclaration, string methodName)
    {
        return typeDeclaration.Members.OfType<MethodDeclarationSyntax>().Any(x => x.Identifier.Text == methodName);
    }

    public static AttributeSyntax? GetAttribute(this MemberDeclarationSyntax typeDeclaration, string attributeName)
    {
        return typeDeclaration.AttributeLists.SelectMany(x => x.Attributes).FirstOrDefault(x => x.Name.GetTypeName() == attributeName);
    }

    public static bool HasAttribute(this MemberDeclarationSyntax typeDeclaration, string attributeName)
    {
        return typeDeclaration.AttributeLists.SelectMany(x => x.Attributes).Any(x => x.Name.GetTypeName() == attributeName);
    }

}
