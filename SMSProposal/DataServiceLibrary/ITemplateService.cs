using DataModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public interface ITemplateService
    {
        Task<ICollection<TemplateViewModel>> Templates(int subcriberId, int skip, int pagesize, string ordercolumn, bool desc);
        Task<int> TotalTemplates(int subcriberId);
        Task<SubscriberTemplate> FindTemplate(int subcriberId, string templatename);
        Task<int> SaveAsync();
    }
}
