using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLibrary
{
    [Table("Class")]
    public partial class Standard
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public virtual Collection<Section> Sections { get; set; }
        
    }
}
