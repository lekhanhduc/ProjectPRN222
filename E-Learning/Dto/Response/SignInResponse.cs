namespace E_Learning.Models.Response
{
    public class SignInResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }

        public SignInResponse(string accessToken, string refreshToken, string role)
        {
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
            Role = role;
        }
    }
}
