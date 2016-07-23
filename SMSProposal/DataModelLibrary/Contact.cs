using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLibrary
{
    [Table("Contact")]
    public partial class Contact
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Required]
        public string RollNo { get; set; }

        public string BloodGroup { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [Required]
        public long Mobile { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        [StringLength(20,MinimumLength=1)]
        public string Class { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string Section { get; set; }
        [Required]
        public int SubscriberId { get; set; }
        public virtual Subscriber Subscriber { get; set; }
        
    }
}
