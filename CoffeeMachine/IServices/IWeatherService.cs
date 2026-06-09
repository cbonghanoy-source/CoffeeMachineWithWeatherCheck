using System;
using CoffeeMachine.Models;
namespace CoffeeMachine.IServices
{
	public interface IWeatherService
	{
		Task<WeatherServiceResponse> GetWeather();
	}
}

