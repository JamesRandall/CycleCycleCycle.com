using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CycleCycleCycle.Controllers;
using CycleCycleCycle.Models;
using CycleCycleCycle.Services;
using CycleCycleCycle.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Route = CycleCycleCycle.Models.Route;

namespace CycleCycleCycle.Tests.Controllers
{
    [TestClass]
    public class RouteControllerTests
    {
        [TestMethod]
        public void Test_Constructor()
        {
            // Arrange
            IAccountService accountService = MockRepository.GenerateStub<IAccountService>();
            IRouteService routeService = MockRepository.GenerateStub<IRouteService>();
            IFavouriteService favouriteService = MockRepository.GenerateStub<IFavouriteService>();

            // Act
            new RouteController(accountService, routeService, favouriteService);

            // Assert
        }

        [TestMethod]
        public void Test_Index()
        {
            // Arrange
            TopRoutes topRoutes = new TopRoutes();
            IAccountService accountService = MockRepository.GenerateStub<IAccountService>();
            IRouteService routeService = MockRepository.GenerateStub<IRouteService>();
            IFavouriteService favouriteService = MockRepository.GenerateStub<IFavouriteService>();
            routeService.Stub(s => s.TopRoutes(Arg<Account>.Is.Anything)).Return(topRoutes);
            RouteController controller = new RouteController(accountService, routeService, favouriteService);

            // Act
            ViewResult result = controller.Index();

            // Assert
            Assert.AreSame(topRoutes, result.Model);
        }

        [TestMethod]
        public void Test_Details_UnauthorizedRequest()
        {
            // Arrange
            bool isFavourited;
            bool isOwner;
            Route route = new Route();
            IAccountService accountService = MockRepository.GenerateStub<IAccountService>();
            IRouteService routeService = MockRepository.GenerateStub<IRouteService>();
            IFavouriteService favouriteService = MockRepository.GenerateStub<IFavouriteService>();
            routeService.Stub(s => s.Details(Arg<int>.Is.Anything, Arg<int?>.Is.Anything, out Arg<bool>.Out(true).Dummy, out Arg<bool>.Out(false).Dummy)).Return(route);
            RouteController controller = new RouteController(accountService, routeService, favouriteService);
            HttpRequestBase requestBase = MockRepository.GenerateStub<HttpRequestBase>();
            HttpContextBase contextBase = MockRepository.GenerateStub<HttpContextBase>();
            contextBase.Stub(s => s.Request).Return(requestBase);
            controller.ControllerContext = new ControllerContext(contextBase, new RouteData(), controller);

            // Act
            ViewResult result = controller.Details(1);

            // Assert
            Assert.AreSame(result.Model, route);
            Assert.IsTrue(result.ViewBag.IsFavourited);
            Assert.IsFalse(result.ViewBag.IsOwner);
            routeService.AssertWasCalled(s => s.Details(Arg<int>.Is.Equal(1), Arg<int?>.Is.Null, out Arg<bool>.Out(true).Dummy, out Arg<bool>.Out(false).Dummy));
        }

        [TestMethod]
        public void Test_Details_AuthorizedRequest()
        {
            // Arrange
            bool isFavourited;
            bool isOwner;
            Route route = new Route();
            IAccountService accountService = MockRepository.GenerateStub<IAccountService>();
            IRouteService routeService = MockRepository.GenerateStub<IRouteService>();
            IFavouriteService favouriteService = MockRepository.GenerateStub<IFavouriteService>();
            routeService.Stub(s => s.Details(Arg<int>.Is.Anything, Arg<int?>.Is.Anything, out Arg<bool>.Out(false).Dummy, out Arg<bool>.Out(true).Dummy)).Return(route);
            RouteController controller = new RouteController(accountService, routeService, favouriteService);
            HttpRequestBase requestBase = MockRepository.GenerateStub<HttpRequestBase>();
            HttpContextBase contextBase = MockRepository.GenerateStub<HttpContextBase>();
            requestBase.Stub(s => s.IsAuthenticated).Return(true);
            contextBase.Stub(s => s.Request).Return(requestBase);
            controller.ControllerContext = new ControllerContext(contextBase, new RouteData(), controller);
            controller.Account = new Account {AccountID = 2};

            // Act
            ViewResult result = controller.Details(1);

            // Assert
            Assert.AreSame(result.Model, route);
            Assert.IsFalse(result.ViewBag.IsFavourited);
            Assert.IsTrue(result.ViewBag.IsOwner);
            routeService.AssertWasCalled(s => s.Details(Arg<int>.Is.Equal(1), Arg<int?>.Is.Equal(2), out Arg<bool>.Out(false).Dummy, out Arg<bool>.Out(true).Dummy));
        }

        [TestMethod]
        public void Test_PointDetails()
        {
            // Arrange
            DateTime dt = new DateTime(2012, 1, 2);
            List<RoutePoint> routePoints = new List<RoutePoint>
                                               {
                                                   new RoutePoint
                                                       {
                                                           Elevation = 1,
                                                           Latitude = 2,
                                                           Longitude = 3,
                                                           RouteID = 4,
                                                           RoutePointID = 5,
                                                           SequenceIndex = 6,
                                                           TimeRecorded = dt
                                                       }
                                               };
            IAccountService accountService = MockRepository.GenerateStub<IAccountService>();
            IRouteService routeService = MockRepository.GenerateStub<IRouteService>();
            IFavouriteService favouriteService = MockRepository.GenerateStub<IFavouriteService>();
            routeService.Stub(s => s.RoutePoints(
                Arg<int>.Is.Anything,
                Arg<string>.Is.Anything,
                Arg<string>.Is.Anything,
                Arg<int>.Is.Anything,
                Arg<int>.Is.Anything,
                out Arg<int>.Out(1).Dummy,
                out Arg<int>.Out(2).Dummy)).Return(routePoints);
            RouteController controller = new RouteController(accountService, routeService, favouriteService);

            // Act
            JsonResult result = controller.PointDetails("", "", 1, 2, 3);

            // Assert
            dynamic jsonData = result.Data;
            Assert.AreEqual(1, jsonData.total);
            Assert.AreEqual(2, jsonData.records);
            Assert.AreEqual(1, jsonData.page);
            Assert.AreEqual(1, jsonData.rows.Length);
            Assert.AreEqual(5, jsonData.rows[0].id);
            Assert.AreEqual("1/2/2012 12:00:00 AM", jsonData.rows[0].cell[0]);
            Assert.AreEqual("1", jsonData.rows[0].cell[1]);
            Assert.AreEqual("2", jsonData.rows[0].cell[2]);
            Assert.AreEqual("3", jsonData.rows[0].cell[3]);
        }

        [TestMethod]
        public void Test_Edit_Get_Authorized()
        {
            // Arrange
            Route route = new Route();
            IAccountService accountService = MockRepository.GenerateStub<IAccountService>();
            IRouteService routeService = MockRepository.GenerateStub<IRouteService>();
            IFavouriteService favouriteService = MockRepository.GenerateStub<IFavouriteService>();
            routeService.Stub(s => s.Details(Arg<int>.Is.Anything, Arg<int?>.Is.Anything, out Arg<bool>.Out(true).Dummy, out Arg<bool>.Out(true).Dummy)).Return(route);
            RouteController controller = new RouteController(accountService, routeService, favouriteService);
            controller.Account = new Account {AccountID = 1};

            // Act
            ViewResult result = (ViewResult)controller.Edit(1);

            // Assert
            Assert.AreSame(route, result.Model);
        }

        [TestMethod]
        public void Test_Edit_Get_UnauthorizedRedirects()
        {
            // Arrange
            Route route = new Route();
            IAccountService accountService = MockRepository.GenerateStub<IAccountService>();
            IRouteService routeService = MockRepository.GenerateStub<IRouteService>();
            IFavouriteService favouriteService = MockRepository.GenerateStub<IFavouriteService>();
            routeService.Stub(s => s.Details(Arg<int>.Is.Anything, Arg<int?>.Is.Anything, out Arg<bool>.Out(true).Dummy, out Arg<bool>.Out(false).Dummy)).Return(route);
            RouteController controller = new RouteController(accountService, routeService, favouriteService);
            controller.Account = new Account { AccountID = 1 };

            // Act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Edit(1);

            // Assert
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Unauthorised", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Test_Edit_PostSaves()
        {
            
        }
    }
}
