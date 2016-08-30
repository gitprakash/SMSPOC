using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModelLibrary;

namespace DataServiceLibrary
{
    public interface IMessageService  
    {
        Task<bool> CheckMessageBalance(int mvmcnt, int messagecount, int subscriberId);
        Task<int> ResendMessage(int subscriberId,Guid messageId);
        Task<List<MessageViewModel>> GetMessageStatusFromAPI(List<MessageViewModel> messageViewModel);
        Task<List<MessageViewModel>> SubmitMessageToServiceAPI(List<MessageViewModel> messageViewModel, string message, int SubscriberId);
        Task<List<MessageViewModel>> LogAllMessageToDB(List<MessageViewModel> messageViewModel, string message,
            int messagecount, int SubscriberId);
        Task<ICollection<SubcriberContactMessageViewModel>> MessageHistory(JgGridParam jgGridParam, int subcriberId);
        Task<int> TotalMessageHistory(int subscriberId);
        Task<Tuple<long, long>> GetMessageBalance(int subscriberId);
    }
}
