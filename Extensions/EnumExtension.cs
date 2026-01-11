namespace QueryMakerLibrary.Extensions;

internal static class EnumExtension
{
	internal static bool IsValid<TEnum>(this TEnum action)
		where TEnum : struct, Enum
	{
		return Enum.IsDefined(action);
	}
}
