using QueryMakerLibrary.Constants;
using static QueryMakerLibrary.Components.Filter;

namespace QueryMakerLibrary.Logic
{
	internal static class EnumMethods
	{
		internal static bool IsValidAction(FilterActions action)
		{
			return Enum.IsDefined(typeof(FilterActions), action);
		}

		internal static string GetActionText(FilterActions action)
		{
			switch(action)
			{
				case FilterActions.Contains:
				case FilterActions.NotContains:
					return "Contains";

				case FilterActions.StartsWith:
				case FilterActions.NotStartsWith:
					return "StartsWith";

				case FilterActions.EndsWith:
				case FilterActions.NotEndsWith:
					return "EndsWith";

				// case Action.Equal:
				// case Action.NotEqual:
				// case Action.GreaterThan:
				// case Action.LessThan:
				// case Action.GreaterThanOrEqual:
				// case Action.LessThanOrEqual:
				default:
					throw new Exception(Errors.ActionText);
			}
		}

		internal static bool IsValidOperation(FilterOperations operation)
		{
			return Enum.IsDefined(typeof(FilterOperations), operation);
		}
	}
}