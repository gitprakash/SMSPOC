using DataModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public interface ISubscriberStandardService
    {
        Task<ICollection<SubscriberStandardViewModel>> GetStandards(int subscriberId);
        Task<ICollection<SubscriberStandardSectionViewModel>> GetSections(int subscirberStandardId);
    }
}
