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
        public virtual DbSet<ClientMaster> ClientMasters { get; set; }
        public virtual DbSet<company> companies { get; set; }
        public virtual DbSet<customer> customers { get; set; }
        public virtual DbSet<department> departments { get; set; }
        public virtual DbSet<function> functions { get; set; }
        public virtual DbSet<function_setting> function_setting { get; set; }
        public virtual DbSet<group_role> group_role { get; set; }
        public virtual DbSet<notification> notifications { get; set; }
        public virtual DbSet<order_product> order_product { get; set; }
        public virtual DbSet<order_service> order_service { get; set; }
        public virtual DbSet<package> packages { get; set; }
        public virtual DbSet<position> positions { get; set; }
        public virtual DbSet<product> products { get; set; }
        public virtual DbSet<project> projects { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<service> services { get; set; }
        public virtual DbSet<staff> staffs { get; set; }
        public virtual DbSet<task> tasks { get; set; }
    }
}
