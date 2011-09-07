using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CycleCycleCycle.Controllers;
using CycleCycleCycle.Models;
using CycleCycleCycle.Models.Repositories;
using CycleCycleCycle.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace CycleCycleCycle.Tests.Controllers
{
    [TestClass]
    public class RouteReviewControllerTests
    {
        [TestMethod]
        public void Test_Constructor()
        {
            // Arrange
            IAccountService accountService = MockRepository.GenerateStub<IAccountService>();
            IRouteService routeService = MockRepository.GenerateStub<IRouteService>();
            IRouteReviewService routeReviewService = MockRepository.GenerateStub<IRouteReviewService>();

            // Act
            new RouteReviewController(accountService, routeService, routeReviewService);

            // Assert
            
        }

        [TestMethod]
        public void Test_Create_Get()
        {
            const int routeId = 1;
            const string routeName = "Test Route";
            const int defaultRating = 3;

            // Arrange
            IAccountService accountService = MockRepository.GenerateStub<IAccountService>();
            IRouteService routeService = MockRepository.GenerateStub<IRouteService>();
            IRouteReviewService routeReviewService = MockRepository.GenerateStub<IRouteReviewService>();
            routeService.Stub(s => s.Details(routeId)).Return(new Route {Name = routeName});
            RouteReviewController controller = new RouteReviewController(accountService, routeService, routeReviewService);

            // Act
            ViewResult result = (ViewResult) controller.Create(1);

            // Assert
            RouteReview model = (RouteReview) result.Model;
            Assert.AreEqual(routeName, result.ViewBag.RouteName);
            Assert.AreEqual(defaultRating, model.Rating);
            Assert.AreEqual(routeId, model.RouteID);
            Assert.IsTrue(String.IsNullOrEmpty(model.Review));
        }

        [TestMethod]
        public void Test_Create_Post_InvalidModelState()
        {
            const string routeName = "Test Route";

            // Arrange
            IAccountService accountService = MockRepository.GenerateStub<IAccountService>();
            IRouteService routeService = MockRepository.GenerateStub<IRouteService>();
            IRouteReviewService routeReviewService = MockRepository.GenerateStub<IRouteReviewService>();
            routeService.Stub(s => s.Details(Arg<int>.Is.Anything)).Return(new Route { Name = routeName });
            RouteReviewController controller = new RouteReviewController(accountService, routeService, routeReviewService);
            RouteReview review = new RouteReview();
            controller.ModelState.AddModelError("key", "model is invalid");

            // Act
            ViewResult result = (ViewResult)controller.Create(review);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(routeName, result.ViewBag.RouteName);
        }

        [TestMethod]
        public void Test_Create_Post_SavesReview()
        {
            // Arrange
            RouteReview review = new RouteReview() { RouteID = 1, Rating = 3 };
            IAccountService accountService = MockRepository.GenerateStub<IAccountService>();
            IRouteService routeService = MockRepository.GenerateStub<IRouteService>();
            IRouteReviewService routeReviewService = MockRepository.GenerateStub<IRouteReviewService>();
            routeReviewService.Stub(s => s.IsRouteReviewedByUser(Arg<int>.Is.Anything, Arg<int>.Is.Anything)).Return(
                false);
            routeReviewService.Stub(s => s.Insert(Arg<RouteReview>.Is.Anything)).Return(review);
            routeService.Stub(s => s.Details(Arg<int>.Is.Anything)).Return(new Route { RouteID = 1 });
            RouteReviewController controller = new RouteReviewController(accountService, routeService, routeReviewService);
            controller.Account = new Account() {AccountID = 1};

            // Act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Create(review);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Route", result.RouteValues["controller"]);
            Assert.AreEqual("Details", result.RouteValues["action"]);
            Assert.AreEqual(1, result.RouteValues["id"]);
        }

        [TestMethod]
        public void Test_Create_Post_ReviewExistsAddsModelError()
        {
            // Arrange
            RouteReview review = new RouteReview() { RouteID = 1, Rating = 3 };
            IAccountService accountService = MockRepository.GenerateStub<IAccountService>();
            IRouteService routeService = MockRepository.GenerateStub<IRouteService>();
            IRouteReviewService routeReviewService = MockRepository.GenerateStub<IRouteReviewService>();
            routeReviewService.Stub(s => s.IsRouteReviewedByUser(Arg<int>.Is.Anything, Arg<int>.Is.Anything)).Return(
                true);
            RouteReviewController controller = new RouteReviewController(accountService, routeService, routeReviewService);
            controller.Account = new Account() { AccountID = 1 };

            // Act
            ViewResult result = (ViewResult)controller.Create(review);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ViewData.ModelState.Keys.Count);
            Assert.AreEqual(1, result.ViewData.ModelState[""].Errors.Count);
            Assert.AreEqual("You have already written a review for this route.", result.ViewData.ModelState[""].Errors[0].ErrorMessage);
        }
    }
}
