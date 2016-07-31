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
        private IGenericRepository<SubscriberContactMessage> sbcribermessageRepository;
        public MessageService(IGenericRepository<SubscriberContactMessage>  messageRepository)
        {
            sbcribermessageRepository = messageRepository;
        }

        public void send(MessageViewModel[] messageViewModel, string message, int messagecount)
        {
            throw new NotImplementedException();
        }
    }
}
