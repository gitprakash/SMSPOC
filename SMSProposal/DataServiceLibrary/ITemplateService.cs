﻿using DataModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public interface ITemplateService
    {
        Task<ICollection<TemplateViewModel>> GetPagedTemplates(int subcriberId, int skip, int pagesize, string ordercolumn, bool desc);
        Task<ICollection<TemplateViewModel>> GetTemplates(int subcriberId);
        Task<int> TotalTemplates(int subcriberId);
        Task<SubscriberTemplate> FindTemplate(int subcriberId,int templateId=0, string templatename=null);
        Task<SubscriberTemplate> AddTemplate(TemplateViewModel templateViewModel,int subscirberId);
        Task<int> SaveAsync();
    }
}