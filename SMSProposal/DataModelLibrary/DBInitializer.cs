using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            IList<SubscriberRoles> subscriberroles = new List<SubscriberRoles>
            {
                new SubscriberRoles{ RoleId=1,SubscriberId=1,Active=true},
                new SubscriberRoles{ RoleId=2,SubscriberId=1,Active=true},
                new SubscriberRoles{ RoleId=2,SubscriberId=2,Active=true}
            };

            context.Roles.AddRange(Roles);
            context.GenderTypes.AddRange(GenderTypes);
            context.AccountTypes.AddRange(AccountTypes);
            context.Subscribers.AddRange(lstsubscribers);
            context.SubscriberRoles.AddRange(subscriberroles);
            base.Seed(context);
        }
    }
}
