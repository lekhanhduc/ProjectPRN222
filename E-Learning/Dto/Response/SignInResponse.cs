namespace E_Learning.Models.Response
{
    public class SignInResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int UserId { get; set; }

        public SignInResponse(string accessToken, string refreshToken, int UserId)
        {
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
            this.UserId = UserId;
        }
    }
}
