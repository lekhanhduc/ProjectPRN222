namespace E_Learning.Servies
{
    public interface IEmailService
    {
        Task SendEmailVerification(string to, string name, string verificationLink, string otp);
        Task SendEmailBuyCourse(string to, string name, string courseName, string courseThumbnail);
    }
}
