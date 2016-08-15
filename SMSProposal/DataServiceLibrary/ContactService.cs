using DataModelLibrary;
using Repositorylibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
        public async Task<IEnumerable<ContactViewModel>> Contacts(int subcriberId, JgGridParam jgGridParam)
        {
            int pageIndex = Convert.ToInt32(jgGridParam.page) - 1;
            int pageSize = jgGridParam.rows;
            string sort = jgGridParam.sord ?? "asc";
            string ordercolumn = jgGridParam.sidx;
            bool desc = sort.ToUpper() == "ASC";
            var filter = GetFilter(jgGridParam);
            Expression<Func<SubscriberStandardContacts, bool>> where = s => s.SubscriberStandards.SubscriberId == subcriberId;
            Expression<Func<SubscriberStandardContacts, ContactViewModel>> select = s => new ContactViewModel
            {
                Section = s.SubscriberStandardSections.SubscriberSection.Section.Name,
                Class = s.SubscriberStandards.Standard.Name, 
                Name = s.Contact.Name,
                BloodGroup = s.Contact.BloodGroup,
                Mobile = s.Contact.Mobile,
                RollNo = s.Contact.RollNo,
                Id = s.Id,
                SubscriberStandardId = s.SubscriberStandardsId,
                SubscriberStandardSectionId = s.SubscriberStandardSectionsId,
                Status=s.Active?"Active":"InActive"
            };
            return await msscRepository.GetPagedResult(pageSize * pageIndex, pageSize, ordercolumn, desc, select, where, filter);
        }
        private static List<Filter> GetFilter(JgGridParam jgGridParam)
        {
            List<Filter> filter = new List<Filter>();
            int rollno = 0;
            if (!string.IsNullOrEmpty(jgGridParam.searchField))
            {
                Filter instancefilter = new Filter { PropertyName = jgGridParam.searchField, Operation = Op.Equals };
                if (jgGridParam.searchField == "RollNo")
                {
                   // int.TryParse(jgGridParam.searchString, out rollno);
                    instancefilter.PropertyName = "Contact.RollNo";
                    instancefilter.Value = rollno;
                }
                else
                {
                    instancefilter.Value = jgGridParam.searchString;
                }
                filter.Add(instancefilter);
            };
            return filter;
        }
        public async Task<int> TotalContacts(int subcriberId)
        {
            Expression<Func<SubscriberStandardContacts, bool>> where = s => s.SubscriberStandards.SubscriberId == subcriberId && s.Active;
            return await msscRepository.CountAsync(where);
        }
        public async Task<Contact> AddContact(ContactViewModel contactvm)
        {
            var contact = new Contact();
            contact = GetContact(contactvm, contact);
            contact.Active = true;
            contact.CreatedDate = DateTime.Now;
            var ssc = new SubscriberStandardContacts
            {
                SubscriberStandardsId = contactvm.SubscriberStandardId,
                Active = true,
                SubscriberStandardSectionsId = contactvm.SubscriberStandardSectionId,
                CreatedAt = DateTime.Now,
                Contact = contact
            };
            var dbsave = await msscRepository.AddAsync(ssc);
            return contact;
            //return await mcontactRepository.AddAsync(contact);
        }

        public async Task<int> EditContact(ContactViewModel contactvm)
        {
            var ssscontact = await msscRepository.GetAsync(contactvm.Id);
            if (ssscontact == null)
            {
                throw new Exception("Unable to find student Contact details");
            }
            ssscontact.Contact = GetContact(contactvm, ssscontact.Contact);
            ssscontact.SubscriberStandardSectionsId = contactvm.SubscriberStandardSectionId;
            ssscontact.SubscriberStandardsId = contactvm.SubscriberStandardId;
            ssscontact.Active = contactvm.Status == "InActive" ? false : true;
            return await msscRepository.SaveAsync();
        }
        public Contact GetContact(ContactViewModel cv, Contact contact)
        {
            contact.Name = cv.Name;
            contact.Mobile = cv.Mobile;
            contact.RollNo = cv.RollNo;
            contact.BloodGroup = cv.BloodGroup;
            return contact;
        }
        public async Task<bool> IsUniqueMobile(long mobileno)
        {
            return await mcontactRepository.AnyAsync(s => s.Mobile == mobileno);
        }

        public async Task<SubscriberStandardContacts> FindContact(long Id)
        {
            return await msscRepository.GetAsync(Id);
        }
        public async Task<int> SaveAsync()
        {
            return await msscRepository.SaveAsync();
        }

        public async Task<bool> IsUniqueRollNo(int subscriberId,string rollNo)
        {
            return
                await
                    msscRepository.AnyAsync(
                        s => s.SubscriberStandards.SubscriberId == subscriberId && s.Contact.RollNo.Equals(rollNo));
        }

        public List<ContactViewModel> ProjectContactsFromDatatable(DataTable dt)
        {
           var result= dt.AsEnumerable().AsParallel().Select(dr => new ContactViewModel()
            {
                RollNo = dr["RollNo"].ToString(), 
                Name = dr["Name"].ToString(),
                Mobile = Convert.ToInt64(dr["Mobile"]),
                BloodGroup = dr["Blood Group"].ToString(),
                Class = dr["Class"].ToString(),
                Section=dr["Section"].ToString()
            }).ToList();
            return result;
        }
    }
}
