using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace JKWeatherAPI.WeatherSource
{
    public interface IWeatherSource
    {
        CurrentWeather GetCurrentWeather(float latitude, float longitude);
        WeeklyWeather GetWeeklyWeather(float latitude, float longitude);
    }

    public class WeatherInfo
    {
        private string _state;
        private float _tempCurrent;
        private float _tempMax;
        private float _tempMin;
        private float _windSpeed;
        private float _windDegree;

        public string State { get { return _state; } set { _state = value; } }
        public float TempCurrent { get { return _tempCurrent; } set { _tempCurrent = value; } }
        public float TempMax { get { return _tempMax; } set { _tempMax = value; } }
        public float TempMin { get { return _tempMin; } set { _tempMin = value; } }
        public float WindSpeed { get { return _windSpeed; } set { _windSpeed = value; } }
        public float WindDegree { get { return _windDegree; } set { _windDegree = value; } }

        public WeatherInfo(string state, float tempCurrent, float tempMax = 0.0f, float tempMin = 0.0f, float windSpeed = 0.0f, float windDegree = 0.0f)
        {
            _state = state;
            _tempCurrent = tempCurrent;
            _tempMax = tempMax;
            _tempMin = tempMin;
            _windSpeed = windSpeed;
            _windDegree = windDegree;
        }
    }

    [DataContract]
    public class CurrentWeather
    {
        private string _location;
        private string _country;
        private WeatherInfo _weatherInfo;

        [DataMember]
        public string Location { get { return _location; } set { _location = value; } }
        [DataMember]
        public string Country { get { return _country; } set { _country = value; } }
        [DataMember]
        public WeatherInfo Weather { get { return _weatherInfo; } set { _weatherInfo = value; } }

        public CurrentWeather(string location, string country, WeatherInfo weatherInfo)
        {
            _location = location;
            _country = country;
            _weatherInfo = weatherInfo;
        }
    }

    [DataContract]
    public class WeeklyWeather
    {
        private string _location;
        private string _country;
        private WeatherInfo[] _weatherInfo;

        [DataMember]
        public string Location { get { return _location; } set { _location = value; } }
        [DataMember]
        public string Country { get { return _country; } set { _country = value; } }
        [DataMember]
        public WeatherInfo[] Weather { get { return _weatherInfo; } set { _weatherInfo = value.Length == 7 ? value : null; } }

        public WeeklyWeather(string location, string country, WeatherInfo[] weatherInfo)
        {
            _location = location;
            _country = country;
            _weatherInfo = weatherInfo.Length == 7 ? weatherInfo : null;
        }
    }
}