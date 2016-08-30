using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModelLibrary;
using Repositorylibrary;
using System.Linq.Expressions;

namespace DataServiceLibrary
{
    public class MessageService : IMessageService
    {
        private IGenericRepository<SubscriberContactMessage> subcribermessageRepository;
        private IGenericRepository<SubscriberMessageBalance> msubscriberMessageBalance;
        private IGenericRepository<SubscriberMessageBalanceHistory> msubscriberMessageBalanceHistory;

        public MessageService(IGenericRepository<SubscriberContactMessage> messageRepository,
            IGenericRepository<SubscriberMessageBalance> subscriberMessageBalance,
             IGenericRepository<SubscriberMessageBalanceHistory> subscriberMessageBalanceHistory
             )
        {
            subcribermessageRepository = messageRepository;
            msubscriberMessageBalance = subscriberMessageBalance;
            msubscriberMessageBalanceHistory = subscriberMessageBalanceHistory;
        }

        

        public async Task<List<MessageViewModel>> SubmitMessage(List<MessageViewModel> messageViewModel, string message, int SubscriberId)
        {
            ExternalMessageServiceAPI smsserviceAPI = new ExternalMessageServiceAPI();
            var msgsubmiturl = ExternalMessageServiceAPI.SubmitMessageApiformaturl(); 
            foreach (var smvm in messageViewModel)
            {
                try 
                {
                    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                    sw.Start();
                    System.Diagnostics.Debug.WriteLine(string.Format("message starting to submit on {0} to mobile {1} ", DateTime.Now, smvm.Mobile));
                    string tempmsgsubmiturl = string.Format(msgsubmiturl, smvm.Mobile, message);
                    smvm.Submittrequesttime = DateTime.Now;
                    string submitid = await smsserviceAPI.SubmitMessage(tempmsgsubmiturl);
                    smvm.Submittresponsetime = DateTime.Now;
                    System.Diagnostics.Debug.WriteLine(string.Format("Got message submit response on {0} to mobile {1} and submitid {2}", sw.ElapsedMilliseconds,
                       smvm.Mobile, submitid));
                    var status = smsserviceAPI.IsMessageSubmitted(submitid);
                    smvm.IsMessageSubmitted = (!status);
                    smvm.SubmitId = submitid; 
                }
                catch (Exception ex)
                {
                    smvm.Submittresponsetime = DateTime.Now;
                    smvm.IsMessageSubmitted = false;
                    smvm.MessageError = ex.Message;
                }
            }
           return messageViewModel;
        }

        public async Task<List<MessageViewModel>> GetMessageStatus(List<MessageViewModel> messageViewModel, string message,
          int messagecount, int SubscriberId)
        {
            ExternalMessageServiceAPI smsserviceAPI = new ExternalMessageServiceAPI();
            var msgStatusurl = ExternalMessageServiceAPI.GetMessageDeliveryReportUrl();
            foreach (var smvm in messageViewModel.Where(mvm=>mvm.IsMessageSubmitted))
            {
                try 
                {
                    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                    sw.Start();
                    string tempmsgStatusurl = string.Format(msgStatusurl, smvm.SubmitId);
                    System.Diagnostics.Debug.WriteLine(string.Format("starting to get delivery status on {0} to mobile {1} ", sw.ElapsedMilliseconds, smvm.Mobile));
                    smvm.DeliveryRequestedtime = DateTime.Now;
                    var deliverystatusid = await smsserviceAPI.GetMessageStatus(tempmsgStatusurl);
                    smvm.DeliveryResponsetime = DateTime.Now; 
                    System.Diagnostics.Debug.WriteLine(string.Format("Got delivery response on {0} to mobile {1} and deliveryid {2} ",
                        sw.ElapsedMilliseconds, smvm.Mobile, deliverystatusid));
                    smvm.IsMessageDelivered = deliverystatusid == "DELIVRD" ? true :false;
                    smvm.DeliveryId = deliverystatusid; 
                }
                catch (Exception ex)
                {
                    smvm.DeliveryResponsetime = DateTime.Now;
                    smvm.IsMessageDelivered = false;
                    smvm.MessageError = ex.Message;
                } 
            }
            var logmsgrestult = await LogAllMessage(messageViewModel, message, messagecount, SubscriberId);
            return logmsgrestult;
        }


        public async Task<List<MessageViewModel>> LogAllMessage(List<MessageViewModel> messageViewModel, string message,
            int messagecount, int SubscriberId)
        { 
            var subcribermessage = CreateMessage(message, messagecount);
            List<SubscriberContactMessage> lstmessages = new List<SubscriberContactMessage>();
            var lstsubsribedmsg = (from mvm in messageViewModel
                                   select new SubscriberContactMessage
                                   {
                                       CreatedAt = DateTime.Now,
                                       Guid = Guid.NewGuid(),
                                       Message = subcribermessage,
                                       MessageStatus = mvm.IsMessageDelivered ? MessageStatusEnum.Delivered :
                                       mvm.DeliveryId == "PENDING" ? MessageStatusEnum.Pending : MessageStatusEnum.NotSent,
                                       SubscriberStandardContactsId = mvm.Id,
                                       SubmitmitId = mvm.SubmitId,
                                       DeliveryId = mvm.DeliveryId,
                                       Submittrequesttime = mvm.Submittrequesttime,
                                       Submittresponsetime = mvm.Submittresponsetime,
                                       DeliveryRequestedtime = mvm.DeliveryRequestedtime,
                                       DeliveryResponsetime = mvm.DeliveryResponsetime,
                                       MessageError = string.IsNullOrEmpty(mvm.MessageError) == false ? new MessageError
                                       {
                                           Guid = Guid.NewGuid(),
                                           CreatedAt = DateTime.Now,
                                           Text = mvm.MessageError
                                       } : null
                                   }).AsParallel().ToList();
            subcribermessageRepository.AddRangeAsyncWithtransaction(lstsubsribedmsg);
            if (messageViewModel.Any(mvm => mvm.IsMessageDelivered == true))
            {
               int dbupdated= await UpdateMessageBalance(messageViewModel, messagecount, SubscriberId);
               if (dbupdated <= 0)
                   throw new Exception("Problem in update Message balance");
            }
            //whole commit ,log msg into db and update msg balance details.
            await subcribermessageRepository.SaveAsync(); 
            return messageViewModel;
        }

        private static Message CreateMessage(string message, int messagecount)
        {
            var subcribermessage = new Message
            {
                Text = message,
                Guid = Guid.NewGuid(),
                MessageCount = messagecount,
                CreatedAt = DateTime.Now,
            };
            return subcribermessage;
        }

        public async Task<int> UpdateMessageBalance(List<MessageViewModel> messageViewModel, int messagecount, int subscriberId)
        {
            int sentmsgcount = messageViewModel.Count(mvm => mvm.IsMessageDelivered == true);
            sentmsgcount = sentmsgcount * messagecount;
            return await UpdateSubscirberMessageBalance(subscriberId, sentmsgcount);
        }

        private async Task<int> UpdateSubscirberMessageBalance(int subscriberId, int sentmsgcount)
        {
            var userMsgBalance = await msubscriberMessageBalance.FindAsync(smb => smb.SubcriberId == subscriberId);
            userMsgBalance.RemainingCount -= sentmsgcount;
            var msgbalhis = await msubscriberMessageBalanceHistory.FindAsync(smb => smb.SubcriberId == subscriberId);
            msgbalhis.RemainingCount -= sentmsgcount;
            await msubscriberMessageBalance.SaveAsync();
            return await msubscriberMessageBalanceHistory.SaveAsync();
        }
        public async Task<bool> CheckMessageBalance(int mvmcnt, int messagecount, int subscriberId)
        {
            int sentmsgcount = mvmcnt * messagecount;
            var userMsgBalance = await msubscriberMessageBalance.FindAsync(smb => smb.SubcriberId == subscriberId);
            return userMsgBalance.RemainingCount > sentmsgcount;
        }
        public async Task<ICollection<SubcriberContactMessageViewModel>> MessageHistory(JgGridParam jgGridParam, int subcriberId)
        {
            int pageIndex = Convert.ToInt32(jgGridParam.page) - 1;
            int pageSize = jgGridParam.rows;
            string sort = jgGridParam.sord ?? "asc";
            string ordercolumn = jgGridParam.sidx;
            bool desc = sort.ToUpper() == "DESC";
            Expression<Func<SubscriberContactMessage, SubcriberContactMessageViewModel>> project = GetContactMessagProjection();
            return await subcribermessageRepository.GetPagedResult(pageSize * pageIndex, pageSize, ordercolumn, desc, project,
                  sc => sc.SubscriberContact.SubscriberStandards.SubscriberId == subcriberId);
        }

        private static Expression<Func<SubscriberContactMessage, SubcriberContactMessageViewModel>> GetContactMessagProjection()
        {
            return scm => new SubcriberContactMessageViewModel
            {
                Id = scm.Guid,
                Message = scm.Message.Text,
                Name = scm.SubscriberContact.Contact.Name,
                Section = scm.SubscriberContact.SubscriberStandardSections.SubscriberSection.Section.Name,
                SentDateTime = scm.CreatedAt,
                MobileNo = scm.SubscriberContact.Contact.Mobile,
                Status = ((MessageStatusEnum)scm.MessageStatus).ToString(),
                Class = scm.SubscriberContact.SubscriberStandards.Standard.Name,
                RollNo = scm.SubscriberContact.Contact.RollNo
            };
        }
        private static  SubcriberContactMessageViewModel GetContactMessageFunProjection(SubscriberContactMessage scm)
        {
            return new SubcriberContactMessageViewModel
            {
                Id = scm.Guid,
                Message = scm.Message.Text,
                Name = scm.SubscriberContact.Contact.Name,
                Section = scm.SubscriberContact.SubscriberStandardSections.SubscriberSection.Section.Name,
                SentDateTime = scm.CreatedAt,
                MobileNo = scm.SubscriberContact.Contact.Mobile,
                Status = ((MessageStatusEnum)scm.MessageStatus).ToString(),
                Class = scm.SubscriberContact.SubscriberStandards.Standard.Name,
                RollNo = scm.SubscriberContact.Contact.RollNo,
                MessageError = scm.MessageStatus == MessageStatusEnum.NotSent ? scm.MessageError.Text : ""
            };
        }
        public async Task<int> TotalMessageHistory(int subscriberId)
        {
            return await subcribermessageRepository.CountAsync(
                scm => scm.SubscriberContact.SubscriberStandards.SubscriberId == subscriberId);
        }
        public async Task<int> ResendMessage(int subscriberId, Guid messageId)
        {
            var smvm = await subcribermessageRepository.FindAsync(sm => sm.Guid == messageId);
            var apiformaturl = ExternalMessageServiceAPI.SubmitMessageApiformaturl();
            ExternalMessageServiceAPI smsserviceAPI = new ExternalMessageServiceAPI();
            try
            {
                apiformaturl = string.Format(apiformaturl, smvm.SubscriberContact.Contact.Mobile, smvm.Message.Text);
                string tuplemsgstatus = await smsserviceAPI.SubmitMessage(apiformaturl);
                smvm.MessageStatus = MessageStatusEnum.Sent;
            }
            catch (Exception ex)
            {
                smvm.MessageStatus = MessageStatusEnum.NotSent;
            }
            smvm.ModifiedAt = DateTime.Now;
            if (smvm.MessageStatus == MessageStatusEnum.Sent)
            {
                await UpdateSubscirberMessageBalance(subscriberId, 1);
            }
            return await subcribermessageRepository.SaveAsync();
        }
        public async Task<Tuple<long, long>> GetMessageBalance(int subscriberId)
        {
            var messagebal = await msubscriberMessageBalance.FindAsync(mb => mb.SubcriberId == subscriberId);
            return Tuple.Create(messagebal.OpeningCount, messagebal.RemainingCount);
        }


    }
}
