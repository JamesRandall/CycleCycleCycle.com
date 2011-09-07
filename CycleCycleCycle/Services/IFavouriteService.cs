using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CycleCycleCycle.Services
{
    public interface IFavouriteService
    {
        void Favourite(int accountId, int favouriteId);
        void Unfavourite(int accountId, int favouriteId);
    }
}
