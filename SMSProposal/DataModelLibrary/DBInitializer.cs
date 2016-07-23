using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
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

            var contacts = new List<Contact>
            {
                new Contact
                {
                    Name = "prakash",
                    Active = true,
                    Class = "1",
                    Section = "A",
                    Mobile = 9940499650,
                    BloodGroup = "AB+",
                    RollNo = "100",
                    CreatedDate = DateTime.Now
                },
                new Contact
                {
                    Name = "prakash1",
                    Active = true,
                    Class = "1",
                    Section = "A",
                    Mobile = 9940499652,
                    BloodGroup = "AB+",
                    RollNo = "101",
                    CreatedDate = DateTime.Now
                },
                new Contact
                {
                    Name = "nazer",
                    Active = true,
                    Class = "2",
                    Section = "A",
                    Mobile = 9940499653,
                    BloodGroup = "B+",
                    RollNo = "102",
                    CreatedDate = DateTime.Now
                },
                new Contact
                {
                    Name = "venki",
                    Active = true,
                    Class = "1",
                    Section = "A",
                    Mobile = 9940499654,
                    BloodGroup = "AB+",
                    RollNo = "103",
                    CreatedDate = DateTime.Now
                },
                new Contact
                {
                    Name = "prakash",
                    Active = true,
                    Class = "1",
                    Section = "A",
                    Mobile = 9940499650,
                    BloodGroup = "O+",
                    RollNo = "104",
                    CreatedDate = DateTime.Now
                },
                new Contact
                {
                    Name = "prakash1",
                    Active = true,
                    Class = "1",
                    Section = "D",
                    Mobile = 9940499652,
                    BloodGroup = "AB+",
                    RollNo = "105",
                    CreatedDate = DateTime.Now
                },
                new Contact
                {
                    Name = "nazer",
                    Active = true,
                    Class = "2",
                    Section = "A",
                    Mobile = 9940499643,
                    BloodGroup = "AB-",
                    RollNo = "106",
                    CreatedDate = DateTime.Now
                },
                new Contact
                {
                    Name = "venki",
                    Active = true,
                    Class = "1",
                    Section = "C",
                    Mobile = 9940499644,
                    BloodGroup = "B-",
                    RollNo = "107",
                    CreatedDate = DateTime.Now
                },
                new Contact
                {
                    Name = "suresh",
                    Active = true,
                    Class = "10",
                    Section = "A",
                    Mobile = 9940499642,
                    BloodGroup = "B+",
                    RollNo = "108",
                    CreatedDate = DateTime.Now
                },
                new Contact
                {
                    Name = "kathick",
                    Active = true,
                    Class = "2",
                    Section = "A",
                    Mobile = 9940499656,
                    BloodGroup = "AB-",
                    RollNo = "109",
                    CreatedDate = DateTime.Now
                },
                new Contact
                {
                    Name = "manoj",
                    Active = true,
                    Class = "11",
                    Section = "B",
                    Mobile = 8940499654,
                    BloodGroup = "AB-",
                    RollNo = "110",
                    CreatedDate = DateTime.Now
                }

            };
            lstsubscribers[1].Contacts=new List<Contact>();
            contacts.ForEach(c => lstsubscribers[1].Contacts.Add(c));
            context.Roles.AddRange(Roles);
            context.GenderTypes.AddRange(GenderTypes);
            context.AccountTypes.AddRange(AccountTypes);
            context.Subscribers.AddRange(lstsubscribers);
            context.SubscriberRoles.AddRange(subscriberroles);
            base.Seed(context);
        }
    }
}
