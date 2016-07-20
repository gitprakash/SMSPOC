using DataModelLibrary;
using Repositorylibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public class UserRoleService:IUserRoleService
    {
        IGenericRepository<SubscriberRoles> msubscriberrolesrepository;
        
        public UserRoleService(IGenericRepository<SubscriberRoles> subscriberrolesrepository )
        {
            msubscriberrolesrepository = subscriberrolesrepository;
            
        }
       public async Task<SubscriberRoles> AddUserRole(SubscriberRoles sroles)
       {
           return await msubscriberrolesrepository.AddAsync(sroles);
       }
       public async Task<bool> CheckExists(string user,string role)
       {
           return await msubscriberrolesrepository.AnyAsync(sr=>sr.Subscriber.Username==user&&sr.role.Name==role);
       }
    }
}
