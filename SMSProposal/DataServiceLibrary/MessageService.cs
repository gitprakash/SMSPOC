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
        private IGenericRepository<MessageStatus> messagestatusRepository;
        public MessageService(IGenericRepository<SubscriberContactMessage> messageRepository,
            IGenericRepository<MessageStatus> messageStatusRepository)
        {
            subcribermessageRepository = messageRepository;
            messagestatusRepository = messageStatusRepository;
        }

        public async Task<bool> Send(List<MessageViewModel> messageViewModel, string message, int messagecount)
        {
            //after send calling sms server api, and collected result
            bool result = false;
            var subcribermessage = new Message
            {
                Text = message,
                SubcriberGuid = Guid.NewGuid(),
                MessageCount = messagecount
            };
            List<SubscriberContactMessage> lstmessages = new List<SubscriberContactMessage>();
            var lstsubsribedmsg = (from mvm in messageViewModel
                                   select new SubscriberContactMessage
                                   {
                                       CreateDateTime = DateTime.Now,
                                       Guid = Guid.NewGuid(),
                                       Message = subcribermessage,
                                       MessageStatus = mvm.SentStatus ? MessageStatusEnum.Sent : MessageStatusEnum.NotSent,
                                       SubscriberStandardContactsId = mvm.Id
                                   }).AsParallel().ToList();

            int dbresult = await subcribermessageRepository.AddRangeAsync(lstsubsribedmsg);
            result = dbresult > 0;
            return result;
        }
    }
}
