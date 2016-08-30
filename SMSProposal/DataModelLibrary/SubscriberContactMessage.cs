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
   public  class SubscriberContactMessage:BaseEntity
    {
        public virtual SubscriberStandardContacts SubscriberContact { get; set; }
        [Required]
        public long SubscriberStandardContactsId { get; set; }
        public virtual Message Message { get; set; }
        [Required] 
        public long MessageId { get; set; }
        [Required]
        public MessageStatusEnum MessageStatus { get; set; }
        public long? MessageErrorId { get; set; }
        public MessageError MessageError { get; set; }
        public string SubmitId { get; set; }
        public string DeliveryId  { get; set; }
        public DateTime? Submittrequesttime { get; set; }
        public DateTime? Submittresponsetime { get; set; } 
        public DateTime? DeliveryRequestedtime { get; set; }
        public DateTime? DeliveryResponsetime { get; set; }


    }
}
