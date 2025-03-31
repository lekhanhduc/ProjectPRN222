using E_Learning.Common;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.Dto.Response
{
    public class AdsCreationResponse
    {
        public long Id { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhone { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string Location { get; set; }

        public string Link { get; set; }

        [JsonProperty("startDate")]
        public DateOnly StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateOnly EndDate { get; set; }

        public decimal? PriceAds { get; set; } 

        public AdsStatus Status { get; set; }

        [JsonProperty("createAt")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
        public DateTime CreateAt { get; set; }
    }
}
