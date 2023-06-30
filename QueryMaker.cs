using QueryMakerLibrary.Components;
using QueryMakerLibrary.Constants;
using static QueryMakerLibrary.Components.Filter;
using static QueryMakerLibrary.Components.Sort;

namespace QueryMakerLibrary
{
	/// <summary>
	/// <see cref="QueryMakerLibrary" />'s main class which contains the components used for making the query.
	/// </summary>
	public sealed class QueryMaker
	{
		#region Constructors

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="QueryMaker" /> class.</para>
		/// <para>Set the parameter for operations which want to perform.</para>
		/// <para>All components default to null.</para>
		/// <para>NOTE: Any component left as null will not perform it's corresponding action.</para>
		/// </summary>
		/// <param name="filter">
		/// <para>Instance of <see cref="Components.Filter" /> used for performing filtering action.</para>
		/// <para>Defaults to null.</para>
		/// NOTE: If left as null filtering action will not be performed
		/// </param>
		/// <param name="sort">
		/// <para>Instance of <see cref="Components.Sort" /> used for performing sorting action.</para>
		/// <para>Defaults to null.</para>
		/// NOTE: If left as null sorting action will not be performed
		/// </param>
		/// <param name="page">
		/// <para>Instance of <see cref="Components.Page" /> used for performing paging action.</para>
		/// <para>Defaults to null.</para>
		/// NOTE: If left as null paging action will not be performed
		/// </param>
		/// <param name="select">
		/// <para>Instance of <see cref="Components.Select" /> used for performing selecting action.</para>
		/// <para>Defaults to null.</para>
		/// NOTE: If left as null selecting action will not be performed
		/// </param>
		public QueryMaker(Filter? filter = null, Sort? sort = null, Page? page = null, Select? select = null)
		{
			Filter = filter;
			Sort = sort;
			Page = page;
			Select = select;
		}

		#endregion Constructors

		#region Private Fields

		private Sort? _sort = null;

		#endregion Private Fields

		#region Private Properties

		private Sort? DeepestSort { get; set; } = null;

		#endregion Private Properties

		#region Public Properties

		/// <summary>
		/// <para>Property of type <see cref="Components.Filter" /> used for performing filtering.</para>
		/// <para>Defaults to null.</para>
		/// NOTE: If left as null filtering will not be performed
		/// </summary>
		public Filter? Filter { get; set; } = null;

		/// <summary>
		/// <para>Property of type <see cref="Components.Sort" /> used for performing sorting.</para>
		/// <para>Defaults to null.</para>
		/// NOTE: If left as null sorting will not be performed
		/// </summary>
		public Sort? Sort
		{
			get => _sort;
			set
			{
				_sort = value;
				DeepestSort = value;
			}
		}

		/// <summary>
		/// <para>Property of type <see cref="Components.Sort" /> used for performing paging.</para>
		/// <para>Defaults to null.</para>
		/// NOTE: If left as null paging will not be performed
		/// </summary>
		public Page? Page { get; set; } = null;

		/// <summary>
		/// <para>Property of type <see cref="Components.Sort" /> used for performing selecting.</para>
		/// <para>Defaults to null.</para>
		/// NOTE: If left as null selecting will not be performed
		/// </summary>
		public Select? Select { get; set; } = null;

		#endregion Public Properties

		#region Public Instance Methods

		/// <summary>
		/// Adds expressions to <paramref name="query" /> using properties from this instance.
		/// </summary>
		/// <returns>
		/// Instance of <see cref="QueryMakerResult{T}" /> with resulting paginated and unpaginated queries.
		/// </returns>
		/// <param name="query">
		/// <para>Instance of <see cref="IQueryable{T}" /> to add expressions</para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <exception cref="Exception" />
		public QueryMakerResult<T> MakeQueryResult<T>(IQueryable<T> query)
		{
			try
			{
				return PerformActions.CreateActionsResult(query, this);
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
		/// Resulting <see cref="IQueryable{T}" /> with performed components actions.
		/// </returns>
		/// <param name="query">
		/// <para>Instance of <see cref="IQueryable{T}" /> to add expressions</para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <exception cref="Exception" />
		public IQueryable<T> MakeQuery<T>(IQueryable<T> query)
			=> MakeQueryResult(query).PaginatedQuery;

		/// <summary>
		/// <para>Shortcut method which only performs filtering on <paramref name="query" /> using <see cref="Filter" /> property from this instance.</para>
		/// <para>For perming all operations, use <see cref="MakeQuery{T}(IQueryable{T})" /> or <see cref="MakeQueryResult{T}(IQueryable{T})" />.</para>
		/// </summary>
		/// <returns>
		/// An <see cref="IQueryable{T}" /> with added filter actions.
		/// </returns>
		/// <param name="query">
		/// <para>Instance of <see cref="IQueryable{T}" /> to add filtering</para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <exception cref="Exception" />
		public IQueryable<T> Filtering<T>(IQueryable<T> query)
			=> new QueryMaker(filter: Filter).MakeQuery(query);

		/// <summary>
		/// <para>Shortcut method which only performs sorting on <paramref name="query" /> using <see cref="Sort" /> property from this instance.</para>
		/// <para>For perming all operations, use <see cref="MakeQuery{T}(IQueryable{T})" /> or <see cref="MakeQueryResult{T}(IQueryable{T})" />.</para>
		/// </summary>
		/// <returns>
		/// An <see cref="IQueryable{T}" /> with added sorting actions.
		/// </returns>
		/// <param name="query">
		/// <para>Instance of <see cref="IQueryable{T}" /> to add sorting</para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <exception cref="Exception" />
		public IQueryable<T> Sorting<T>(IQueryable<T> query)
			=> new QueryMaker(sort: Sort).MakeQuery(query);

		/// <summary>
		/// <para>Shortcut method which only performs paging on <paramref name="query" /> using <see cref="Page" /> property from this instance.</para>
		/// <para>For perming all operations, use <see cref="MakeQuery{T}(IQueryable{T})" /> or <see cref="MakeQueryResult{T}(IQueryable{T})" />.</para>
		/// </summary>
		/// <returns>
		/// An <see cref="IQueryable{T}" /> with added paging actions.
		/// </returns>
		/// <param name="query">
		/// <para>Instance of <see cref="IQueryable{T}" /> to add paging</para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <exception cref="Exception" />
		public IQueryable<T> Paging<T>(IQueryable<T> query)
			=> new QueryMaker(page: Page).MakeQuery(query);

		/// <summary>
		/// <para>Shortcut method which only performs selecting on <paramref name="query" /> using <see cref="Select" />  property from this instance.</para>
		/// <para>For perming all operations, use <see cref="MakeQuery{T}(IQueryable{T})" /> or <see cref="MakeQueryResult{T}(IQueryable{T})" />.</para>
		/// </summary>
		/// <returns>
		/// An <see cref="IQueryable{T}" /> with added select actions.
		/// </returns>
		/// <param name="query">
		/// <para>Instance of <see cref="IQueryable{T}" /> to add selecting</para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <exception cref="Exception" />
		public IQueryable<T> Selecting<T>(IQueryable<T> query)
			=> new QueryMaker(select: Select).MakeQuery(query);

		/// <summary>
		/// Add <see cref="Filter" /> component to this instance
		/// </summary>
		/// <param name="filter">
		/// <para>Instance of <see cref="Components.Filter" /> to add.</para>
		/// </param>
		/// <returns>
		/// This instance of <see cref="QueryMaker" /> with added <see cref="Filter" /> component.
		/// </returns>
		public QueryMaker FilterBy(Filter filter)
		{
			Filter = filter;
			return this;
		}

		/// <summary>
		/// Add <see cref="Filter" /> component to this instance.
		/// </summary>
		/// <param name="field">
		/// <para>Field to perform filtering on.</para>
		/// <para>If performing filtering on a primitive type enumerable (like a list of strings) then can leave empty.</para>
		/// </param>
		/// <param name="action">
		/// <para>Action to perform.</para>
		/// <para>Refer to <see cref="FilterActions" /> for possible values.</para>
		/// <para>NOTE: If not set to a valid <see cref="FilterActions" /> value, will throw exception when performing filtering.</para>
		/// </param>
		/// <param name="value">
		/// <para>Value to filter by.</para>
		/// </param>
		/// <param name="ignoreCase">
		/// <para>Set true to ignore case sensitivity on evaluation performed on filtering.</para>
		/// <para>Defaults to true.</para>
		/// </param>
		/// <param name="negate">
		/// <para>Set true to negate evaluation performed on filtering.</para>
		/// <para>Defaults to false.</para>
		/// </param>
		/// <returns>
		/// This instance of <see cref="QueryMaker" /> with added <see cref="Filter" /> component.
		/// </returns>
		public QueryMaker FilterBy(string field, FilterActions action, object? value,
			bool ignoreCase = true, bool negate = false)
		{
			return FilterBy(new Filter(field, action, value, ignoreCase, negate));
		}
		
		/// <summary>
		/// Add <see cref="Filter" /> component to this instance with required parameters for performing filtering using multiple fields.
		/// </summary>
		/// <param name="fields">
		/// <para>Array of fields to perform filtering on.</para>
		/// <para>If performing filtering on a primitive type enumerable (like a list of strings) then can leave empty.</para>
		/// </param>
		/// <param name="action">
		/// <para>Action to perform.</para>
		/// <para>Refer to <see cref="FilterActions" /> for possible values.</para>
		/// <para>NOTE: If not set to a valid <see cref="FilterActions" /> value, will throw exception when performing filtering.</para>
		/// </param>
		/// <param name="value">
		/// <para>Value to filter by.</para>
		/// </param>
		/// <param name="fieldsOperation">
		/// <para>Operation to perform between <paramref name="fields" />.</para>
		/// <para>Refer to <see cref="FilterOperations" /> for possible values</para>
		/// <para>Defaults to <see cref="FilterOperations.OrElse" />.</para>
		/// <para>NOTE: If not set to a valid <see cref="FilterOperations" /> value, then will throw exception when performing filtering.</para>
		/// </param>
		/// <param name="ignoreCase">
		/// <para>Set true to ignore case sensitivity on evaluation performed on filtering.</para>
		/// <para>Defaults to true.</para>
		/// </param>
		/// <param name="negate">
		/// <para>Set true to negate evaluation performed on filtering.</para>
		/// <para>Defaults to false.</para>
		/// </param>
		/// <returns>
		/// This instance of <see cref="QueryMaker" /> with added <see cref="Filter" /> component.
		/// </returns>
		public QueryMaker FilterBy(string[] fields, FilterActions action, object? value,
			FilterOperations fieldsOperation = FilterOperations.OrElse, bool ignoreCase = true, bool negate = false)
		{
			return FilterBy(new Filter(fields, action, value, fieldsOperation, ignoreCase, negate));
		}

		/// <summary>
		/// <para>Add <see cref="Filter" /> component to this instance as a joiner with subfilters to join.</para>
		/// <para><see cref="Filter.IsJoiner" /> will be set to true by default</para>
		/// </summary>
		/// <param name="subFiltersOperation">
		/// <para>Operation to perform between passed <paramref name="subFilters" />.</para>
		/// <para>Refer to <see cref="FilterOperations" /> for possible values</para>
		/// <para>NOTE: If not set to a valid <see cref="FilterOperations" /> value and <see cref="Filter.SubFilters" /> property has items, then will throw exception when joining subfilters.</para>
		/// </param>
		/// <param name="subFilters">
		/// <para>Array of <see cref="Components.Filter" /> instances to be joined</para>
		/// NOTE: If this property has items and <see cref="Filter.SubFiltersOperation" /> property is not set to a valid value, then will throw exception
		/// </param>
		/// <returns>
		/// This instance of <see cref="QueryMaker" /> added <paramref name="subFilters" /> on <see cref="Filter" /> component.
		/// </returns>
		public QueryMaker FilterBy(FilterOperations subFiltersOperation, params Filter[] subFilters)
		{
			return FilterBy(new Filter(subFiltersOperation, subFilters));
		}

		/// <summary>
		/// <para>Add subfilters to be performed after <see cref="Filter" /> with AndAlso evaluation.</para>
		/// </summary>
		/// <param name="subFilters">
		/// <para>Array of <see cref="Components.Filter" /> instances to be joined</para>
		/// </param>
		/// <returns>
		/// This instance of <see cref="QueryMaker" /> with added <paramref name="subFilters" /> on <see cref="Filter" /> component.
		/// </returns>
		public QueryMaker AndAlsoFilterBy(params Filter[] subFilters)
		{
			if (Filter is not null)
			{
				Filter.SubFiltersOperation = FilterOperations.AndAlso;
				Filter.SubFilters = subFilters;
			}
			return this;
		}

		/// <summary>
		/// <para>Add subfilters to be performed after <see cref="Filter" /> with AndAlso evaluation.</para>
		/// </summary>
		/// <param name="field">
		/// <para>Field to perform filtering on.</para>
		/// <para>If performing filtering on a primitive type enumerable (like a list of strings) then can leave empty.</para>
		/// </param>
		/// <param name="action">
		/// <para>Action to perform.</para>
		/// <para>Refer to <see cref="FilterActions" /> for possible values.</para>
		/// <para>NOTE: If not set to a valid <see cref="FilterActions" /> value, will throw exception when performing filtering.</para>
		/// </param>
		/// <param name="value">
		/// <para>Value to filter by.</para>
		/// </param>
		/// <param name="ignoreCase">
		/// <para>Set true to ignore case sensitivity on evaluation performed on filtering.</para>
		/// <para>Defaults to true.</para>
		/// </param>
		/// <param name="negate">
		/// <para>Set true to negate evaluation performed on filtering.</para>
		/// <para>Defaults to false.</para>
		/// </param>
		/// <returns>
		/// This instance of <see cref="QueryMaker" /> with added <see cref="Filter.SubFilters" /> on <see cref="Filter" /> component.
		/// </returns>
		public QueryMaker AndAlsoFilterBy(string field, FilterActions action, object? value,
			bool ignoreCase = true, bool negate = false)
		{
			return AndAlsoFilterBy(new Filter(field, action, value, ignoreCase, negate));
		}

		/// <summary>
		/// <para>Add subfilters to be performed after <see cref="Filter" /> with AndAlso evaluation.</para>
		/// </summary>
		/// <param name="fields">
		/// <para>Array of fields to perform filtering on.</para>
		/// <para>If performing filtering on a primitive type enumerable (like a list of strings) then can leave empty.</para>
		/// </param>
		/// <param name="action">
		/// <para>Action to perform.</para>
		/// <para>Refer to <see cref="FilterActions" /> for possible values.</para>
		/// <para>NOTE: If not set to a valid <see cref="FilterActions" /> value, will throw exception when performing filtering.</para>
		/// </param>
		/// <param name="value">
		/// <para>Value to filter by.</para>
		/// </param>
		/// <param name="fieldsOperation">
		/// <para>Operation to perform between <paramref name="fields" />.</para>
		/// <para>Refer to <see cref="FilterOperations" /> for possible values</para>
		/// <para>Defaults to <see cref="FilterOperations.OrElse" />.</para>
		/// <para>NOTE: If not set to a valid <see cref="FilterOperations" /> value, then will throw exception when performing filtering.</para>
		/// </param>
		/// <param name="ignoreCase">
		/// <para>Set true to ignore case sensitivity on evaluation performed on filtering.</para>
		/// <para>Defaults to true.</para>
		/// </param>
		/// <param name="negate">
		/// <para>Set true to negate evaluation performed on filtering.</para>
		/// <para>Defaults to false.</para>
		/// </param>
		/// <returns>
		/// This instance of <see cref="QueryMaker" /> with added <see cref="Filter.SubFilters" /> on <see cref="Filter" /> component.
		/// </returns>
		public QueryMaker AndAlsoFilterBy(string[] fields, FilterActions action, object? value,
			FilterOperations fieldsOperation = FilterOperations.OrElse, bool ignoreCase = true, bool negate = false)
		{
			return AndAlsoFilterBy(new Filter(fields, action, value, fieldsOperation, ignoreCase, negate));
		}

		/// <summary>
		/// <para>Add subfilters to be performed after <see cref="Filter" /> with OrElse evaluation.</para>
		/// </summary>
		/// <param name="subFilters">
		/// <para>Array of <see cref="Components.Filter" /> instances to be joined</para>
		/// </param>
		/// <returns>
		/// This instance of <see cref="QueryMaker" /> with added <paramref name="subFilters" /> on <see cref="Filter" /> component.
		/// </returns>
		public QueryMaker OrElseFilterBy(params Filter[] subFilters)
		{
			if (Filter is not null)
			{
				Filter.SubFiltersOperation = FilterOperations.OrElse;
				Filter.SubFilters = subFilters;
			}
			return this;
		}

		/// <summary>
		/// <para>Add subfilters to be performed after <see cref="Filter" /> with OrElse evaluation.</para>
		/// </summary>
		/// <param name="field">
		/// <para>Field to perform filtering on.</para>
		/// <para>If performing filtering on a primitive type enumerable (like a list of strings) then can leave empty.</para>
		/// </param>
		/// <param name="action">
		/// <para>Action to perform.</para>
		/// <para>Refer to <see cref="FilterActions" /> for possible values.</para>
		/// <para>NOTE: If not set to a valid <see cref="FilterActions" /> value, will throw exception when performing filtering.</para>
		/// </param>
		/// <param name="value">
		/// <para>Value to filter by.</para>
		/// </param>
		/// <param name="ignoreCase">
		/// <para>Set true to ignore case sensitivity on evaluation performed on filtering.</para>
		/// <para>Defaults to true.</para>
		/// </param>
		/// <param name="negate">
		/// <para>Set true to negate evaluation performed on filtering.</para>
		/// <para>Defaults to false.</para>
		/// </param>
		/// <returns>
		/// This instance of <see cref="QueryMaker" /> with added <see cref="Filter.SubFilters" /> on <see cref="Filter" /> component.
		/// </returns>
		public QueryMaker OrElseFilterBy(string field, FilterActions action, object? value,
			bool ignoreCase = true, bool negate = false)
		{
			return OrElseFilterBy(new Filter(field, action, value, ignoreCase, negate));
		}

		/// <summary>
		/// <para>Add subfilters to be performed after <see cref="Filter" /> with OrElse evaluation.</para>
		/// </summary>
		/// <param name="fields">
		/// <para>Array of fields to perform filtering on.</para>
		/// <para>If performing filtering on a primitive type enumerable (like a list of strings) then can leave empty.</para>
		/// </param>
		/// <param name="action">
		/// <para>Action to perform.</para>
		/// <para>Refer to <see cref="FilterActions" /> for possible values.</para>
		/// <para>NOTE: If not set to a valid <see cref="FilterActions" /> value, will throw exception when performing filtering.</para>
		/// </param>
		/// <param name="value">
		/// <para>Value to filter by.</para>
		/// </param>
		/// <param name="fieldsOperation">
		/// <para>Operation to perform between <paramref name="fields" />.</para>
		/// <para>Refer to <see cref="FilterOperations" /> for possible values</para>
		/// <para>Defaults to <see cref="FilterOperations.OrElse" />.</para>
		/// <para>NOTE: If not set to a valid <see cref="FilterOperations" /> value, then will throw exception when performing filtering.</para>
		/// </param>
		/// <param name="ignoreCase">
		/// <para>Set true to ignore case sensitivity on evaluation performed on filtering.</para>
		/// <para>Defaults to true.</para>
		/// </param>
		/// <param name="negate">
		/// <para>Set true to negate evaluation performed on filtering.</para>
		/// <para>Defaults to false.</para>
		/// </param>
		/// <returns>
		/// This instance of <see cref="QueryMaker" /> with added <see cref="Filter.SubFilters" /> on <see cref="Filter" /> component.
		/// </returns>
		public QueryMaker OrElseFilterBy(string[] fields, FilterActions action, object? value,
			FilterOperations fieldsOperation = FilterOperations.OrElse, bool ignoreCase = true, bool negate = false)
		{
			return OrElseFilterBy(new Filter(fields, action, value, fieldsOperation, ignoreCase, negate));
		}

		/// <summary>
		/// Add <see cref="Sort" /> component to this instance
		/// </summary>
		/// <param name="sort">
		/// <para>Instance of <see cref="Components.Sort" /> to add.</para>
		/// </param>
		/// <returns>
		/// This instance of <see cref="QueryMaker" /> with added <see cref="Sort" /> component.
		/// </returns>
		public QueryMaker SortBy(Sort sort)
		{
			Sort = sort;
			return this;
		}

		/// <summary>
		/// Add <see cref="Sort" /> component to this instance
		/// </summary>
		/// <param name="field">
		/// <para>Field to sort by.</para>
		/// <para>Defaults to empty string value.</para>
		/// <para>NOTE: If left empty or null, then will throw exception.</para>
		/// </param>
		/// <param name="direction">
		/// <para>Sorting direction.</para>
		/// <para>Refer to <see cref="SortDirections" /> for possible values</para>
		/// <para>Defaults to <see cref="SortDirections.Ascending" />.</para>
		/// <para>NOTE: If not a valid direction, then will throw exception.</para>
		/// </param>
		/// <param name="then">
		/// <para>Sorting performed after this one.</para>
		/// <para>Defaults to null.</para>
		/// </param>
		/// <returns>
		/// This instance of <see cref="QueryMaker" /> with added <see cref="Sort" /> component.
		/// </returns>
		public QueryMaker SortBy(string field, SortDirections direction = SortDirections.Ascending, Sort? then = null)
		{
			return SortBy(new (field, direction, then));
		}

		/// <summary>
		/// <para>Add sorting to be performed after the previous sort set.</para>
		/// </summary>
		/// <param name="sort">
		/// <para>Instance of <see cref="Sort" /> to perform after the previous sorting set.</para>
		/// </param>
		/// <returns>
		/// This instance of <see cref="QueryMaker" /> with added <paramref name="sort" /> on <see cref="Sort.Then" /> property of <see cref="Sort" /> component.
		/// </returns>
		public QueryMaker AndThenSortBy(Sort sort)
		{
			if (DeepestSort is null)
			{
				return SortBy(sort);
			}
			else
			{
				DeepestSort = DeepestSort.AndThen(sort).Then
					?? throw new NullReferenceException(Errors.AddingSort);
			}
			return this;
		}

		/// <summary>
		/// <para>Add sorting to be performed after previous sorting set.</para>
		/// </summary>
		/// <param name="field">
		/// <para>Field to sort by.</para>
		/// <para>Defaults to empty string value.</para>
		/// <para>NOTE: If left empty or null, then will throw exception.</para>
		/// </param>
		/// <param name="direction">
		/// <para>Sorting direction.</para>
		/// <para>Refer to <see cref="SortDirections" /> for possible values</para>
		/// <para>Defaults to <see cref="SortDirections.Ascending" />.</para>
		/// <para>NOTE: If not a valid direction, then will throw exception.</para>
		/// </param>
		/// <param name="then">
		/// <para>Sorting performed after this one.</para>
		/// <para>Defaults to null.</para>
		/// </param>
		/// <returns>
		/// This instance of <see cref="QueryMaker" /> with added on <see cref="Sort.Then" /> property of <see cref="Sort" /> component.
		/// </returns>
		public QueryMaker AndThenSortBy(string field, SortDirections direction = SortDirections.Ascending, Sort? then = null)
		{
			if (DeepestSort is null)
			{
				return SortBy(field, direction, then);
			}
			else
			{
				DeepestSort = DeepestSort.AndThen(field, direction, then).Then
					?? throw new NullReferenceException(Errors.AddingSort);
			}
			return this;
		}

		/// <summary>
		/// Add <see cref="Page" /> component to this instance
		/// </summary>
		/// <param name="page">
		/// <para>Instance of <see cref="Components.Page" /> to add.</para>
		/// </param>
		/// <returns>
		/// This instance of <see cref="QueryMaker" /> with added <see cref="Page" /> component.
		/// </returns>
		public QueryMaker WithPage(Page page)
		{
			Page = page;
			return this;
		}

		/// <summary>
		/// Add <see cref="Page" /> component to this instance
		/// </summary>
		/// <param name="skip">
		/// <para>Quantity of elements to skip on paging action.</para>
		/// <para>Defaults to 0.</para>
		/// <para>NOTE: If left as 0 then will not perform skip.</para>
		/// </param>
		/// <param name="take">
		/// <para>Quantity of elements to get on paging action.</para>
		/// <para>Defaults to 0.</para>
		/// <para>NOTE: If left as 0 then will not perform take.</para>
		/// </param>
		/// <param name="index">
		/// <para>Field used as index for faster pagination.</para>
		/// <para>Defaults to empty string.</para>
		/// <para>NOTE: If left empty, then regular pagination will be performed without using an index.</para>
		/// </param>
		/// <returns>
		/// This instance of <see cref="QueryMaker" /> with added <see cref="Page" /> component.
		/// </returns>
		public QueryMaker WithPage(int skip = 0, int take = 0, string index = "")
		{
			return WithPage(new (skip, take, index));
		}

		/// <summary>
		/// Add <see cref="Select" /> component to this instance
		/// </summary>
		/// <param name="select">
		/// <para>Instance of <see cref="Components.Select" /> to add.</para>
		/// </param>
		public QueryMaker WithSelect(Select select)
		{
			Select = select;
			return this;
		}

		/// <summary>
		/// Add <see cref="Select" /> component to this instance
		/// </summary>
		/// <param name="fields">
		/// <para>Fields to select on query.</para>
		/// </param>
		/// <returns>
		/// This instance of <see cref="QueryMaker" /> with added <see cref="Select" /> component.
		/// </returns>
		public QueryMaker WithSelect(params string[] fields)
		{
			return WithSelect(new Select(fields));
		}

		/// <summary>
		/// Add <see cref="Select" /> component to this instance with distinct by operation.
		/// <para>Note: If <paramref name="fields" /> is left empty then will return query as is.</para>
		/// </summary>
		/// <param name="fields">
		/// <para>Fields by which to distinct by.</para>
		/// </param>
		/// <returns>
		/// This instance of <see cref="QueryMaker" /> with added <see cref="Select" /> component.
		/// </returns>
		public QueryMaker WithDistinctBy(params string[] fields)
		{
			return WithSelect(new Select(fields, true));
		}

		#endregion Public Instance Methods

		#region Public Static Methods

		/// <summary>
		/// Adds expressions to <paramref name="query" /> using properties from <paramref name="queryMaker" /> instance.
		/// </summary>
		/// <returns>
		/// Instance of <see cref="QueryMakerResult{T}" /> with resulting paginated and unpaginated queries.
		/// </returns>
		/// <param name="query">
		/// <para>Instance of <see cref="IQueryable{T}" /> to add expressions</para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <param name="queryMaker">
		/// Instance of QueryMaker class with components of actions to perform
		/// <para>NOTE: If set null then will perform no actions and return  and return <paramref name="query" /> as is.</para>
		/// </param>
		/// <exception cref="Exception" />
		public static QueryMakerResult<T> MakeQueryResult<T>(IQueryable<T> query, QueryMaker queryMaker)
		{
			try
			{
				return queryMaker.MakeQueryResult(query);
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
		/// An <see cref="IQueryable{T}" /> with performed <paramref name="queryMaker" /> components actions.
		/// </returns>
		/// <param name="query">
		/// <para>The current instance of <see cref="IQueryable{T}" /></para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <param name="queryMaker">
		/// Instance of QueryMaker class with components of actions to perform
		/// <para>NOTE: If set null then will perform no actions and return  and return <paramref name="query" /> as is.</para>
		/// </param>
		/// <exception cref="Exception" />
		public static IQueryable<T> MakeQuery<T>(IQueryable<T> query, QueryMaker queryMaker)
			=> queryMaker.MakeQuery(query);

		/// <summary>
		/// <para>Shortcut method which only performs filtering on <paramref name="query" /> using <paramref name="filter" />.</para>
		/// <para>For perming all operations, use <see cref="MakeQuery{T}(IQueryable{T}, QueryMaker)" /> or <see cref="MakeQueryResult{T}(IQueryable{T}, QueryMaker)" />.</para>
		/// </summary>
		/// <returns>
		/// An <see cref="IQueryable{T}" /> with added filter actions.
		/// </returns>
		/// <param name="query">
		/// <para>The current instance of <see cref="IQueryable{T}" /></para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <param name="filter">
		/// The instance of <see cref= "Components.Filter" /> component to use for applying the filter on <paramref name="query" />.
		/// <para>NOTE: If set null then will not perform filtering and return <paramref name="query" /> as is.</para>
		/// </param>
		/// <exception cref="Exception" />
		public static IQueryable<T> Filtering<T>(IQueryable<T> query, Filter filter)
			=> new QueryMaker(filter: filter).MakeQuery(query);

		/// <summary>
		/// <para>Shortcut method which only performs sorting on <paramref name="query" /> using <paramref name="sort" /> instance.</para>
		/// <para>For perming all operations, use <see cref="MakeQuery{T}(IQueryable{T}, QueryMaker)" /> or <see cref="MakeQueryResult{T}(IQueryable{T}, QueryMaker)" />.s</para>
		/// </summary>
		/// <returns>
		/// An <see cref="IQueryable{T}" /> with added sorting actions.
		/// </returns>
		/// <param name="query">
		/// <para>The current instance of <see cref="IQueryable{T}" /></para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <param name="sort">
		/// The instance of <see cref= "Components.Sort" /> component to use for applying the sort on <paramref name="query" />.
		/// <para>NOTE: If set null then will not perform sorting and return <paramref name="query" /> as is.</para>
		/// </param>
		/// <exception cref="Exception" />
		public static IQueryable<T> Sorting<T>(IQueryable<T> query, Sort sort)
			=> new QueryMaker(sort: sort).MakeQuery(query);

		/// <summary>
		/// <para>Shortcut method which only performs paging actions on <paramref name="query" /> using <paramref name="page" /> instance.</para>
		/// <para>For perming all operations, use <see cref="MakeQuery{T}(IQueryable{T}, QueryMaker)" /> or <see cref="MakeQueryResult{T}(IQueryable{T}, QueryMaker)" />.</para>
		/// </summary>
		/// <returns>
		/// An <see cref="IQueryable{T}" /> with added paging actions.
		/// </returns>
		/// <param name="query">
		/// <para>Instance of <see cref="IQueryable{T}" /> to add paging</para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <param name="page">
		/// The instance of <see cref= "Components.Page" /> component to use for applying the page on <paramref name="query" />.
		/// <para>NOTE: If set null then will not perform paging and return <paramref name="query" /> as is.</para>
		/// </param>
		/// <exception cref="Exception" />
		public static IEnumerable<T> Paging<T>(IQueryable<T> query, Page page)
			=> new QueryMaker(page: page).MakeQuery(query);

		/// <summary>
		/// <para>Shortcut method which only performs selecting on <paramref name="query" /> using <paramref name="select" /> instance.</para>
		/// <para>For perming all operations, use <see cref="MakeQuery{T}(IQueryable{T}, QueryMaker)" /> or <see cref="MakeQueryResult{T}(IQueryable{T}, QueryMaker)" />.</para>
		/// </summary>
		/// <returns>
		/// An <see cref="IQueryable{T}" /> with added select actions.
		/// </returns>
		/// <param name="query">
		/// <para>The current instance of <see cref="IQueryable{T}" /></para>
		/// NOTE: If set null will throw exception
		/// </param>
		/// <param name="select">
		/// The instance of <see cref= "Components.Select" /> component to use for applying the page on <paramref name="query" />.
		/// <para>NOTE: If set null then will not perform selecting and return <paramref name="query" /> as is.</para>
		/// </param>
		/// <exception cref="Exception" />
		public static IQueryable<T> Selecting<T>(IQueryable<T> query, Select select)
			=> new QueryMaker(select: select).MakeQuery(query);

		#endregion Public Static Methods
	}
}