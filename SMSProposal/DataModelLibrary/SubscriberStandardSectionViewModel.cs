using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLibrary
{
    public class SubscriberStandardSectionViewModel
    {
        public int SubscriberStandardId { get; set; }
        public int SubscriberStandardSectionId { get; set; }
        public string SectionName { get; set; }
        public bool Active { get; set; }
    }
}
