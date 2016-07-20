using DataModelLibrary;
using Repositorylibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public class RoleService:IRoleService
    {
        IGenericRepository<Role> mrole;
        public RoleService(IGenericRepository<Role> role)
        {
            mrole = role;
        }
        public async Task<Role> Add(Role role)
        {
           return await mrole.AddAsync(role);
        }
        public async Task<Role> Edit(Role role)
        {
            return await mrole.UpdateAsync(role,role.Id);
        }
        public async Task<int> Delete(Role role)
        {
            return await mrole.DeleteAsync(role);
        }
        public async Task<Role> FindRole(string name)
        {
            return await mrole.FindAsync(r=>r.Name==name);
        }
       public async Task<IEnumerable<Role>> GetRoles(int skip, int pagesize,string ordercolumn,bool desc)
        {
            return await mrole.GetPagedResult(skip, pagesize, ordercolumn,desc);
        }
       public async Task<int> TotalRoles()
       {
           return await mrole.CountAsync();
       }

       public async Task<string[]> GetAllRoles()
       {
           return await mrole.ToArrayAsync(r=>r.Name);
       }
       public async Task<bool> IsRoleExists(string name)
       {
           return await mrole.AnyAsync(r => r.Name.Equals(name));
       }

    }
}
