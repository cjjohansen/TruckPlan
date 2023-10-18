using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckPlanning
{
    public class QuerySpecifications
    {
    }


    public class QueryDescriptor
    {
        public int Age { get; set; }
        public MonthInYear? MonthInYear { get; set; }
        public string? CountryCode { get; set; }
    }

    public record MonthInYear(uint Year, uint Month);




}
