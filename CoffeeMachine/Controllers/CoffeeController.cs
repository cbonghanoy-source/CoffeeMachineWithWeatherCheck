using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeMachine.IServices;
using CoffeeMachine.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoffeeMachine.Controllers
{
    [ApiController]
    [Route("/")]
    public class CoffeeController : Controller
    {
        private readonly ICoffeeService _coffeeService;

        public CoffeeController(ICoffeeService coffeeService)
        {
            _coffeeService = coffeeService;
        }
        
        [HttpGet("brew-coffee")]
        public IActionResult Get()
        {
            var (statusCode, brewResponse) = _coffeeService.BrewCoffee();
            
            return new ObjectResult(brewResponse)
            {
                StatusCode = statusCode
            };
        }
        
    }
}

