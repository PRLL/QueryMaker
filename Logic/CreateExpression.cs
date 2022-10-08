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
		internal static ConstantExpression ConstantExpression(Type propertyType, string field, object? value)
		{
			Object? actualValue = value;
			try
			{
				if (value is JsonElement)
				{
					actualValue = TypeConversions.ConvertJsonElementToValidType(propertyType, (JsonElement)value);
				}
				else if (value is not null)
				{
					actualValue = TypeConversions.ConvertValueToPropertyType(propertyType, value);
				}
			}
			catch (Exception ex)
			{
				throw Errors.Exception(Errors.GenerateExpressionError, field, value, ex);
			}

			return Expression.Constant(actualValue);
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