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

		internal static QueryMakerResult<T> CreateActionsResult<T>(IQueryable<T> query, QueryMaker queryMaker, bool getTotalCount)
		{
			if (query is null)
			{
				throw Errors.Exception(Errors.IQueryableNull);
			}

			IQueryable<T> nonPaginatedQuery = CreateSelectedQuery(
				CreateSortedQuery(
					CreateFilteredQuery(query,
						queryMaker.Filter),
					queryMaker.Sort),
				queryMaker.Select);

			return QueryMakerResult<T>.Construct(
				CreatePagedQuery(nonPaginatedQuery, queryMaker.Page),
				getTotalCount ? nonPaginatedQuery.Count() : null);
		}

		internal static IQueryable<T> CreateActionsQuery<T>(IQueryable<T> query, QueryMaker queryMaker)
		{
			if (query is null)
			{
				throw Errors.Exception(Errors.IQueryableNull);
			}

			return
				CreatePagedQuery(
					CreateSelectedQuery(
						CreateSortedQuery(
							CreateFilteredQuery(query,
								queryMaker.Filter),
							queryMaker.Sort),
						queryMaker.Select),
					queryMaker.Page);
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

		private static IQueryable<T> CreateSortedQuery<T>(IQueryable<T> query, Sort? sort = null)
		{
			if (sort is not null)
			{
				if (string.IsNullOrWhiteSpace(sort.Field))
				{
					throw Errors.Exception(Errors.SortFieldEmpty);
				}

				switch(sort.Direction)
				{
					case SortDirections.Ascending:
						return query.OrderBy(SelectMethods.CreateSelectExpression<T>(
							Expression.Parameter(typeof(T), Miscellaneous.INSTANCE),
							sort.Field))
						.AsQueryable<T>();
					case SortDirections.Descending:
						return query.OrderByDescending(SelectMethods.CreateSelectExpression<T>(
							Expression.Parameter(typeof(T), Miscellaneous.INSTANCE),
							sort.Field))
						.AsQueryable<T>();
					default:
						throw Errors.Exception(Errors.InvalidSortingDirection, sort.Direction);
				}
			}

			return query;
		}

		private static IQueryable<T> CreateSelectedQuery<T>(IQueryable<T> query, Select? select = null)
		{
			if (select is not null)
			{
				if (select.DistinctBy.Any())
				{
					ParameterExpression parameterExpression = Expression.Parameter(typeof(T), Miscellaneous.INSTANCE);

					return query.GroupBy(SelectMethods.CreateObjectSelectExpression<T>(parameterExpression, select.DistinctBy))
					.Select(grp => grp.AsQueryable()
						.Select(SelectMethods.CreateObjectSelectExpression<T>(parameterExpression, 
							select.Fields.Any() ? select.Fields : select.DistinctBy))
						.First());
				}

				if (select.Fields.Any())
				{
					return query.Select(SelectMethods.CreateObjectSelectExpression<T>(
						Expression.Parameter(typeof(T), Miscellaneous.INSTANCE),
						select.Fields));
				}
			}

			return query;
		}

		private static IQueryable<T> CreatePagedQuery<T>(IQueryable<T> query, Page? page = null)
		{
			if (page is not null)
			{
				if (page.Skip > 0)
				{
					query = query.Skip((int)page.Skip);
				}

				if (page.Take > 0)
				{
					return query.Take((int)page.Take);
				}
			}

			return query;
		}

		#endregion Private Methods
	}
}