using System;
using System.Web.Mvc;
using CycleCycleCycle.Controllers;
using CycleCycleCycle.Models;
using CycleCycleCycle.Models.Repositories;
using CycleCycleCycle.Services;
using CycleCycleCycle.Services.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace CycleCycleCycle.Tests.Controllers
{
    [TestClass]
    public class RideControllerTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            // Arrange
            IAccountService accountService = MockRepository.GenerateStub<IAccountService>();
            IRideService stubRideService = MockRepository.GenerateStub<IRideService>();

            // Act
            new RideController(accountService, stubRideService);

            // Assert
        }

        [TestMethod]
        public void Test_Create_ReturnsTrueOnSuccess()
        {
            // Arrange
            IAccountService accountService = MockRepository.GenerateStub<IAccountService>();
            IRideService stubRideService = MockRepository.GenerateStub<IRideService>();
            stubRideService.Stub(s => s.Create(0, 0, DateTime.Now, null, null, null)).IgnoreArguments().Return(true);
            RideController controller = new RideController(accountService, stubRideService);
            controller.Account = new Account
            {
                AccountID = 1
            };

            // Act
            JsonResult result = controller.Create(0, DateTime.Now, null, null, null);

            // Assert
            Assert.IsTrue((bool)result.Data);
        }

        [TestMethod]
        public void Test_Create_ReturnsFalseOnFailure()
        {
            // Arrange
            IAccountService accountService = MockRepository.GenerateStub<IAccountService>();
            IRideService stubRideService = MockRepository.GenerateStub<IRideService>();
            stubRideService.Stub(s => s.Create(0, 0, DateTime.Now, null, null, null)).IgnoreArguments().Return(false);
            RideController controller = new RideController(accountService, stubRideService);
            controller.Account = new Account
            {
                AccountID = 1
            };

            // Act
            JsonResult result = controller.Create(0, DateTime.Now, null, null, null);

            // Assert
            Assert.IsFalse((bool)result.Data);
        }

        [TestMethod]
        public void Test_Create_ReturnsFalseOnException()
        {
            // Arrange
            IAccountService accountService = MockRepository.GenerateStub<IAccountService>();
            IRideService stubRideService = MockRepository.GenerateStub<IRideService>();
            stubRideService.Stub(s => s.Create(0, 0, DateTime.Now, null, null, null)).IgnoreArguments().Throw(
                new ServiceException());
            RideController controller = new RideController(accountService, stubRideService);
            controller.Account = new Account
            {
                AccountID = 1
            };

            // Act
            JsonResult result = controller.Create(0, DateTime.Now, null, null, null);

            // Assert
            Assert.IsFalse((bool)result.Data);
        }
    }
}
