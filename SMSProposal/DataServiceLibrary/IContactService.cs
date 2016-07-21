using DataModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public interface IContactService
    {
        Task<IEnumerable<Contact>> Contacts(int subcriberId, int skip, int pagesize, string ordercolumn, bool desc);
        Task<int> TotalContacts(int subcriberId);
    }
}
