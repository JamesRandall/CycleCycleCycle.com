using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CycleCycleCycle.Controllers;
using CycleCycleCycle.Models;
using CycleCycleCycle.Models.Repositories;
using CycleCycleCycle.Services;
using CycleCycleCycle.ViewModels.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace CycleCycleCycle.Tests.Controllers
{
    [TestClass]
    public class ProfileControllerTests
    {
        [TestMethod]
        public void Test_Constructor()
        {
            // Arrange
            IAccountService accountService = MockRepository.GenerateStub<IAccountService>();
            IRouteToRouteResultMapper stubResultMapper = MockRepository.GenerateStub<IRouteToRouteResultMapper>();

            // Act
            new ProfileController(accountService, stubResultMapper);

            // Assert
        }

        [TestMethod]
        public void Test_Index()
        {
            // Arrange
            IAccountService accountService = MockRepository.GenerateStub<IAccountService>();
            IRouteToRouteResultMapper stubResultMapper = MockRepository.GenerateStub<IRouteToRouteResultMapper>();
            ProfileController controller = new ProfileController(accountService, stubResultMapper);
            controller.Account = new Account
                                     {
                                         AccountID = 1,
                                         Favourites = new List<Favourite>(),
                                         Rides = new List<Ride>(),
                                         RouteReviews = new List<RouteReview>(),
                                         Routes = new Collection<Route>()
                                     };

            // Act
            ViewResult result = (ViewResult) controller.Index();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
        }
    }
}
