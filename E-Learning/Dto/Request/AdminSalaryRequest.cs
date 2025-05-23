﻿using Newtonsoft.Json;

namespace E_Learning.Dto.Request
{
    public class AdminSalaryRequest
    {
        public long? AuthorId { get; set; }

        public decimal Money { get; set; }


        [JsonProperty("createdAt")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? CreatedAt { get; set; }

        public class CustomDateTimeConverter : JsonConverter<DateTime>
        {
            public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.String)
                {
                    var dateString = reader.Value.ToString();
                    return DateTime.ParseExact(dateString, "dd/MM/yyyy HH:mm:ss", null);
                }
                return DateTime.MinValue;
            }

            public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString("dd/MM/yyyy HH:mm:ss"));
            }
        }
    }
}
