using System.Net;
using CoffeeMachine.Controllers;
using CoffeeMachine.IServices;
using CoffeeMachine.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CoffeeMachine.Test;

public class CoffeeControllerTests
{
    private Mock<ICoffeeService> _coffeeServiceMock;
    private CoffeeController _coffeeController;


    [SetUp]
    public void Setup()
    {
        _coffeeServiceMock = new Mock<ICoffeeService>();
        _coffeeController = new CoffeeController(_coffeeServiceMock.Object);
    }

    [Test]
    public void BrewCoffee_Returns200()
    {
        //Assert.Pass();
        var response = new BrewResponse 
            { Message = "Your piping hot coffee is ready",
             Prepared = "2026-06-08T10:00:00+00:00"};
        var statusCode = (int)HttpStatusCode.OK;

        _coffeeServiceMock.Setup(service => service.BrewCoffee()).Returns((statusCode, response));

        var result = _coffeeController.Get() as ObjectResult;

        Assert.That(result.StatusCode, Is.EqualTo(statusCode));
        Assert.That(result.Value, Is.EqualTo(response));
    }

    [Test]
    public void BrewCoffee_Returns418()
    {
        var statusCode = (int)StatusCodes.Status418ImATeapot;

        _coffeeServiceMock.Setup(service => service.BrewCoffee()).Returns((statusCode, null));

        var result = _coffeeController.Get() as ObjectResult;

        Assert.That(result.StatusCode, Is.EqualTo(statusCode));
        Assert.That(result.Value, Is.Null);
    }

    [Test]
    public void BrewCoffee_Returns503()
    {
        var statusCode = (int)StatusCodes.Status503ServiceUnavailable;

        _coffeeServiceMock.Setup(service => service.BrewCoffee()).Returns((statusCode, null));

        var result = _coffeeController.Get() as ObjectResult;

        Assert.That(result.StatusCode, Is.EqualTo(statusCode));
        Assert.That(result.Value, Is.Null);
    }
}
