using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MovieNight.Services;

public class EmailSender : IEmailSender
{
    public IConfiguration Configuration { get; }
    public EmailSender(IConfiguration configuration)
    {
        Configuration = configuration;
    } 
    
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        using (MailMessage mm = new MailMessage(Configuration["NetMail:sender"], email))
        {
            mm.Subject = subject;
            string body = htmlMessage;
            mm.Body = body;
            mm.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = Configuration["NetMail:smtpHost"];
            smtp.EnableSsl = true;
            NetworkCredential networkCred = new NetworkCredential(Configuration["NetMail:sender"], 
                Configuration["NetMail:senderpassword"]);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = networkCred;
            smtp.Port = 587;
            await smtp.SendMailAsync(mm);
            mm.Dispose();
        }
    } 
}