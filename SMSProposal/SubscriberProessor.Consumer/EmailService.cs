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

        private MailMessage getmailmessage()
        {
            var mailmsg = new MailMessage();
            mailmsg.To.Add(smsadminemail);
            mailmsg.CC.Add(adminmail);
            mailmsg.From = new MailAddress(smtpUName);
            mailmsg.Subject = "Need sender Id for addressed subscriber";
            mailmsg.IsBodyHtml = true;
            mailmsg.Body = "Please find the attachement, and verify subscriber details";
            return mailmsg;
        }
        private void ReadAgreementFile(string filepath)
        {
        }
        public async Task SendAgreementFormMail(SubscriberSavedMessage ssm)
        {
            var mailmsg = getmailmessage();
            ReadAgreementFile(ssm.Agreementfilename);
            mailmsg.Attachments.Add(new Attachment(@"C:\prakash rajendran\Learning\PrakashGit\SMSPOC\SMSProposal\SMSPOCWeb\Content\Upload\krishuser_636092707358679564_Agreement PDF.pdf"));
            await SendMail(mailmsg);
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
