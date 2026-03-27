using ItunesMoviePriceTracker.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace ItunesMoviePriceTracker.Services.Implementation;

public class EmailNotificationService(IConfiguration configuration,
    ILogger<EmailNotificationService> logger) : INotificationService
{
    public async Task SendPriceAlertAsync(string movieTitle, decimal currentPrice, decimal watchPrice)
    {
        var host = configuration["Notifications:SmtpHost"]!;
        var port = configuration.GetValue<int>("Notifications:SmtpPort");
        var user = configuration["Notifications:SmtpUser"]!;
        var password = configuration["Notifications:SmtpPassword"]!;
        var toEmail = configuration["Notifications:ToEmail"]!;

        try
        {
            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(user, password),
                EnableSsl = true
            };

            var message = new MailMessage
            {
                From = new MailAddress(user),
                Subject = $"🎬 Price Alert — {movieTitle}",
                Body = $"""
                Price alert for {movieTitle}!

                Current price: {currentPrice:N2} 
                Your watch price: {watchPrice:N2} 

                The price has dropped below your watch price.
                """,
                IsBodyHtml = false
            };

            message.To.Add(toEmail);

            await client.SendMailAsync(message);

            logger.LogInformation("Price alert email sent for {MovieTitle}. Current price: {CurrentPrice} kr, Watch price: {WatchPrice} kr",
                movieTitle, currentPrice, watchPrice);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send price alert email for {MovieTitle}", movieTitle);            
        }
        
    }
}
