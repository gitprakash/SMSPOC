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
            Expression<Func<Contact, bool>> where = c => c.SubscriberId == subcriberId && c.Active == true;
            return await mcontactRepository.GetPagedResult(skip, pagesize, ordercolumn, desc, where);
        }
        public async Task<int> TotalContacts(int subcriberId)
        {
            Expression<Func<Contact, bool>> where = c => c.SubscriberId == subcriberId && c.Active==true;
            return await mcontactRepository.CountAsync(where);
        }
        public async Task<Contact> AddContact(Contact contact)
        {
            return await mcontactRepository.AddAsync(contact);
        }
        public async Task<bool> IsUniqueMobile(long mobileno)
        {
            return await mcontactRepository.AnyAsync(s => s.Mobile == mobileno);
        }

        public async Task<Contact> FindContact(long Id)
        {
            return await mcontactRepository.GetAsync(Id);
        }
        public async Task<int> SaveAsync()
        {
            return await mcontactRepository.SaveAsync();
        }
    }
}
