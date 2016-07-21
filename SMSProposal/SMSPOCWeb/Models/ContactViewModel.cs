namespace SMSPOCWeb.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class ContactViewModel
    {
        
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        [System.Web.Mvc.Remote("IsUserNameExists", "Contact", HttpMethod="Post")]
        public string Name { get; set; }

        [StringLength(200)]
        [EmailAddress]
        [System.Web.Mvc.Remote("IsUserEmailExists", "Contact", HttpMethod = "Post")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"^((\+91-?)|0)?[0-9]{10}$", ErrorMessage = "Please enter 10 digit Mobile Number")]
        [System.Web.Mvc.Remote("IsUniqueMobile", "Contact", HttpMethod = "Post")]
        public long Mobile { get; set; }


    }
}

