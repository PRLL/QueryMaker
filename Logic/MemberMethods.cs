using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using QueryMakerLibrary.Constants;

namespace QueryMakerLibrary.Logic
{
	internal static class MemberMethods
	{
		internal static MemberExpression GetPropertyOrField<T>(ParameterExpression parameterExpression, string memberName)
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

		internal static bool IsEnumerableType(Type type)
		{
			return type.Name != nameof(String) && type.GetInterface(nameof(IEnumerable)) != null;
		}

		internal static bool IsNullableType(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}
	}
}