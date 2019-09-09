using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;

namespace JKWeatherAPI.WeatherSource
{
    public class OpenWeatherAPI : IWeatherSource
    {
        private string _apiKey;

        public OpenWeatherAPI(string apiKey)
        {
            _apiKey = apiKey;
        }

        private void RequestToServer(string url)
        {
            var request = WebRequest.Create(url);
            var response = request.GetResponse();

            var responseStream = response.GetResponseStream();
            var readStream = new StreamReader(responseStream);
            var read = readStream.ReadToEnd();
        }

        public CurrentWeather GetCurrentWeather(float latitude, float longitude)
        {
            var currentWeatherUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={ latitude.ToString() }&lon={ longitude.ToString() }&appid={_apiKey}";
            RequestToServer(currentWeatherUrl);

            return new CurrentWeather("", "", new WeatherInfo("",10.0f));
        }

        public WeeklyWeather GetWeeklyWeather(float latitude, float longitude)
        {
            var weeklyWeatherUrl = $"https://api.openweathermap.org/data/2.5/forecast/daily?lat={ latitude.ToString() }&lon={ longitude.ToString() }&cnt=7&appid={_apiKey}";
            RequestToServer(weeklyWeatherUrl);

            return new WeeklyWeather("", "", new WeatherInfo[7]);
        }
    }
}