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
        public MessageService(IGenericRepository<SubscriberContactMessage>  messageRepository)
        {
            subcribermessageRepository = messageRepository;
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

              messageViewModel.ForEach( async (mvm) =>
            {

                var scm = new SubscriberContactMessage
                {
                    CreateDateTime = DateTime.Now, 
                    Guid = Guid.NewGuid(),
                    Message = subcribermessage, 
                    MessageStatus = new MessageStatus
                    {
                        Name = MessageStatusEnum.Sent
                    },
                    SubscriberStandardContactsId = mvm.Id
                };
                SubscriberContactMessage dbresult = await subcribermessageRepository.AddAsync(scm);
             result = dbresult.Id > 0;
            });
            return result;
        }
    }
}
