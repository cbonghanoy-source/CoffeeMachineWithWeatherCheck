using System;
namespace CoffeeMachine.Models
{
	public class MeteoWeatherResponse
	{
        public CurrentWeather? Current { get; set; }
    }

    public class CurrentWeather
    {
        public double Temperature_2m { get; set; }
        public double Apparent_Temperature { get; set; }
        public double Wind_Speed_10m { get; set; }
        public int Relative_Humidity_2m { get; set; }
        public int Weather_Code { get; set; }
        public int Is_Day { get; set; }
    }
}

