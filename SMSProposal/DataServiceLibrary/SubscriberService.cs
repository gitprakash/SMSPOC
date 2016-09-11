using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModelLibrary;
using Repositorylibrary;

namespace DataServiceLibrary
{
    public class SubscriberService : ISubscriberService
    {
        IGenericRepository<Subscriber> msubscriberrepository;
        public SubscriberService(IGenericRepository<Subscriber> subscriberrepository)
        {
            msubscriberrepository=subscriberrepository;
        }
        public Task<List<Subscriber>> Getsubscribers()
        {
            throw new NotImplementedException();
        }

        public async Task<Subscriber> Subscriber(int subscriberId)
        {
           return await msubscriberrepository.GetAsync(subscriberId);
        }
    }
}
