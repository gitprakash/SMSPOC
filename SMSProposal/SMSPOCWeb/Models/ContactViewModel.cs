namespace SMSPOCWeb.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class ContactViewModel
    {
        
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"^((\+91-?)|0)?[0-9]{10}$", ErrorMessage = "Please enter 10 digit Mobile Number")]
        public long Mobile { get; set; }

        [Required(AllowEmptyStrings = false)] 
        [StringLength(20, MinimumLength = 1)]
        public string Class { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 1)]
        public string Section { get; set; }

    }
}

