namespace DataModelLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Subscriber")]
    public partial class Subscriber
    {
        [Key]
        [DatabaseGenerated((DatabaseGeneratedOption.Identity))]
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

       [Required]
       [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
       [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$",ErrorMessage="Password should contain small letter Capital letter with integer")]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        [MinLength(3)]
        public string FirstName { get; set; }

        [StringLength(50)]
        [MinLength(3)]
        public string LastName { get; set; }

        [Required]
        [StringLength(200)]
        [EmailAddress]
        public string EmailID { get; set; }

        public bool Active { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedDate { get; set; }

        [Required]
        [StringLength(50)]
        public string ModifiedBy { get; set; }
        [DataType(DataType.PhoneNumber)]
        public int Mobile { get; set; }
        [Required]
        public int AccountTypeID { get; set; }
        public virtual AccountType AccountType { get; set; }
        [Required]
        public int GenderTypeID { get; set; }
        public virtual GenderType GenderType { get; set; }

    }
}
