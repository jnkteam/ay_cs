namespace OriginalStudio.WebComponents
{
    using OriginalStudio.BLL;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Text;

    public class EmailHelper
    {
        private MailMessage mail;

        public EmailHelper(string from, string to, string subject, string body, bool isHtml, Encoding encoding)
        {
            if (string.IsNullOrEmpty(from))
            {
                from = SysConfig.MailFrom;
            }
            this.mail = new MailMessage(new MailAddress(from, SysConfig.MailDisplayName), new MailAddress(to));
            this.mail.Subject = subject;
            this.mail.Body = body;
            this.mail.BodyEncoding = encoding;
            this.mail.IsBodyHtml = isHtml;
            this.mail.SubjectEncoding = encoding;
        }

        public bool Send()
        {
            bool flag = false;
            try
            {                
                string mailDomain = SysConfig.MailDomain;
                int mailDomainPort = SysConfig.MailDomainPort;
                string mailServerUserName = SysConfig.MailServerUserName;
                string mailServerPassWord = SysConfig.MailServerPassWord;
                if (!(((string.IsNullOrEmpty(mailDomain) || (mailDomainPort <= 0)) || string.IsNullOrEmpty(mailServerUserName)) || string.IsNullOrEmpty(mailServerPassWord)))
                {
                    SmtpClient client = new SmtpClient(mailDomain, mailDomainPort);
                    client.Credentials = new NetworkCredential(mailServerUserName, mailServerPassWord);
                    client.EnableSsl = SysConfig.MailIsSsl == "1";
                    client.Send(this.mail);
                    flag = true;
                }
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
            }
            return flag;
        }

        public void Send2()
        {
            try
            {
                string mailDomain = SysConfig.MailDomain;
                int mailDomainPort = SysConfig.MailDomainPort;
                string mailServerUserName = SysConfig.MailServerUserName;
                string mailServerPassWord = SysConfig.MailServerPassWord;
                if (((!string.IsNullOrEmpty(mailDomain) && (mailDomainPort > 0)) && !string.IsNullOrEmpty(mailServerUserName)) && !string.IsNullOrEmpty(mailServerPassWord))
                {
                    SmtpClient client = new SmtpClient(mailDomain, mailDomainPort);
                    client.Credentials = new NetworkCredential(mailServerUserName, mailServerPassWord);
                    client.EnableSsl = SysConfig.MailIsSsl == "1";
                    client.Send(this.mail);
                }
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
            }
        }
    }
}

