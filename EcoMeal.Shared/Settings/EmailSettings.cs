namespace EcoMeal.Shared.Settings;

public class EmailSettings
{
    public string SmtpServer { get; set; } = "smtp.gmail.com";
    public int Port { get; set; } = 587;
    public string SenderName { get; set; } = "EcoMeal";
    public string SenderEmail { get; set; } = "ecomealapp@gmail.com";
    public string Password { get; set; } = "";
    public bool EnableSsl { get; set; } = true;
}
