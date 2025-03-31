using E_Learning.Common;
using System.Text.Json.Serialization;

namespace E_Learning.Dto.Request
{
    public class PeriodTypeRequest
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PeriodType PeriodType { get; set; }
    }

}
