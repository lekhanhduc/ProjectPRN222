using Newtonsoft.Json;

namespace E_Learning.Dto.Response
{
    public class ReviewResponse
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("avatar")]
        public string? Avatar { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("rating")]
        public int? Rating { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }


        [JsonProperty("replies")]
        public List<ReviewResponse> Replies { get; set; } = new List<ReviewResponse>();
    }
}
