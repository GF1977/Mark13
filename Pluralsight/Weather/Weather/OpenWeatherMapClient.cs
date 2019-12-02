namespace WeatherConsole
{
	using System;
	using System.Net.Http;
	using System.Threading.Tasks;
    using System.Runtime.Serialization.Json;
    using System.Collections.Generic;
    using System.IO;

    public class OpenWeatherMapClient
	{
        private readonly string _AppId ;
		private readonly string _Units;
		private readonly HttpClient _Client;
		private const string ApiRoot = "http://api.openweathermap.org/data/2.5";
        private const string _File = "./WeatherAPI.key";
        static string ReadApiKey(string api_file)
        {
                FileStream fileStream = new FileStream(api_file, FileMode.Open);
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string line = reader.ReadLine();
                    return line;
                }
        }
        public OpenWeatherMapClient(string units = "metric")
		{
			_Units = units;
			_Client = new HttpClient();
            _AppId = ReadApiKey(_File);
        }
        public async Task<CurrentWeather> GetCurrentWeatherByCity(string city)
		{
			// note: no error handling
			string currentWeatherApiUrl = $"{ApiRoot}/weather?q={city}&appid={_AppId}&units={_Units}";
			var response = await _Client.GetAsync(currentWeatherApiUrl);
			var responseString = await response.Content.ReadAsStringAsync();
			Console.WriteLine("\nJSON response:");
			Console.WriteLine(responseString);
			var serializer = new DataContractJsonSerializer(typeof(CurrentWeather));
			return serializer.ReadObject(await response.Content.ReadAsStreamAsync()) as CurrentWeather;
		}
	}
}