using System.Runtime.Remoting;

namespace DataModelLibrary
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;
    public  class SubscriberModel
    {
         
        public string Username { get; set; }
        
        public string Password { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Email { get; set; }
        //public bool Active { get; set; }
        //[Column(TypeName = "date")]
        //public DateTime CreatedDate { get; set; }
        //[Required]
        //[StringLength(50)]
        //public string CreatedBy { get; set; }
        //[Column(TypeName = "date")]
        //public DateTime ModifiedDate { get; set; }
        //[Required]
        //[StringLength(50)]
        //public string ModifiedBy { get; set; }
        //[DataType(DataType.PhoneNumber)]
        //public int Mobile { get; set; }
        //public int AccountTypeId { get; set; }
        //public virtual AccountType AccountType { get; set; }
        //public int GenderTypeId { get; set; }
        //public virtual GenderType GenderType { get; set; }

    }
}
