using System.Linq.Expressions;
using QueryMakerLibrary.Components;
using QueryMakerLibrary.Constants;
using QueryMakerLibrary.Logic;
using static QueryMakerLibrary.Components.Sort;

namespace QueryMakerLibrary
{
	internal static class PerformActions
	{
		#region Internal Methods

		internal static QueryMakerResult<T> CreateActionsResult<T>(IQueryable<T> query, QueryMaker queryMaker)
		{
			if (query is null)
			{
				throw Errors.Exception(Errors.IQueryableNull);
			}

			IQueryable<T> filteredQuery = CreateFilteredQuery(query, queryMaker.Filter);

			if (queryMaker.Select is not null && queryMaker.Select.DistinctBy)
			{
				Select select = new(queryMaker.Select.Fields, true);

				IQueryable<T> unpaginatedQuery =
					CreateSortedQuery(
						CreateSelectedQuery(
							filteredQuery,
							select),
						queryMaker.Sort,
						select.Fields);

				select.DistinctBy = false;

				if (queryMaker.Page is not null
					&& !string.IsNullOrWhiteSpace(queryMaker.Page.Index))
				{
					if (!select.Fields.Contains(queryMaker.Page.Index))
					{
						return new(CreatePagedQuery(
								query, unpaginatedQuery,
								new(queryMaker.Page.Skip,
									queryMaker.Page.Take)),
							unpaginatedQuery);
					}
					else
					{
						return new(CreateSelectedQuery(
							CreateSortedQuery(
								CreatePagedQuery(
									query,
									unpaginatedQuery,
									queryMaker.Page),
								queryMaker.Sort,
								select.Fields),
							select),
							unpaginatedQuery);
					}
				}
				else
				{
					return new(CreatePagedQuery(
							query, unpaginatedQuery,
							queryMaker.Page),
						unpaginatedQuery);
				}
			}
			else if (queryMaker.Page is not null
				&& !string.IsNullOrWhiteSpace(queryMaker.Page.Index))
			{
				IQueryable<T> sortedQuery = CreateSortedQuery(
					filteredQuery, queryMaker.Sort);

				return new(CreateSelectedQuery(
					CreateSortedQuery(
						CreatePagedQuery(
							query,
							sortedQuery,
							queryMaker.Page),
						queryMaker.Sort),
					queryMaker.Select),
					CreateSelectedQuery(
						sortedQuery,
						queryMaker.Select));
			}
			else
			{
				IQueryable<T> unpaginatedQuery =
					CreateSelectedQuery(
						CreateSortedQuery(
							filteredQuery,
							queryMaker.Sort),
						queryMaker.Select);

				return new(CreatePagedQuery(
						query,
						unpaginatedQuery,
						queryMaker.Page),
					unpaginatedQuery);
			}
		}

		internal static IQueryable<T> CreateActionsQuery<T>(IQueryable<T> query, QueryMaker queryMaker)
		{
			if (query is null)
			{
				throw Errors.Exception(Errors.IQueryableNull);
			}

			return CreateActionsResult(query, queryMaker).PaginatedQuery;
		}

		#endregion Internal Methods

		#region Private Methods

		private static IQueryable<T> CreateFilteredQuery<T>(IQueryable<T> query, Filter? filter = null)
		{
			if (filter is not null)
			{
				ParameterExpression parameterExpression = Expression.Parameter(typeof(T), Miscellaneous.INSTANCE);

				return query.Where(
					Expression.Lambda<Func<T, bool>>(
						FilterMethods.FilterAction<T>(parameterExpression, filter),
						parameterExpression));
			}

			return query;
		}

		private static IQueryable<T> CreateSortedQuery<T>(IQueryable<T> query, Sort? sort = null,
			string[]? allowedFields = null, IOrderedQueryable<T>? orderedQuery = null)
		{
			if (sort is not null)
			{
				if (string.IsNullOrWhiteSpace(sort.Field))
				{
					throw Errors.Exception(Errors.SortFieldEmpty);
				}
				else if (allowedFields is not null
					&& allowedFields.Length > 0
					&& !allowedFields.Contains(sort.Field))
				{
					return sort.Then is not null
						? CreateSortedQuery(query, sort.Then, allowedFields, orderedQuery)
						: orderedQuery is null ? query : orderedQuery;
				}

				Expression<Func<T, object>> selectExpression = SelectMethods.CreateSelectExpression<T>(
						Expression.Parameter(typeof(T), Miscellaneous.INSTANCE),
						sort.Field);

				orderedQuery = sort.Direction switch
				{
					SortDirections.Ascending => orderedQuery is null
						? query.OrderBy(selectExpression)
						: orderedQuery.ThenBy(selectExpression),

					SortDirections.Descending => orderedQuery is null
						? query.OrderByDescending(selectExpression)
						: orderedQuery.ThenByDescending(selectExpression),

					_ => throw Errors.Exception(Errors.InvalidSortingDirection, sort.Direction)
				};

				if (sort.Then is not null)
				{
					return CreateSortedQuery(query, sort.Then, allowedFields, orderedQuery);
				}

				return orderedQuery;
			}

			return query;
		}

		private static IQueryable<T> CreateSelectedQuery<T>(IQueryable<T> query, Select? select = null)
		{
			if (select is not null
				&& select.Fields.Any())
			{
				if (select.DistinctBy)
				{
					return query.GroupBy(SelectMethods.CreateObjectSelectExpression<T>(
						Expression.Parameter(typeof(T), Miscellaneous.INSTANCE),
						select.Fields))
					.Select(group => group.Key);
				}
				else
				{
					return query.Select(SelectMethods.CreateObjectSelectExpression<T>(
						Expression.Parameter(typeof(T), Miscellaneous.INSTANCE),
						select.Fields));
				}
			}

			return query;
		}

		private static IQueryable<T> CreatePagedQuery<T>(IQueryable<T> unfilteredQuery, IQueryable<T> filteredQuery, Page? page = null)
		{
			if (page is not null)
			{
				if (page.Skip > 0)
				{
					filteredQuery = filteredQuery.Skip(page.Skip);
				}

				if (page.Take > 0)
				{
					filteredQuery = filteredQuery.Take(page.Take);
				}

				if (!string.IsNullOrWhiteSpace(page.Index))
				{
					ParameterExpression parameterExpression = Expression.Parameter(typeof(T), Miscellaneous.INSTANCE);
					MemberExpression memberExpression = MemberMethods.GetPropertyOrField<T>(parameterExpression, page.Index);

					return unfilteredQuery.Where(Expression.Lambda<Func<T, bool>>(
						Expression.Call(
							EnumerableMethods.GetEnumerableTypedMethod(memberExpression.Type,
								EnumMethods.GetActionText(Filter.FilterActions.Contains)),
							SelectMethods.CreateTypedSelectConstantExpression(filteredQuery, memberExpression.Type.Name,
								parameterExpression, page.Index),
							memberExpression),
						parameterExpression));
				}
			}

			return filteredQuery;
		}

		#endregion Private Methods
	}
}