namespace ERP.Data.ModelsERP
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model2 : DbContext
    {
        public Model2()
            : base("name=Model2")
        {
        }

        public virtual DbSet<customer_group> customer_group { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<customer_group>()
                .Property(e => e.cg_thumbnail)
                .IsFixedLength();

            modelBuilder.Entity<customer_group>()
                .Property(e => e.cg_description)
                .IsUnicode(false);
        }
    }
}
