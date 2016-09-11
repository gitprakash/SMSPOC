using DataModelLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public interface ISubscriberService
    {
        Task<List<Subscriber>> Getsubscribers();
        Task<Subscriber> Subscriber(int subscriberId);

    }
}
