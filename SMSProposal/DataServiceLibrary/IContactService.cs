using DataModelLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public interface IContactService
    {
        Task<IEnumerable<ContactViewModel>> Contacts(int subcriberId, JgGridParam jgGridParam);
        Task<bool> IsUniqueRollNo(int subscriberId, string rollNo);
        Task<int> TotalContacts(int subcriberId);
        Task<Contact> AddContact(ContactViewModel contact);
        Task<int> EditContact(ContactViewModel contactvm);
        Task<bool> IsUniqueMobile(long mobileno);
        Task<SubscriberStandardContacts> FindContact(long Id);
        List<ContactViewModel> ProjectContactsFromDatatable(DataTable dt);
        Task<int> SaveAsync();
    }
}
