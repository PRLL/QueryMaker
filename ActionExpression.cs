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

        private FilterActions _action;
        private Expression? _memberExpression;
		private ConstantExpression? _valueExpression;
		private Type? _actualMemberType;

		#endregion PRIVATE MEMBERS

		#region INTERNAL MEMBERS

		internal string Field { get; set; }

        internal FilterActions Action
        {
            get => _action;
            set
            {
                _action = value;
                IsContentAction = Action is FilterActions.Contains or FilterActions.NotContains
                    or FilterActions.StartsWith or FilterActions.NotStartsWith
                    or FilterActions.EndsWith or FilterActions.NotEndsWith;
            }
        }

        internal bool Negate { get; set; } = false;
		internal bool IgnoreCase { get; set; } = false;

		internal Expression MemberExpression
		{
			get =>_memberExpression ?? throw new Exception(NotNullalbleMember("MemberExpression"));
			set
			{
				_memberExpression = value ?? throw new Exception(NotNullalbleMember("MemberExpression"));

				IsMemberEnumerable = MemberMethods.IsEnumerableType(MemberExpression.Type);
				ActualMemberType = IsMemberEnumerable || MemberMethods.IsNullableType(MemberExpression.Type)
                    ? MemberExpression.Type.GetGenericArguments()[0]
                    : MemberExpression.Type;
                
				IsMemberExpressionString = ActualMemberType == typeof(string);
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

		internal bool IsContentAction { get; private set; }

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
				field, value, IsContentAction);
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