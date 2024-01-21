namespace MT5WebAPI.Common.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public abstract class CustomJsonConverter<T> : JsonConverter<T>
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected start of object");

            var dictionary = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(ref reader, options);

            if (dictionary == null)
                return default; // or throw an exception, depending on your requirements

            return Parse(dictionary);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            // Implement serialization logic if needed
            throw new NotImplementedException();
        }

        protected abstract T Parse(Dictionary<string, JsonElement> dictionary);
    }

}
