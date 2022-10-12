using System.Reflection;

namespace QueryMakerLibrary.Logic
{
	internal static class EnumerableMethods
	{
		internal static MethodInfo GetEnumerableTypedMethod(Type type, string methodName)
		{
			return typeof(Enumerable)
				.GetMethods()
				.Where(method =>
					method.Name == methodName)
				.Single(x => x.GetParameters().Length == 2)
				.MakeGenericMethod(type);
		}
	}
}