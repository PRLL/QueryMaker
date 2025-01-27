namespace QueryMakerLibrary.Constants
{
	internal static class Errors
	{
		internal const string ErrorMessage = "QueryMaker Exception: {0}",
			IQueryableNull = "Instance of 'IQueryable' to work with cannot be null",
			QueryMakerNull = "Instance of 'QueryMaker' passed cannot be null",
			SearchParametersEmpty = "Some required parameter(s) were empty for Search object '{0}'",
			EnummerableNotImplemented = "Doing {0} operations for Enummerable root object is not implemented yet",
			SearchFieldEmpty = "One of the passed fields for global search was empty",
			FilterFieldsEmpty = "No Fields passed for a Filter action",
			FilterParametersEmpty = "Some required parameter(s) are empty of single Filter object passed '{0}'",
			FilterInvalidAction = "Invalid 'Action' value passed for a Filter",
			InvalidOperationValue = "Invalid 'Operation' value '{0}'",
			SortFieldEmpty = "Field for sorting was empty",
			InvalidSortingDirection = "Invalid sorting direction '{0}'",
			DistinctFieldEmpty = "One of the passed fields for distinctBy was empty",
			InvalidTypeConversion = "Invalid type conversion",
			ConvertJsonArrayToList = "Converting passed json array value '{0}' to a List",
			GenerateExpressionError = "Creating expression for field '{0}' with value '{1}'.",
			DistincByAfterSelect = "Tried using 'DistinctBy' and 'Select' together, but some fields sent on 'DistinctBy' are missing from 'Select'",
			ListToListNotImplemeneted = "{0} list to list content evaluation expression not implemented yet",
			ExpressionValueToList = "Error ocurred converting expression value to List",
			NoPropertyOrField = "{0} does not contain property or field {1}",
			ActionText = "Not Text Implemented",
			NotNullableMember = "Value for Member {0} cannot be null for class {1}",
			ClassMembersNoOperation = "Passed a Filter object without 'Fields', tried using fields and properties from root class and multiple where found, "
				+ "but no 'FieldsOperation' was passed to join them",
			MultipleFieldsNoOperation = "Passed a Filter object with multiple 'Fields', but value on 'FieldsOperation' is not a valid operation",
			AddingSort = "Cannot set ThenSortBy because there's no root Sort set";

		internal static Exception Exception(string errorMessage)
		{
			return Exception(errorMessage, Array.Empty<object?>());
		}

		internal static Exception Exception(string errorMessage, params object?[] values)
		{
			return new Exception(values.Any()
				? string.Format(errorMessage, values)
				: errorMessage);
		}
	}
}