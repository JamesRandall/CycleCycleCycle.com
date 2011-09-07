using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CycleCycleCycle.Models;
using CycleCycleCycle.Models.Repositories;
using CycleCycleCycle.Services.Implementation;
using CycleCycleCycle.Tests.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace CycleCycleCycle.Tests.Services
{
    [TestClass]
    public class RideServiceTests
    {
        [TestMethod]
        public void Test_Create_SucceedsWithTimeTaken()
        {
            // Arrange
            IUnitOfWork stubUnitOfWork = MockRepository.GenerateStub<IUnitOfWork>();
            StubRepository<Ride> stubRideRepository = new StubRepository<Ride>();
            stubUnitOfWork.Stub(s => s.GetRepository<Ride>()).Return(stubRideRepository);
            RideService service = new RideService(stubUnitOfWork);
            DateTime dateRidden = new DateTime(2011, 01, 01);

            // Act
            bool result = service.Create(1, 1, dateRidden, 1, 2, 3);

            // Assert
            Ride ride = stubRideRepository.InsertItems[0];
            Assert.IsTrue(result);
            Assert.AreEqual(1, ride.RouteID);
            Assert.AreEqual(dateRidden, ride.TimeOfRide);
            Assert.AreEqual(new TimeSpan(0, 1, 2, 3), ride.TimeTaken);
            Assert.AreEqual(1, ride.AccountID);
            stubUnitOfWork.AssertWasCalled(uw => uw.Save());
        }

        [TestMethod]
        public void Test_Create_SucceedsWithNoTimeTaken()
        {
            // Arrange
            IUnitOfWork stubUnitOfWork = MockRepository.GenerateStub<IUnitOfWork>();
            StubRepository<Ride> stubRideRepository = new StubRepository<Ride>();
            stubUnitOfWork.Stub(s => s.GetRepository<Ride>()).Return(stubRideRepository);
            RideService service = new RideService(stubUnitOfWork);
            DateTime dateRidden = new DateTime(2011, 01, 01);

            // Act
            bool result = service.Create(1, 1, dateRidden, null, null, null);

            // Assert
            Ride ride = stubRideRepository.InsertItems[0];
            Assert.IsTrue(result);
            Assert.AreEqual(1, ride.RouteID);
            Assert.AreEqual(dateRidden, ride.TimeOfRide);
            Assert.AreEqual(null, ride.TimeTaken);
            Assert.AreEqual(1, ride.AccountID);
            stubUnitOfWork.AssertWasCalled(uw => uw.Save());
        }

        [TestMethod]
        public void Test_Create_ReturnsFalseForInvalidHours()
        {
            // Arrange
            IUnitOfWork stubUnitOfWork = MockRepository.GenerateStub<IUnitOfWork>();
            RideService controller = new RideService(stubUnitOfWork);
            DateTime dateRidden = new DateTime(2011, 01, 01);

            // Act
            bool result = controller.Create(1, 1, dateRidden, -1, 0, 0);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Test_Create_ReturnsFalseForInvalidMinutes()
        {
            // Arrange
            IUnitOfWork stubUnitOfWork = MockRepository.GenerateStub<IUnitOfWork>();
            RideService controller = new RideService(stubUnitOfWork);
            DateTime dateRidden = new DateTime(2011, 01, 01);

            // Act
            bool result = controller.Create(1, 1, dateRidden, 0, -1, 0);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Test_Create_ReturnsFalseForInvalidSeconds()
        {
            // Arrange
            IUnitOfWork stubUnitOfWork = MockRepository.GenerateStub<IUnitOfWork>();
            RideService controller = new RideService(stubUnitOfWork);
            DateTime dateRidden = new DateTime(2011, 01, 01);

            // Act
            bool result = controller.Create(1, 1, dateRidden, 0, 0, -1);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Test_Create_ThrowsServiceExceptionOnUnitOfWorkError()
        {
            // Arrange
            IUnitOfWork stubUnitOfWork = MockRepository.GenerateStub<IUnitOfWork>();
            StubRepository<Ride> stubRideRepository = new StubRepository<Ride>();
            stubUnitOfWork.Stub(s => s.GetRepository<Ride>()).Return(stubRideRepository);
            stubUnitOfWork.Stub(s => s.Save()).Throw(new Exception());
            RideService service = new RideService(stubUnitOfWork);
            DateTime dateRidden = new DateTime(2011, 01, 01);
            bool exceptionThrown = false;

            // Act
            try
            {
                service.Create(1, 1, dateRidden, null, null, null);
            }
            catch (ServiceException)
            {
                exceptionThrown = true;
            }
            

            // Assert
            Assert.IsTrue(exceptionThrown);
        }
    }
}
