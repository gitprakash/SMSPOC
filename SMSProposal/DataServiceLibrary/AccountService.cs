using DataModelLibrary;
using Repositorylibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public class AccountService:IAccountService
    {
        IGenericRepository<Subscriber> msubscriberrepository;
        IGenericRepository<AccountType> maccountyprepository;
        IGenericRepository<GenderType> mgendertyprepository;
        public AccountService(IGenericRepository<AccountType> accountyprepository,
            IGenericRepository<GenderType> gendertyprepository,
            IGenericRepository<Subscriber> subscriberrepository)
        {
            mgendertyprepository = gendertyprepository;
            maccountyprepository = accountyprepository;
            msubscriberrepository = subscriberrepository;
        } 
        public async Task<IEnumerable<AccountType>> Accounttypes()
        {
          return await  maccountyprepository.GetAllAsync();
        }

        public async Task<IEnumerable<GenderType>> Gendertypes()
        {
            return await mgendertyprepository.GetAllAsync();
        }

        public async Task<Subscriber> Add(Subscriber subscriber)
        {
            return await msubscriberrepository.AddAsync(subscriber);
        }
    }
}
