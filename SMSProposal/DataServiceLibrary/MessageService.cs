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
        public async Task SendMessage(List<MessageViewModel> messageViewModel, string message, int messagecount, int SubscriberId)
        {   
            ExternalMessageServiceAPI smsserviceAPI = new ExternalMessageServiceAPI();
            string apiformaturl = ConfigUtility.GetAPIConfigvalue();
            apiformaturl = apiformaturl + "&dest_mobileno={0}&message={1}&response=Y";
            foreach (var smvm in messageViewModel)
            {
                try
                {
                    apiformaturl = string.Format(apiformaturl, smvm.Mobile, message);
                    Tuple<bool, DateTime> tuplemsgstatus = await smsserviceAPI.SendMessage(apiformaturl);
                    if (tuplemsgstatus.Item1)
                    {
                        smvm.SentStatus = true;
                        smvm.SentTime = tuplemsgstatus.Item2;
                    }
                }
                catch (Exception ex)
                {
                    smvm.SentStatus = false;
                }
            }
            await LogAllMessage(messageViewModel, message, messagecount, SubscriberId);
        }

        public async Task<bool> LogAllMessage(List<MessageViewModel> messageViewModel, string message, int messagecount, int SubscriberId)
        {
            bool result = false;
            var subcribermessage = CreateMessage(message, messagecount);
            List<SubscriberContactMessage> lstmessages = new List<SubscriberContactMessage>();
            var lstsubsribedmsg = (from mvm in messageViewModel
                                   select new SubscriberContactMessage
                                   {
                                       CreatedAt = mvm.SentTime ?? DateTime.Now,
                                       Guid = Guid.NewGuid(),
                                       Message = subcribermessage,
                                       MessageStatus = mvm.SentStatus ? MessageStatusEnum.Sent : MessageStatusEnum.NotSent,
                                       SubscriberStandardContactsId = mvm.Id
                                   }).AsParallel().ToList();

            int dbresult = await subcribermessageRepository.AddRangeAsync(lstsubsribedmsg);
            if (messageViewModel.Any(mvm => mvm.SentStatus == true))
            {
                await UpdateMessageBalance(messageViewModel, messagecount, SubscriberId);
            }
            result = dbresult > 0;
            return result;
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
            int sentmsgcount = messageViewModel.Count(mvm => mvm.SentStatus == true);
            sentmsgcount = sentmsgcount * messagecount;
            return await UpdateSubscirberMessageBalance(subscriberId, sentmsgcount);
        }

        private async Task<int> UpdateSubscirberMessageBalance(int subscriberId, int sentmsgcount)
        {
            var userMsgBalance = await msubscriberMessageBalance.FindAsync(smb => smb.SubcriberId == subscriberId);
            userMsgBalance.RemainingCount = -sentmsgcount;
            var msgbalhis = await msubscriberMessageBalanceHistory.FindAsync(smb => smb.SubcriberId == subscriberId);
            msgbalhis.RemainingCount = -sentmsgcount;
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
            Expression<Func<SubscriberContactMessage, SubcriberContactMessageViewModel>> project =
                scm => new SubcriberContactMessageViewModel
                {
                    Id = scm.Guid,
                    Message = scm.Message.Text,
                    Name = scm.SubscriberContact.Contact.Name,
                    Section = scm.SubscriberContact.SubscriberStandardSections.Sections.Name,
                    SentDateTime = scm.CreatedAt,
                    MobileNo = scm.SubscriberContact.Contact.Mobile,
                    Status = ((MessageStatusEnum)scm.MessageStatus).ToString(),
                    Class = scm.SubscriberContact.SubscriberStandards.Standard.Name,
                    RollNo = scm.SubscriberContact.Contact.RollNo
                };
            return await subcribermessageRepository.GetPagedResult(pageSize * pageIndex, pageSize, ordercolumn, desc, project,
                  sc => sc.SubscriberContact.SubscriberStandards.SubscriberId == subcriberId);
        }

        public async Task<int> TotalMessageHistory(int subscriberId)
        {
            return await subcribermessageRepository.CountAsync(
                scm => scm.SubscriberContact.SubscriberStandards.SubscriberId == subscriberId);
        }
        public async Task<int> ResendMessage(int subscriberId, Guid messageId)
        {
            var scm = await subcribermessageRepository.FindAsync(sm => sm.Guid == messageId);
            //call service to update
            scm.MessageStatus = MessageStatusEnum.Sent;
            if (scm.MessageStatus == MessageStatusEnum.Sent)
            {
                await UpdateSubscirberMessageBalance(subscriberId, 1);
            }
            return await subcribermessageRepository.SaveAsync();
        }

    }
}
