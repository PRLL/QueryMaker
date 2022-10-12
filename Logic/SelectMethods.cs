using System.Linq.Expressions;
using System.Reflection;
using QueryMakerLibrary.Constants;

namespace QueryMakerLibrary.Logic
{
	internal static class SelectMethods
	{
		internal static Expression<Func<T, object>> CreateSelectExpression<T>(ParameterExpression parameterExpression, string field)
		{
			return Expression.Lambda<Func<T, object>>( // o => Convert(o.Field, typeof(object))
				Expression.Convert( // Convert(o.Field, typeof(object))
					MemberMethods.GetPropertyOrField<T>(parameterExpression, field), // o.Field
					typeof(object)),
				parameterExpression);
		}

		internal static ConstantExpression CreateTypedSelectConstantExpression<T>(IQueryable<T> pagedQuery, string typeName, ParameterExpression parameterExpression, string field)
		{
			switch (typeName)
			{
				case PrimtiveTypes.BOOL:
					return Expression.Constant(
						pagedQuery.Select(
							Expression.Lambda<Func<T, bool>>(
								MemberMethods.GetPropertyOrField<T>(parameterExpression, field),
								parameterExpression)));

				case PrimtiveTypes.BYTE:
					return Expression.Constant(
						pagedQuery.Select(
							Expression.Lambda<Func<T, byte>>(
								MemberMethods.GetPropertyOrField<T>(parameterExpression, field),
								parameterExpression)));

				case PrimtiveTypes.SBYTE:
					return Expression.Constant(
						pagedQuery.Select(
							Expression.Lambda<Func<T, sbyte>>(
								MemberMethods.GetPropertyOrField<T>(parameterExpression, field),
								parameterExpression)));

				case PrimtiveTypes.SHORT:
					return Expression.Constant(
						pagedQuery.Select(
							Expression.Lambda<Func<T, short>>(
								MemberMethods.GetPropertyOrField<T>(parameterExpression, field),
								parameterExpression)));

				case PrimtiveTypes.USHORT:
					return Expression.Constant(
						pagedQuery.Select(
							Expression.Lambda<Func<T, ushort>>(
								MemberMethods.GetPropertyOrField<T>(parameterExpression, field),
								parameterExpression)));

				case PrimtiveTypes.INT:
					return Expression.Constant(
						pagedQuery.Select(
							Expression.Lambda<Func<T, int>>(
								MemberMethods.GetPropertyOrField<T>(parameterExpression, field),
								parameterExpression)));

				case PrimtiveTypes.UINT:
					return Expression.Constant(
						pagedQuery.Select(
							Expression.Lambda<Func<T, uint>>(
								MemberMethods.GetPropertyOrField<T>(parameterExpression, field),
								parameterExpression)));

				case PrimtiveTypes.LONG:
					return Expression.Constant(
						pagedQuery.Select(
							Expression.Lambda<Func<T, long>>(
								MemberMethods.GetPropertyOrField<T>(parameterExpression, field),
								parameterExpression)));

				case PrimtiveTypes.ULONG:
					return Expression.Constant(
						pagedQuery.Select(
							Expression.Lambda<Func<T, ulong>>(
								MemberMethods.GetPropertyOrField<T>(parameterExpression, field),
								parameterExpression)));

				case PrimtiveTypes.INTPTR:
					return Expression.Constant(
						pagedQuery.Select(
							Expression.Lambda<Func<T, IntPtr>>(
								MemberMethods.GetPropertyOrField<T>(parameterExpression, field),
								parameterExpression)));

				case PrimtiveTypes.UINTPTR:
					return Expression.Constant(
						pagedQuery.Select(
							Expression.Lambda<Func<T, UIntPtr>>(
								MemberMethods.GetPropertyOrField<T>(parameterExpression, field),
								parameterExpression)));

				case PrimtiveTypes.CHAR:
					return Expression.Constant(
						pagedQuery.Select(
							Expression.Lambda<Func<T, char>>(
								MemberMethods.GetPropertyOrField<T>(parameterExpression, field),
								parameterExpression)));

				case PrimtiveTypes.FLOAT:
					return Expression.Constant(
						pagedQuery.Select(
							Expression.Lambda<Func<T, float>>(
								MemberMethods.GetPropertyOrField<T>(parameterExpression, field),
								parameterExpression)));

				case PrimtiveTypes.DOUBLE:
					return Expression.Constant(
						pagedQuery.Select(
							Expression.Lambda<Func<T, double>>(
								MemberMethods.GetPropertyOrField<T>(parameterExpression, field),
								parameterExpression)));

				case PrimtiveTypes.DECIMAL:
					return Expression.Constant(
						pagedQuery.Select(
							Expression.Lambda<Func<T, decimal>>(
								MemberMethods.GetPropertyOrField<T>(parameterExpression, field),
								parameterExpression)));

				case PrimtiveTypes.STRING:
					return Expression.Constant(
						pagedQuery.Select(
							Expression.Lambda<Func<T, string>>(
								MemberMethods.GetPropertyOrField<T>(parameterExpression, field),
								parameterExpression)));

				case PrimtiveTypes.DATETIME:
					return Expression.Constant(
						pagedQuery.Select(
							Expression.Lambda<Func<T, DateTime>>(
								MemberMethods.GetPropertyOrField<T>(parameterExpression, field),
								parameterExpression)));

				case PrimtiveTypes.GUID:
					return Expression.Constant(
						pagedQuery.Select(
							Expression.Lambda<Func<T, Guid>>(
								MemberMethods.GetPropertyOrField<T>(parameterExpression, field),
								parameterExpression)));

				// case PrimtiveTypes.ENUM:
				// 	return Enum.Parse(value);

				case PrimtiveTypes.OBJECT:
					return Expression.Constant(
						pagedQuery.Select(
							Expression.Lambda<Func<T, object>>(
								MemberMethods.GetPropertyOrField<T>(parameterExpression, field),
								parameterExpression)));

				default:
					throw Errors.Exception(Errors.InvalidTypeConversion);
			}
		}

		internal static Expression<Func<T, T>> CreateObjectSelectExpression<T>(ParameterExpression parameterExpression, string field)
		{
			return CreateObjectSelectExpression<T>(parameterExpression, new string[1] { field });
		}

		internal static Expression<Func<T, T>> CreateObjectSelectExpression<T>(ParameterExpression parameterExpression, string[] fields)
		{
			return Expression.Lambda<Func<T,T>>( // o => new T { Field1 = o.Field1, Field2 = o.Field2 }
				Expression.MemberInit( // new T { Field1 = o.Field1, Field2 = o.Field2 }
					Expression.New(typeof(T)), // new instance of T()
					fields.Select(field => // create initializer
					{
						MemberExpression memberExpression = MemberMethods.GetPropertyOrField<T>(parameterExpression, field);

						return Expression.Bind( // "o.Field1"
							(PropertyInfo)memberExpression.Member,
							memberExpression);
					})),
				parameterExpression);
		}
	}
}