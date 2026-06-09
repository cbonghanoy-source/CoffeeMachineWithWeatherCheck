using System;
namespace CoffeeMachine.IServices
{
	public interface IDateTimeProvider
	{
		DateTime Now { get; }
	}
}

