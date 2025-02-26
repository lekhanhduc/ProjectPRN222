namespace E_Learning.Dto.Response
{
    public class VerificationResponse
    {
        bool Success { get; set; }

        public VerificationResponse(bool Success)
        {
            this.Success = Success;
        }
    }
}
