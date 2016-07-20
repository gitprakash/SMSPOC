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
        IGenericRepository<SubscriberRoles> msubscriberrolesrepository;
        public AccountService(IGenericRepository<AccountType> accountyprepository,
            IGenericRepository<GenderType> gendertyprepository,
            IGenericRepository<Subscriber> subscriberrepository,
            IGenericRepository<SubscriberRoles> subscriberrolesrepository)
        {
            mgendertyprepository = gendertyprepository;
            maccountyprepository = accountyprepository;
            msubscriberrepository = subscriberrepository;
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
        public  Subscriber Finduser(string username)
        {
            return   msubscriberrepository.Find(s => s.Username.Equals((username)));
        }

        public dynamic  GetUserRole()
        {
            //var tuple=Tuple.Create()
         return    msubscriberrolesrepository.ToArrayAsync(sr => new { Id = sr.Id, sr.Subscriber.Username, RoleNmae = sr.role.Name});
               // .Select(sr => new { Id = sr.Id, sr.Subscriber.Username, RoleNmae = sr.role.Name }).ToArrayAsync();

        }


        public Task<Tuple<int, string, string[]>> GetUserRoles()
        {
            throw new NotImplementedException();
        }
    }
}
