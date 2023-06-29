namespace QueryMakerLibrary.Extensions
{
	/// <summary>
	/// Extension which adds <see cref="QueryMaker" /> funtions to instances of <see cref="IQueryable{T}" />
	/// </summary>
	public static class QueryMakerExtension
	{
		/// <summary>
		/// Extension method which adds <see cref="QueryMaker.MakeQueryResult{T}(IQueryable{T})" /> to instances of <see cref="IQueryable{T}" />
		/// </summary>
		/// <returns>
		/// Instance of <see cref="QueryMakerResult{T}" /> with resulting paginated and unpaginated queries.
		/// </returns>
		/// <param name="query">
		/// This instance of <see cref="IQueryable{T}" />
		/// </param>
		/// <param name="queryMaker">
		/// Instance of QueryMaker class with components of actions to perform
		/// <para>NOTE: If null then will not perform any actions and return this <see cref="IQueryable{T}" /> instance as is on result.</para>
		/// </param>
		public static QueryMakerResult<T> MakeQueryResult<T>(this IQueryable<T> query, QueryMaker queryMaker)
			=> queryMaker is null ? new(query, query) : queryMaker.MakeQueryResult(query);

		/// <summary>
		/// Extension method which adds <see cref="QueryMaker.MakeQueryResult{T}(IQueryable{T})" /> to instances of <see cref="IEnumerable{T}" />
		/// </summary>
		/// <returns>
		/// Instance of <see cref="QueryMakerResult{T}" /> with resulting paginated and unpaginated queries.
		/// </returns>
		/// <param name="query">
		/// This instance of <see cref="IEnumerable{T}" />
		/// </param>
		/// <param name="queryMaker">
		/// Instance of QueryMaker class with components of actions to perform
		/// <para>NOTE: If null then will not perform any actions and return this <see cref="IQueryable{T}" /> instance as is on result.</para>
		/// </param>
		public static QueryMakerResult<T> MakeQueryResult<T>(this IEnumerable<T> query, QueryMaker queryMaker)
			=> query.AsQueryable().MakeQueryResult(queryMaker);

		/// <summary>
		/// Extension method which adds <see cref="QueryMaker.MakeQueryResult{T}(IQueryable{T})" /> to instances of <see cref="ICollection{T}" />
		/// </summary>
		/// <returns>
		/// Instance of <see cref="QueryMakerResult{T}" /> with resulting paginated and unpaginated queries.
		/// </returns>
		/// <param name="query">
		/// This instance of <see cref="ICollection{T}" />
		/// </param>
		/// <param name="queryMaker">
		/// Instance of QueryMaker class with components of actions to perform
		/// <para>NOTE: If null then will not perform any actions and return this <see cref="IQueryable{T}" /> instance as is on result.</para>
		/// </param>
		public static QueryMakerResult<T> MakeQueryResult<T>(this ICollection<T> query, QueryMaker queryMaker)
			=> query.AsQueryable().MakeQueryResult(queryMaker);

		/// <summary>
		/// Extension method which adds <see cref="QueryMaker.MakeQueryResult{T}(IQueryable{T})" /> to instances of <see cref="IList{T}" />
		/// </summary>
		/// <returns>
		/// Instance of <see cref="QueryMakerResult{T}" /> with resulting paginated and unpaginated queries.
		/// </returns>
		/// <param name="query">
		/// This instance of <see cref="IList{T}" />
		/// </param>
		/// <param name="queryMaker">
		/// Instance of QueryMaker class with components of actions to perform
		/// <para>NOTE: If null then will not perform any actions and return this <see cref="IQueryable{T}" /> instance as is on result.</para>
		/// </param>
		public static QueryMakerResult<T> MakeQueryResult<T>(this IList<T> query, QueryMaker queryMaker)
			=> query.AsQueryable().MakeQueryResult(queryMaker);

		/// <summary>
		/// Extension method which adds <see cref="QueryMaker.MakeQuery{T}(IQueryable{T})" /> to instances of <see cref="IQueryable{T}" />
		/// </summary>
		/// <returns>
		/// An <see cref="IQueryable{T}" /> with added <paramref name="queryMaker" /> actions.
		/// </returns>
		/// <param name="query">
		/// This instance of <see cref="IQueryable{T}" />
		/// </param>
		/// <param name="queryMaker">
		/// Instance of QueryMaker class with components of actions to perform
		/// <para>NOTE: If null then will not perform any actions and return this <see cref="IQueryable{T}" /> instance as is.</para>
		/// </param>
		public static IQueryable<T> MakeQuery<T>(this IQueryable<T> query, QueryMaker queryMaker)
			=> query.MakeQueryResult(queryMaker).PaginatedQuery;

		/// <summary>
		/// Extension method which adds <see cref="QueryMaker.MakeQuery{T}(IQueryable{T})" /> to instances of <see cref="IEnumerable{T}" />
		/// </summary>
		/// <returns>
		/// An <see cref="IQueryable{T}" /> with added <paramref name="queryMaker" /> actions.
		/// </returns>
		/// <param name="query">
		/// This instance of <see cref="IEnumerable{T}" />
		/// </param>
		/// <param name="queryMaker">
		/// Instance of QueryMaker class with components of actions to perform
		/// <para>NOTE: If null then will not perform any actions and return this <see cref="IQueryable{T}" /> instance as is.</para>
		/// </param>
		public static IQueryable<T> MakeQuery<T>(this IEnumerable<T> query, QueryMaker queryMaker)
			=> query.AsQueryable().MakeQuery(queryMaker);

		/// <summary>
		/// Extension method which adds <see cref="QueryMaker.MakeQuery{T}(IQueryable{T})" /> to instances of <see cref="ICollection{T}" />
		/// </summary>
		/// <returns>
		/// An <see cref="IQueryable{T}" /> with added <paramref name="queryMaker" /> actions.
		/// </returns>
		/// <param name="query">
		/// This instance of <see cref="ICollection{T}" />
		/// </param>
		/// <param name="queryMaker">
		/// Instance of QueryMaker class with components of actions to perform
		/// <para>NOTE: If null then will not perform any actions and return this <see cref="IQueryable{T}" /> instance as is.</para>
		/// </param>
		public static IQueryable<T> MakeQuery<T>(this ICollection<T> query, QueryMaker queryMaker)
			=> query.AsQueryable().MakeQuery(queryMaker);

		/// <summary>
		/// Extension method which adds <see cref="QueryMaker.MakeQuery{T}(IQueryable{T})" /> to instances of <see cref="IList{T}" />
		/// </summary>
		/// <returns>
		/// An <see cref="IQueryable{T}" /> with added <paramref name="queryMaker" /> actions.
		/// </returns>
		/// <param name="query">
		/// This instance of <see cref="IList{T}" />
		/// </param>
		/// <param name="queryMaker">
		/// Instance of QueryMaker class with components of actions to perform
		/// <para>NOTE: If null then will not perform any actions and return this <see cref="IQueryable{T}" /> instance as is.</para>
		/// </param>
		public static IQueryable<T> MakeQuery<T>(this IList<T> query, QueryMaker queryMaker)
			=> query.AsQueryable().MakeQuery(queryMaker);
	}
}