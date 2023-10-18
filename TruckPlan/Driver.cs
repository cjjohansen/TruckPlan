using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckPlanning
{
    public class Driver : Person
    {
        /// Should driver haave a specific driver id different from person id?
        /// Should Birtday also be a value object, and not just a primitive type ?
        /// 
        /// 
        /// 
        /// 
        public Driver(PersonId personId, PersonName name, DateOnly birthday): base(personId, name, birthday)
        {

        }
    }
}
