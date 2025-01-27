using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using QueryMakerLibrary.Constants;
using static QueryMakerLibrary.Components.Filter;

namespace QueryMakerLibrary.Logic
{
	internal static class ContentEvaluation
	{
		internal static Expression? CreateEvaluationExpression(ActionExpression actionExpression, object? value)
		{
			bool valueIsNull = value is null;
			Type? valueType = value?.GetType();

			Expression typedMemberExpression = !actionExpression.IsMemberExpressionString
				&& !valueIsNull
				&& (valueType != actionExpression.ActualMemberType || actionExpression.IsContentAction)
					? Expression.Call(
						actionExpression.MemberExpression,
						"ToString",
						Type.EmptyTypes)
					: actionExpression.MemberExpression;

			Expression typedValueExpression = Expression.Constant(
				!valueIsNull && valueType != typeof(string)
				&& (valueType != actionExpression.ActualMemberType || actionExpression.IsContentAction)
					? Convert.ToString(value)
					: value);

			bool isMemberExpressionStringType = typedMemberExpression.Type == typeof(string);

            if (actionExpression.IsContentAction && valueIsNull
				|| (isMemberExpressionStringType && actionExpression.IsGreaterOrLessThanAction))
            {
                typedValueExpression = Expression.Constant("null");
            }
			else if (MemberMethods.IsNullableType(typedMemberExpression.Type)
				&& !MemberMethods.IsNullableType(typedValueExpression.Type))
			{
                typedValueExpression = Expression.Convert(typedValueExpression, typedMemberExpression.Type);
            }
			else if (valueIsNull && !MemberMethods.IsNullableType(typedValueExpression.Type))
			{
				typedValueExpression = Expression.Convert(typedValueExpression, typeof(Nullable<>).MakeGenericType(typedMemberExpression.Type));
				typedMemberExpression = Expression.Convert(typedMemberExpression, typeof(Nullable<>).MakeGenericType(typedMemberExpression.Type));
			}

            if (!valueIsNull && actionExpression.IgnoreCase && isMemberExpressionStringType)
			{
				typedMemberExpression = Expression.Call(typedMemberExpression,
					"ToLower",
					Type.EmptyTypes);

				typedValueExpression = Expression.Call(
					typedValueExpression,
					"ToLower",
					Type.EmptyTypes);
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

					evaluationExpression = actionExpression.Action switch
					{
						FilterActions.NotContains or FilterActions.NotStartsWith or FilterActions.NotEndsWith
							=> Expression.Not(evaluationExpression),

						_ => evaluationExpression,
					};

					break;
				case FilterActions.Equal:
					evaluationExpression = Expression.Equal(typedMemberExpression, typedValueExpression);
					break;

				case FilterActions.NotEqual:
					evaluationExpression = Expression.NotEqual(typedMemberExpression, typedValueExpression);
					break;

				case FilterActions.GreaterThan: case FilterActions.LessThan:
				case FilterActions.GreaterThanOrEqual: case FilterActions.LessThanOrEqual:
					if (typedMemberExpression.Type == typeof(bool))
					{
						typedMemberExpression = Expression.Convert(
							actionExpression.MemberExpression,
							typeof(int));

						typedValueExpression = Expression.Convert(
							Expression.Constant(value),
							typeof(int));
					}
					else if (isMemberExpressionStringType)
					{
						PropertyInfo stringLengthProperty = typeof(string).GetProperty(nameof(string.Length))
							?? throw new NullReferenceException($"Property {nameof(string.Length)} does not exist in type {typeof(string).Name}");

						typedMemberExpression = Expression.MakeMemberAccess(typedMemberExpression, stringLengthProperty);
						typedValueExpression = Expression.MakeMemberAccess(typedValueExpression, stringLengthProperty);
					}

					switch (actionExpression.Action)
					{
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
					}
					break;

				default:
					throw Errors.Exception(Errors.FilterInvalidAction);
			}

			return actionExpression.Negate && evaluationExpression is not null
				? Expression.Not(evaluationExpression)
				: evaluationExpression;
		}

		internal static Expression? CreateMemberToListEvaluationExpression(ActionExpression actionExpression)
		{
			IList valuesList = (actionExpression.ValueExpression.Value as IList) ?? throw Errors.Exception(Errors.ExpressionValueToList);

			Expression? valueItemsEvaluationExpression = null;
			foreach (object? item in valuesList)
			{
				Expression? newExpression = CreateEvaluationExpression(actionExpression, item);

				valueItemsEvaluationExpression = valueItemsEvaluationExpression is null
					? newExpression
					: newExpression is null
						? valueItemsEvaluationExpression
						: Expression.OrElse(valueItemsEvaluationExpression, newExpression);
			}

			return valueItemsEvaluationExpression;
		}

		internal static Expression? CreateListContentEvaluationExpression(ActionExpression actionExpression)
		{
			Expression memberExpression = actionExpression.MemberExpression;
			Type memberType = actionExpression.ActualMemberType;

			actionExpression.MemberExpression = Expression.Parameter(memberType, "a");

			MethodInfo? methodInfo = typeof(ContentEvaluation)
				.GetMethod(nameof(CreateListToValueEvaluationExpression), BindingFlags.NonPublic | BindingFlags.Static)?
				.MakeGenericMethod(new Type[] { memberType });

			if (!actionExpression.IsValueEnumerable)
			{
				return (Expression?)methodInfo?.Invoke(null, new object?[]{ memberExpression, actionExpression, actionExpression.ValueExpression.Value });
			}
			else
			{
				Expression? anyValueItemsEvaluationExpression = null;
				foreach (object? item in (actionExpression.ValueExpression.Value as IList) ?? throw Errors.Exception(Errors.ExpressionValueToList))
				{
					Expression? newExpression = (Expression?)methodInfo?.Invoke(null, new object?[] { memberExpression, actionExpression, item });

					anyValueItemsEvaluationExpression = anyValueItemsEvaluationExpression is null
						? newExpression
						: newExpression is null
							? anyValueItemsEvaluationExpression
							: Expression.OrElse(anyValueItemsEvaluationExpression, newExpression);
				}

				return anyValueItemsEvaluationExpression;
			}
		}

		internal static Expression? CreateListToValueEvaluationExpression<T>(Expression MemberExpression, ActionExpression anyActionExpression, object? item)
		{
			Expression? filterExpression = CreateEvaluationExpression(anyActionExpression, item);

			return filterExpression is null
				? filterExpression
				: Expression.Call(typeof(Enumerable),
					"Any",
					new[] { typeof(T) },
					MemberExpression,
					Expression.Lambda<Func<T, bool>>(
						filterExpression,
						(ParameterExpression)anyActionExpression.MemberExpression));
		}
	}
}