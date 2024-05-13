using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment.Application.TodoLists.Queries.GetTodos;
using Assignment.Domain.Entities;

namespace Assignment.Application.TodoLists.Queries.GetCountries;
public class CountryDto
{
    public CountryDto()
    {
        Cities = Array.Empty<City>();
    }

    public int Id { get; init; }

    public string? Name { get; init; }

    public IList<City> Cities { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Country, CountryDto>();
        }
    }
}
