using DataModelLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public interface ISubscriberStandardService
    {
        Task<ICollection<SubscriberStandardViewModel>> GetStandards(int subscriberId);
        Task<ICollection<SubscriberStandardSectionViewModel>> GetSections(int subscirberStandardId);
        Task<List<string>> ClassDictionaries(int subscriberId);
        Task<List<string>> SectionDictionaries(int subscriberId);
        Task AddSectionsifNotExists(DataTable dt, int subscriberId);
        Task AddClassifNotExists(DataTable dt, int subscriberId);
    }
}
