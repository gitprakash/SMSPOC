using DataModelLibrary;
using Repositorylibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public class SubscriberStandardService:ISubscriberStandardService
    {
        IGenericRepository<SubscriberStandards> mclassRepository;
        IGenericRepository<SubscriberSection> msubscribersectionRepository;
        IGenericRepository<SubscriberStandardSections> msectionRepository;
        public SubscriberStandardService(IGenericRepository<SubscriberStandards> classRepository,
            IGenericRepository<SubscriberStandardSections> sectionRepository,
             IGenericRepository<SubscriberSection> subscribersectionRepository)
        {
            mclassRepository = classRepository;
            msectionRepository = sectionRepository;
            msubscribersectionRepository = subscribersectionRepository;
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
                SubscriberStandardSectionId   = s.Id,SectionName = s.SubscriberSection.Section.Name };
            Expression<Func<SubscriberStandardSectionViewModel, string>> orderby = s => s.SectionName;
            return await msectionRepository.FindAllAsync(c => c.SubscriberStandardsId == subscirberStandardId && c.Active, select, orderby);
        }

        public async Task<List<string>> ClassDictionaries(int subscriberId)
        {
            Expression<Func<SubscriberStandards, bool>> whereExpression = ss => ss.SubscriberId == subscriberId;
            var result = await mclassRepository.FindAllAsync(whereExpression, ss => ss.Standard.Name);
           return  result.ToList();
        }

        public async Task<List<string>> SectionDictionaries(int subscriberId)
        {
            Expression<Func<SubscriberSection, bool>> whereExpression = ss => ss.SubscriberId == subscriberId;
            var result = await msubscribersectionRepository.FindAllAsync(whereExpression, ss => ss.Section.Name);
            return result.ToList();
        }

        public async Task AddClassifNotExists(DataTable dt, int subscriberId)
        {
            var excelclasslist = dt.AsEnumerable().Select(c => c["Class"].ToString()).Distinct().ToList();
            var dbclass = await ClassDictionaries(subscriberId);
            var newclass = excelclasslist.Where(n=> dbclass.Contains(n) == false).AsParallel().ToList();
            var newdbclasslist = newclass.Select(c => new SubscriberStandards
            {
                SubscriberId = subscriberId,
                Standard = new Standard { Name=c },Active = true, CreatedAt = DateTime.Now, Guid = Guid.NewGuid()
            }).AsParallel().ToList();
            if (newdbclasslist.Count > 0)
            {
                await mclassRepository.AddRangeAsync(newdbclasslist);
            }
        }
        public async Task AddSectionsifNotExists(DataTable dt, int subscriberId)
        {
            var excelsectionlist = dt.AsEnumerable().Select(c => c["Section"].ToString()).Distinct().ToList();
            var dbsections = await SectionDictionaries(subscriberId);
            var newclass = excelsectionlist.Where(n => dbsections.Contains(n) == false).AsParallel().ToList();
            var newdbsectionlist = newclass.Select(c => new SubscriberStandards
            {
                SubscriberId = subscriberId,
                Standard = new Standard { Name = c },
                Active = true,
                CreatedAt = DateTime.Now,
                Guid = Guid.NewGuid()
            }).AsParallel().ToList();
            if (newdbsectionlist.Count > 0)
            {
                await mclassRepository.AddRangeAsync(newdbsectionlist);
            }
        }

        public async Task AddClassSectionLinkIfNotExists(DataTable dt,int subscriberId)
        {
            var uniqueclasssectionlst =
                dt.AsEnumerable().Where(c => c["Class"] != DBNull.Value && c["Section"] != DBNull.Value)
                    .Distinct()
                    .Select(d => new {Class = d["Class"].ToString(), SectionName = d["Section"].ToString()})
                    .ToList();
            //uniqueclasssectionlst.ForEach(cs=>cs);
            var dbclasssectionlst =
                await msectionRepository.FindAllAsync(s => s.SubscriberStandards.SubscriberId == subscriberId,
                    s =>
                        new
                        {
                            Class = s.SubscriberStandards.Standard.Name,
                            SectionName = s.SubscriberSection.Section.Name
                        });

            var newclasssection= uniqueclasssectionlst.Where(c => dbclasssectionlst.Contains(c) == false).AsParallel().ToList();

        }

    }
}
