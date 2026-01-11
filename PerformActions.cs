using QueryMakerLibrary.Components;
using QueryMakerLibrary.Constants;
using QueryMakerLibrary.Extensions;
using QueryMakerLibrary.Logic;
using System.Linq.Expressions;
using System.Reflection;
using static QueryMakerLibrary.Components.Sort;

namespace QueryMakerLibrary
{
	internal static class PerformActions
	{
		#region Internal Methods

		internal static QueryMakerResult<T> CreateActionsResult<T>(this QueryMaker queryMaker, IQueryable<T> query)
		{
			if (query is null)
			{
				throw Errors.Exception(Errors.IQueryableNull);
			}

			IQueryable<T> filteredQuery = query.Filter(queryMaker.Filter);

			IQueryable<T> paginatedQuery;
			IQueryable<T> unpaginatedQuery;
			if (queryMaker.Select is not null && queryMaker.Select.DistinctBy)
			{
				Select select = new(queryMaker.Select.Fields, true);

				unpaginatedQuery = filteredQuery.Select(select).Sort(queryMaker.Sort, select.Fields);

				select.DistinctBy = false;

				if (queryMaker.Page is not null && !string.IsNullOrWhiteSpace(queryMaker.Page.Index))
				{
					if (!select.Fields.Contains(queryMaker.Page.Index))
					{
						paginatedQuery = query.Page(unpaginatedQuery, new(queryMaker.Page.Skip, queryMaker.Page.Take));
					}
					else
					{
						paginatedQuery = query.Page(unpaginatedQuery, queryMaker.Page).Sort(queryMaker.Sort, select.Fields).Select(select);
					}
				}
				else
				{
					paginatedQuery = query.Page(unpaginatedQuery, queryMaker.Page);
				}
			}
			else if (queryMaker.Page is not null && !string.IsNullOrWhiteSpace(queryMaker.Page.Index))
			{
				IQueryable<T> sortedQuery = filteredQuery.Sort(queryMaker.Sort);

				paginatedQuery = query.Page(sortedQuery, queryMaker.Page).Sort(queryMaker.Sort).Select(queryMaker.Select);
				unpaginatedQuery = sortedQuery.Select(queryMaker.Select);
			}
			else
			{
				unpaginatedQuery = filteredQuery.Sort(queryMaker.Sort).Select(queryMaker.Select);
				paginatedQuery = query.Page(unpaginatedQuery, queryMaker.Page);
			}

			return new(paginatedQuery, unpaginatedQuery);
		}

		#endregion Internal Methods

		#region Private Methods

		private static IQueryable<T> Filter<T>(this IQueryable<T> query, Filter? filter = null)
		{
			if (filter is not null)
			{
				ParameterExpression parameterExpression = Expression.Parameter(typeof(T), Miscellaneous.INSTANCE);

				Expression? filterExpression = FilterMethods.FilterAction<T>(parameterExpression, filter);
				if (filterExpression is not null)
				{
					return query.Where(
						Expression.Lambda<Func<T, bool>>(
							filterExpression,
							parameterExpression));
				}
			}

			return query;
		}

		private static IQueryable<T> Sort<T>(this IQueryable<T> query, Sort? sort = null,
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
						? query.Sort(sort.Then, allowedFields, orderedQuery)
						: orderedQuery is null
							? query
							: orderedQuery;
				}

				orderedQuery = InvokeCreateOrderedQuery(query, orderedQuery,
					Expression.Parameter(typeof(T), Miscellaneous.INSTANCE), sort.Field, sort.Direction);

				if (sort.Then is not null)
				{
					return query.Sort(sort.Then, allowedFields, orderedQuery);
				}

				return orderedQuery ?? query;
			}

			return query;
		}

		private static IOrderedQueryable<T> CreateOrderedQuery<T, U>(IQueryable<T> query,
			IOrderedQueryable<T>? orderedQuery, ParameterExpression parameterExpression,
			string field, SortDirections sortDirection)
		{
			Expression<Func<T, U>> selectExpression = SelectMethods.CreateSelectExpression<T, U>(parameterExpression, field);

			return sortDirection switch
			{
				SortDirections.Ascending => orderedQuery is null
					? query.OrderBy(selectExpression)
					: orderedQuery.ThenBy(selectExpression),

				SortDirections.Descending => orderedQuery is null
					? query.OrderByDescending(selectExpression)
					: orderedQuery.ThenByDescending(selectExpression),

				_ => throw Errors.Exception(Errors.InvalidSortingDirection, sortDirection)
			};
		}

		private static IQueryable<T> Select<T>(this IQueryable<T> query, Select? select = null)
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

		private static IQueryable<T> Page<T>(this IQueryable<T> unfilteredQuery, IQueryable<T> filteredQuery, Page? page = null)
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
					MemberExpression memberExpression = parameterExpression.GetPropertyOrField<T>(page.Index);

					return unfilteredQuery.Where(Expression.Lambda<Func<T, bool>>(
						Expression.Call(
							EnumerableMethods.GetEnumerableTypedMethod(memberExpression.Type,
								Components.Filter.FilterActions.Contains.GetActionText()),

							(ConstantExpression)(typeof(SelectMethods)
								.GetMethod(nameof(SelectMethods.CreateTypedSelectConstantExpression), BindingFlags.NonPublic | BindingFlags.Static)?
								.MakeGenericMethod(new Type[] { typeof(T), memberExpression.Type })?
								.Invoke(null, new object?[] { filteredQuery, parameterExpression, page.Index })
									?? throw Errors.Exception(Errors.CreateTypedSelectConstantExpression)),

							memberExpression),
						parameterExpression));
				}
			}

			return filteredQuery;
		}

		#endregion Private Methods

		#region Helpers

		private static IOrderedQueryable<T> InvokeCreateOrderedQuery<T>(IQueryable<T> query,
			IOrderedQueryable<T>? orderedQuery, ParameterExpression parameterExpression,
			string field, SortDirections sortDirection)
		{
			MethodInfo? createOrderedQueryMethod = typeof(PerformActions)
				.GetMethod(nameof(CreateOrderedQuery), BindingFlags.NonPublic | BindingFlags.Static)?
				.MakeGenericMethod(new Type[] { typeof(T), parameterExpression.GetPropertyOrField<T>(field).Type });

			return (IOrderedQueryable<T>?)createOrderedQueryMethod?.Invoke(null,
				new object?[]{ query, orderedQuery, parameterExpression, field, sortDirection })
				?? throw Errors.Exception(Errors.CreateOrderedQuery);
		}

		#endregion Helpers
	}
}