using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CoffeeMachine.IServices;
using CoffeeMachine.Models;
using CoffeeMachine.Services;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace CoffeeMachine.Test;

public class WeatherServiceTests
{
    private Mock<IHttpClientWrapper> _httpClientMock;
    private WeatherService _weatherService;

    [SetUp]
    public void Setup()
    {
        _httpClientMock = new Mock<IHttpClientWrapper>();
        _weatherService = new WeatherService(_httpClientMock.Object);
    }

    [Test]
    public void GetWeather_ReturnsExpectedTemperature()
    {
        var expectedTemperature = 25.0;
        var meteoResponse = new 
        {
            current = new 
            {
                temperature_2m = expectedTemperature,
                apparent_temperature = 35.0,
                relative_humidity_2m = 80,
                weather_code = 1,
                wind_speed_10m = 10.0,
                is_day = 1
            }
        };

        var json = JsonSerializer.Serialize(meteoResponse);

        _httpClientMock.Setup(client => client.GetFromJsonAsync<MeteoWeatherResponse>(It.IsAny<string>(), It.IsAny<JsonSerializerOptions>()))
            .ReturnsAsync(JsonSerializer.Deserialize<MeteoWeatherResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }));

        var result = _weatherService.GetWeather().Result;

        Assert.That(result.Temperature, Is.EqualTo(expectedTemperature));
    }
}
