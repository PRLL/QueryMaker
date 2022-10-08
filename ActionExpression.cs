using System.ComponentModel;
using System.Linq.Expressions;
using QueryMakerLibrary.Constants;
using QueryMakerLibrary.Logic;
using static QueryMakerLibrary.Components.Filter;

namespace QueryMakerLibrary
{
	internal sealed class ActionExpression
	{
		#region PRIVATE MEMBERS

		private Expression? _memberExpression;
		private ConstantExpression? _valueExpression;
		private Type? _actualMemberType;

		#endregion PRIVATE MEMBERS

		#region INTERNAL MEMBERS

		internal string Field { get; set; }
		internal FilterActions Action { get; set; }
		internal bool Negate { get; set; } = false;
		internal bool IgnoreCase { get; set; } = false;

		internal Expression MemberExpression
		{
			get =>_memberExpression ?? throw new Exception(NotNullalbleMember("MemberExpression"));
			set
			{
				_memberExpression = value ?? throw new Exception(NotNullalbleMember("MemberExpression"));

				IsMemberEnumerable = MemberMethods.IsEnumerableType(MemberExpression.Type);
				IsMemberExpressionString = (IsMemberEnumerable
					? MemberExpression.Type.GetGenericArguments()[0]
					: MemberExpression.Type).Equals(typeof(string));

				ActualMemberType = IsMemberEnumerable
					? MemberExpression.Type.GetGenericArguments()[0]
					: MemberExpression.Type;
			}
		}

		internal ConstantExpression ValueExpression
		{
			get =>_valueExpression ?? throw new Exception(NotNullalbleMember("ValueExpression"));
			set
			{
				_valueExpression = value ?? throw new Exception(NotNullalbleMember("ValueExpression"));
				IsValueEnumerable = MemberMethods.IsEnumerableType(ValueExpression.Type);
			}
		}

		#region READONLY MEMBERS

		internal bool IsMemberEnumerable { get; private set; }

		internal bool IsValueEnumerable { get; private set; }

		internal bool IsMemberExpressionString { get; private set; }

		internal bool ConvertMemberExpressionToString
		{
			get => !MemberExpression.Type.Equals(ValueExpression.Type)
					&& !MemberExpression.Type.Equals(typeof(string));
		}
		internal bool ConvertValueExpressionToString
		{
			get => !MemberExpression.Type.Equals(ValueExpression.Type)
					&& !ValueExpression.Type.Equals(typeof(string));
		}

		[Category("Read Only")]
		[Description("If Member's Expression is an Enumerable, then return type of Enumerable")]
		internal Type ActualMemberType
		{
			get => _actualMemberType ?? throw Errors.Exception(Errors.FieldTypeNull, this.Field);
			private set => _actualMemberType = value;
		}

		#endregion READONLY MEMBERS

		#endregion INTERNAL MEMBERS


		#region CONSTRUCTOR


		internal ActionExpression(string field, FilterActions action, bool negate, bool ignoreCase,
			Expression memberExpression, object? value)
		{
			Field = field;
			Action = action;
			Negate = negate;
			IgnoreCase = ignoreCase;
			MemberExpression = memberExpression;
			ValueExpression = CreateExpression.ConstantExpression(ActualMemberType,
				field, value);
		}

		internal ActionExpression(string field, FilterActions action, bool negate, bool ignoreCase,
			Expression memberExpression, ConstantExpression valueExpression)
		{
			Field = field;
			Action = action;
			Negate = negate;
			IgnoreCase = ignoreCase;
			MemberExpression = memberExpression;
			ValueExpression = valueExpression;
		}

		#endregion CONSTRUCTOR


		#region METHODS

		private static string NotNullalbleMember(string member)
		{
			return string.Format(Errors.NotNullableMember, member, "ActionExpression");
		}

		#endregion METHODS
	}
}