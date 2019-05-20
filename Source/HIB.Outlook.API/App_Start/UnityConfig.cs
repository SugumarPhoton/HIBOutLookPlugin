using HIB.Outlook.BAL.Managers;
using HIB.Outlook.BAL.Managers.Interfaces;
using HIB.Outlook.BAL.Repository;
using HIB.Outlook.BAL.Repository.Interfaces;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace HIB.Outlook.API
{
    public static class UnityConfig
    {
        public static void RegisterComponents(HttpConfiguration config)
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            // GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

            container.RegisterType<IActivityRepository, ActivityRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IActivities, Activities>(new HierarchicalLifetimeManager());

            container.RegisterType<IClientRepository, ClientRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IClients, Clients>(new HierarchicalLifetimeManager());

            container.RegisterType<IFavouriteRepository, FavouriteRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IFavourites, Favourites>(new HierarchicalLifetimeManager());
          

            container.RegisterType<IFolderRepository, FolderRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IFolders, Folders>(new HierarchicalLifetimeManager());

            container.RegisterType<ILogRepository, LogRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ILogs, Logs>(new HierarchicalLifetimeManager());

            container.RegisterType<IPolicyLineTypeRepository, PolicyLineTypeRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IPolicyLineTypes, PolicyLineTypes>(new HierarchicalLifetimeManager());

            config.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}