namespace QueryMakerLibrary
{
	/// <summary>
	/// Class used for returning results of QueryMaker operations.
	/// </summary>
	public sealed class QueryMakerResult<T>
	{
		private QueryMakerResult(IQueryable<T>? result, int? totalCount)
		{
			Result = result;
			TotalCount = totalCount;
		}

		internal static QueryMakerResult<T> Construct(IQueryable<T>? result, int? totalCount)
		{
			return new QueryMakerResult<T>(result, totalCount);
		}

		/// <summary>
		/// <para>Resulting IQueryable after performing all operations.</para>
		/// <para>Defaults to null.</para>
		/// </summary>
		public IQueryable<T>? Result { get; private set; } = null;

		/// <summary>
		/// <para>Number of total resulting items withouth pagination.</para>
		/// <para>Defaults to null.</para>
		/// <para>NOTE: If didn't specify wanted to perform count, then will return null.</para>
		/// </summary>
		public int? TotalCount { get; private set; } = null;
	}
}