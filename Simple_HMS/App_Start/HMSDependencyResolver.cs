using Autofac;
using Autofac.Integration.Mvc;
using Simple_HMS.Concrete;
using Simple_HMS.Interface;
using System.Configuration;
using System.Web.Mvc;

namespace Simple_HMS
{
    public class HMSDependencyResolver
    {
        private static IContainer Container;

        public static void Initialize()
        {
            Initialize(RegisterServices(new ContainerBuilder()));
        }

        private static void Initialize(IContainer container)
        {
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            var connString = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;

            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();

            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .WithParameter("connString", connString)
                .InstancePerLifetimeScope();

            builder.RegisterType<PatientRepository>()
                .As<IPatientRepository>()
                .WithParameter("connString", connString)
                .InstancePerRequest();

            Container = builder.Build();

            return Container;
        }
    }
}