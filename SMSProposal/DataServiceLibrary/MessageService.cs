using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModelLibrary;
using Repositorylibrary;

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

        public async Task<bool> LogAllMessage(List<MessageViewModel> messageViewModel, string message, int messagecount)
        {
            //after send calling sms server api, and collected result
            bool result = false;
            var subcribermessage = new Message
            {
                Text = message,
                Guid = Guid.NewGuid(),
                MessageCount = messagecount,
                CreatedAt = DateTime.Now,
            };
            List<SubscriberContactMessage> lstmessages = new List<SubscriberContactMessage>();
            var lstsubsribedmsg = (from mvm in messageViewModel
                                   select new SubscriberContactMessage
                                   {
                                       CreatedAt = DateTime.Now,
                                       Guid = Guid.NewGuid(),
                                       Message = subcribermessage,
                                       MessageStatus = mvm.SentStatus ? MessageStatusEnum.Sent : MessageStatusEnum.NotSent,
                                       SubscriberStandardContactsId = mvm.Id
                                   }).AsParallel().ToList();

            int dbresult = await subcribermessageRepository.AddRangeAsync(lstsubsribedmsg);
            result = dbresult > 0;
            return result;
        }

        public async Task<int> UpdateMessageBalance(List<MessageViewModel> messageViewModel, int messagecount, int subscriberId)
        {
            int sentmsgcount = messageViewModel.Where(mvm => mvm.SentStatus == true).Count();
            sentmsgcount = sentmsgcount * messagecount;
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

    }
}
