using QueryMakerLibrary.Logic;

namespace QueryMakerLibrary.Components
{
	/// <summary>
	/// Class for filter component set on property <see cref="QueryMakerLibrary.QueryMaker.Filter" />.
	/// </summary>
	public sealed class Filter
	{
		#region Constructors

		/// <summary>
		/// <para>Initializes a new instance of <see cref="QueryMakerLibrary.Components.Filter" /> class with all properties set to their default values.</para>
		/// </summary>
		public Filter()
		{
			//
		}

		/// <summary>
		/// <para>Initializes a new instance of <see cref="QueryMakerLibrary.Components.Filter" /> class.</para>
		/// </summary>
		/// <param name="field">
		/// <para>Field to perform filtering on.</para>
		/// <para>If performing filtering on a primitive type enumerable (like a list of strings) then can leave empty.</para>
		/// </param>
		/// <param name="action">
		/// <para>Action to perform.</para>
		/// <para>Refer to <see cref="QueryMakerLibrary.Components.Filter.FilterActions" /> for possible values.</para>
		/// <para>NOTE: If not set to a valid <see cref="QueryMakerLibrary.Components.Filter.FilterActions" /> value, will throw exception when performing filtering.</para>
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
		public Filter(string field, FilterActions action, object? value, bool ignoreCase = true, bool negate = false)
		{
			Field = field;
			Action = action;
			Value = value;
			IgnoreCase = ignoreCase;
			Negate = negate;
		}

		/// <summary>
		/// <para>Initializes a new instance of <see cref="QueryMakerLibrary.Components.Filter" /> class with required parameters for performing filtering using multiple fields.</para>
		/// </summary>
		/// <param name="fields">
		/// <para>Array of fields to perform filtering on.</para>
		/// <para>If performing filtering on a primitive type enumerable (like a list of strings) then can leave empty.</para>
		/// </param>
		/// <param name="action">
		/// <para>Action to perform.</para>
		/// <para>Refer to <see cref="QueryMakerLibrary.Components.Filter.FilterActions" /> for possible values.</para>
		/// <para>NOTE: If not set to a valid <see cref="QueryMakerLibrary.Components.Filter.FilterActions" /> value, will throw exception when performing filtering.</para>
		/// </param>
		/// <param name="value">
		/// <para>Value to filter by.</para>
		/// </param>
		/// <param name="fieldsOperation">
		/// <para>Operation to perform between <paramref name="fields" />.</para>
		/// <para>Refer to <see cref="QueryMakerLibrary.Components.Filter.FilterOperations" /> for possible values</para>
		/// <para>Defaults to <see cref="QueryMakerLibrary.Components.Filter.FilterOperations.OrElse" />.</para>
		/// <para>NOTE: If not set to a valid <see cref="QueryMakerLibrary.Components.Filter.FilterOperations" /> value, then will throw exception when performing filtering.</para>
		/// </param>
		/// <param name="ignoreCase">
		/// <para>Set true to ignore case sensitivity on evaluation performed on filtering.</para>
		/// <para>Defaults to true.</para>
		/// </param>
		/// <param name="negate">
		/// <para>Set true to negate evaluation performed on filtering.</para>
		/// <para>Defaults to false.</para>
		/// </param>
		public Filter(string[] fields, FilterActions action, object? value, FilterOperations fieldsOperation = FilterOperations.OrElse, bool ignoreCase = true, bool negate = false)
		{
			Value = value;
			Action = action;
			Fields = fields;
			FieldsOperation = fieldsOperation;
			IgnoreCase = ignoreCase;
			Negate = negate;
		}
		
		/// <summary>
		/// <para>Initializes a new instance of <see cref="QueryMakerLibrary.Components.Filter" /> class as a joiner with subfilters to join.</para>
		/// <para><see cref="QueryMakerLibrary.Components.Filter.IsJoiner" /> will be set to true by default</para>
		/// </summary>
		/// <param name="subFiltersOperation">
		/// <para>Operation to perform between passed <paramref name="subFilters" />.</para>
		/// <para>Refer to <see cref="QueryMakerLibrary.Components.Filter.FilterOperations" /> for possible values</para>
		/// <para>NOTE: If not set to a valid <see cref="QueryMakerLibrary.Components.Filter.FilterOperations" /> value and <see cref="QueryMakerLibrary.Components.Filter.SubFilters" /> property has items, then will throw exception when joining subfilters.</para>
		/// </param>
		/// <param name="subFilters">
		/// <para>Array of <see cref="QueryMakerLibrary.Components.Filter" /> instances to be joined</para>
		/// NOTE: If this property has items and <see cref="QueryMakerLibrary.Components.Filter.SubFiltersOperation" /> property is not set to a valid value, then will throw exception
		/// </param>
		public Filter(FilterOperations subFiltersOperation, params Filter[] subFilters)
		{
			IsJoiner = true;
			SubFiltersOperation = subFiltersOperation;
			SubFilters = subFilters;
		}

		#endregion Constructors

		#region Private Fields

		private string[] _fields = new string[0];
		private Filter[] _subFilters = new Filter[0];

		#endregion Private Fields

		#region Public Properties

		/// <summary>
		/// <para>If set to true, this filter instance will only serve as joiner for subfilters;
		/// If left as false, then will perform action as normal.</para>
		/// <para>Defaults to false.</para>
		/// </summary>
		public bool IsJoiner { get; set; } = false;

		/// <summary>
		/// <para>Value to filter by.</para>
		/// <para>Defaults to null.</para>
		/// </summary>
		public object? Value { get; set; } = null;

		/// <summary>
		/// <para>Action to perform on this filter instance .</para>
		/// <para>Refer to <see cref="QueryMakerLibrary.Components.Filter.FilterActions" /> for possible values</para>
		/// <para>Defaults to 0.</para>
		/// <para>NOTE: If not set to a valid <see cref="QueryMakerLibrary.Components.Filter.FilterActions" /> value, will throw exception when performing Filter action.</para>
		/// </summary>
		public FilterActions Action { get; set; }

		/// <summary>
		/// <para>Set true to negate action performed on this filter instance.</para>
		/// <para>Defaults to false.</para>
		/// </summary>
		public bool Negate { get; set; } = false;

		/// <summary>
		/// <para>Set true to ignore case sensitivity on action performed on this filter instance.</para>
		/// <para>Defaults to true.</para>
		/// </summary>
		public bool IgnoreCase { get; set; } = true;

		/// <summary>
		/// <para>Shortcut property for passing a single field which will be set as array[1] on <see cref="QueryMakerLibrary.Components.Filter.Fields" /> property.</para>
		/// <para>If performing filtering on a primitive type enumerable (like a list of strings) then can leave empty.</para>
		/// <para>NOTE: This property is only used for set, if you want to get the value use <see cref="QueryMakerLibrary.Components.Filter.Fields" /> property.</para>
		/// </summary>
		public string Field
		{
			set
			{
				Fields = value is not null ? new string[1] { value } : new string[0];
			}
		}

		/// <summary>
		/// <para>Array of fields to perform filtering on.</para>
		/// <para>If performing filtering on a primitive type enumerable (like a list of strings) then can leave empty.</para>
		/// <para>If set more than one field, then <see cref="QueryMakerLibrary.Components.Filter.FieldsOperation" /> property is required to join each field's action.</para>
		/// <para>Defaults to empty array of strings.</para>
		/// <para>NOTE: If required and left empty, then will throw exception when performing Filter action.</para>
		/// </summary>
		public string[] Fields
		{
			get => _fields;
			set
			{
				_fields = PropertyMethods.CleanFieldsArray(value ?? new string[0]);
			}
		}

		/// <summary>
		/// <para>Operation to perform between multiple fields when passed more than one on <see cref="QueryMakerLibrary.Components.Filter.Fields" /> property.</para>
		/// <para>Refer to <see cref="QueryMakerLibrary.Components.Filter.FilterOperations" /> for possible values</para>
		/// <para>Defaults to <see cref="QueryMakerLibrary.Components.Filter.FilterOperations.OrElse" />.</para>
		/// <para>NOTE: If not set to a valid <see cref="QueryMakerLibrary.Components.Filter.FilterOperations" /> value and <see cref="QueryMakerLibrary.Components.Filter.Fields" /> property contains more than one field, then will throw exception when performing Filtering action.</para>
		/// </summary>
		public FilterOperations FieldsOperation { get; set; } = FilterOperations.OrElse;

		/// <summary>
		/// <para>Shortcut property to set the value of <see cref="QueryMakerLibrary.Components.Filter.SubFilters" /> property as a single filter
		/// with <see cref="QueryMakerLibrary.Components.Filter.SubFiltersOperation" /> set to <see cref="QueryMakerLibrary.Components.Filter.FilterOperations.OrElse" />.</para>
		/// <para>NOTE: This property is only used for set, if you want to get the value use <see cref="QueryMakerLibrary.Components.Filter.SubFilters" /> property.</para>
		/// </summary>
		public Filter Or
		{
			set
			{
				if (value is not null)
				{
					SubFiltersOperation = FilterOperations.OrElse;
					SubFilters = new Filter[1] { value };
				}
			}
		}

		/// <summary>
		/// <para>Shortcut property to set the value of <see cref="QueryMakerLibrary.Components.Filter.SubFilters" /> property
		/// with <see cref="QueryMakerLibrary.Components.Filter.SubFiltersOperation" /> set to <see cref="QueryMakerLibrary.Components.Filter.FilterOperations.OrElse" />.</para>
		/// </summary>
		public Filter[] Ors
		{
			get => SubFilters;
			set
			{
				if (value is not null)
				{
					SubFiltersOperation = FilterOperations.OrElse;
					SubFilters = value;
				}
			}
		}

		/// <summary>
		/// <para>Shortcut property to set the value of <see cref="QueryMakerLibrary.Components.Filter.SubFilters" /> property as a single filter
		/// with <see cref="QueryMakerLibrary.Components.Filter.SubFiltersOperation" /> set to <see cref="QueryMakerLibrary.Components.Filter.FilterOperations.AndAlso" />.</para>
		/// <para>NOTE: This property is only used for set, if you want to get the value use <see cref="QueryMakerLibrary.Components.Filter.SubFilters" /> property.</para>
		/// </summary>
		public Filter And
		{
			set
			{
				if (value is not null)
				{
					SubFiltersOperation = FilterOperations.AndAlso;
					SubFilters = new Filter[1] { value };
				}
			}
		}

		/// <summary>
		/// <para>Shortcut property to set the value of <see cref="QueryMakerLibrary.Components.Filter.SubFilters" /> property
		/// with <see cref="QueryMakerLibrary.Components.Filter.SubFiltersOperation" /> set to <see cref="QueryMakerLibrary.Components.Filter.FilterOperations.AndAlso" />.</para>
		/// </summary>
		public Filter[] Ands
		{
			get => SubFilters;
			set
			{
				if (value is not null)
				{
					SubFiltersOperation = FilterOperations.AndAlso;
					SubFilters = value;
				}
			}
		}

		/// <summary>
		/// <para>Array of Filter instances to join with this one; Set <see cref="QueryMakerLibrary.Components.Filter.IsJoiner" /> property to true to instead use this instance as joiner between subfilters.</para>
		/// <para>Defaults to empty array of Filters.</para>
		/// <para>NOTE: If this property has items and <see cref="QueryMakerLibrary.Components.Filter.SubFiltersOperation" /> property is not set to a valid value, then will throw exception</para>
		/// </summary>
		public Filter[] SubFilters
		{
			get => _subFilters;
			set
			{
				_subFilters = value ?? new Filter[0];
			}
		}

		/// <summary>
		/// <para>Operation to perform between subfilters set on <see cref="QueryMakerLibrary.Components.Filter.SubFilters" /> property.</para>
		/// <para>Refer to <see cref="QueryMakerLibrary.Components.Filter.FilterOperations" /> for possible values</para>
		/// <para>Defaults to 0.</para>
		/// <para>NOTE: If not set to a valid <see cref="QueryMakerLibrary.Components.Filter.FilterOperations" /> value and <see cref="QueryMakerLibrary.Components.Filter.SubFilters" /> property has items, then will throw exception when joining subfilters.</para>
		/// </summary>
		public FilterOperations SubFiltersOperation { get; set; }

		#endregion Public Properties

		#region Methods

		/// <summary>
		/// <para>Add subfilters to be performed after this one with AndAlso evaluation.</para>
		/// </summary>
		public Filter AndAlso(params Filter[] subfilters)
		{
			SubFiltersOperation = FilterOperations.AndAlso;
			SubFilters = subfilters;
			return this;
		}

		/// <summary>
		/// <para>Add subfilters to be performed after this one with OrAlso evaluation.</para>
		/// </summary>
		public Filter OrElse(params Filter[] subfilters)
		{
			SubFiltersOperation = FilterOperations.OrElse;
			SubFilters = subfilters;
			return this;
		}

		#endregion Methods

		#region Enums

		/// <summary>
		/// Enum containing the possible values for <see cref="QueryMakerLibrary.Components.Filter.Action" /> property.
		/// </summary>
		public enum FilterActions
		{
			/// <summary>
			/// Contains action.
			/// </summary>
			Contains = 1,

			/// <summary>
			/// StartsWith action.
			/// </summary>
			StartsWith = 2,

			/// <summary>
			/// EndsWith action.
			/// </summary>
			EndsWith = 3,

			/// <summary>
			/// Equal action.
			/// </summary>
			Equal = 4,

			/// <summary>
			/// GreaterThan action.
			/// </summary>
			GreaterThan = 5,

			/// <summary>
			/// LessThan action.
			/// </summary>
			LessThan = 6,

			/// <summary>
			/// GreaterThanOrEqual action.
			/// </summary>
			GreaterThanOrEqual = 7,

			/// <summary>
			/// LessThanOrEqual action.
			/// </summary>
			LessThanOrEqual = 8,

			/// <summary>
			/// NotContains action.
			/// </summary>
			NotContains = 9,

			/// <summary>
			/// NotStartsWith action.
			/// </summary>
			NotStartsWith = 10,

			/// <summary>
			/// NotStartsWith action.
			/// </summary>
			NotEndsWith = 11,

			/// <summary>
			/// NotEqual action.
			/// </summary>
			NotEqual = 12
		}

		/// <summary>
		/// Enum containing the possible values for <see cref="QueryMakerLibrary.Components.Filter.FieldsOperation" /> and <see cref="QueryMakerLibrary.Components.Filter.SubFiltersOperation" /> properties.
		/// </summary>
		public enum FilterOperations
		{
			/// <summary>
			/// OrElse operation.
			/// </summary>
			OrElse = 1,

			/// <summary>
			/// AndAlso operation.
			/// </summary>
			AndAlso = 2,

			/// <summary>
			/// And operation.
			/// </summary>
			And = 3,

			/// <summary>
			/// Or operation.
			/// </summary>
			Or = 4
		}

		#endregion Enums
	}
}