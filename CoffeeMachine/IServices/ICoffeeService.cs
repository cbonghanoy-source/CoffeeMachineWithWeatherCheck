using System;
using CoffeeMachine.Models;
namespace CoffeeMachine.IServices
{
	public interface ICoffeeService
	{
		(int StatusCode, BrewResponse? brewResponse) BrewCoffee();
	}
}


