using System;
using System.ComponentModel.DataAnnotations;

namespace E_Learning.Dto.Request
{
    public class AdsCreationRequest
    {
        public string ContactEmail { get; set; }

        public string ContactPhone { get; set; }

        public string Title { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        [DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateOnly EndDate { get; set; }

        public IFormFile Image { get; set; }
    }
}
