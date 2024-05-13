using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Domain.Entities;

public class Country : BaseAuditableEntity
{
    public string? Name { get; set; }

    public IList<City> Cities { get; private set; } = new List<City>();
}
