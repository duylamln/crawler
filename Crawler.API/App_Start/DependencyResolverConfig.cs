using Crawler.API.Interfaces;
using Crawler.API.Services;
using Crawler.API.Shared;
using Unity;

namespace Crawler.API.App_Start
{
    public static class DependencyResolverConfig
    {
        public static UnityResolver Config()
        {
            RegisterTypeToContainer(out UnityContainer container);
            return new UnityResolver(container);
        }

        private static void RegisterTypeToContainer(out UnityContainer container)
        {
            container = new UnityContainer();
            container.RegisterType<IHttpClientService, HttpClientService>();
        }
    }
}