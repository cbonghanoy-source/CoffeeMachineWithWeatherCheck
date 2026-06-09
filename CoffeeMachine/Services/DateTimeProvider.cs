using System;
using CoffeeMachine.IServices;
namespace CoffeeMachine.Services
{
	public class DateTimeProvider : IDateTimeProvider
	{
		public DateTime Now => DateTime.Now;
	}
}

