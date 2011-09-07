using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CycleCycleCycle.Models;
using CycleCycleCycle.Models.Repositories;

namespace CycleCycleCycle.Services.Implementation
{
    public class FavouriteService : IFavouriteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Favourite> _favouriteRepository;

        public FavouriteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _favouriteRepository = _unitOfWork.GetRepository<Favourite>();
        }

        public void Favourite(int accountId, int routeId)
        {
            Favourite favourite = new Favourite
            {
                AccountID = accountId,
                RouteID = routeId
            };
            _favouriteRepository.Insert(favourite);
            _unitOfWork.Save();
        }

        public void Unfavourite(int accountId, int routeId)
        {
            Favourite favourite =
                _favouriteRepository.All.Where(f => f.AccountID == accountId && f.RouteID == routeId).FirstOrDefault();
            if (favourite != null)
            {
                _favouriteRepository.Delete(favourite.FavouriteID);
                _unitOfWork.Save();
            }
        }
    }
}