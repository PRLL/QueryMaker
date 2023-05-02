namespace QueryMakerLibrary
{
	/// <summary>
	/// Class used for returning results of QueryMaker operations.
	/// </summary>
	public readonly struct QueryMakerResult<T>
	{
		internal QueryMakerResult(IQueryable<T> paginatedQuery, IQueryable<T> unpaginatedQuery)
		{
			PaginatedQuery = paginatedQuery;
			UnpaginatedQuery = unpaginatedQuery;
		}

		/// <summary>
		/// <para>Resulting <see cref="IQueryable{T}" /> after performing all <see cref="QueryMaker" /> operations.</para>
		/// </summary>
		public readonly IQueryable<T> PaginatedQuery { get; }

		/// <summary>
		/// <para>Resulting <see cref="IQueryable{T}" /> after performing all <see cref="QueryMaker" /> operations, except pagination.</para>
		/// <para>Can be used to call <see cref="Enumerable.Count{TSource}(IEnumerable{TSource})" /> and get total list of items.</para>
		/// </summary>
		public readonly IQueryable<T> UnpaginatedQuery { get; }
	}
}