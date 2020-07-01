using System;
using System.Net.Mail;
using System.Windows;

namespace Listen2Html
{
    static class UserInfo
    {
        public static string EmailServer;
        public static string UserName;
        public static string Password;
        public static string ToAddress;

        //TODO: Async me
        public static void SendTestEmail()
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(EmailServer);

                mail.From = new MailAddress(ToAddress);
                mail.To.Add(ToAddress);
                mail.Subject = "Listen2Html - Test Mail";
                mail.Body = "This is a test email from Listen2Html - if you see this, your credentials have worked!";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(UserName, Password);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                MessageBox.Show("Mail Sent Succesfully!" + "\n\nIf you do not see it in your inbox, please check your spam folders");
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was a problem sending a test message.  Please check your credentials and verify that your provider allows third party connections to it's SMTP server.\n\n" +
                    "Details: \n\n" + ex.ToString());
            }
        }

        //TODO: Send newhtml and oldhtml as attachments
        //TODO: Async me
        public static void SendEmail(string newhtml, string oldhtml, string url)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(EmailServer);

                mail.From = new MailAddress(ToAddress);
                mail.To.Add(ToAddress);
                mail.Subject = $"Listen2Html - Changes Detected on {url}";
                mail.Body = $"A Change has been detected on {url}!  Please review the text below for changes:  \n\n PREVIOUS HTML:\n {oldhtml} \n\n NEW HTML: \n {newhtml}";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(UserName, Password);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was a problem sending a change alert email.  Please check your credentials and verify that your provider allows connections to it's SMTP server.\n\n" +
                    "Details: \n\n" + ex.ToString());
            }
        }
    }
}
