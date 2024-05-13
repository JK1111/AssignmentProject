using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment.Application.Common.Interfaces;
using Assignment.Application.Common.Security;
using Assignment.Application.TodoLists.Queries.GetTodos;

namespace Assignment.Application.WeatherForecast.Queries;

[Authorize]
public record GetWeatherForecastQuery : IRequest<WeatherForecastDto>
{
    public required string CityName { get; set; }
    public DateTime CurrentTime { get; set;}
}

public class GetWeatherForecastQueryHandler : IRequestHandler<GetWeatherForecastQuery, WeatherForecastDto>
{
    private readonly IWeatherForecastApi _weatherForecastApi;
    private readonly IMapper _mapper;

    public GetWeatherForecastQueryHandler(IWeatherForecastApi weatherForecastApi, IMapper mapper)
    {
        _weatherForecastApi = weatherForecastApi;
        _mapper = mapper;
    }

    public async Task<WeatherForecastDto> Handle(GetWeatherForecastQuery request, CancellationToken cancellationToken)
    {
        var weatherForecastDto = new WeatherForecastDto
        {
            CityName = request.CityName,
            CurrentTime = request.CurrentTime,
        };

        var currentTemprature = _weatherForecastApi.GetTemperature(request.CityName, request.CurrentTime);

        weatherForecastDto.Temperature = currentTemprature;

        return await Task.FromResult(weatherForecastDto);
    }
}
