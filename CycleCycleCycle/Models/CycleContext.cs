using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace CycleCycleCycle.Models
{
    public class CycleContext : DbContext
    {
        public DbSet<Route> Routes { get; set; }
        public DbSet<RoutePoint> RoutePoints { get; set; }
        public DbSet<RouteReview> RouteReviews { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Ride> Rides { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}