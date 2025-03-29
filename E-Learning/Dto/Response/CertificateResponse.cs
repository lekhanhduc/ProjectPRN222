using Newtonsoft.Json;

namespace E_Learning.Dto.Response
{
    public class CertificateResponse
    {
        [JsonProperty("certificateId")]
        public long CertificateId { get; set; }

        [JsonProperty("courseName")]
        public string? CourseName { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("username")]
        public string? Username { get; set; }

        [JsonProperty("author")]
        public string? Author { get; set; }

        [JsonProperty("issueDate")]
        public DateTime IssueDate { get; set; }

        [JsonProperty("certificateUrl")]
        public string? CertificateUrl { get; set; }
    }
}
