using System.Linq.Expressions;
using System.Reflection;

namespace QueryMakerLibrary.Logic
{
	internal static class SelectMethods
	{
		internal static Expression<Func<T, object>> CreateSelectExpression<T>(ParameterExpression parameterExpression, string field)
		{
			// expression o => Convert(o.Field, typeof(object))
			return Expression.Lambda<Func<T, object>>(
				// expression Convert(o.Field, typeof(object))
				Expression.Convert(
					// expression o.Field
					MemberMethods.GetPropertyOrField<T>(parameterExpression, field),
					typeof(object)),
				parameterExpression);
		}

		internal static Expression<Func<T, T>> CreateObjectSelectExpression<T>(ParameterExpression parameterExpression, string field)
		{
			return CreateObjectSelectExpression<T>(parameterExpression, new string[1] { field });
		}

		internal static Expression<Func<T, T>> CreateObjectSelectExpression<T>(ParameterExpression parameterExpression, string[] fields)
		{
			// o => new T { Field1 = o.Field1, Field2 = o.Field2 }
			return Expression.Lambda<Func<T,T>>(
				// new T { Field1 = o.Field1, Field2 = o.Field2 }
				Expression.MemberInit(
					// new T()
					Expression.New(typeof(T)),
					// create initializers
					fields.Select(field =>
					{
						MemberExpression memberExpression = MemberMethods.GetPropertyOrField<T>(parameterExpression, field);

						return Expression.Bind(
							(PropertyInfo)memberExpression.Member,
							// original value "o.Field1"
							memberExpression);
					})),
				parameterExpression);
		}
	}
}