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
    public class ContactService : IContactService
    {
        IGenericRepository<Contact> mcontactRepository;
      //  IGenericRepository<SubscriberStandards> mssRepository;
        IGenericRepository<SubscriberStandardContacts> msscRepository;
        public ContactService(IGenericRepository<Contact> contactRepository, IGenericRepository<SubscriberStandardContacts> sscRepository)
        {
            mcontactRepository = contactRepository;
            msscRepository = sscRepository;

        }
        public async Task<IEnumerable<ContactViewModel>> Contacts(int subcriberId, int skip, int pagesize, string ordercolumn, bool desc)
        {
           Expression<Func<SubscriberStandardContacts, bool>> where = s=>s.SubscriberStandards.SubscriberId==subcriberId && s.Active;
           Expression<Func<SubscriberStandardContacts, ContactViewModel>> select = s => new ContactViewModel
           {
               Section = s.SubscriberStandardSections.Sections.Name,
               Class = s.SubscriberStandards.Standard.Name,
               Id = s.Contact.Id,
               Name = s.Contact.Name,
               BloodGroup = s.Contact.BloodGroup,
               Mobile = s.Contact.Mobile,
               RollNo = s.Contact.RollNo,
               SubscriberContactId = s.Id,
               SubscriberStandardId=s.SubscriberStandardsId,
               SubscriberStandardSectionId=s.SubscriberStandardSectionsId
           };
           return await msscRepository.GetPagedResult(skip, pagesize, ordercolumn, desc,select, where);
        }
         
        public async Task<int> TotalContacts(int subcriberId)
        {
            Expression<Func<SubscriberStandardContacts, bool>> where = s => s.SubscriberStandards.SubscriberId == subcriberId && s.Active;
            return await msscRepository.CountAsync(where);
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
