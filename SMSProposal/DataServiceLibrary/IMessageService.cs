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
        void Send(List<MessageViewModel> messageViewModel, string message, int messagecount);
    }
}
