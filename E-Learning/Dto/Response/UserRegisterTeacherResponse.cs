using E_Learning.Common;

namespace E_Learning.Dto.Response
{
    public class UserRegisterTeacherResponse
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Expertise { get; set; }
        public double? YearsOfExperience { get; set; }
        public string Bio { get; set; }
        public string FacebookLink { get; set; }
        public string Certificate { get; set; }
        public string CvUrl { get; set; }
        public string RegistrationStatus { get; set; }
        public string Qr { get; set; }
    }
}
