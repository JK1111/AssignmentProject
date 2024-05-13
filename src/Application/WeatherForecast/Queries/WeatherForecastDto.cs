namespace Assignment.Application.WeatherForecast.Queries;

public class WeatherForecastDto
{
    public required string CityName { get; set; }
    public DateTime CurrentTime { get; set; } = DateTime.Now;
    public int Temperature { get; set; }
}
