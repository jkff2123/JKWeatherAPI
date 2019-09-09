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
            var currentWeatherUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={ latitude.ToString() }&lon={ longitude.ToString() }&appid=cb0b1a243a5d3f3f6cef60b1497ccf19";
            RequestToServer(currentWeatherUrl);

            return new CurrentWeather("", "", new WeatherInfo("",10.0f));
        }

        public WeeklyWeather GetWeeklyWeather(float latitude, float longitude)
        {
            var weeklyWeatherUrl = $"https://api.openweathermap.org/data/2.5/forecast/daily?lat={ latitude.ToString() }&lon={ longitude.ToString() }&cnt=7&appid=cb0b1a243a5d3f3f6cef60b1497ccf19";
            RequestToServer(weeklyWeatherUrl);

            return new WeeklyWeather("", "", new WeatherInfo[7]);
        }
    }
}