using DataModelLibrary;
using Repositorylibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public class SubscriberStandardService:ISubscriberStandardService
    {
        IGenericRepository<SubscriberStandards> mclassRepository;
        IGenericRepository<SubscriberStandardSections> msectionRepository;
        public SubscriberStandardService(IGenericRepository<SubscriberStandards> classRepository,
            IGenericRepository<SubscriberStandardSections> sectionRepository)
        {
            mclassRepository = classRepository;
            msectionRepository = sectionRepository;
        }
        public async Task<ICollection<SubscriberStandardViewModel>> GetStandards(int subscriberId)
        {
            Expression<Func<SubscriberStandards, SubscriberStandardViewModel>> select = s => new SubscriberStandardViewModel { SubscriberStandardId = s.Id, StandardName = s.Standard.Name };
            Expression<Func<SubscriberStandardViewModel, string>> orderby = s => s.StandardName;
            return await mclassRepository.FindAllAsync(c => c.SubscriberId == subscriberId && c.Active, select, orderby);
        }
        public async Task<ICollection<SubscriberStandardSectionViewModel>> GetSections(int subscirberStandardId)
        {
            Expression<Func<SubscriberStandardSections, SubscriberStandardSectionViewModel>> select = s => new SubscriberStandardSectionViewModel { SubscriberStandardId = s.SubscriberStandardsId, 
                SubscriberStandardSectionId   = s.Id,SectionName = s.SubscriberSection.Sections.Name };
            Expression<Func<SubscriberStandardSectionViewModel, string>> orderby = s => s.SectionName;
            return await msectionRepository.FindAllAsync(c => c.SubscriberStandardsId == subscirberStandardId && c.Active, select, orderby);
        }
    }
}
