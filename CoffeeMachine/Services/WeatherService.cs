using System;
using System.Text.Json;
using CoffeeMachine.IServices;
using CoffeeMachine.Models;

namespace CoffeeMachine.Services
{
	public class WeatherService : IWeatherService
	{
        private readonly IHttpClientWrapper _httpClient;
		public WeatherService(IHttpClientWrapper httpClient)
        {
            _httpClient = httpClient;
		}

        public async Task<WeatherServiceResponse> GetWeather()
        {
            // Simulate fetching weather data based on hardcoded coordinates
            // current values are cebu city coordinates
            var latitude = 10.33333;
            var longitude = 123.75;

            var weatherUrl = $"https://api.open-meteo.com/v1/forecast" +
                             $"?latitude={latitude}&longitude={longitude}" +
                             $"&current=temperature_2m,apparent_temperature,relative_humidity_2m,weather_code,wind_speed_10m,is_day" +
                             $"&wind_speed_unit=kmh&timezone=auto";

            var response = await _httpClient.GetFromJsonAsync<MeteoWeatherResponse>(weatherUrl, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var currentWeather = response?.Current;

            return new WeatherServiceResponse { Temperature = currentWeather?.Temperature_2m ?? 0 };
        }
    }
}

