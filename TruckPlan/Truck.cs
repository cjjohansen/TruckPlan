using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckPlanning
{
    public class Truck
    {

        public TruckId TruckId { get; init; }

        public Truck(TruckId truckId) {

            TruckId = truckId;
        }  
    }


    public record TruckId(Guid Id);
}
