using System.Linq.Expressions;
using System.Text.Json;
using QueryMakerLibrary.Constants;

namespace QueryMakerLibrary.Logic
{
	internal static class CreateExpression
	{
		/// <summary>
		/// Convert 'value' to expression's property and return as ConstantExpression
		/// </summary>
		internal static ConstantExpression ConstantExpression(Type propertyType, string field, object? value, bool isContentAction)
		{
			try
			{
				return Expression.Constant(value is JsonElement jsonElement
					? TypeConversions.ConvertJsonElementToValidType(propertyType, jsonElement, isContentAction)
					: TypeConversions.ConvertValueToPropertyType(propertyType, value, isContentAction));
			}
			catch (Exception ex)
			{
				throw Errors.Exception(Errors.GenerateExpressionError, field, value, ex);
			}
		}

		/// <summary>
		/// Constructs and returns either a BinaryExpression or a MethodCallExpression based on 'action'
		/// </summary>
		internal static Expression ActionExpression(ActionExpression actionExpression)
		{
			// if both 'memberExpression' and 'valueExpression' are enumerable
			if (actionExpression.IsMemberEnumerable)
			{
				return ContentEvaluation.CreateListContentEvaluationExpression(actionExpression);
			}
			// else if 'memberExpression' is not enumerable but 'valueExpression' is an enumerable, 
			else if (actionExpression.IsValueEnumerable)
			{
				return ContentEvaluation.CreateMemberToListEvaluationExpression(actionExpression);
			}
			else
			{
				return ContentEvaluation.CreateEvaluationExpression(actionExpression, actionExpression.ValueExpression.Value);
			}
		}
	}
}