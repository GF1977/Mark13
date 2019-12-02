namespace WeatherConsole
{
	using System.Threading.Tasks;
	using static System.Console;

	public class Program
	{
        public static void Main(string[] args)
        {
            Run().Wait();
        }

        public static string WindDirection(double degree)
        {
            
            string[] direction = {"North", "North-East", "East", "South-East", "South", "South-West", "West", "North-West"};
            int index = (int)(degree / 45);
            return direction[index];
        }

		private static async Task Run()
		{
            bool bContinue = true;
            var city = "Berlin, DE";
            OpenWeatherMapClient client = null;
            try
            {
                 client = new OpenWeatherMapClient();
            }
            catch (System.Exception)
            {
                WriteLine("Can't create OpenWeatherMapClient");
                bContinue = false;
                //throw;
            }

            if (bContinue)
            {
                WriteLine($"Fetching weather for {city}");
                var weather = await client.GetCurrentWeatherByCity(city);
                if (weather == null)
                {
                    WriteLine("Failed to fetch weather information.");
                    return;
                }
                string direction = WindDirection(weather.Wind.WindDirection);
                WriteLine($"\nCity: {weather.CityName}");
                WriteLine($"Temp: {weather.Main?.Temperature}");
                WriteLine($"Low: {weather.Main?.MinTemperature}");
                WriteLine($"High: {weather.Main?.MaxTemperature}");
                WriteLine($"Humidity: {weather.Main?.Humidity}%");
                WriteLine($"Condition: {weather.FirstCondition?.Description}");
                WriteLine($"Wind: {weather.Wind.WindSpeed} meter/sec, {direction}");
                
            }
		}

	}
}