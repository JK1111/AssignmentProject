using Assignment.Application.Common.Interfaces;

namespace Assignment.Infrastructure.WeatherForecast;

public class WeatherForecastApi : IWeatherForecastApi
{
    readonly Random _random;

    public WeatherForecastApi() => _random = new Random();

    public int GetTemperature(string cityName, DateTime time)
    {
        return _random.Next(-30, 50);
    }
}
