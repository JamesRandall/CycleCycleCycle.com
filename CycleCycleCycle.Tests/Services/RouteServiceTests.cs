using System;
using System.IO;
using System.Security;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CycleCycleCycle.Cacheing;
using CycleCycleCycle.Models;
using CycleCycleCycle.Models.Repositories;
using CycleCycleCycle.Services;
using CycleCycleCycle.Services.Implementation;
using CycleCycleCycle.Services.RouteCreators;
using CycleCycleCycle.Services.RouteFileCreator;
using CycleCycleCycle.Services.Utilities;
using CycleCycleCycle.ViewModels.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace CycleCycleCycle.Tests.Services
{
    [TestClass]
    public class RouteServiceTests
    {
        private IRouteCreator _routeCreator;
        private IHeightMapImageBuilder _heightMapImageBuilder;
        private IHeightMapImageCache _heightMapImageCache;
        private IGeocodeService _geocodeService;
        private IRouteToRouteResultMapper _routeToRouteResultMapper;
        private IUnitOfWork _unitOfWork;
        private ISortSecurity _sortSecurity;
        private IRouteFileCreator _routeFileCreator;
        private RouteService _routeService;
        private IRepository<Route> _routeRepository;
        
        [TestInitialize]
        public void Setup()
        {
            _routeCreator = MockRepository.GenerateStub<IRouteCreator>();
            _heightMapImageBuilder = MockRepository.GenerateStub<IHeightMapImageBuilder>();
            _heightMapImageCache = MockRepository.GenerateStub<IHeightMapImageCache>();
            _geocodeService = MockRepository.GenerateStub<IGeocodeService>();
            _routeToRouteResultMapper = MockRepository.GenerateStub<IRouteToRouteResultMapper>();
            _unitOfWork = MockRepository.GenerateStub<IUnitOfWork>();
            _sortSecurity = MockRepository.GenerateStub<ISortSecurity>();
            _routeFileCreator = MockRepository.GenerateStub<IRouteFileCreator>();
            _routeRepository = MockRepository.GenerateStub<IRepository<Route>>();
            _unitOfWork.Stub(s => s.GetRepository<Route>()).Return(_routeRepository);

            _routeService = new RouteService(_routeCreator,
                                                         _heightMapImageBuilder,
                                                         _heightMapImageCache,
                                                         _geocodeService,
                                                         _routeToRouteResultMapper,
                                                         _unitOfWork,
                                                         _sortSecurity,
                                                         _routeFileCreator);
        }

        [TestMethod]
        public void Test_Update_Succeeds()
        {
            // Arrange
            Route repositoryRoute = new Route() {AccountID = 1, Name = "Route1", RouteID = 1};
            Route updateRoute = new Route() {AccountID = 1, Name = "Update", RouteID = 1};
            _routeRepository.Stub(f => f.Find(1)).Return(repositoryRoute);

            // Act
            _routeService.Update(updateRoute, 1);

            // Assert
            Assert.AreEqual("Update", repositoryRoute.Name);
            _unitOfWork.AssertWasCalled(s => s.Save());
        }

        [TestMethod]
        public void Test_Update_FailsOnAuthorizationCheck()
        {
            // Arrange
            Route repositoryRoute = new Route() { AccountID = 2, Name = "Route1", RouteID = 1 };
            Route updateRoute = new Route() { AccountID = 1, Name = "Update", RouteID = 1 };
            _routeRepository.Stub(f => f.Find(1)).Return(repositoryRoute);
            bool exceptionRaised = false;
            
            // Act
            try
            {
                _routeService.Update(updateRoute, 1);
            }
            catch (SecurityException se)
            {
                exceptionRaised = true;
            }
            

            // Assert
            Assert.IsTrue(exceptionRaised);
        }

        [TestMethod]
        public void Test_Download_Succeeds()
        {
            // Arrange
            
            XDocument document = XDocument.Parse("<route/>");
            _routeFileCreator.Stub(s => s.CreateRoute(Arg<Route>.Is.Anything, out Arg<string>.Out("afile.gpx").Dummy)).
                Return(document);
            string filename;

            // Act
            Stream stream = _routeService.Download(1, out filename);
            XDocument result = XDocument.Load(stream);

            // Assert
            Assert.AreEqual("route", result.Root.Name);
            Assert.AreEqual("afile.gpx", filename);
        }
    }
}
