using DataModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public interface IAccountService
    {
          Task<IEnumerable<AccountType>> Accounttypes();
          Task<IEnumerable<GenderType>> Gendertypes();
          Task<Subscriber> Add(Subscriber role);
          Task<bool> IsUserNameExists(string username);
          Task<bool> IsUserEmailExists(string email);
          Task<bool> IsUniqueMobile(long mobileno);
          Task<Tuple<bool, bool>> CheckLogin(string username, string password);
    }
}
