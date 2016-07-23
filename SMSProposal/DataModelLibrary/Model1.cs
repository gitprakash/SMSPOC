namespace DataModelLibrary
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=POCDBconnection")
        {
            Database.SetInitializer(new DBInitializer());
           // this.Configuration.LazyLoadingEnabled = false; 
        }
        public virtual DbSet<GenderType> GenderTypes { get; set; }
        public virtual DbSet<AccountType> AccountTypes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Subscriber> Subscribers { get; set; }
        public virtual DbSet<SubscriberRoles> SubscriberRoles { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Standard> Standards { get; set; }
        public virtual DbSet<Section> Sections { get; set; }

         
    }
}
