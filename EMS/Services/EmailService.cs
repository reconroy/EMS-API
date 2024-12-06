using System.Net.Mail;
using System.Net;
using EMS.Data;

namespace EMS.Services
{
    public class EmailService
    {
        private readonly EMSDbContext _context;
        private readonly IConfiguration _configuration;

        public EmailService(EMSDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string SendEmail(string to, string subject, string body)
        {
            try
            {
                string senderEmail = _configuration["EmailSettings:Email"] 
                    ?? throw new InvalidOperationException("Email address not configured");
                    
                string senderPassword = _configuration["EmailSettings:Password"]
                    ?? throw new InvalidOperationException("Email password not configured");
                    
                string smtpServer = _configuration["EmailSettings:Host"]
                    ?? throw new InvalidOperationException("SMTP host not configured");
                    
                int smtpPort = _configuration.GetValue<int>("EmailSettings:Port");
                if (smtpPort <= 0)
                {
                    smtpPort = 587; // Default to standard TLS port if not configured
                }

                var smtpClient = new SmtpClient(smtpServer)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(to);
                
                smtpClient.Send(mailMessage);
                return "Email sent successfully";
            }
            catch (Exception ex)
            {
                // Log the full exception details for debugging
                Console.WriteLine($"Email sending failed: {ex}");
                return $"Failed to send email: {ex.Message}";
            }
        }
    }
}
