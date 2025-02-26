namespace E_Learning.Models.Response
{
    public class SignInResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public long UserId { get; set; }

        public SignInResponse(string accessToken, string refreshToken, long UserId)
        {
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
            this.UserId = UserId;
        }
    }
}
