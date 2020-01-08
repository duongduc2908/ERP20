using Autofac;
using ERP.API.Modules;
using ERP.Data.DbContext;

namespace ERP.Modules
{
    public class ApplicaitonModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new AutoMapperModule());
            builder.RegisterType<ERPDbContext>().InstancePerLifetimeScope();
        }
    }
}