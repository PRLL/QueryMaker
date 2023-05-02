using QueryMakerLibrary.Logic;

namespace QueryMakerLibrary.Components
{
	/// <summary>
	/// Class for select component set on property <see cref="QueryMaker.Select" />.
	/// </summary>
	public sealed class Select
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of <see cref="Select" /> class with all properties set to their defaults.
		/// </summary>
		public Select()
			: this(Array.Empty<string>(), false)
		{
			//
		}

		/// <summary>
		/// Initializes a new instance of <see cref="Select" /> class.
		/// <para>Note: If <paramref name="fields" /> is left empty then will return query as is.</para>
		/// </summary>
		/// <param name="fields">
		/// <para>Fields to select on query.</para>
		/// </param>
		public Select(params string[] fields)
			: this(fields, false)
		{
			//
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Select" /> class.
		/// <para>Note: If <paramref name="fields" /> is left empty then will return query as is.</para>
		/// </summary>
		/// <param name="fields">
		/// <para>Fields to select on query.</para>
		/// </param>
		/// <param name="distinctBy">
		/// <para>If set to true, will return resulting items which values are distinct on <see cref="Fields" />.</para>
		/// <para>If <see cref="Fields" /> property is left empty, then will not perform ditinct by operation.</para>
		/// <para>Defaults to false.</para>
		/// </param>
		public Select(string[] fields, bool distinctBy)
		{
			Fields = fields;
			DistinctBy = distinctBy;
		}

		#endregion Constructors

		#region Private Fields

		private string[] _fields = Array.Empty<string>();

		#endregion Private Fields

		#region Public Properties

		/// <summary>
		/// <para>Fields to select on query.</para>
		/// <para>Defaults to empty string array.</para>
		/// <para>NOTE: If left empty will not perform selection.</para>
		/// </summary>
		public string[] Fields
		{
			get => _fields;
			set => _fields = PropertyMethods.CleanFieldsArray(value ?? Array.Empty<string>());
		}

		/// <summary>
		/// <para>If set to true, will return resulting items which values are distinct on provided <see cref="Fields" />.</para>
		/// <para>If <see cref="Fields" /> property is left empty, then will not perform ditinct by operation.</para>
		/// <para>Defaults to false.</para>
		/// </summary>
		public bool DistinctBy { get; set; } = false;

		#endregion Public Properties
	}
}