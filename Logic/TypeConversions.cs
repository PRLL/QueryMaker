using System.Collections;
using System.ComponentModel;
using System.Text.Json;
using QueryMakerLibrary.Constants;

namespace QueryMakerLibrary.Logic
{
	internal static class TypeConversions
	{
		#region Object Conversions

		internal static object? ConvertValueToPropertyType(Type propertyType, object? value)
		{
			if (value is null)
			{
				return value;
			}
			else
			{
				TypeConverter converter = TypeDescriptor.GetConverter(propertyType);
				if (MemberMethods.IsEnumerableType(value.GetType()))
				{
					// try converting all item to same propertyType and store on 'objectsList'
					List<object?> objectsList = new List<object?>();
					foreach (object? item in (value as IList) ?? throw Errors.Exception(Errors.ExpressionValueToList))
					{
						objectsList.Add(ConvertObjectToType(converter, item));
					}

					// return list of different objects
					return objectsList;
				}
				else
				{
					return ConvertObjectToType(converter, value);
				}
			}
		}

		private static object? ConvertObjectToType(TypeConverter converter, object? value)
		{
			if (value is not null)
			{
				try
				{
					if (converter.CanConvertFrom(value.GetType()))
					{
						value = converter.ConvertFrom(value);
					}
				}
				catch (Exception)
				{
					//
				}
			}

			return value;
		}

		#endregion Object Conversions

		#region JsonElement Conversions

		internal static Object? ConvertJsonElementToValidType(Type propertyType, JsonElement jsonElement)
		{
			switch (jsonElement.ValueKind)
			{
				case JsonValueKind.Undefined: case JsonValueKind.Null:
					return null;

				case JsonValueKind.Object:
					return DeserializeJsonElement(propertyType, jsonElement);

				case JsonValueKind.Array:
					// IList list = (IList)(Activator.CreateInstance(
					// 	typeof(List<>).MakeGenericType(new [] { typeof(object) }))
					// 		?? throw Errors.Exception(Errors.ConvertJsonArrayToList, jsonElement));

					List<object?> objectsList = new List<object?>();
					jsonElement.EnumerateArray().ToList()
						.ForEach(o => objectsList.Add(ConvertJsonElementToValidType(propertyType, o)));

					// return list of different objects
					return objectsList;

				case JsonValueKind.String: case JsonValueKind.Number:
				case JsonValueKind.False: case JsonValueKind.True:
				default:
					return ConvertJsonElementValueToValidType(propertyType, jsonElement);
			}
		}

		private static Object? DeserializeJsonElement<T>(T objectType, JsonElement jsonElement)
		{
			return jsonElement.Deserialize<T>();
		}

		private static Object ConvertJsonElementValueToValidType(Type objectType, JsonElement jsonElement)
		{
			bool isString = jsonElement.ValueKind is JsonValueKind.String;

			try
			{
				switch (objectType.Name)
				{
					case PrimtiveTypes.BOOL:
						return isString ? Convert.ToBoolean(jsonElement.ToString()) : jsonElement.GetBoolean();

					case PrimtiveTypes.BYTE:
						return isString ? Convert.ToByte(jsonElement.ToString()) : jsonElement.GetByte();

					case PrimtiveTypes.SBYTE:
						return isString ? Convert.ToSByte(jsonElement.ToString()) : jsonElement.GetSByte();

					case PrimtiveTypes.SHORT:
						return isString ? Convert.ToInt16(jsonElement.ToString()) : jsonElement.GetInt16();

					case PrimtiveTypes.USHORT:
						return isString ? Convert.ToUInt16(jsonElement.ToString()) : jsonElement.GetUInt16();

					case PrimtiveTypes.INT:
						return isString ? Convert.ToInt32(jsonElement.ToString()) : jsonElement.GetInt32();

					case PrimtiveTypes.UINT:
						return isString ? Convert.ToUInt32(jsonElement.ToString()) : jsonElement.GetUInt32();

					case PrimtiveTypes.LONG:
						return isString ? Convert.ToInt64(jsonElement.ToString()) : jsonElement.GetInt64();

					case PrimtiveTypes.ULONG:
						return isString ? Convert.ToUInt64(jsonElement.ToString()) : jsonElement.GetUInt64();

					case PrimtiveTypes.INTPTR:
						return IntPtr.Parse(jsonElement.ToString());

					case PrimtiveTypes.UINTPTR:
						return UIntPtr.Parse(jsonElement.ToString());

					case PrimtiveTypes.CHAR:
						return Convert.ToChar(jsonElement.ToString());

					case PrimtiveTypes.FLOAT:
						return isString ? Convert.ToSingle(jsonElement.ToString()) : jsonElement.GetSingle();

					case PrimtiveTypes.DOUBLE:
						return isString ? Convert.ToDouble(jsonElement.ToString()) : jsonElement.GetDouble();

					case PrimtiveTypes.DECIMAL:
						return isString ? Convert.ToDecimal(jsonElement.ToString()) : jsonElement.GetDecimal();

					case PrimtiveTypes.STRING:
						return jsonElement.ToString();

					case PrimtiveTypes.DATETIME:
						return isString ? Convert.ToDateTime(jsonElement.ToString()) : jsonElement.GetDateTime();

					case PrimtiveTypes.GUID:
						return isString ? Guid.Parse(jsonElement.ToString()) : jsonElement.GetGuid();

					// case PrimtiveTypes.ENUM:
					// 	return Enum.Parse(value);

					case PrimtiveTypes.OBJECT:
						return (object)jsonElement;

					default:
						throw Errors.Exception(Errors.InvalidTypeConversion);
				}
			}
			catch (Exception)
			{
				return jsonElement.ToString();
			}
		}

		#endregion JsonElement Conversions
	}
}