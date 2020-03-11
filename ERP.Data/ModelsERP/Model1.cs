namespace ERP.Data.ModelsERP
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<staff> staffs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<staff>()
                .Property(e => e.sta_code)
                .IsUnicode(false);

            modelBuilder.Entity<staff>()
                .Property(e => e.sta_thumbnai)
                .IsUnicode(false);

            modelBuilder.Entity<staff>()
                .Property(e => e.sta_username)
                .IsUnicode(false);

            modelBuilder.Entity<staff>()
                .Property(e => e.sta_password)
                .IsUnicode(false);

            modelBuilder.Entity<staff>()
                .Property(e => e.sta_email)
                .IsUnicode(false);

            modelBuilder.Entity<staff>()
                .Property(e => e.sta_mobile)
                .IsUnicode(false);

            modelBuilder.Entity<staff>()
                .Property(e => e.sta_identity_card)
                .IsUnicode(false);

            modelBuilder.Entity<staff>()
                .Property(e => e.sta_note)
                .IsUnicode(false);
        }
    }
}
