using System;
using System.Net;
using CoffeeMachine.IServices;
using CoffeeMachine.Models;
namespace CoffeeMachine.Services
{
	public class CoffeeService : ICoffeeService
	{
		private readonly IRequestCountService _requestCountService;
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly IWeatherService _weatherService;

		public CoffeeService(IDateTimeProvider dateTimeProvider, IWeatherService weatherService, IRequestCountService requestCountService)
		{
			_dateTimeProvider = dateTimeProvider;
			_weatherService = weatherService;
			_requestCountService = requestCountService;
		}

        public (int StatusCode, BrewResponse? brewResponse) BrewCoffee()
        {
			var dateTimeNow = _dateTimeProvider.Now;

			if(IsAprilFirst(dateTimeNow))
			{
				return ((int)StatusCodes.Status418ImATeapot, null);
			}

			if(isOutOfCoffee())
			{
				return ((int)HttpStatusCode.ServiceUnavailable, null);
			}

            var response = CreateBrewResponse(dateTimeNow);
	
			return ((int)HttpStatusCode.OK, response);
        }

		private bool IsAprilFirst(DateTime dateTimeNow)
		{
			return dateTimeNow.Month == (int)Months.April && dateTimeNow.Day == 1;
		}

		private bool isOutOfCoffee()
		{
			var requestCount = _requestCountService.GetRequestCount();
			return requestCount % 5 == 0;
		}

		private BrewResponse CreateBrewResponse(DateTime dateTimeNow)
		{
			var temperature = _weatherService.GetWeather().Result;

			return new BrewResponse
			{
				Message = temperature.Temperature < 30 ? "Your piping hot coffee is ready" : "Your refreshing iced coffee is ready",
				Prepared = dateTimeNow.ToString("yyyy-MM-dd'T'HH:mm:ssK")
			};
		}
    }
}

