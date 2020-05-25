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
        public virtual DbSet<relatives_staff> relatives_staff { get; set; }
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
        public virtual DbSet<service_time> service_time { get; set; }
        public virtual DbSet<service_category> service_category { get; set; }
        public virtual DbSet<email> emails { get; set; }
        public virtual DbSet<email_strategy> email_strategy { get; set; }
        public virtual DbSet<email_template> email_template { get; set; }
        public virtual DbSet<field> fields { get; set; }
        public virtual DbSet<field_template> field_template { get; set; }
        public virtual DbSet<sms> sms { get; set; }
        public virtual DbSet<sms_strategy> sms_strategy { get; set; }
        public virtual DbSet<sms_template> sms_template { get; set; }
        public virtual DbSet<customer_order> customer_order { get; set; }
        public virtual DbSet<product_category> product_category { get; set; }
        public virtual DbSet<supplier> suppliers { get; set; }
        public virtual DbSet<supplier_type> supplier_type { get; set; }
        public virtual DbSet<source> sources { get; set; }
        public virtual DbSet<product_order> product_order { get; set; }
        public virtual DbSet<customer_group> customer_group { get; set; }
        public virtual DbSet<Province> province { get; set; }
        public virtual DbSet<District> district { get; set; }
        public virtual DbSet<Ward> ward { get; set; }
        public virtual DbSet<ship_address> ship_address { get; set; }
        public virtual DbSet<undertaken_location> undertaken_location { get; set; }
        public virtual DbSet<transaction> transactions { get; set; }
        public virtual DbSet<social> socials { get; set; }
        public virtual DbSet<executor> executors { get; set; }
        public virtual DbSet<training> trainings { get; set; }
        public virtual DbSet<training_staff> training_staffs { get; set; }
        public virtual DbSet<staff_work_time> staff_work_times { get; set; }
        public virtual DbSet<customer_phone> customer_phones { get; set; }
        public virtual DbSet<company_funtion> company_funtion { get; set; }
        public virtual DbSet<customer_type> customer_type { get; set; }
        public virtual DbSet<transaction_deal_type> transaction_deal_type { get; set; }
        public virtual DbSet<transaction_evaluate> transaction_evaluate { get; set; }
        public virtual DbSet<transaction_priority> transaction_priority { get; set; }
        public virtual DbSet<service_type> service_type { get; set; }
        public virtual DbSet<product_unit> product_unit { get; set; }
        public virtual DbSet<attachment> attachments { get; set; }
        public virtual DbSet<bank> banks { get; set; }
        public virtual DbSet<bank_branch> bank_branch { get; set; }
        public virtual DbSet<bank_category> bank_category { get; set; }
        public virtual DbSet<bonus_staff> bonus_staff { get; set; }
        public virtual DbSet<staff_brank> staff_brank { get; set; }
    }
}
