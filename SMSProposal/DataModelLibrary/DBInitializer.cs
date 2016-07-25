using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
namespace DataModelLibrary
{
    class DBInitializer : DropCreateDatabaseIfModelChanges<Model1>
    {
        protected override void Seed(Model1 context)
        {
            IList<GenderType> GenderTypes = new List<GenderType>
            {
                new GenderType{ Name="Male"},
                new GenderType{ Name="Female"}
            };
            IList<AccountType> AccountTypes = new List<AccountType>
            {
                new AccountType{ Name="Free"},
                new AccountType{ Name="Preminum"}
            };
            IList<Role> Roles = new List<Role>
            {
                new Role{ Name="Admin",Description="Admin all access", CreatedBy="prakash",CreatedDate=DateTime.Now},
                new Role{ Name="Subscriber",Description="application users", CreatedBy="prakash",CreatedDate=DateTime.Now},
                new Role{ Name="User",Description="user under subscriber", CreatedBy="prakash",CreatedDate=DateTime.Now},

            };
            IList<Subscriber> lstsubscribers = new List<Subscriber>
            {
                new Subscriber{ Username="prakash", FirstName="prakash", Password="password", AccountTypeId=2, GenderTypeId=1, Active=true, Email="prakashr@hcl.com",
                                LastName="rajendran", Mobile=9940499650},
                new Subscriber{ Username="testuser", FirstName="test", Password="password", AccountTypeId=2, GenderTypeId=1, Active=true, Email="test@hcl.com",
                                LastName="rajendran", Mobile=9940499651}
            };

            var subscriberroles = new List<SubscriberRoles>
            {
                new SubscriberRoles{ RoleId=1,SubscriberId=1,Active=true},
                new SubscriberRoles{ RoleId=2,SubscriberId=1,Active=true},
                new SubscriberRoles{ RoleId=2,SubscriberId=2,Active=true}
            };

            var Standards = new List<Standard> { 
                 new Standard{  Name="1", Active=true},
                 new Standard{  Name="2",Active=true},
                 new Standard{  Name="3",Active=true},
                 new Standard{  Name="4",Active=true},
                 new Standard{  Name="5",Active=true},
                 new Standard{  Name="6",Active=true},
                 new Standard{  Name="7",Active=true},
                 new Standard{  Name="8",Active=true},
                 new Standard{  Name="9",Active=true},
                 new Standard{  Name="10",Active=true},
                 new Standard{  Name="11",Active=true},
                 new Standard{  Name="12",Active=true},
                 new Standard{  Name="PRE-KG",Active=true},
                 new Standard{  Name="UKG",Active=true},
                 new Standard{  Name="LKG",Active=true},
            };
            var lstSection = new List<Section> { 
                 new Section{  Name="A"},
                 new Section{  Name="B"},
                 new Section{  Name="C"}
            };
            
           
            var SubscriberStandards = new List<SubscriberStandards>();
            Standards.ForEach(s => { SubscriberStandards.Add(new SubscriberStandards { Active = true, Subscriber = lstsubscribers[1], Standard = s }); });
            
            var lstSubscriberStandardSections = new List<SubscriberStandardSections>();
            lstSection.ForEach(s => { lstSubscriberStandardSections.Add(new SubscriberStandardSections { Active = true, SubscriberStandards = SubscriberStandards[4], Sections = s }); });
            lstSection.ForEach(s => { lstSubscriberStandardSections.Add(new SubscriberStandardSections { Active = true, SubscriberStandards = SubscriberStandards[9], Sections = s }); });
            lstSection.ForEach(s => { lstSubscriberStandardSections.Add(new SubscriberStandardSections { Active = true, SubscriberStandards = SubscriberStandards[11], Sections = s }); });
            var lstcontacts = new List<Contact>();
            for (int i = 0; i < 20; i++)
            {
                lstcontacts.Add(new Contact
                 {
                     Name = "Student" + i,
                     Active = true,
                     Mobile = 7040499650 + i,
                     BloodGroup = "AB+",
                     RollNo = "100" + i,
                     CreatedDate = DateTime.Now
                 });
            }

            var lstssc = new List<SubscriberStandardContacts>();
            SubscriberStandards.ForEach(s => {
                lstcontacts.ForEach(c=>{
                    lstssc.Add(new SubscriberStandardContacts { Active = true,  SubscriberStandards = s,Contact=c,SubscriberStandardSections=lstSubscriberStandardSections[0] });
                });
            });

            context.Roles.AddRange(Roles);
            context.GenderTypes.AddRange(GenderTypes);
            context.AccountTypes.AddRange(AccountTypes);
            context.Subscribers.AddRange(lstsubscribers);
            context.SubscriberRoles.AddRange(subscriberroles);
            context.Standards.AddRange(Standards);
            context.SubscriberStandards.AddRange(SubscriberStandards);
            context.Sections.AddRange(lstSection);
            context.SubscriberStandardSections.AddRange(lstSubscriberStandardSections);
            context.Contacts.AddRange(lstcontacts);
            context.SubscriberStandardContacts.AddRange(lstssc);
            base.Seed(context);
        }
    }
}
