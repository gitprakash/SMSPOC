using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataModelLibrary;
using Repositorylibrary;

namespace DataServiceLibrary
{
    public class TemplateService : ITemplateService
    {
        private IGenericRepository<SubscriberTemplate> mtemplateRepository;
        public TemplateService(IGenericRepository<SubscriberTemplate> templateRepository)
        {
            mtemplateRepository = templateRepository;
        }
        public async Task<ICollection<TemplateViewModel>> Templates(int subcriberId, int skip, int pagesize, string ordercolumn, bool desc)
        {
            Expression<Func<SubscriberTemplate, bool>> where = st => st.SubscriberId == subcriberId;
            Expression<Func<SubscriberTemplate, TemplateViewModel>> select = st =>
                new TemplateViewModel
                {
                    Id = st.Id, Name = st.Templates.Name, Description = st.Templates.Description,
                    Status = st.Active?"Active":"InActive"
                   
                };
            return await mtemplateRepository.GetPagedResult(skip, pagesize, ordercolumn, desc, select, where);
        }

        public async Task<int> TotalTemplates(int subcriberId)
        {
            Expression<Func<SubscriberTemplate, bool>> where = st => st.SubscriberId == subcriberId;
            return await mtemplateRepository.CountAsync(where);
        }

        public async Task<SubscriberTemplate> FindTemplate(int subcriberId, string templatename)
        {
            Expression<Func<SubscriberTemplate, bool>> where = st => st.SubscriberId == subcriberId && st.Templates.Name == templatename;
            return await mtemplateRepository.FindAsync(where);
        }

        public Task<int> SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
