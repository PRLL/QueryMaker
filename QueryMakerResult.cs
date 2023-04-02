using QueryMakerLibrary.Constants;

namespace QueryMakerLibrary
{
	/// <summary>
	/// Class used for returning results of QueryMaker operations.
	/// </summary>
	public struct QueryMakerResult<T>
	{
		private IEnumerable<T> _items = Enumerable.Empty<T>();
		private int _totalCount = 0;

		internal QueryMakerResult(IQueryable<T> items, IQueryable<T> totalCount)
		{
			ItemsQuery = items;
			TotalCountQuery = totalCount;
		}

		private readonly IQueryable<T> ItemsQuery { get; }
		private readonly IQueryable<T> TotalCountQuery { get; }

		private bool PerformedToList { get; set; } = false;
		private bool PerformedCount { get; set; } = false;

		/// <summary>
		/// <para>Resulting Items after performing all provided <see cref="QueryMaker" /> operations.</para>
		/// <para>Defaults to empty list.</para>
		/// </summary>
		public IEnumerable<T> Items
		{
			get
			{
				if (!PerformedToList)
				{
					try
					{
						_items = ItemsQuery.AsEnumerable();
					}
					catch
					{
						throw Errors.Exception(Errors.GetResulItems);
					}

					PerformedToList = true;
				}

				return _items;
			}
		}

		/// <summary>
		/// <para>Number of total resulting items without pagination.</para>
		/// <para>Defaults to 0.</para>
		/// </summary>
		public int TotalCount
		{
			get
			{
				if (!PerformedCount)
				{
					try
					{
						_totalCount = TotalCountQuery.Count();
					}
					catch
					{
						throw Errors.Exception(Errors.GetResulTotalCount);
					}

					PerformedCount = true;
				}

				return _totalCount;
			}
		}

		/// <summary>
		/// <para>Provides resulting IQueryable after performing all <see cref="QueryMaker" /> operations.</para>
		/// </summary>
		public IQueryable<T> GetItemsQuery()
		{
			return ItemsQuery;
		}

		/// <summary>
		/// <para>Provides resulting IQueryable without pagination to perform <see cref="System.Linq.Enumerable.Count{TSource}(IEnumerable{TSource})" /> on.</para>
		/// </summary>
		public IQueryable<T> GetTotalCount()
		{
			return TotalCountQuery;
		}
	}
}