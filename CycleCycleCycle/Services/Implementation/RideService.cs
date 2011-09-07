using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CycleCycleCycle.Models;
using CycleCycleCycle.Models.Repositories;

namespace CycleCycleCycle.Services.Implementation
{
    public class RideService : IRideService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Ride> _rideRepository;

        public RideService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _rideRepository = unitOfWork.GetRepository<Ride>();
        }

        public bool Create(int accountId, int routeId, DateTime dateRidden, int? hours, int? minutes, int? seconds)
        {
            if (hours < 0 || minutes < 0 || seconds < 0)
                return false;

            if (!hours.HasValue) hours = 0;
            if (!minutes.HasValue) minutes = 0;
            if (!seconds.HasValue) seconds = 0;

            try
            {
                Ride ride = new Ride
                                {
                                    AccountID = accountId,
                                    RouteID = routeId,
                                    TimeOfRide = dateRidden,
                                    TimeTaken =
                                        (hours == 0 && minutes == 0 && seconds == 0)
                                            ? null
                                            : new TimeSpan?(new TimeSpan(0, hours.Value, minutes.Value, seconds.Value))
                                };
                _rideRepository.Insert(ride);
                _unitOfWork.Save();
            }
            catch(Exception ex)
            {
                throw new ServiceException("Error occurred saving ride", ex);
            }

            return true;
        }
    }
}