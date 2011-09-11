using System.Web.Mvc;
using CycleCycleCycle.Cacheing;
using CycleCycleCycle.Models;
using CycleCycleCycle.Models.Repositories;
using CycleCycleCycle.Services;
using CycleCycleCycle.Services.Implementation;
using CycleCycleCycle.Services.RouteCreators;
using CycleCycleCycle.Services.Utilities;
using CycleCycleCycle.ViewModels.Mappers;
using Microsoft.Practices.Unity;
using Unity.Mvc3;

namespace CycleCycleCycle.App_Start
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // repositories   
            container.RegisterType<IDbContextFactory, CycleContextFactory>();
            container.RegisterType<IUnitOfWork, EntityFrameworkUnitOfWork>();
            container.RegisterType<IRepository<Account>, EntityFrameworkRepository<Account>>();
            container.RegisterType<IRepository<Route>, EntityFrameworkRepository<Route>>();
            container.RegisterType<IRepository<RoutePoint>, EntityFrameworkRepository<RoutePoint>>();

            // services
            container.RegisterType<IRouteService, RouteService>();
            container.RegisterType<IRideService, RideService>();
            container.RegisterType<IAccountService, AccountService>();
            container.RegisterType<IFavouriteService, FavouriteService>();
            container.RegisterType<IRouteReviewService, RouteReviewService>();
            container.RegisterType<IGeocodeService, YahooGeocodeService>();

            // utilities
            container.RegisterType<IDistanceCalculator, HaversineDistanceCalculator>();
            container.RegisterType<IHeightMapImageBuilder, HeightMapImageBuilder>();
            container.RegisterType<IHeightMapImageCache, HeightMapImageCache>();
            container.RegisterType<IRouteCreator, GpxRouteCreator>();
            container.RegisterType<ISortSecurity, SortSecurity>();

            // type mappers
            container.RegisterType<IRouteToRouteResultMapper, RouteToRouteResultMapper>();

            container.RegisterControllers();

            return container;
        }
    }
}