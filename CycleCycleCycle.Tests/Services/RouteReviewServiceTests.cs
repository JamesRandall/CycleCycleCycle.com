using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using CycleCycleCycle.Models;
using CycleCycleCycle.Models.Repositories;
using CycleCycleCycle.Services.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace CycleCycleCycle.Tests.Services
{
    [TestClass]
    public class RouteReviewServiceTests
    {
        [TestMethod]
        public void Test_Constructor()
        {
            // Arrange
            IUnitOfWork unitOfWork = MockRepository.GenerateStub<IUnitOfWork>();

            // Act
            new RouteReviewService(unitOfWork);

            // Assert
        }

        [TestMethod]
        public void Test_IsRouteReviewedByUser_ReturnsTrue()
        {
            // Arrange
            List<RouteReview> testData = new List<RouteReview>
                                             {
                                                 new RouteReview
                                                     {
                                                         AccountID = 1,
                                                         RouteID = 2
                                                     }
                                             };
            IUnitOfWork unitOfWork = MockRepository.GenerateStub<IUnitOfWork>();
            IRepository<RouteReview> routeReviewRepository = MockRepository.GenerateStub<IRepository<RouteReview>>();
            unitOfWork.Stub(s => s.GetRepository<RouteReview>()).Return(routeReviewRepository);
            routeReviewRepository.Stub(s => s.All).Return(testData.AsQueryable());
            RouteReviewService service = new RouteReviewService(unitOfWork);

            // Act
            bool result = service.IsRouteReviewedByUser(2, 1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Test_IsRouteReviewedByUser_ReturnsFalse()
        {
            // Arrange
            List<RouteReview> testData = new List<RouteReview>
                                             {
                                                 new RouteReview
                                                     {
                                                         AccountID = 1,
                                                         RouteID = 2
                                                     }
                                             };
            IUnitOfWork unitOfWork = MockRepository.GenerateStub<IUnitOfWork>();
            IRepository<RouteReview> routeReviewRepository = MockRepository.GenerateStub<IRepository<RouteReview>>();
            unitOfWork.Stub(s => s.GetRepository<RouteReview>()).Return(routeReviewRepository);
            routeReviewRepository.Stub(s => s.All).Return(testData.AsQueryable());
            RouteReviewService service = new RouteReviewService(unitOfWork);

            // Act
            bool result = service.IsRouteReviewedByUser(1, 2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Test_Insert_Succeeds()
        {
            // Arrange
            List<RouteReview> testData = new List<RouteReview>
                                             {
                                                 new RouteReview
                                                     {
                                                         AccountID = 1,
                                                         RouteID = 2,
                                                         Rating = 2
                                                     }
                                             };
            Route route = new Route {RouteID = 2};
            IUnitOfWork unitOfWork = MockRepository.GenerateStub<IUnitOfWork>();
            IRepository<RouteReview> routeReviewRepository = MockRepository.GenerateStub<IRepository<RouteReview>>();
            IRepository<Route> routeRepository = MockRepository.GenerateStub<IRepository<Route>>();
            unitOfWork.Stub(s => s.GetRepository<RouteReview>()).Return(routeReviewRepository);
            unitOfWork.Stub(s => s.GetRepository<Route>()).Return(routeRepository);
            routeRepository.Stub(s => s.Find(Arg<int>.Is.Anything)).Return(route);
            routeReviewRepository.Stub(s => s.All).Return(testData.AsQueryable());
            RouteReview review = new RouteReview
                                     {
                                         Rating = 4,
                                         DateCreated = DateTime.MinValue,
                                         RouteID = 2
                                     };
            RouteReviewService service = new RouteReviewService(unitOfWork);

            // Act
            RouteReview result = service.Insert(review);

            // Assert
            Assert.IsTrue(result.DateCreated != DateTime.MinValue);
            routeReviewRepository.AssertWasCalled(s => s.Insert(Arg<RouteReview>.Is.Anything));
            routeRepository.AssertWasCalled(s => s.Update(Arg<Route>.Is.Anything));
            unitOfWork.AssertWasCalled(s => s.Save());
        }

        [TestMethod]
        public void Test_Insert_SetsAverageRating()
        {
            // Arrange
            List<RouteReview> testData = new List<RouteReview>
                                             {
                                                 new RouteReview
                                                     {
                                                         AccountID = 1,
                                                         RouteID = 2,
                                                         Rating = 2
                                                     }
                                             };
            Route route = new Route { RouteID = 2 };
            IUnitOfWork unitOfWork = MockRepository.GenerateStub<IUnitOfWork>();
            IRepository<RouteReview> routeReviewRepository = MockRepository.GenerateStub<IRepository<RouteReview>>();
            IRepository<Route> routeRepository = MockRepository.GenerateStub<IRepository<Route>>();
            unitOfWork.Stub(s => s.GetRepository<RouteReview>()).Return(routeReviewRepository);
            unitOfWork.Stub(s => s.GetRepository<Route>()).Return(routeRepository);
            routeRepository.Stub(s => s.Find(Arg<int>.Is.Anything)).Return(route);
            routeReviewRepository.Stub(s => s.All).Return(testData.AsQueryable());
            RouteReview review = new RouteReview
            {
                Rating = 4,
                DateCreated = DateTime.MinValue,
                RouteID = 2
            };
            RouteReviewService service = new RouteReviewService(unitOfWork);

            // Act
            service.Insert(review);

            // Assert
            Assert.AreEqual(3, route.AverageRating);
        }

        [TestMethod]
        public void Test_Insert_ThrowsServiceExceptionOnUnitOfWorkError()
        {
            // Arrange
            List<RouteReview> testData = new List<RouteReview>
                                             {
                                                 new RouteReview
                                                     {
                                                         AccountID = 1,
                                                         RouteID = 2,
                                                         Rating = 2
                                                     }
                                             };
            Route route = new Route { RouteID = 2 };
            IUnitOfWork unitOfWork = MockRepository.GenerateStub<IUnitOfWork>();
            IRepository<RouteReview> routeReviewRepository = MockRepository.GenerateStub<IRepository<RouteReview>>();
            IRepository<Route> routeRepository = MockRepository.GenerateStub<IRepository<Route>>();
            unitOfWork.Stub(s => s.GetRepository<RouteReview>()).Return(routeReviewRepository);
            unitOfWork.Stub(s => s.GetRepository<Route>()).Return(routeRepository);
            unitOfWork.Stub(s => s.Save()).Throw(new Exception());
            routeRepository.Stub(s => s.Find(Arg<int>.Is.Anything)).Return(route);
            routeReviewRepository.Stub(s => s.All).Return(testData.AsQueryable());
            RouteReview review = new RouteReview
            {
                Rating = 4,
                DateCreated = DateTime.MinValue,
                RouteID = 2
            };
            RouteReviewService service = new RouteReviewService(unitOfWork);
            bool exceptionThrown = false;

            // Act
            try
            {
                service.Insert(review);
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
