namespace QueryMakerLibrary.Components
{
	/// <summary>
	/// Class for page component set on property <see cref="QueryMakerLibrary.QueryMaker.Page" />.
	/// </summary>
	public sealed class Page
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="QueryMakerLibrary.Components.Page" /> class with an index.
		/// </summary>
		/// <param name="skip">
		/// <para>Quantity of elements to skip on paging action.</para>
		/// <para>Defaults to 0.</para>
		/// <para>NOTE: If set to equal or less than 0, then will not perform skip action.</para>
		/// </param>
		/// <param name="take">
		/// <para>Quantity of elements to get on paging action.</para>
		/// <para>Defaults to 0.</para>
		/// <para>NOTE: If set to equal or less than 0, then will not perform take action.</para>
		/// </param>
		/// <param name="index">
		/// <para>Field used as index for faster pagination.</para>
		/// <para>Defaults to empty string.</para>
		/// <para>NOTE: If left empty, then pagination will be performed without an index.</para>
		/// </param>
		public Page(int skip = 0, int take = 0, string index = "")
		{
			Skip = skip;
			Take = take;
			Index = index;
		}

		#endregion Constructors

		#region Public Properties

		/// <summary>
		/// <para>Field used as index for faster pagination.</para>
		/// <para>Defaults to empty string.</para>
		/// <para>NOTE: If left empty, then pagination will be performed without an index.</para>
		/// </summary>
		public string Index { get; set; } = string.Empty;

		/// <summary>
		/// <para>Quantity of elements to skip on paging action.</para>
		/// <para>Defaults to 0.</para>
		/// <para>NOTE: If set to equal or less than 0, then will not perform skip action.</para>
		/// </summary>
		public int Skip { get; set; } = 0;

		/// <summary>
		/// <para>Quantity of elements to get on paging action.</para>
		/// <para>Defaults to 0.</para>
		/// <para>NOTE: If set to equal or less than 0, then will not perform take action.</para>
		/// </summary>
		public int Take { get; set; } = 0;

		#endregion Public Properties
	}
}