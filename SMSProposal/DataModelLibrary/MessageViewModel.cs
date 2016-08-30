using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLibrary 
{
    public class MessageViewModel 
    {
        [Required] 
        public long Id { get; set; } 
       
        public string Name { get; set; }
        [Required]
        public long Mobile { get; set; }

        public string Standard { get; set; }

        public string Section { get; set; }

        public string RollNo { get; set; }

        public DateTime? Submittrequesttime { get; set; }
        public DateTime? Submittresponsetime { get; set; }

        public DateTime? DeliveryRequestedtime { get; set; }
        public DateTime? DeliveryResponsetime { get; set; }


        public string MessageError { get; set; }

        public string SubmitId { get; set; }

        public string DeliveryId { get; set; }

        public bool IsMessageSubmitted { get; set; }

        public bool IsMessageDelivered { get; set; }
    }
}
