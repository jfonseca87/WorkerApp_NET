using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using WorkerApp.Repository.Interfaces;
using WorkerApp.Repository.MongoImplementation;
using WorkerApp.Service.Interfaces;
using WorkerApp.Service.ServicesImplementations;

namespace WorkerApp.MVC.App_Start
{
    public static class IocConfiguration
    {
        public static void Configure() 
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterSource(new ViewRegistrationSource());

            // Registration of repositories
            RegisterRepositories(builder);

            // Registration of services
            RegisterServices(builder);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        public static void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<FileRepository>().As<IFileRepository>();
            builder.RegisterType<PersonRepository>().As<IPersonRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
        }

        public static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<FileAttachedService>().As<IFileAttachedService>();
            builder.RegisterType<FileService>().As<IFileService>();
            builder.RegisterType<PersonService>().As<IPersonService>();
            builder.RegisterType<UserService>().As<IUserService>();
        }
    }
}