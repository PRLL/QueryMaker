using QueryMakerLibrary.Logic;

namespace QueryMakerLibrary.Components
{
	/// <summary>
	/// Class for select component set on property <see cref="QueryMakerLibrary.QueryMaker.Select" />.
	/// </summary>
	public sealed class Select
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of <see cref="QueryMakerLibrary.Components.Select" /> class with all properties set to their defaults.
		/// </summary>
		public Select()
		{
			//
		}

		/// <summary>
		/// Initializes a new instance of <see cref="QueryMakerLibrary.Components.Select" /> class.
		/// <para>Note: If left <paramref name="fields" /> empty then will return query as is.</para>
		/// </summary>
		/// <param name="fields">
		/// <para>Fields to select.</para>
		/// </param>
		public Select(params string[] fields)
		{
			Fields = fields;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="QueryMakerLibrary.Components.Select" /> class.
		/// <para>Note: If both properties are empty then will return query as is.</para>
		/// </summary>
		/// <param name="fields">
		/// <para>Fields to select on query.</para>
		/// <para>Defaults to empty string array.</para>
		/// <para>NOTE: If left empty will not perform selection.</para>
		/// </param>
		/// <param name="distinctBy">
		/// <para>Fields to distinguish query by. 
		/// If <see cref="QueryMakerLibrary.Components.Select.Fields" /> property is left empty, then selection will be performed using this fields</para>
		/// <para>Defaults to empty string array.</para>
		/// <para>NOTE: If left empty distinction will not be performed.</para>
		/// </param>
		public Select(string[]? fields = null, string[]? distinctBy = null)
		{
			Fields = fields ?? new string[0];
			DistinctBy = distinctBy ?? new string[0];
		}

		#endregion Constructors

		#region Private Fields

		private string[] _fields = new string[0];
		private string[] _distinctBy = new string[0];

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
			set
			{
				_fields = PropertyMethods.CleanFieldsArray(value ?? new string[0]);
			}
		}

		/// <summary>
		/// <para>Fields to distinguish query by. 
		/// If <see cref="QueryMakerLibrary.Components.Select.Fields" /> property is left empty, then selection will be performed using this fields</para>
		/// <para>Defaults to empty string array.</para>
		/// <para>NOTE: If left empty distinction will not be performed.</para>
		/// </summary>
		public string[] DistinctBy
		{
			get => _distinctBy;
			set
			{
				_distinctBy = PropertyMethods.CleanFieldsArray(value ?? new string[0]);
			}
		}

		#endregion Public Properties
	}
}