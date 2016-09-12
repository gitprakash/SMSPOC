using DataModelLibrary;
using DataServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SubscriberProessor.Consumer
{
    public class EmailService : IEmailService
    {
        public ISubscriberService msubscriberService;

        public static string host
        {
            get
            {
                return ConfigUtility.GetAppkeyvalues("host");
            }
        }

        public static string smtpUName
        {
            get
            {
               return ConfigUtility.GetAppkeyvalues("from");
            }
        }
        public static string smtpUNamePwd
        {
            get
            {
                return ConfigUtility.GetAppkeyvalues("password");
            }
        }
        public static int port
        {
            get
            {
                return Convert.ToInt32(ConfigUtility.GetAppkeyvalues("port"));
            }
        }
        public static string smsadminemail
        {
            get
            {
                return ConfigUtility.GetAppkeyvalues("smsadminemail");
            }
        }
        public static string adminmail
        {
            get
            {
                return ConfigUtility.GetAppkeyvalues("adminmail");
            }
        }
        public EmailService(ISubscriberService subscriberService)
        {
            msubscriberService = subscriberService;
        }

        private MailMessage getmailmessage(string subject, string body)
        {
            var mailmsg = new MailMessage();
            mailmsg.To.Add(smsadminemail);
            mailmsg.CC.Add(adminmail);
            mailmsg.From = new MailAddress(smtpUName);
            mailmsg.Subject = subject;
            mailmsg.IsBodyHtml = true;
           mailmsg.Body =body;
            return mailmsg;
        }
        private void ReadAgreementFile(string filepath)
        {
        }
        public async Task SendAgreementFormMail(SubscriberSavedMessage ssm)
        {
            string body = await getSubscriberInfo(ssm.Id);
            var mailmsg = getmailmessage("Need sender Id for addressed subscriber", body);
            mailmsg.Attachments.Add(new Attachment(ssm.Agreementfilename));
            await SendMail(mailmsg);
        }
        private async Task<string> getSubscriberInfo(int subscriberId)
        {
            var subscriber = await msubscriberService.Subscriber(subscriberId);
            string text =  "Dear Service Provider</br>Please find the attachement, and verify subscriber details</br>";
              text =string.Format( "<table><tr><td>First Name</td><td>Last Name </td><td>Email </td></td><td>Mobilee</td></tr><tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr></table>", subscriber.FirstName,subscriber.LastName,subscriber.Email,subscriber.Mobile);
            text = text + "</br>Thanks</br>Admin Team";
            return text;
        }

        public async Task SendMail(MailMessage mailmsg)
        {
            var smtpclient = new SmtpClient(host, port);
            smtpclient.EnableSsl = true;
            smtpclient.UseDefaultCredentials = true;
            //smtpClient.TargetName = "STARTTLS/smtp.office365.com";
            var credentials = new  NetworkCredential(smtpUName, smtpUNamePwd);
            smtpclient.Credentials = credentials;
            // smtpclient.UseDefaultCredentials = false; 
            await smtpclient.SendMailAsync(mailmsg);
            Console.WriteLine("Mail Send successfully");
            mailmsg.Dispose();
            smtpclient.Dispose(); 
        }
    }
}
