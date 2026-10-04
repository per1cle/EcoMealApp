namespace EcoMeal.BusinessLogic.Services.Interfaces;

public interface IEmailService
{
    Task SendOrderConfirmedEmailAsync(
        string customerEmail,
        string customerName,
        int orderNumber,
        string businessName,
        string businessAddress,
        double? latitude,
        double? longitude,
        decimal totalAmount,
        List<(string PackageName, int Quantity, decimal Price)> items);

    Task SendOrderCompletedEmailAsync(
        string customerEmail,
        string customerName,
        int orderNumber,
        string businessName,
        string businessAddress,
        decimal totalAmount);

    Task SendOrderCancelledEmailAsync(
        string customerEmail,
        string customerName,
        int orderNumber,
        string businessName,
        string? reason = null);

    Task SendEmailAsync(string toEmail, string subject, string htmlBody);
}
