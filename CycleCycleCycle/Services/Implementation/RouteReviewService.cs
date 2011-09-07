using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CycleCycleCycle.Models;
using CycleCycleCycle.Models.Repositories;

namespace CycleCycleCycle.Services.Implementation
{
    public class RouteReviewService : IRouteReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<RouteReview> _routeReviewRepository;
        private readonly IRepository<Route> _routeRepository; 

        public RouteReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _routeRepository = _unitOfWork.GetRepository<Route>();
            _routeReviewRepository = _unitOfWork.GetRepository<RouteReview>();
        }

        public bool IsRouteReviewedByUser(int routeId, int accountId)
        {
            return _routeReviewRepository.All.Where(rv => rv.AccountID == accountId && rv.RouteID == routeId).Any();
        }

        public RouteReview Insert(RouteReview routeReview)
        {
            try
            {
                routeReview.DateCreated = DateTime.Now;
                _routeReviewRepository.Insert(routeReview);

                Route route = _routeRepository.Find(routeReview.RouteID);
                route.AverageRating =
                    _routeReviewRepository.All.Where(rv => rv.RouteID == routeReview.RouteID).Average(rv => rv.Rating);
                _routeRepository.Update(route);
                _unitOfWork.Save();

                return routeReview;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Unable to insert route review.", ex);
            }
            
        }
    }
}