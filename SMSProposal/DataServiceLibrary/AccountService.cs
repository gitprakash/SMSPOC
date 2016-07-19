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


        public async Task<bool> IsUserNameExists(string username)
        {
            return await msubscriberrepository.AnyAsync(s => s.Username == username);
        }
        public async Task<bool> IsUserEmailExists(string email)
        {
            return await msubscriberrepository.AnyAsync(s => s.Email == email);

        }
        public async Task<bool> IsUniqueMobile(long mobileno)
        {
            return await msubscriberrepository.AnyAsync(s => s.Mobile == mobileno);
        }
        public async Task<Tuple<bool, bool>> CheckLogin(string username, string password)
        {
            Tuple<bool, bool> logintuple = new Tuple<bool, bool>(
                    await msubscriberrepository.AnyAsync(s => s.Username == username),
                    await msubscriberrepository.AnyAsync(s => s.Username == username && s.Password == password)
                );
             return logintuple;
        }


    }
}
