using System.Runtime.Remoting;

namespace DataModelLibrary
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;
    [Table("Subscriber")]
    public partial class Subscriber
    {
        [Key]
        [DatabaseGenerated((DatabaseGeneratedOption.Identity))]
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        [Remote("IsUserNameExists", "Account", HttpMethod = "POST", ErrorMessage = "UserName already exists")]
        public string Username { get; set; }
        [Required]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
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
        [Remote("IsUserEmailExists", "Account", HttpMethod = "POST", ErrorMessage = "Email already exists")]
        public string Email { get; set; }
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
        public int AccountTypeId { get; set; }
        public virtual AccountType AccountType { get; set; }
        public int GenderTypeId { get; set; }
        public virtual GenderType GenderType { get; set; }

    }
}
