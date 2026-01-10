using QueryMakerLibrary.Constants;
using System.Linq.Expressions;
using System.Reflection;

namespace QueryMakerLibrary.Extensions;

internal static class ParameterExpressionExtension
{
    internal static MemberExpression GetPropertyOrField<T>(this ParameterExpression parameterExpression, string memberName)
    {
        var propertyInfo = typeof(T).GetProperty(memberName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        var fieldInfo = typeof(T).GetField(memberName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

        return Expression.PropertyOrField(
            parameterExpression,
            propertyInfo is not null
                ? propertyInfo.Name
                : fieldInfo is not null
                    ? fieldInfo.Name
                    : throw Errors.Exception(Errors.NoPropertyOrField, typeof(T).Name, memberName));
    }
}
