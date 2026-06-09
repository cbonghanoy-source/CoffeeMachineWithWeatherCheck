using NUnit.Framework;
using Moq;
using CoffeeMachine.Services;
using System.Net;
using CoffeeMachine.IServices;
using Microsoft.AspNetCore.Http;
using CoffeeMachine.Models;

namespace CoffeeMachine.Test;

public class CoffeeServiceTests
{
    private Mock<IDateTimeProvider> _dateTimeProviderMock;
    private Mock<IWeatherService> _weatherServiceMock;
    private Mock<IRequestCountService> _requestCountServiceMock;
    private CoffeeService _coffeeService;


    [SetUp]
    public void Setup()
    {
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _weatherServiceMock = new Mock<IWeatherService>();
        _requestCountServiceMock = new Mock<IRequestCountService>();
        _coffeeService = new CoffeeService(_dateTimeProviderMock.Object, _weatherServiceMock.Object, _requestCountServiceMock.Object);
    }

    [Test]
    public void BrewCoffee_Returns200()
    {
        _weatherServiceMock.Setup(service => service.GetWeather()).ReturnsAsync(new WeatherServiceResponse { Temperature = 20 });
        _requestCountServiceMock.Setup(service => service.GetRequestCount()).Returns(1);
        var (statusCode, response) = _coffeeService.BrewCoffee();

        Assert.That(statusCode, Is.EqualTo((int)HttpStatusCode.OK));
        Assert.That(response?.Message, Is.EqualTo("Your piping hot coffee is ready"));
        Assert.That(response?.Prepared, Is.Not.Null);
    }
    [Test]
    public void BrewCoffee_Returns200_PreparedTimeIsInCorrectFormat()
    {
        _weatherServiceMock.Setup(service => service.GetWeather()).ReturnsAsync(new WeatherServiceResponse { Temperature = 20 });
        _requestCountServiceMock.Setup(service => service.GetRequestCount()).Returns(1);
        var (statusCode, response) = _coffeeService.BrewCoffee();

        Assert.That(statusCode, Is.EqualTo((int)HttpStatusCode.OK));
        Assert.That(DateTime.TryParse(response?.Prepared, out _), Is.True);
    }

    [Test]
    public void BrewCoffee_Returns200_WhenTemperatureIsBelow30()
    {
        _weatherServiceMock.Setup(service => service.GetWeather()).ReturnsAsync(new WeatherServiceResponse { Temperature = 25 });
        _requestCountServiceMock.Setup(service => service.GetRequestCount()).Returns(1);

        var (statusCode, response) = _coffeeService.BrewCoffee();

        Assert.That(statusCode, Is.EqualTo((int)HttpStatusCode.OK));
        Assert.That(response?.Message, Is.EqualTo("Your piping hot coffee is ready"));
    }

    [Test]
    public void BrewCoffee_Returns200_WhenTemperatureIs30()
    {
        _weatherServiceMock.Setup(service => service.GetWeather()).ReturnsAsync(new WeatherServiceResponse { Temperature = 30 });
        _requestCountServiceMock.Setup(service => service.GetRequestCount()).Returns(1);
        var (statusCode, response) = _coffeeService.BrewCoffee();

        Assert.That(statusCode, Is.EqualTo((int)HttpStatusCode.OK));
        Assert.That(response?.Message, Is.EqualTo("Your refreshing iced coffee is ready"));
    }

    [Test]
    public void BrewCoffee_Returns200_WhenTemperatureIsAbove30()
    {
        _weatherServiceMock.Setup(service => service.GetWeather()).ReturnsAsync(new WeatherServiceResponse { Temperature = 35 });
        _requestCountServiceMock.Setup(service => service.GetRequestCount()).Returns(1);

        var (statusCode, response) = _coffeeService.BrewCoffee();

        Assert.That(statusCode, Is.EqualTo((int)HttpStatusCode.OK));
        Assert.That(response?.Message, Is.EqualTo("Your refreshing iced coffee is ready"));
    }


    [Test]
    public void BrewCoffee_Returns503()
    {
        _weatherServiceMock.Setup(service => service.GetWeather()).ReturnsAsync(new WeatherServiceResponse { Temperature = 20 });
        _requestCountServiceMock.Setup(service => service.GetRequestCount()).Returns(5);

        var (statusCode, response) = _coffeeService.BrewCoffee();

        Assert.That(statusCode, Is.EqualTo((int)HttpStatusCode.ServiceUnavailable));
        Assert.That(response, Is.Null);
    }

    [Test]
    public void BrewCoffee_Returns503_EveryFifthRequest()
    {
        _weatherServiceMock.Setup(service => service.GetWeather()).ReturnsAsync(new WeatherServiceResponse { Temperature = 20 });
        _requestCountServiceMock.SetupSequence(service => service.GetRequestCount())
            .Returns(10);

        var (statusCode, response) = _coffeeService.BrewCoffee();

        Assert.That(statusCode, Is.EqualTo((int)HttpStatusCode.ServiceUnavailable));
        Assert.That(response, Is.Null);
    }

    [Test]
    public void BrewCoffee_Returns418_OnAprilFirst()
    {
        var aprilFirst = new DateTime(2026, 4, 1);
        _dateTimeProviderMock.Setup(provider => provider.Now).Returns(aprilFirst);

        var (statusCode, response) = _coffeeService.BrewCoffee();

        Assert.That(statusCode, Is.EqualTo((int)StatusCodes.Status418ImATeapot));
        Assert.That(response, Is.Null);
    }

    [Test]
    public void BrewCoffee_Returns200_OnNonAprilFirst()
    {
        var nonAprilFirst = new DateTime(2026, 6, 8);
        _dateTimeProviderMock.Setup(provider => provider.Now).Returns(nonAprilFirst);
        _weatherServiceMock.Setup(service => service.GetWeather()).ReturnsAsync(new WeatherServiceResponse { Temperature = 20 });
        _requestCountServiceMock.Setup(service => service.GetRequestCount()).Returns(1);

        var (statusCode, response) = _coffeeService.BrewCoffee();

        Assert.That(statusCode, Is.EqualTo((int)HttpStatusCode.OK));
        Assert.That(response, Is.Not.Null);
    }
}
