using System;

namespace CycleCycleCycle.Services.Implementation
{
    public class ServiceException : Exception
    {
        public ServiceException() : base() { }
        public ServiceException(string message) : base(message) { }
        public ServiceException(string message, Exception exception) : base(message, exception) { }
    }
}