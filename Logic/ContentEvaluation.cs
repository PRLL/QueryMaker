using System.Collections;
using System.Linq.Expressions;
using QueryMakerLibrary.Constants;
using static QueryMakerLibrary.Components.Filter;

namespace QueryMakerLibrary.Logic
{
	internal static class ContentEvaluation
	{
		internal static Expression CreateEvaluationExpression(ActionExpression actionExpression, object? value)
		{
			bool itemSameTypeAsMember = value is not null && value.GetType().Equals(actionExpression.MemberExpression.Type);
			bool isContentAction = false;
			
			switch (actionExpression.Action)
			{
				case FilterActions.Contains: case FilterActions.NotContains:
				case FilterActions.StartsWith: case FilterActions.NotStartsWith:
				case FilterActions.EndsWith: case FilterActions.NotEndsWith:
					isContentAction = true;
					break;
			}

			Expression typedMemberExpression = (!itemSameTypeAsMember && !actionExpression.IsMemberExpressionString)
				|| (isContentAction && !actionExpression.IsMemberExpressionString)
					? Expression.Call(
						actionExpression.MemberExpression,
						"ToString",
						Type.EmptyTypes)
					: actionExpression.MemberExpression;
			Expression typedValueExpression = Expression.Constant((!itemSameTypeAsMember && (value is not null
				&& !value.GetType().Equals(typeof(string)))) || (isContentAction
				&& value is not null && !value.GetType().Equals(typeof(string)))
					? Convert.ToString(value)
					: value);

			if (actionExpression.IgnoreCase)
			{
				if (actionExpression.IsMemberExpressionString)
				{
					typedMemberExpression = Expression.Call(typedMemberExpression,
						"ToLower",
						Type.EmptyTypes);
				}

				if (value is not null && value.GetType().Equals(typeof(string)))
				{
					typedValueExpression = Expression.Call(
						typedValueExpression,
						"ToLower",
						Type.EmptyTypes);
				}
			}

			Expression? evaluationExpression = null;
			switch (actionExpression.Action)
			{
				case FilterActions.Contains: case FilterActions.NotContains:
				case FilterActions.StartsWith: case FilterActions.NotStartsWith:
				case FilterActions.EndsWith: case FilterActions.NotEndsWith:
					evaluationExpression = Expression.Call(
						typedMemberExpression,
						EnumMethods.GetActionText(actionExpression.Action),
						null,
						typedValueExpression);

					switch (actionExpression.Action)
					{
						case FilterActions.NotContains:
						case FilterActions.NotStartsWith:
						case FilterActions.NotEndsWith:
							return Expression.Not(evaluationExpression);

						default:
							return evaluationExpression;
					}

				case FilterActions.Equal:
					evaluationExpression = Expression.Equal(typedMemberExpression, typedValueExpression);
					break;

				case FilterActions.NotEqual:
					evaluationExpression = Expression.NotEqual(typedMemberExpression, typedValueExpression);
					break;

				case FilterActions.GreaterThan:
					evaluationExpression = Expression.GreaterThan(typedMemberExpression, typedValueExpression);
					break;

				case FilterActions.LessThan:
					evaluationExpression = Expression.LessThan(typedMemberExpression, typedValueExpression);
					break;

				case FilterActions.GreaterThanOrEqual:
					evaluationExpression = Expression.GreaterThanOrEqual(typedMemberExpression, typedValueExpression);
					break;

				case FilterActions.LessThanOrEqual:
					evaluationExpression = Expression.LessThanOrEqual(typedMemberExpression, typedValueExpression);
					break;

				default:
					throw Errors.Exception(Errors.FilterInvalidAction);
			}

			if (evaluationExpression is null)
			{
				throw Errors.Exception(Errors.GeneratedExpressionNull);
			}

			return actionExpression.Negate
				? Expression.Not(evaluationExpression)
				: evaluationExpression;
		}

		internal static Expression CreateMemberToListEvaluationExpression(ActionExpression actionExpression)
		{
			IList valuesList = (actionExpression.ValueExpression.Value as IList) ?? throw Errors.Exception(Errors.ExpressionValueToList);

			Expression? valueItemsEvaluationExpression = null;
			foreach (object? item in valuesList)
			{
				Expression newExpression = CreateEvaluationExpression(actionExpression, item);

				valueItemsEvaluationExpression = valueItemsEvaluationExpression is null
					? newExpression
					: Expression.OrElse(valueItemsEvaluationExpression,
						newExpression ?? throw Errors.Exception(Errors.GeneratedExpressionNull));
			}

			return valueItemsEvaluationExpression ?? throw Errors.Exception(Errors.GeneratedExpressionNull);
		}

		internal static Expression CreateListContentEvaluationExpression(ActionExpression actionExpression)
		{
			Expression memberExpression = actionExpression.MemberExpression;
			Type memberType = actionExpression.ActualMemberType;

			// reuse passed 'actionExpression' and change it's Member Expression to create the parameter evaluation expressions
			actionExpression.MemberExpression = Expression.Parameter(memberType, "a");

			if (!actionExpression.IsValueEnumerable)
			{
				return CreateListToValueEvaluationExpression(memberType.Name, memberExpression, actionExpression, actionExpression.ValueExpression.Value);
			}
			else
			{
				Expression? anyValueItemsEvaluationExpression = null;
				foreach (object? item in (actionExpression.ValueExpression.Value as IList) ?? throw Errors.Exception(Errors.ExpressionValueToList))
				{
					Expression newExpression = CreateListToValueEvaluationExpression(memberType.Name, memberExpression, actionExpression, item);

					anyValueItemsEvaluationExpression = anyValueItemsEvaluationExpression is null
						? newExpression
						: Expression.OrElse(anyValueItemsEvaluationExpression,
							newExpression);
				}

				return anyValueItemsEvaluationExpression ?? throw Errors.Exception(Errors.GeneratedExpressionNull);
			}
		}

		private static Expression CreateListToValueEvaluationExpression(string membertTypeName, Expression MemberExpression, ActionExpression anyActionExpression, object? item)
		{
			switch (membertTypeName)
			{
				case PrimtiveTypes.BOOL:
					return Expression.Call(typeof(Enumerable),
						"Any",
						new[] { typeof(bool) },
						MemberExpression,
						Expression.Lambda<Func<bool, bool>>(
							CreateEvaluationExpression(anyActionExpression, item),
							(ParameterExpression)anyActionExpression.MemberExpression));

				case PrimtiveTypes.BYTE:
					return Expression.Call(typeof(Enumerable),
						"Any",
						new[] { typeof(byte) },
						MemberExpression,
						Expression.Lambda<Func<byte, bool>>(
							CreateEvaluationExpression(anyActionExpression, item),
							(ParameterExpression)anyActionExpression.MemberExpression));

				case PrimtiveTypes.SBYTE:
					return Expression.Call(typeof(Enumerable),
						"Any",
						new[] { typeof(sbyte) },
						MemberExpression,
						Expression.Lambda<Func<sbyte, bool>>(
							CreateEvaluationExpression(anyActionExpression, item),
							(ParameterExpression)anyActionExpression.MemberExpression));

				case PrimtiveTypes.SHORT:
					return Expression.Call(typeof(Enumerable),
						"Any",
						new[] { typeof(short) },
						MemberExpression,
						Expression.Lambda<Func<short, bool>>(
							CreateEvaluationExpression(anyActionExpression, item),
							(ParameterExpression)anyActionExpression.MemberExpression));

				case PrimtiveTypes.USHORT:
					return Expression.Call(typeof(Enumerable),
						"Any",
						new[] { typeof(ushort) },
						MemberExpression,
						Expression.Lambda<Func<ushort, bool>>(
							CreateEvaluationExpression(anyActionExpression, item),
							(ParameterExpression)anyActionExpression.MemberExpression));

				case PrimtiveTypes.INT:
					return Expression.Call(typeof(Enumerable),
						"Any",
						new[] { typeof(int) },
						MemberExpression,
						Expression.Lambda<Func<int, bool>>(
							CreateEvaluationExpression(anyActionExpression, item),
							(ParameterExpression)anyActionExpression.MemberExpression));

				case PrimtiveTypes.UINT:
					return Expression.Call(typeof(Enumerable),
						"Any",
						new[] { typeof(uint) },
						MemberExpression,
						Expression.Lambda<Func<uint, bool>>(
							CreateEvaluationExpression(anyActionExpression, item),
							(ParameterExpression)anyActionExpression.MemberExpression));

				case PrimtiveTypes.LONG:
					return Expression.Call(typeof(Enumerable),
						"Any",
						new[] { typeof(long) },
						MemberExpression,
						Expression.Lambda<Func<long, bool>>(
							CreateEvaluationExpression(anyActionExpression, item),
							(ParameterExpression)anyActionExpression.MemberExpression));

				case PrimtiveTypes.ULONG:
					return Expression.Call(typeof(Enumerable),
						"Any",
						new[] { typeof(ulong) },
						MemberExpression,
						Expression.Lambda<Func<ulong, bool>>(
							CreateEvaluationExpression(anyActionExpression, item),
							(ParameterExpression)anyActionExpression.MemberExpression));

				case PrimtiveTypes.INTPTR:
					return Expression.Call(typeof(Enumerable),
						"Any",
						new[] { typeof(IntPtr) },
						MemberExpression,
						Expression.Lambda<Func<IntPtr, bool>>(
							CreateEvaluationExpression(anyActionExpression, item),
							(ParameterExpression)anyActionExpression.MemberExpression));

				case PrimtiveTypes.UINTPTR:
					return Expression.Call(typeof(Enumerable),
						"Any",
						new[] { typeof(UIntPtr) },
						MemberExpression,
						Expression.Lambda<Func<UIntPtr, bool>>(
							CreateEvaluationExpression(anyActionExpression, item),
							(ParameterExpression)anyActionExpression.MemberExpression));

				case PrimtiveTypes.CHAR:
					return Expression.Call(typeof(Enumerable),
						"Any",
						new[] { typeof(char) },
						MemberExpression,
						Expression.Lambda<Func<char, bool>>(
							CreateEvaluationExpression(anyActionExpression, item),
							(ParameterExpression)anyActionExpression.MemberExpression));

				case PrimtiveTypes.FLOAT:
					return Expression.Call(typeof(Enumerable),
						"Any",
						new[] { typeof(float) },
						MemberExpression,
						Expression.Lambda<Func<float, bool>>(
							CreateEvaluationExpression(anyActionExpression, item),
							(ParameterExpression)anyActionExpression.MemberExpression));

				case PrimtiveTypes.DOUBLE:
					return Expression.Call(typeof(Enumerable),
						"Any",
						new[] { typeof(double) },
						MemberExpression,
						Expression.Lambda<Func<double, bool>>(
							CreateEvaluationExpression(anyActionExpression, item),
							(ParameterExpression)anyActionExpression.MemberExpression));

				case PrimtiveTypes.DECIMAL:
					return Expression.Call(typeof(Enumerable),
						"Any",
						new[] { typeof(decimal) },
						MemberExpression,
						Expression.Lambda<Func<decimal, bool>>(
							CreateEvaluationExpression(anyActionExpression, item),
							(ParameterExpression)anyActionExpression.MemberExpression));

				case PrimtiveTypes.STRING:
					return Expression.Call(typeof(Enumerable),
						"Any",
						new[] { typeof(string) },
						MemberExpression,
						Expression.Lambda<Func<string, bool>>(
							CreateEvaluationExpression(anyActionExpression, item),
							(ParameterExpression)anyActionExpression.MemberExpression));

				case PrimtiveTypes.DATETIME:
					return Expression.Call(typeof(Enumerable),
						"Any",
						new[] { typeof(DateTime) },
						MemberExpression,
						Expression.Lambda<Func<DateTime, bool>>(
							CreateEvaluationExpression(anyActionExpression, item),
							(ParameterExpression)anyActionExpression.MemberExpression));

				case PrimtiveTypes.GUID:
					return Expression.Call(typeof(Enumerable),
						"Any",
						new[] { typeof(Guid) },
						MemberExpression,
						Expression.Lambda<Func<Guid, bool>>(
							CreateEvaluationExpression(anyActionExpression, item),
							(ParameterExpression)anyActionExpression.MemberExpression));

				// case PrimtiveTypes.ENUM:
				// 	return Enum.Parse(value);

				case PrimtiveTypes.OBJECT:
					return Expression.Call(typeof(Enumerable),
						"Any",
						new[] { typeof(object) },
						MemberExpression,
						Expression.Lambda<Func<object, bool>>(
							CreateEvaluationExpression(anyActionExpression, item),
							(ParameterExpression)anyActionExpression.MemberExpression));

				default:
					throw Errors.Exception(Errors.InvalidTypeConversion);
			}
		}
	}
}