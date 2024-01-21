using System.Globalization;
using System.Text.Json;

namespace MT5WebAPI.Common.Utils
{
    public static class ConvertHelper
    {
        private static NumberFormatInfo GetNumberFormatInfo()
        {
            return new NumberFormatInfo { NumberDecimalSeparator = "." };
        }
        public static T TypeConversation<T>(object obj)
        {
            try
            {
                if (Convert.IsDBNull(obj))
                {
                    return default;
                }

                Type targetType = typeof(T);

                if (obj is JsonElement jsonElement)
                {
                    if (targetType == typeof(double))
                    {
                        if (jsonElement.ValueKind == JsonValueKind.Number)
                        {
                            if (jsonElement.TryGetDouble(out double jsonDouble))
                            {
                                return (T)(object)jsonDouble;
                            }
                        }
                        else if (jsonElement.ValueKind == JsonValueKind.String && double.TryParse(jsonElement.GetString(), out double parsedValue))
                        {
                            return (T)(object)parsedValue;
                        }
                    }
                }

                if (targetType == typeof(byte))
                {
                    return (T)(object)Convert.ToByte(obj);
                }
                else if (targetType == typeof(short))
                {
                    return (T)(object)Convert.ToInt16(obj);
                }
                else if (targetType == typeof(short?))
                {
                    return (T)(object)(short?)Convert.ToInt16(obj);
                }
                else if (targetType == typeof(int))
                {
                    return (T)(object)Convert.ToInt32(obj);
                }
                else if (targetType == typeof(int?))
                {
                    return (T)(object)(int?)Convert.ToInt32(obj);
                }
                else if (targetType == typeof(long))
                {
                    return (T)(object)Convert.ToInt64(obj);
                }
                else if (targetType == typeof(long?))
                {
                    return (T)(object)(long?)Convert.ToInt64(obj);
                }
                else if (targetType == typeof(uint))
                {
                    return (T)(object)Convert.ToUInt32(obj);
                }
                else if (targetType == typeof(ulong))
                {
                    return (T)(object)Convert.ToUInt64(obj);
                }
                else if (targetType == typeof(ulong?))
                {
                    return (T)(object)(ulong?)Convert.ToUInt64(obj);
                }
                else if (targetType == typeof(double))
                {
                    return (T)(object)Convert.ToDouble(obj, GetNumberFormatInfo());
                }
                else if (targetType == typeof(float))
                {
                    return (T)(object)Convert.ToSingle(obj, GetNumberFormatInfo());
                }
                else if (targetType == typeof(double?))
                {
                    return (T)(object)(double?)Convert.ToDouble(obj, GetNumberFormatInfo());
                }
                else if (targetType == typeof(string))
                {
                    return (T)(object)Convert.ToString(obj);
                }
                else if (targetType == typeof(decimal))
                {
                    return (T)(object)Convert.ToDecimal(obj);
                }
                else if (targetType == typeof(decimal?))
                {
                    return (T)(object)(decimal?)Convert.ToDecimal(obj);
                }
                else if (targetType == typeof(bool))
                {
                    return (T)(object)Convert.ToBoolean(obj);
                }
                else if (targetType == typeof(bool?))
                {
                    return (T)(object)(bool?)Convert.ToBoolean(obj);
                }
                else if (targetType == typeof(byte[]))
                {
                    return (T)(object)(Convert.IsDBNull(obj) ? null : (byte[])obj);
                }
                else if (targetType == typeof(DateTime))
                {
                    return (T)(object)Convert.ToDateTime(obj);
                }
                else if (targetType == typeof(DateTime?))
                {
                    return (T)(object)(DateTime?)Convert.ToDateTime(obj);
                }

                throw new InvalidOperationException($"Unsupported type: {targetType.Name}");
            }
            catch (Exception exc)
            {
                Console.Write(exc.Message.ToString());
                throw;
            }
        }
    }

}
