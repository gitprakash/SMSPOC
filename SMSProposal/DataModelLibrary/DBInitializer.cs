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
                new GenderType{ Name="Femaile"}
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
            context.Roles.AddRange(Roles);
            context.GenderTypes.AddRange(GenderTypes);
            context.AccountTypes.AddRange(AccountTypes);
            base.Seed(context);
        }
    }
}
