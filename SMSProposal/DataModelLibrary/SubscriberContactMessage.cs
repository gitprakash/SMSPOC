using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLibrary
{
    [Table("SubscriberContactMessage")]
   public  class SubscriberContactMessage
    {
        [Key]
        public Guid Id { get; set; }
        public virtual SubscriberStandardContacts SubscriberContact { get; set; }
        [Required]
        public long SubscriberStandardContactsId { get; set; }
        public virtual Message Message { get; set; }
        [Required]
        public Guid MessageId { get; set; }
        public virtual MessageStatus MessageStatus { get; set; }
        [Required]
        public int MessageStatusId { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreateDateTime { get; set; }
    }
}
