using System;

namespace CycleCycleCycle.Services.Implementation
{
    public class GeocodingException : Exception
    {
        public GeocodingException() : base() { }
        public GeocodingException(string message) : base(message) { }
        public GeocodingException(string message, Exception exception) : base(message, exception) { }
    }
}