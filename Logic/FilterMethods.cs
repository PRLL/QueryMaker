using System.Linq.Expressions;
using System.Reflection;
using QueryMakerLibrary.Components;
using QueryMakerLibrary.Constants;
using static QueryMakerLibrary.Components.Filter;

namespace QueryMakerLibrary.Logic
{
	internal static class FilterMethods
	{
		internal static Expression? FilterAction<T>(ParameterExpression parameterExpression, Filter filter)
		{
			if (MemberMethods.IsEnumerableType(typeof(T)))
			{
				if (!EnumMethods.IsValidAction(filter.Action))
				{
					throw Errors.Exception(Errors.FilterInvalidAction);
				}

				return ContentEvaluation.CreateListContentEvaluationExpression(new ActionExpression(
					string.Empty, filter.Action, filter.Negate, filter.IgnoreCase,
					parameterExpression, filter.Value));
			}

			Expression? filterExpression = filter.IsJoiner ? null : CreateFilterExpression<T>(parameterExpression, filter);

			if (filter.SubFilters.Any())
			{
				if (!EnumMethods.IsValidOperation(filter.SubFiltersOperation))
				{
					throw Errors.Exception(Errors.InvalidOperationValue, filter.SubFiltersOperation);
				}

				Expression? predicatesExpression = null;
				foreach (Filter predicate in filter.SubFilters)
				{
					predicatesExpression = FilterAction<T>(parameterExpression, predicate);

					filterExpression = filterExpression is null
						? predicatesExpression
						: predicatesExpression is null
							? filterExpression
							: WhereExpressionOperationHandler(filterExpression, predicatesExpression, filter.SubFiltersOperation);
				}
			}

			return filterExpression;
		}

		private static Expression? CreateFilterExpression<T>(ParameterExpression parameterExpression, Filter filter, Expression? newExpression = null)
		{
			if (filter.Fields.Length < 1)
			{
				return CreateExpression.ActionExpression(new(
					null, filter.Action, filter.Negate, filter.IgnoreCase,
					parameterExpression, filter.Value));
			}

			if (filter.Fields.Length > 1 && !EnumMethods.IsValidOperation(filter.FieldsOperation))
			{
				throw Errors.Exception(Errors.MultipleFieldsNoOperation);
			}

			foreach (string field in filter.Fields)
			{
				if (!EnumMethods.IsValidAction(filter.Action))
				{
					throw Errors.Exception(Errors.FilterInvalidAction);
				}

				Expression? createdExpression = CreateExpression.ActionExpression(new ActionExpression(
					field, filter.Action, filter.Negate, filter.IgnoreCase,
					MemberMethods.GetPropertyOrField<T>(parameterExpression, field),
					filter.Value));

				newExpression = newExpression is null
					? createdExpression
					: createdExpression is null
						? newExpression
						: WhereExpressionOperationHandler(newExpression, createdExpression, filter.FieldsOperation);
			}

			return newExpression;
		}

		/// <summary>
		/// Handles operation assignment between 'Where' expressions.
		/// </summary>
		private static Expression WhereExpressionOperationHandler(Expression? currentExpression, Expression newExpression, FilterOperations operation)
		{
			if (currentExpression is null)
			{
				return newExpression;
			}

			switch (operation)
			{
				case FilterOperations.And:
					return Expression.And(currentExpression,
						newExpression);

				case FilterOperations.Or:
					return Expression.Or(currentExpression,
						newExpression);

				case FilterOperations.AndAlso:
					return Expression.AndAlso(currentExpression,
						newExpression);

				case FilterOperations.OrElse:
					return Expression.OrElse(currentExpression,
						newExpression);

				default:
					throw Errors.Exception(Errors.InvalidOperationValue, operation);
			}
		}
	}
}