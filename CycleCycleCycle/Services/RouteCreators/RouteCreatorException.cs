using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CycleCycleCycle.Services.RouteCreators
{
    public class RouteCreatorException : Exception
    {
        public RouteCreatorException() : base() { }
        public RouteCreatorException(string message) : base(message) { }
        public RouteCreatorException(string message, Exception exception) : base(message, exception) { }
    }
}