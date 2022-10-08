using QueryMakerLibrary.Components;
using QueryMakerLibrary.Constants;

namespace QueryMakerLibrary
{
	/// <summary>
	/// <see cref="QueryMakerLibrary" />'s main class which contains the components used for making the query.
	/// </summary>
	public sealed class QueryMaker
	{
		#region Constructors

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="QueryMakerLibrary.QueryMaker" /> class.</para>
		/// <para>Set the parameter for operations which want to perform.</para>
		/// <para>All components default to null.</para>
		/// <para>NOTE: Any component left as null will not perform it's corresponding action.</para>
		/// </summary>
		/// <param name="filter">
		/// <para>Instance of <see cref="QueryMakerLibrary.Components.Filter" /> used for performing filtering action.</para>
		/// <para>Defaults to null.</para>
		/// NOTE: If left as null filtering action will not be performed
		/// </param>
		/// <param name="sort">
		/// <para>Instance of <see cref="QueryMakerLibrary.Components.Sort" /> used for performing sorting action.</para>
		/// <para>Defaults to null.</para>
		/// NOTE: If left as null sorting action will not be performed
		/// </param>
		/// <param name="select">
		/// <para>Instance of <see cref="QueryMakerLibrary.Components.Select" /> used for performing selecting action.</para>
		/// <para>Defaults to null.</para>
		/// NOTE: If left as null selecting action will not be performed
		/// </param>
		/// <param name="page">
		/// <para>Instance of <see cref="QueryMakerLibrary.Components.Page" /> used for performing paging action.</para>
		/// <para>Defaults to null.</para>
		/// NOTE: If left as null paging action will not be performed
		/// </param>
		public QueryMaker(Filter? filter = null, Sort? sort = null, Select? select = null, Page? page = null)
		{
			Filter = filter;
			Sort = sort;
			Select = select;
			Page = page;
		}

		#endregion Constructors

		#region Public Properties

		/// <summary>
		/// <para>Property of type <see cref="QueryMakerLibrary.Components.Filter" /> used for performing filtering.</para>
		/// <para>Defaults to null.</para>
		/// NOTE: If left as null filtering will not be performed
		/// </summary>
		public Filter? Filter { get; set; } = null;

		/// <summary>
		/// <para>Property of type <see cref="QueryMakerLibrary.Components.Sort" /> used for performing sorting.</para>
		/// <para>Defaults to null.</para>
		/// NOTE: If left as null sorting will not be performed
		/// </summary>
		public Sort? Sort { get; set; } = null;

		/// <summary>
		/// <para>Property of type <see cref="QueryMakerLibrary.Components.Sort" /> used for performing selecting.</para>
		/// <para>Defaults to null.</para>
		/// NOTE: If left as null selecting will not be performed
		/// </summary>
		public Select? Select { get; set; } = null;

		/// <summary>
		/// <para>Property of type <see cref="QueryMakerLibrary.Components.Sort" /> used for performing paging.</para>
		/// <para>Defaults to null.</para>
		/// NOTE: If left as null paging will not be performed
		/// </summary>
		public Page? Page { get; set; } = null;

		// useful when return filter result object
		// public bool ContinueWithErrors { get; set; } = false;
		// public List<string> ErrorsList { get; set; } = new List<string>();

		#endregion Public Properties

		#region Public Instance Methods

		/// <summary>
		/// Adds expressions to <paramref name="query" /> using properties from this instance.
		/// </summary>
		/// <returns>
		/// Instance of <see cref="QueryMakerLibrary.QueryMakerResult{T}" /> with resulting query and count of unpaginated results if specified.
		/// </returns>
		/// <param name="query">
		/// <para>Instance of <see cref="System.Linq.IQueryable{T}" /> to add expressions</para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <param name="getTotalCount">
		/// <para>Set to true to return count of unpaginated results on <see cref="QueryMakerLibrary.QueryMakerResult{T}.TotalCount" /> property</para>
		/// NOTE: Defaults to false
		/// </param>
		/// <exception cref="System.Exception" />
		public QueryMakerResult<T> MakeQueryResult<T>(IQueryable<T> query, bool getTotalCount)
		{
			try
			{
				return PerformActions.CreateActionsResult(query, this, getTotalCount);
			}
			catch (Exception exception)
			{
				throw Errors.Exception(Errors.ErrorMessage, exception.Message);
			}
		}

		/// <summary>
		/// Adds expressions to <paramref name="query" /> using properties from this instance
		/// </summary>
		/// <returns>
		/// Resulting <see cref="System.Linq.IQueryable{T}" /> with performed components actions.
		/// </returns>
		/// <param name="query">
		/// <para>Instance of <see cref="System.Linq.IQueryable{T}" /> to add expressions</para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <exception cref="System.Exception" />
		public IQueryable<T> MakeQuery<T>(IQueryable<T> query)
		{
			try
			{
				return PerformActions.CreateActionsQuery(query, this);
			}
			catch (Exception exception)
			{
				throw Errors.Exception(Errors.ErrorMessage, exception.Message);
			}
		}

		/// <summary>
		/// Performs filtering on <paramref name="query" /> using <see cref="QueryMakerLibrary.QueryMaker.Filter" /> property from this instance.
		/// </summary>
		/// <returns>
		/// An <see cref="System.Linq.IQueryable{T}" /> with added filter actions.
		/// </returns>
		/// <param name="query">
		/// <para>Instance of <see cref="System.Linq.IQueryable{T}" /> to add filtering</para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <exception cref="System.Exception" />
		public IQueryable<T> Filtering<T>(IQueryable<T> query)
		{
			return PerformActions.CreateActionsQuery(query,
				new QueryMaker(filter: this.Filter));
		}

		/// <summary>
		/// Performs sorting on <paramref name="query" /> using <see cref="QueryMakerLibrary.QueryMaker.Sort" /> property from this instance.
		/// </summary>
		/// <returns>
		/// An <see cref="System.Linq.IQueryable{T}" /> with added sorting actions.
		/// </returns>
		/// <param name="query">
		/// <para>Instance of <see cref="System.Linq.IQueryable{T}" /> to add sorting</para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <exception cref="System.Exception" />
		public IQueryable<T> Sorting<T>(IQueryable<T> query)
		{
			return PerformActions.CreateActionsQuery(query,
				new QueryMaker(sort: this.Sort));
		}

		/// <summary>
		/// Performs selecting on <paramref name="query" /> using <see cref="QueryMakerLibrary.QueryMaker.Select" />  property from this instance.
		/// </summary>
		/// <returns>
		/// An <see cref="System.Linq.IQueryable{T}" /> with added select actions.
		/// </returns>
		/// <param name="query">
		/// <para>Instance of <see cref="System.Linq.IQueryable{T}" /> to add selecting</para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <exception cref="System.Exception" />
		public IQueryable<T> Selecting<T>(IQueryable<T> query)
		{
			return PerformActions.CreateActionsQuery(query,
				new QueryMaker(select: this.Select));
		}

		/// <summary>
		/// Performs paging actions on <paramref name="query" /> using <see cref="QueryMakerLibrary.QueryMaker.Page" /> property from this instance.
		/// </summary>
		/// <returns>
		/// An <see cref="System.Linq.IQueryable{T}" /> with added paging actions.
		/// </returns>
		/// <param name="query">
		/// <para>Instance of <see cref="System.Linq.IQueryable{T}" /> to add paging</para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <exception cref="System.Exception" />
		public IQueryable<T> Paging<T>(IQueryable<T> query)
		{
			return PerformActions.CreateActionsQuery(query,
				new QueryMaker(page: this.Page));
		}

		#endregion Public Instance Methods

		#region Public Static Methods

		/// <summary>
		/// Adds expressions to <paramref name="query" /> using properties from <paramref name="queryMaker" /> instance.
		/// </summary>
		/// <returns>
		/// Instance of <see cref="QueryMakerLibrary.QueryMakerResult{T}" /> with resulting query and count of unpaginated results if specified.
		/// </returns>
		/// <param name="query">
		/// <para>Instance of <see cref="System.Linq.IQueryable{T}" /> to add expressions</para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <param name="queryMaker">
		/// Instance of QueryMaker class with components of actions to perform
		/// <para>NOTE: If set null then will perform no actions and return  and return <paramref name="query" /> as is.</para>
		/// </param>
		/// <param name="getTotalCount">
		/// <para>Set to true to return count of unpaginated results on <see cref="QueryMakerLibrary.QueryMakerResult{T}.TotalCount" /> property</para>
		/// NOTE: Defaults to false
		/// </param>
		/// <exception cref="System.Exception" />
		public static QueryMakerResult<T> MakeQueryResult<T>(IQueryable<T> query, QueryMaker queryMaker, bool getTotalCount)
		{
			try
			{
				return queryMaker.MakeQueryResult(query, getTotalCount);
			}
			catch (Exception exception)
			{
				throw Errors.Exception(Errors.ErrorMessage, exception.Message);
			}
		}

		/// <summary>
		/// Adds expressions to <paramref name="query" /> using properties from <paramref name="queryMaker" /> instance
		/// </summary>
		/// <returns>
		/// An <see cref="System.Linq.IQueryable{T}" /> with performed <paramref name="queryMaker" /> components actions.
		/// </returns>
		/// <param name="query">
		/// <para>The current instance of <see cref="System.Linq.IQueryable{T}" /></para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <param name="queryMaker">
		/// Instance of QueryMaker class with components of actions to perform
		/// <para>NOTE: If set null then will perform no actions and return  and return <paramref name="query" /> as is.</para>
		/// </param>
		/// <exception cref="System.Exception" />
		public static IQueryable<T> MakeQuery<T>(IQueryable<T> query, QueryMaker queryMaker)
		{
			try
			{
				return queryMaker.MakeQuery(query);
			}
			catch (Exception exception)
			{
				throw Errors.Exception(Errors.ErrorMessage, exception.Message);
			}
		}

		/// <summary>
		/// Performs filtering on <paramref name="query" /> using <paramref name="filter" /> instance.
		/// </summary>
		/// <returns>
		/// An <see cref="System.Linq.IQueryable{T}" /> with added filter actions.
		/// </returns>
		/// <param name="query">
		/// <para>The current instance of <see cref="System.Linq.IQueryable{T}" /></para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <param name="filter">
		/// The instance of Filter
		/// <para>NOTE: If set null then will not perform filtering and return <paramref name="query" /> as is.</para>
		/// </param>
		/// <exception cref="System.Exception" />
		public static IQueryable<T> Filtering<T>(IQueryable<T> query, Filter filter)
		{
			return new QueryMaker(filter: filter).MakeQuery(query);
		}

		/// <summary>
		/// Performs sorting on <paramref name="query" /> using <paramref name="sort" /> instance.
		/// </summary>
		/// <returns>
		/// An <see cref="System.Linq.IQueryable{T}" /> with added sorting actions.
		/// </returns>
		/// <param name="query">
		/// <para>The current instance of <see cref="System.Linq.IQueryable{T}" /></para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <param name="sort">
		/// The instance of Sort
		/// <para>NOTE: If set null then will not perform sorting and return <paramref name="query" /> as is.</para>
		/// </param>
		/// <exception cref="System.Exception" />
		public static IQueryable<T> Sorting<T>(IQueryable<T> query, Sort sort)
		{
			return new QueryMaker(sort: sort).MakeQuery(query);
		}

		/// <summary>
		/// Performs selecting on <paramref name="query" /> using <paramref name="select" /> instance.
		/// </summary>
		/// <returns>
		/// An <see cref="System.Linq.IQueryable{T}" /> with added select actions.
		/// </returns>
		/// <param name="query">
		/// <para>The current instance of <see cref="System.Linq.IQueryable{T}" /></para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <param name="select">
		/// The instance of Select
		/// <para>NOTE: If set null then will not perform selecting and return <paramref name="query" /> as is.</para>
		/// </param>
		/// <exception cref="System.Exception" />
		public static IQueryable<T> Selecting<T>(IQueryable<T> query, Select select)
		{
			return new QueryMaker(select: select).MakeQuery(query);
		}

		/// <summary>
		/// Performs paging actions on <paramref name="query" /> using <paramref name="page" /> instance.
		/// </summary>
		/// <returns>
		/// An <see cref="System.Linq.IQueryable{T}" /> with added paging actions.
		/// </returns>
		/// <param name="query">
		/// <para>Instance of <see cref="System.Linq.IQueryable{T}" /> to add paging</para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <param name="page">
		/// The instance of Page
		/// <para>NOTE: If set null then will not perform paging and return <paramref name="query" /> as is.</para>
		/// </param>
		/// <exception cref="System.Exception" />
		public static IEnumerable<T> Paging<T>(IQueryable<T> query, Page page)
		{
			return new QueryMaker(page: page).MakeQuery(query);
		}

		#endregion Public Static Methods
	}
}