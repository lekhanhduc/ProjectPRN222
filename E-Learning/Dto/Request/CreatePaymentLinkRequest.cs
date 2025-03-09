namespace E_Learning.Dto.Request
{
    public record CreatePaymentLinkRequest(
        int orderCode,
        int courseId,
        string productName,
        string description,
        int price,
        string returnUrl,
        string cancelUrl
    );
}
