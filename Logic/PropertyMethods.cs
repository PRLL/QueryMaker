namespace QueryMakerLibrary.Logic
{
	internal static class PropertyMethods
	{
		internal static string[] CleanFieldsArray(string[] fields)
		{
			List<string> cleanFields = new List<string>();

			foreach (string field in fields)
			{
				string trimmedField = field.Replace(" ", string.Empty);
				if (!string.IsNullOrWhiteSpace(trimmedField))
				{
					cleanFields.Add(trimmedField);
				}
			}

			return cleanFields.Distinct().ToArray();
		}
	}
}