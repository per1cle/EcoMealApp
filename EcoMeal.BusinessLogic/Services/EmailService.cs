using System.Net;
using System.Net.Mail;
using EcoMeal.BusinessLogic.Services.Interfaces;
using EcoMeal.Shared.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EcoMeal.BusinessLogic.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> settings, ILogger<EmailService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task SendOrderConfirmedEmailAsync(
        string customerEmail,
        string customerName,
        int orderNumber,
        string businessName,
        string businessAddress,
        double? latitude,
        double? longitude,
        decimal totalAmount,
        List<(string PackageName, int Quantity, decimal Price)> items)
    {
        string mapUrl = (latitude.HasValue && longitude.HasValue)
            ? $"https://maps.google.com/?q={latitude.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)},{longitude.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}"
            : $"https://www.google.com/maps/search/?api=1&query={Uri.EscapeDataString(businessAddress)}";

        string itemsRows = string.Join("", items.Select(i => $@"
            <tr>
                <td style=""padding: 10px 12px; border-bottom: 1px solid #e2e8f0; color: #334155;"">{i.PackageName}</td>
                <td style=""padding: 10px 12px; border-bottom: 1px solid #e2e8f0; text-align: center; color: #334155;"">{i.Quantity}</td>
                <td style=""padding: 10px 12px; border-bottom: 1px solid #e2e8f0; text-align: right; color: #0f172a; font-weight: 600;"">{(i.Price * i.Quantity):0.00} RON</td>
            </tr>"));

        string htmlBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"" />
    <style>
        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 0; padding: 0; background-color: #f8fafc; }}
        .container {{ max-width: 600px; margin: 20px auto; background-color: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.08); border: 1px solid #e2e8f0; }}
        .header {{ background: linear-gradient(135deg, #10b981 0%, #059669 100%); color: #ffffff; padding: 30px 24px; text-align: center; }}
        .header h1 {{ margin: 0; font-size: 26px; font-weight: 700; letter-spacing: -0.5px; }}
        .header p {{ margin: 8px 0 0 0; opacity: 0.9; font-size: 15px; }}
        .content {{ padding: 28px 24px; color: #334155; line-height: 1.6; }}
        .badge {{ display: inline-block; background-color: #ecfdf5; color: #059669; font-weight: 700; padding: 6px 14px; border-radius: 9999px; font-size: 14px; margin-bottom: 16px; border: 1px solid #a7f3d0; }}
        .card {{ background-color: #f8fafc; border-radius: 8px; padding: 18px; margin: 20px 0; border: 1px solid #e2e8f0; }}
        .btn {{ display: inline-block; background-color: #10b981; color: #ffffff !important; text-decoration: none; padding: 12px 24px; border-radius: 8px; font-weight: 600; font-size: 15px; margin-top: 12px; box-shadow: 0 2px 6px rgba(16, 185, 129, 0.3); }}
        .table {{ width: 100%; border-collapse: collapse; margin-top: 12px; font-size: 14px; }}
        .table th {{ background-color: #f1f5f9; padding: 10px 12px; text-align: left; font-weight: 600; color: #475569; }}
        .footer {{ text-align: center; padding: 20px; font-size: 12px; color: #94a3b8; border-top: 1px solid #f1f5f9; }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h1>🌱 EcoMeal</h1>
            <p>Rescuing good food together!</p>
        </div>
        <div class=""content"">
            <span class=""badge"">✓ Order Confirmed</span>
            <h2 style=""margin-top: 0; color: #0f172a;"">Hello, {customerName}!</h2>
            <p>Your order <strong>#{orderNumber}</strong> has been confirmed by the vendor and is being prepared for pickup.</p>
            
            <div class=""card"">
                <h3 style=""margin: 0 0 10px 0; color: #0f172a; font-size: 16px;"">📍 Pickup Location</h3>
                <p style=""margin: 4px 0; font-size: 15px; font-weight: 700; color: #1e293b;"">{businessName}</p>
                <p style=""margin: 4px 0; color: #64748b;"">{businessAddress}</p>
                <a href=""{mapUrl}"" target=""_blank"" class=""btn"">🗺️ View Location on Map</a>
            </div>

            <h3 style=""margin: 24px 0 10px 0; color: #0f172a; font-size: 16px;"">📦 Ordered Packages</h3>
            <table class=""table"">
                <thead>
                    <tr>
                        <th>Package</th>
                        <th style=""text-align: center;"">Quantity</th>
                        <th style=""text-align: right;"">Price</th>
                    </tr>
                </thead>
                <tbody>
                    {itemsRows}
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan=""2"" style=""padding: 12px; font-weight: 700; color: #0f172a; text-align: right;"">Total:</td>
                        <td style=""padding: 12px; font-weight: 700; color: #10b981; font-size: 16px; text-align: right;"">{totalAmount:0.00} RON</td>
                    </tr>
                </tfoot>
            </table>

            <p style=""margin-top: 24px; font-size: 14px; color: #64748b;"">Please arrive at the store within the scheduled pickup window.</p>
        </div>
        <div class=""footer"">
            <p>© {DateTime.UtcNow.Year} EcoMeal. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";

        await SendEmailAsync(customerEmail, $"[EcoMeal] Order #{orderNumber} has been confirmed!", htmlBody);
    }

    public async Task SendOrderCompletedEmailAsync(
        string customerEmail,
        string customerName,
        int orderNumber,
        string businessName,
        string businessAddress,
        decimal totalAmount)
    {
        string htmlBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"" />
    <style>
        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 0; padding: 0; background-color: #f8fafc; }}
        .container {{ max-width: 600px; margin: 20px auto; background-color: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.08); border: 1px solid #e2e8f0; }}
        .header {{ background: linear-gradient(135deg, #10b981 0%, #059669 100%); color: #ffffff; padding: 30px 24px; text-align: center; }}
        .header h1 {{ margin: 0; font-size: 26px; font-weight: 700; }}
        .content {{ padding: 28px 24px; color: #334155; line-height: 1.6; }}
        .badge {{ display: inline-block; background-color: #ecfdf5; color: #059669; font-weight: 700; padding: 6px 14px; border-radius: 9999px; font-size: 14px; margin-bottom: 16px; border: 1px solid #a7f3d0; }}
        .card {{ background-color: #f0fdf4; border-radius: 8px; padding: 18px; margin: 20px 0; border: 1px solid #bbf7d0; text-align: center; }}
        .footer {{ text-align: center; padding: 20px; font-size: 12px; color: #94a3b8; border-top: 1px solid #f1f5f9; }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h1>🌱 EcoMeal</h1>
            <p>Thank you for fighting against food waste!</p>
        </div>
        <div class=""content"">
            <span class=""badge"">✓ Order Completed</span>
            <h2 style=""margin-top: 0; color: #0f172a;"">Hello, {customerName}!</h2>
            <p>Your order <strong>#{orderNumber}</strong> from <strong>{businessName}</strong> has been marked as <strong>completed</strong>.</p>
            
            <div class=""card"">
                <h3 style=""margin: 0 0 8px 0; color: #166534;"">🎉 Enjoy your meal!</h3>
                <p style=""margin: 0; color: #15803d; font-size: 15px;"">You saved delicious food today and helped protect the environment.</p>
            </div>

            <p style=""color: #64748b; font-size: 14px;"">Total paid: <strong>{totalAmount:0.00} RON</strong></p>
            <p style=""color: #64748b; font-size: 14px;"">Location: {businessName}, {businessAddress}</p>
        </div>
        <div class=""footer"">
            <p>© {DateTime.UtcNow.Year} EcoMeal. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";

        await SendEmailAsync(customerEmail, $"[EcoMeal] Order #{orderNumber} completed successfully!", htmlBody);
    }

    public async Task SendOrderCancelledEmailAsync(
        string customerEmail,
        string customerName,
        int orderNumber,
        string businessName,
        string? reason = null)
    {
        string reasonBlock = !string.IsNullOrWhiteSpace(reason)
            ? $"<p style=\"color: #b91c1c;\"><strong>Reason:</strong> {reason}</p>"
            : "";

        string htmlBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"" />
    <style>
        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 0; padding: 0; background-color: #f8fafc; }}
        .container {{ max-width: 600px; margin: 20px auto; background-color: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.08); border: 1px solid #e2e8f0; }}
        .header {{ background: linear-gradient(135deg, #ef4444 0%, #dc2626 100%); color: #ffffff; padding: 30px 24px; text-align: center; }}
        .header h1 {{ margin: 0; font-size: 26px; font-weight: 700; }}
        .content {{ padding: 28px 24px; color: #334155; line-height: 1.6; }}
        .badge {{ display: inline-block; background-color: #fef2f2; color: #dc2626; font-weight: 700; padding: 6px 14px; border-radius: 9999px; font-size: 14px; margin-bottom: 16px; border: 1px solid #fecaca; }}
        .card {{ background-color: #fff1f2; border-radius: 8px; padding: 18px; margin: 20px 0; border: 1px solid #fecdd3; }}
        .footer {{ text-align: center; padding: 20px; font-size: 12px; color: #94a3b8; border-top: 1px solid #f1f5f9; }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h1>🌱 EcoMeal</h1>
            <p>Order Update</p>
        </div>
        <div class=""content"">
            <span class=""badge"">✕ Order Cancelled</span>
            <h2 style=""margin-top: 0; color: #0f172a;"">Hello, {customerName},</h2>
            <p>We are sorry to inform you that your order <strong>#{orderNumber}</strong> from <strong>{businessName}</strong> has been cancelled.</p>
            
            <div class=""card"">
                {reasonBlock}
                <p style=""margin: 0; color: #475569; font-size: 14px;"">If you have any questions, please reach out to the store or our support team.</p>
            </div>

            <p style=""color: #64748b; font-size: 14px;"">Feel free to explore other active surprise packages available on the EcoMeal shop.</p>
        </div>
        <div class=""footer"">
            <p>© {DateTime.UtcNow.Year} EcoMeal. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";

        await SendEmailAsync(customerEmail, $"[EcoMeal] Order #{orderNumber} has been cancelled", htmlBody);
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        if (string.IsNullOrWhiteSpace(toEmail))
        {
            _logger.LogWarning("Cannot send email: recipient address is empty.");
            return;
        }

        try
        {
            using var mailMessage = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            using var smtpClient = new SmtpClient(_settings.SmtpServer, _settings.Port)
            {
                EnableSsl = _settings.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            if (!string.IsNullOrWhiteSpace(_settings.Password))
            {
                smtpClient.Credentials = new NetworkCredential(_settings.SenderEmail, _settings.Password);
            }

            await smtpClient.SendMailAsync(mailMessage);
            _logger.LogInformation("Email sent successfully to {ToEmail} with subject '{Subject}'.", toEmail, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {ToEmail}. Please check SMTP configuration.", toEmail);
        }
    }
}
