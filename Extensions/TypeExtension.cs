using System.Collections;

namespace QueryMakerLibrary.Extensions;

internal static class TypeExtension
{
    internal static bool IsEnumerableType(this Type type)
    {
        return type != typeof(string) && type.GetInterface(nameof(IEnumerable)) is not null;
    }

    internal static bool IsNullableType(this Type type)
    {
        return !type.IsValueType || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
    }

    internal static Type GetActualType(this Type type) => Nullable.GetUnderlyingType(type) ?? type;
}
