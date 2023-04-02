namespace QueryMakerLibrary.Extensions
{
	/// <summary>
	/// Extension which adds <see cref="QueryMakerLibrary.QueryMaker" /> funtions to instances of <see cref="System.Linq.IQueryable{T}" />
	/// </summary>
	public static class QueryMakerExtension
	{
		/// <summary>
		/// Extension method which adds <see cref="QueryMakerLibrary.QueryMaker.MakeQueryResult{T}(IQueryable{T}, bool)" /> to instances of <see cref="System.Linq.IQueryable{T}" />
		/// </summary>
		/// <returns>
		/// Instance of <see cref="QueryMakerLibrary.QueryMakerResult{T}" /> with resulting query and count of unpaginated results.
		/// </returns>
		/// <param name="query">
		/// This instance of <see cref="System.Linq.IQueryable{T}" />
		/// </param>
		/// <param name="queryMaker">
		/// Instance of QueryMaker class with components of actions to perform
		/// <para>NOTE: If null then will not perform any actions and return this <see cref="System.Linq.IQueryable{T}" /> instance as is on result.</para>
		/// </param>
		/// <exception cref="System.Exception" />
		public static QueryMakerResult<T> MakeQueryResult<T>(this IQueryable<T> query, QueryMaker queryMaker)
		{
			try
			{
				return queryMaker is null ? new(query, query) : queryMaker.MakeQueryResult(query);
			}
			catch (Exception exception)
			{
				throw new Exception(exception.Message);
			}
		}

		/// <summary>
		/// Extension method which adds <see cref="QueryMakerLibrary.QueryMaker.MakeQuery{T}(IQueryable{T})" /> to instances of <see cref="System.Linq.IQueryable{T}" />
		/// </summary>
		/// <returns>
		/// An <see cref="System.Linq.IQueryable{T}" /> with performed <paramref name="queryMaker" /> actions.
		/// </returns>
		/// <param name="query">
		/// This instance of <see cref="System.Linq.IQueryable{T}" />
		/// </param>
		/// <param name="queryMaker">
		/// Instance of QueryMaker class with components of actions to perform
		/// <para>NOTE: If null then will not perform any actions and return this <see cref="System.Linq.IQueryable{T}" /> instance as is.</para>
		/// </param>
		/// <exception cref="System.Exception" />
		public static IQueryable<T> MakeQuery<T>(this IQueryable<T> query, QueryMaker queryMaker)
		{
			try
			{
				return queryMaker is null ? query : queryMaker.MakeQuery(query);
			}
			catch (Exception exception)
			{
				throw new Exception(exception.Message);
			}
		}
	}
}