
namespace E_Learning.Dto.Request
{
    public class UserRegisterTeacherRequest
    {

        public string Phone { get; set; }

        public string Expertise { get; set; }

        public double YearsOfExperience { get; set; }

        public string Bio { get; set; }
        public string FacebookLink { get; set; }

        public IFormFile Cv { get; set; }

        public IFormFile Certificate{ get; set; }

        public IFormFile Qr { get; set; }
    }
}
