using System.Net.Mail;
namespace PPIJ
{
    [System.Web.Services.WebMethod]
        public class SendMessage{
        public static void SendMessage(string name, string fromEmail, string subject, string message)
        {
        const string SERVER = "smtp.gmail.com";
               const string TOEMAIL = "codebistro15@gmail.com";
               MailAddress from = new MailAddress(fromEmail);
               MailAddress to = new MailAddress(TOEMAIL);
               MailMessage message = new MailMessage(from, to);

               message.Subject = subject;
               message.Body = "Message from: " + name + " at " + 
                              fromEmail + "\n\n" + message;
               message.IsBodyHtml = false;
               SmtpClient client = new SmtpClient(SERVER);
               client.Send(message);
         }
    }
}
