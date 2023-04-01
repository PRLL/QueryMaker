namespace QueryMakerLibrary
{
	/// <summary>
	/// Class used for returning results of QueryMaker operations.
	/// </summary>
	public struct QueryMakerResult<T>
	{
		private int _totalCount = 0;

		internal QueryMakerResult(IQueryable<T>? result, IQueryable<T>? totalCount)
		{
			Result = result;
			TotalCountQuery = totalCount;
		}

		private IQueryable<T>? TotalCountQuery { get; init; } = null;
		private bool PerformedCount { get; set; } = false;

		/// <summary>
		/// <para>Resulting IQueryable after performing all provided <see cref="QueryMaker" /> operations.</para>
		/// <para>Defaults to null.</para>
		/// </summary>
		public IQueryable<T>? Result { get; private init; } = null;


		/// <summary>
		/// <para>Number of total resulting items without pagination.</para>
		/// <para>Defaults to 0.</para>
		/// </summary>
		public int TotalCount
		{
			get
			{
				if (!PerformedCount && TotalCountQuery is not null)
				{
					try
					{
						_totalCount = TotalCountQuery.Count();
					}
					catch
					{
						_totalCount = 0;
					}

					PerformedCount = true;
				}

				return _totalCount;
			}
		}
	}
}