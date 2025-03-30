using Newtonsoft.Json;

namespace E_Learning.Dto.Response.Admin
{
    public class AdminCourseResponse
    {
        public long Id { get; set; }
        public long AuthorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public string AuthorName { get; set; }
        public string Language { get; set; }
        public string Level { get; set; }
        public int Duration { get; set; }
        public string Thumbnail { get; set; }

        [JsonProperty("createdAt")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? UpdatedAt { get; set; }
    }

    // Optional: Custom DateTime converter if needed
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
