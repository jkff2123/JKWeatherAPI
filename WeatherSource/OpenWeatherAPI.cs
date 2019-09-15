using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json.Linq;


namespace JKWeatherAPI.WeatherSource
{
    public class OpenWeatherAPI : IWeatherSource
    {
        private string _apiKey;

        public OpenWeatherAPI(string apiKey)
        {
            _apiKey = apiKey;
        }

        private JObject RequestToServer(string url)
        {
            var request = WebRequest.Create(url);
            var response = request.GetResponse();

            var responseStream = response.GetResponseStream();
            var readStream = new StreamReader(responseStream);
            var jsonObject = JObject.Parse(readStream.ReadToEnd());
            return jsonObject;
        }

        private WeatherInfo[] ParseWeatherInfo(JObject jsonObject)
        {
            List<WeatherInfo> weatherList = new List<WeatherInfo>();

            try
            {
                if (jsonObject.ContainsKey("List"))
                {
                    foreach(var element in jsonObject["List"])
                    {
                        var amountOfRainOrSnow = 0.0f;
                        if (element["rain"] != null)
                        {
                            amountOfRainOrSnow = (float)element["rain"];
                        }
                        else if (element["snow"] != null)
                        {
                            amountOfRainOrSnow = (float)element["snow"];
                        }

                        weatherList.Add(
                            new WeatherInfo(
                                element["weather"][0]["main"].ToString(), 
                                (float)element["temp"]["day"], 
                                (float)element["temp"]["max"], 
                                (float)element["temp"]["min"], 
                                (float)element["clouds"], 
                                amountOfRainOrSnow, 
                                (float)element["speed"], 
                                (float)element["deg"]
                            ));
                    }
                }
                else
                {
                    var amountOfRainOrSnow = 0.0f;
                    if (jsonObject["rain"] != null)
                    {
                        amountOfRainOrSnow = (float)jsonObject["rain"]["1h"];
                    }
                    else if (jsonObject["snow"] != null)
                    {
                        amountOfRainOrSnow = (float)jsonObject["snow"]["1h"];
                    }

                    weatherList.Add(
                        new WeatherInfo(
                            jsonObject["weather"][0]["main"].ToString(),
                                (float)jsonObject["main"]["temp"],
                                (float)jsonObject["main"]["temp_max"],
                                (float)jsonObject["main"]["temp_min"],
                                (float)jsonObject["clouds"]["all"],
                                amountOfRainOrSnow,
                                (float)jsonObject["wind"]["speed"],
                                (float)jsonObject["wind"]["deg"]
                            ));
                }
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex);
            }

            return weatherList.ToArray();
        }

        public CurrentWeather GetCurrentWeather(float latitude, float longitude)
        {
            var currentWeatherUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={ latitude.ToString() }&lon={ longitude.ToString() }&appid={_apiKey}";
            var weatherInfoResult = RequestToServer(currentWeatherUrl);
            var parsedWeatherData = ParseWeatherInfo(weatherInfoResult);

            return new CurrentWeather(weatherInfoResult["name"].ToString(), weatherInfoResult["sys"]["country"].ToString(), parsedWeatherData[0]);
        }

        public WeeklyWeather GetWeeklyWeather(float latitude, float longitude)
        {
            var weeklyWeatherUrl = $"https://api.openweathermap.org/data/2.5/forecast/daily?lat={ latitude.ToString() }&lon={ longitude.ToString() }&cnt=7&appid={_apiKey}";
            RequestToServer(weeklyWeatherUrl);

            return new WeeklyWeather("", "", new WeatherInfo[7]);
        }
    }
}