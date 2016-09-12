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



        public async Task<List<MessageViewModel>> SubmitMessageToServiceAPI(List<MessageViewModel> messageViewModel, string message, int SubscriberId)
        {
            ExternalMessageServiceAPI smsserviceAPI = new ExternalMessageServiceAPI();
            var msgsubmiturl = ExternalMessageServiceAPI.SubmitMessageApiformaturl();
            foreach (var smvm in messageViewModel)
            {
                try
                {
                    string tempmsgsubmiturl = string.Format(msgsubmiturl, smvm.Mobile, message);
                    smvm.Submittrequesttime = DateTime.Now;
                    string submitid = await smsserviceAPI.SubmitMessage(tempmsgsubmiturl);
                    smvm.Submittresponsetime = DateTime.Now;
                    var status = smsserviceAPI.IsMessageSubmitted(submitid);
                    smvm.IsMessageSubmitted = (status);
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

        public async Task<List<MessageViewModel>> GetMessageStatusFromAPI(List<MessageViewModel> messageViewModel)
        {
            ExternalMessageServiceAPI smsserviceAPI = new ExternalMessageServiceAPI();
            var msgStatusurl = ExternalMessageServiceAPI.GetMessageDeliveryReportUrl();
            foreach (var smvm in messageViewModel.Where(mvm => mvm.IsMessageSubmitted))
            {
                try
                {
                    string tempmsgStatusurl = string.Format(msgStatusurl, smvm.SubmitId);
                    smvm.DeliveryRequestedtime = DateTime.Now;
                    var deliverystatusid = await smsserviceAPI.GetMessageStatus(tempmsgStatusurl);
                    smvm.DeliveryResponsetime = DateTime.Now;
                    smvm.IsMessageDelivered = deliverystatusid == "DELIVRD" ? true : false;
                    smvm.DeliveryId = deliverystatusid;
                }
                catch (Exception ex)
                {
                    smvm.DeliveryResponsetime = DateTime.Now;
                    smvm.IsMessageDelivered = false;
                    smvm.MessageError = ex.Message;
                }
            }
            return messageViewModel;
        }


        public async Task<List<MessageViewModel>> LogAllMessageToDB(List<MessageViewModel> messageViewModel, string message,
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
                                       MessageStatus = mvm.IsMessageDelivered ? MessageStatusEnum.Delivered : mvm.DeliveryId == "PENDING" ? MessageStatusEnum.Pending : MessageStatusEnum.NotSent,
                                       SubscriberStandardContactsId = mvm.Id,
                                       SubmitId = mvm.SubmitId,
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
            if (messageViewModel.Any(mvm => mvm.IsMessageSubmitted == true))
            {
                await UpdateMessageBalance(messageViewModel, messagecount, SubscriberId);
                //if (dbupdated <= 0)
                //    throw new Exception("Problem in update Message balance");
            }
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

        public async Task  UpdateMessageBalance(List<MessageViewModel> messageViewModel, int messagecount, int subscriberId)
        {
            int sentmsgcount = messageViewModel.Count(mvm => mvm.IsMessageSubmitted == true);
            sentmsgcount = sentmsgcount * messagecount;
              await UpdateSubscirberMessageBalance(subscriberId, sentmsgcount);
        }

        private async Task  UpdateSubscirberMessageBalance(int subscriberId, int sentmsgcount)
        {
            var userMsgBalance = await msubscriberMessageBalance.FindAsync(smb => smb.SubcriberId == subscriberId);
            userMsgBalance.RemainingCount -= sentmsgcount;
            var msgbalhis = await msubscriberMessageBalanceHistory.FindAsync(smb => smb.SubcriberId == subscriberId);
            msgbalhis.RemainingCount -= sentmsgcount;
           // await msubscriberMessageBalance.SaveAsync();
           // return await msubscriberMessageBalanceHistory.SaveAsync();
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
            var result = await subcribermessageRepository.GetPagedResult(pageSize * pageIndex, pageSize, ordercolumn, desc, project,
                  sc => sc.SubscriberContact.SubscriberStandards.SubscriberId == subcriberId);
            await UpdatePendingMessageDeliveryStatusFromSMSAPI(result);
            return result;
        }

        private async Task UpdatePendingMessageDeliveryStatusFromSMSAPI(ICollection<SubcriberContactMessageViewModel> result)
        {
            var pendingstatusfilter = result.Where(scm => scm.Status.ToUpper() == "PENDING").ToList();
            if (pendingstatusfilter.Count() > 0)
            {
                ExternalMessageServiceAPI smsserviceAPI = new ExternalMessageServiceAPI();
                var msgStatusurl = ExternalMessageServiceAPI.GetMessageDeliveryReportUrl();
                foreach (var smvm in pendingstatusfilter)
                {
                    string tempmsgStatusurl = string.Format(msgStatusurl, smvm.SubmitId);
                    var deliverystatusid = await smsserviceAPI.GetMessageStatus(tempmsgStatusurl);
                    var updateresult = result.SingleOrDefault(r => r.SubscriberContactId == smvm.SubscriberContactId);
                    updateresult.DeliveryId = updateresult != null ? deliverystatusid : updateresult.DeliveryId;
                    updateresult.Status = deliverystatusid.Contains("DELIVRD") ? (MessageStatusEnum.Delivered).ToString() : deliverystatusid;
                }
                await UpdateMessageDeliveryStatusToDB(pendingstatusfilter);
            }
        }
        private async Task UpdateMessageDeliveryStatusToDB(IEnumerable<SubcriberContactMessageViewModel> result)
        {
            bool changesdetected = false;
            foreach (var scmv in result)
            {
                var dbscm = await subcribermessageRepository.FindAsync(scm => scm.Id == scmv.SubscriberContactId);
                if (dbscm != null)
                {
                    if (dbscm.DeliveryId != scmv.DeliveryId)
                    {
                        changesdetected = true;
                        dbscm.DeliveryId = scmv.DeliveryId;
                        dbscm.MessageStatus = scmv.DeliveryId.Contains("DELIVRD") ? MessageStatusEnum.Delivered : dbscm.MessageStatus;
                    }
                }
            }
            if (changesdetected)
                await subcribermessageRepository.SaveAsync();
        }



        private static Expression<Func<SubscriberContactMessage, SubcriberContactMessageViewModel>> GetContactMessagProjection()
        {
            return scm => new SubcriberContactMessageViewModel
            {
                SubscriberContactId = scm.Id,
                Id = scm.Guid,
                Message = scm.Message.Text,
                Name = scm.SubscriberContact.Contact.Name,
                Section = scm.SubscriberContact.SubscriberStandardSections.SubscriberSection.Section.Name,
                SentDateTime = scm.CreatedAt,
                MobileNo = scm.SubscriberContact.Contact.Mobile,
                Status = ((MessageStatusEnum)scm.MessageStatus).ToString(),
                Class = scm.SubscriberContact.SubscriberStandards.Standard.Name,
                RollNo = scm.SubscriberContact.Contact.RollNo,
                SubmitId = scm.SubmitId,
                DeliveryId = scm.DeliveryId,
            };
        }
        private static SubcriberContactMessageViewModel GetContactMessageFunProjection(SubscriberContactMessage scm)
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
                SubmitId = scm.SubmitId,
                DeliveryId = scm.DeliveryId,
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
