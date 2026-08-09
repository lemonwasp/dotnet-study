using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using Framework48Mvc.Services;
using Framework48Mvc.Repositories;
using Unity.Lifetime;
using Framework48Mvc.Data;
using AutoMapper;
using Framework48Mvc.Mappings;
using Microsoft.Extensions.Logging;

namespace Framework48Mvc
{
    public static class UnityConfig 
    {
        public static IUnityContainer Container { get; } = CreateContainer();
        private static IUnityContainer CreateContainer()
        {
            var container = new UnityContainer();

            RegisterTypes(container);

            return container;
        }
        public static void RegisterTypes(IUnityContainer container)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
            });

            var mapperConfig = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile<MessageProfile>();
                },
                loggerFactory);

            var mapper = mapperConfig.CreateMapper();

            container.RegisterInstance<IMapper>(mapper);
            //container.RegisterType<IHomeRepository, HomeRepository>(
            //    new ContainerControlledLifetimeManager()
            //);
            container.RegisterType<ApplicationDbContext>();
            container.RegisterType<IHomeRepository, EntityFrameworkHomeRepository>();
            container.RegisterType<IHomeService, HomeService>();
        }
    }
}