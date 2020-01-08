namespace ERP.Data.DbContext
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using ERP.Data.ModelsERP;

    public partial class ERPDbContext : DbContext
    {
        public ERPDbContext()
            : base("name=ERPDbContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public ERPDbContext(string connectionString) : base(connectionString)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public static ERPDbContext Create()
        {
            return new ERPDbContext();
        }
        public virtual DbSet<staff> staffs { get; set; }
    }
}
