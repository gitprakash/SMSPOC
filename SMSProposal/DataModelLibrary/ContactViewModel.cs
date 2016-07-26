﻿namespace DataModelLibrary
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class ContactViewModel
    {
        
        public long Id { get; set; }

        public long SubscriberContactId { get; set; }

        public int SubscriberStandardId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(200)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"^((\+91-?)|0)?[0-9]{10}$", ErrorMessage = "Please enter 10 digit Mobile Number")]
        public long Mobile { get; set; }

        [Required(AllowEmptyStrings = false)] 
        [StringLength(20, MinimumLength = 1)]
        public string Class { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 1)]
        public string Section { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string RollNo { get; set; }

        public string BloodGroup { get; set; }

    }
}

