using System;
using CoffeeMachine.IServices;

namespace CoffeeMachine.Services
{
    public class RequestCountService : IRequestCountService
    {
        private int _requestCount = 0;
        public int GetRequestCount()
        {
            return Interlocked.Increment(ref _requestCount);
        }
    }
}
