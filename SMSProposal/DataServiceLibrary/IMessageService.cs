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
        Task<bool> LogAllMessage(List<MessageViewModel> messageViewModel, string message, int messagecount);
    }
}
