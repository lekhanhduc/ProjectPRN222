namespace E_Learning.Dto.Response
{
    public record PaymentResponse(
        int error,
        String message,
        object? data
     );
    
}
