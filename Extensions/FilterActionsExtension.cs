using QueryMakerLibrary.Constants;
using static QueryMakerLibrary.Components.Filter;

namespace QueryMakerLibrary.Extensions;

internal static class FilterActionsExtension
{
	internal static string GetActionText(this FilterActions action)
	{
		return action switch
		{
			FilterActions.Contains or FilterActions.NotContains => "Contains",
			FilterActions.StartsWith or FilterActions.NotStartsWith => "StartsWith",
			FilterActions.EndsWith or FilterActions.NotEndsWith => "EndsWith",
			_ => throw new Exception(Errors.ActionText)
		};
	}
}
