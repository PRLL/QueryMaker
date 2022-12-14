namespace QueryMakerLibrary.Components
{
	/// <summary>
	/// Class for page component set on property <see cref="QueryMakerLibrary.QueryMaker.Sort" />.
	/// </summary>
	public sealed class Sort
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="QueryMakerLibrary.Components.Sort" /> class.
		/// </summary>
		/// <param name="field">
		/// <para>Field to sort by.</para>
		/// <para>Defaults to empty string value.</para>
		/// <para>NOTE: If left empty or null, then will throw exception.</para>
		/// </param>
		/// <param name="direction">
		/// <para>Sorting direction.</para>
		/// <para>Refer to <see cref="QueryMakerLibrary.Components.Sort.SortDirections" /> for possible values</para>
		/// <para>Defaults to <see cref="QueryMakerLibrary.Components.Sort.SortDirections.Ascending" />.</para>
		/// <para>NOTE: If not a valid direction, then will throw exception.</para>
		/// </param>
		/// <param name="then">
		/// <para>Sorting performed after this one.</para>
		/// <para>Defaults to null.</para>
		/// </param>
		public Sort(string field, SortDirections direction = SortDirections.Ascending, Sort? then = null)
		{
			Field = field;
			Direction = direction;
			Then = then;
		}

		#endregion Constructors

		#region Public Properties

		/// <summary>
		/// <para>Field to sort by.</para>
		/// <para>Defaults to empty string value.</para>
		/// <para>NOTE: If left empty or null, then will throw exception.</para>
		/// </summary>
		public string Field { get; set; } = string.Empty;

		/// <summary>
		/// <para>Sorting direction.</para>
		/// <para>Set 1 for Ascending; 2 for Descending.</para>
		/// <para>Defaults to 1.</para>
		/// <para>NOTE: If not a valid direction, then will throw exception.</para>
		/// </summary>
		public SortDirections Direction { get; set; } = SortDirections.Ascending;

		/// <summary>
		/// <para>Sorting performed after this one.</para>
		/// <para>Defaults to null.</para>
		/// </summary>
		public Sort? Then { get; set; } = null;

		#endregion Public Properties

		#region Methods

		/// <summary>
		/// <para>Add sorting to be performed after this one</para>
		/// </summary>
		/// <param name="sort">
		/// <para>Instance of <see cref="Sort" /> to perform after this one.</para>
		/// </param>
		public Sort AndThen(Sort sort)
		{
			Then = sort;
			return this;
		}

		/// <summary>
		/// <para>Add sorting to be performed after this one</para>
		/// </summary>
		/// <param name="field">
		/// <para>Field to sort by.</para>
		/// <para>Defaults to empty string value.</para>
		/// <para>NOTE: If left empty or null, then will throw exception.</para>
		/// </param>
		/// <param name="direction">
		/// <para>Sorting direction.</para>
		/// <para>Refer to <see cref="QueryMakerLibrary.Components.Sort.SortDirections" /> for possible values</para>
		/// <para>Defaults to <see cref="QueryMakerLibrary.Components.Sort.SortDirections.Ascending" />.</para>
		/// <para>NOTE: If not a valid direction, then will throw exception.</para>
		/// </param>
		/// <param name="then">
		/// <para>Sorting performed after this one.</para>
		/// <para>Defaults to null.</para>
		/// </param>
		public Sort AndThen(string field, SortDirections direction = SortDirections.Ascending, Sort? then = null)
		{
			Then = new (field, direction, then);
			return this;
		}

		#endregion Methods

		#region Enums

		/// <summary>
		/// Enum containing the possible values for <see cref="QueryMakerLibrary.Components.Sort.Direction" /> property.
		/// </summary>
		public enum SortDirections
		{
			/// <summary>
			/// Ascending direction.
			/// </summary>
			Ascending = 1,

			/// <summary>
			/// Descending direction.
			/// </summary>
			Descending = 2
		}

		#endregion Enums
	}
}