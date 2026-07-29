using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using Framework48Mvc.Services;

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
            container.RegisterType<IHomeService, HomeService>();
        }
    }
}