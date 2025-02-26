namespace E_Learning.Servies
{
    public interface IRedisService
    {
        string GetOtp(string email);
        void DeleteOtp(string email);
        void SaveOtp(string email, string otp, TimeSpan timeSpan);
    }
}
