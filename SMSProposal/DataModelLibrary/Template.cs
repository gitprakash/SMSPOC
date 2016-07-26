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
    [Table("Template")]
    public partial class Template
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
		[Required]
		public bool Action{get;set;}
		[Required]
		public DateTime CreatedAt{get;set;}
		[Required]
		public long SubcriberId{get;set;}
		public virtual Subscriber Subscriber{get;set;}

	   
    }
}
