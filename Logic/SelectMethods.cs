using System.Linq.Expressions;
using System.Reflection;
using QueryMakerLibrary.Extensions;

namespace QueryMakerLibrary.Logic
{
	internal static class SelectMethods
	{
		internal static Expression<Func<T, U>> CreateSelectExpression<T, U>(ParameterExpression parameterExpression, string field)
		{
			return Expression.Lambda<Func<T, U>>(
				parameterExpression.GetPropertyOrField<T>(field),
				parameterExpression);
		}

		internal static ConstantExpression CreateTypedSelectConstantExpression<T, U>(IQueryable<T> query, ParameterExpression parameterExpression, string field)
		{
			return Expression.Constant(query.Select(CreateSelectExpression<T, U>(parameterExpression, field)));
		}

		internal static Expression<Func<T, T>> CreateObjectSelectExpression<T>(ParameterExpression parameterExpression, string[] fields)
		{
			return Expression.Lambda<Func<T,T>>( // o => new T { Field1 = o.Field1, Field2 = o.Field2 }
				Expression.MemberInit( // new T { Field1 = o.Field1, Field2 = o.Field2 }
					Expression.New(typeof(T)), // new instance of T()
					fields.Select(field => // create initializer
					{
						MemberExpression memberExpression = parameterExpression.GetPropertyOrField<T>(field);

						return Expression.Bind( // "o.Field1"
							(PropertyInfo)memberExpression.Member,
							memberExpression);
					})),
				parameterExpression);
		}
	}
}