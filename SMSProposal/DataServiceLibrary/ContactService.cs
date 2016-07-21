using DataModelLibrary;
using Repositorylibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public class ContactService:IContactService
    {
        IGenericRepository<Contact> mcontactRepository;
        public ContactService(IGenericRepository<Contact> contactRepository)
        {
            mcontactRepository = contactRepository;
        }
        public async Task<IEnumerable<Contact>> Contacts(int subcriberId,int skip, int pagesize, string ordercolumn, bool desc)
        {
            Expression<Func<Contact, bool>> where = c => c.SubscriberId == subcriberId;
            return await mcontactRepository.GetPagedResult(skip, pagesize, ordercolumn, desc, where);
        }
        public async Task<int> TotalContacts(int subcriberId)
        {
            Expression<Func<Contact, bool>> where = c => c.SubscriberId == subcriberId;
            return await mcontactRepository.CountAsync(where);
        }

    }
}
