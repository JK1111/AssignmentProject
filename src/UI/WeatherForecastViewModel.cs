using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Assignment.Application.TodoLists.Commands.CreateTodoList;
using Assignment.Application.TodoLists.Queries.GetCountries;
using Assignment.Application.TodoLists.Queries.GetTodos;
using Assignment.Application.WeatherForecast.Queries;
using Assignment.Domain.Entities;
using Caliburn.Micro;
using MediatR;

namespace Assignment.UI;
/// <summary>
/// Interaction logic for WeatherForecastView.xaml
/// </summary>
internal class WeatherForecastViewModel : Screen
{
    private readonly ISender _sender;
    private readonly IWindowManager _windowManager;

    private IList<CountryDto> _countries;
    public IList<CountryDto> Countries
    {
        get
        {
            return _countries;
        }
        set
        {
            _countries = value;
            NotifyOfPropertyChange(() => Countries);
        }
    }

    private CountryDto _selectedCountry;
    public CountryDto SelectedCountry
    {
        get => _selectedCountry;
        set
        {
            _selectedCountry = value;
            NotifyOfPropertyChange(() => SelectedCountry);
        }
    }

    private City _selectedCity;
    public City SelectedCity
    {
        get => _selectedCity;
        set
        {
            _selectedCity = value;
            NotifyOfPropertyChange(() => SelectedCity);
            NotifyOfPropertyChange(() => TemperatureText);
            GetWeatherForecast();
        }
    }
    private int _temperature;
    public int Temperature
    {
        get => _temperature;
        set
        {
            _temperature = value;
            NotifyOfPropertyChange(() => Temperature);
            NotifyOfPropertyChange(() => TemperatureText);
        }
    }

    public string TemperatureText 
    { 
        get
        {
            return SelectedCity?.Id == null ? "" : $"Current temperature is: {Temperature} °C";
        } 
    }

    public WeatherForecastViewModel(ISender sender, IWindowManager windowManager)
    {
        _sender = sender;
        _windowManager = windowManager;
        Initialize();
    }

    private async void Initialize()
    {
        await RefereshCountries();
    }

    private async void GetWeatherForecast()
    {
        if (SelectedCity?.Name != null)
        {
            var forecast = await _sender.Send(new GetWeatherForecastQuery { CityName = SelectedCity.Name, CurrentTime = DateTime.UtcNow });
            Temperature = forecast.Temperature;
        }
    }

    private async Task RefereshCountries()
    {
        var selectedCountryId = SelectedCountry?.Id;

        Countries = await _sender.Send(new GetCountriesQuery());
    }
}
