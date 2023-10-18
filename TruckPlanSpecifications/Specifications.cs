using FluentAssertions;
using Newtonsoft.Json;
using System.Device.Location;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using TruckPlanning;
using static System.Net.WebRequestMethods;

namespace TruckPlanSpecifications
{
    public class Specifications
    {
        private const double EarthRadiusInMeters = 6376500.0;

        const string API_KEY = "5f20b96754804e67a412890371d0c4dc";

        const string url = $"https://api.geoapify.com/v1/geocode/reverse";

        CivicAddressServiceConfiguration _configuration;

        HttpClient _httpClient;

        ICivicAddressService _civicAddressService;

        DistanceCalculator _distanceCalculator;

        public Specifications()
        {
            _configuration = new CivicAddressServiceConfiguration( API_KEY, url );

            _httpClient = new HttpClient();

            _civicAddressService = new CivicAddressService( _httpClient, _configuration );

            _distanceCalculator = new DistanceCalculator( _civicAddressService );

        }

        [ Fact]
        public void route_with_registered_way_points_should_return_distance_driven()
        {

            //arrange
            var timestamp = DateTimeOffset.UtcNow;

            var geoPositions = new[]{

                 new GeoPosition<GeoCoordinate>(timestamp, new GeoCoordinate(50, 50)),
                 new GeoPosition<GeoCoordinate>(timestamp.AddMinutes(5), new GeoCoordinate(50, 51)),
                 new GeoPosition<GeoCoordinate>(timestamp.AddMinutes(10), new GeoCoordinate(50, 52)),
                 new GeoPosition<GeoCoordinate>(timestamp.AddMinutes(15), new GeoCoordinate(50, 53)),
                 new GeoPosition<GeoCoordinate>(timestamp.AddMinutes(20), new GeoCoordinate(50, 54))
            };


            const double expectedDistanceInMeters = 286143.56585954688;


            var route = TruckRoute.Create();


            foreach(var position in geoPositions)
            {
                route.RegisterNewTruckLocation(position.Location, position.Timestamp);
            }

            var distanceDriven =  _distanceCalculator.GetDistanceDrivenAt(route.RoutePositions.ToList(),  new MonthInYear((uint)timestamp.Year,(uint) timestamp.Month));

            Math.Abs(distanceDriven - expectedDistanceInMeters).Should().BeLessThan(0.1);

        }


        [Fact]
        public async void can_get_country_from_geo_coordinate()
        {

            //Arrange
            const double latitude = 51.21709661403662;
            const double longitude = 6.7782883744862374;

            //Act
            var countryDescriptor = await _civicAddressService.GetCountryFromGeoLocationAsync(latitude, longitude);


            //Assert
            countryDescriptor.CountryName.Should().Be( "Germany" );
            countryDescriptor.CountryCode.Should().Be( "de" );
        }

        [Fact]
        public async void for_driver_in_country_on_date()
        {

            //arrange 


            var youngDriver = new Driver(
                new PersonId( Guid.Parse( "00000000-0000-0000-0000-000000000001" ) ),
                new PersonName( new FirstName( "John" ), new LastName( "Doe" ) ),
                new DateOnly( 2000, 1, 1 )
                );

            var oldDriver = new Driver(
                new PersonId( Guid.Parse( "00000000-0000-0000-0000-000000000002" ) ),
                new PersonName( new FirstName( "Old" ), new LastName( "Joe" ) ),
                new DateOnly( 1971, 1, 1 )
                );

            var redTruck = new Truck( new TruckId( Guid.Parse( "10000000-0000-0000-0000-000000000001" ) ) );
            var blueTruck = new Truck( new TruckId( Guid.Parse( "10000000-0000-0000-0000-000000000002" ) ) );


            var routes = new Context().ArrangeTruckRoutes();

            var truckPlans = new[]
            {
                TruckPlan.CreateFrom(new TruckPlanId(Guid.Parse("20000000-0000-0000-0000-000000000001")),
                oldDriver,
                redTruck,
                new DateOnly(2018,2,17)
                ).WithRoute(routes[0]),
                TruckPlan.CreateFrom(new TruckPlanId(Guid.Parse("20000000-0000-0000-0000-000000000002")),
                oldDriver,
                redTruck,
                new DateOnly(2018,3,17)
                ).WithRoute(routes[1]),
                TruckPlan.CreateFrom(new TruckPlanId(Guid.Parse("20000000-0000-0000-0000-000000000003")),
                youngDriver,
                blueTruck,
                new DateOnly(2018,2,17)
                ).WithRoute(routes[2]),
                TruckPlan.CreateFrom(new TruckPlanId(Guid.Parse("20000000-0000-0000-0000-000000000004")),
                youngDriver,
                blueTruck,
                new DateOnly(2018,3,17)
                ).WithRoute(routes[3]),
            };


            var querySpecification = new QueryDescriptor
            {
                Age = 51,
                CountryCode = "de",
                MonthInYear = new MonthInYear( 2018, 2 )
            };


            var now = DateTime.UtcNow;


            var expectedDistanceInMeters = 209126.67963497125;

            //Act

            var relevantTruckPlans = from tp in truckPlans
            where tp.PlannedDate.Year == querySpecification.MonthInYear.Year &&
            tp.PlannedDate.Month == querySpecification.MonthInYear.Month &&
            now.Year - tp.Driver.Birthday.Year > querySpecification.Age
            select tp;


            var positions = relevantTruckPlans.SelectMany( tp => tp.Route.RoutePositions).ToList();



            var distance = await  _distanceCalculator.GetDistanceDrivenInCountryAsync( positions, new MonthInYear( 2018, 2 ), "de" );


            //Assert

            Math.Abs(distance - expectedDistanceInMeters).Should().BeLessThan( 1 );
            
        }
    }


    public class QueryingTruckPlans
    {


      

    }


    public class Context
    {

        public TruckRoute[] ArrangeTruckRoutes()
        {

            // TODO: Consider using autofixture or similar.

            var routes = new List <TruckRoute>();

            //arrange
            var feb2018 = new DateTimeOffset( 2018, 2, 17, 12, 0, 0, 0,0 , TimeSpan.Zero);
            var march2018 = new DateTimeOffset( 2018, 3, 17, 12, 0, 0, 0, 0, TimeSpan.Zero );

            var geoPositions = new[]{

                 new GeoPosition<GeoCoordinate>(feb2018, new GeoCoordinate(50, 50)),
                 new GeoPosition<GeoCoordinate>(feb2018.AddMinutes(5), new GeoCoordinate(51.21709661403662,6.7782883744862374)),
                 new GeoPosition<GeoCoordinate>(feb2018.AddMinutes(10), new GeoCoordinate(51.21709661403662,7.778288374486237450)),
                 new GeoPosition<GeoCoordinate>(feb2018.AddMinutes(15), new GeoCoordinate(51.21709661403662,8.778288374486237450)),
                 new GeoPosition<GeoCoordinate>(feb2018.AddMinutes(20), new GeoCoordinate(51.21709661403662,9.778288374486237450))
            };

            var route = TruckRoute.Create();

            foreach ( var position in geoPositions )
            {
                route.RegisterNewTruckLocation( position.Location, position.Timestamp );
            }


            routes.Add( route );


            //////////////////

            geoPositions = new[]{

                 new GeoPosition<GeoCoordinate>(feb2018, new GeoCoordinate(50, 50)),
                 new GeoPosition<GeoCoordinate>(feb2018.AddMinutes(5), new GeoCoordinate(51.21709661403662,36.7782883744862374)),
                 new GeoPosition<GeoCoordinate>(feb2018.AddMinutes(10), new GeoCoordinate(51.21709661403662,37.778288374486237450)),
                 new GeoPosition<GeoCoordinate>(feb2018.AddMinutes(15), new GeoCoordinate(51.21709661403662,38.778288374486237450)),
                 new GeoPosition<GeoCoordinate>(feb2018.AddMinutes(20), new GeoCoordinate(51.21709661403662,39.778288374486237450))
            };

            route = TruckRoute.Create();

            foreach ( var position in geoPositions )
            {
                route.RegisterNewTruckLocation( position.Location, position.Timestamp );
            }


            routes.Add( route );

            //////////////////

            geoPositions = new[]{

                 new GeoPosition<GeoCoordinate>(march2018, new GeoCoordinate(50, 50)),
                 new GeoPosition<GeoCoordinate>(march2018.AddMinutes(5), new GeoCoordinate(51.21709661403662,6.7782883744862374)),
                 new GeoPosition<GeoCoordinate>(march2018.AddMinutes(10), new GeoCoordinate(51.21709661403662,7.778288374486237450)),
                 new GeoPosition<GeoCoordinate>(march2018.AddMinutes(15), new GeoCoordinate(51.21709661403662,8.778288374486237450)),
                 new GeoPosition<GeoCoordinate>(march2018.AddMinutes(20), new GeoCoordinate(51.21709661403662,9.778288374486237450))
            };

            route = TruckRoute.Create();

            foreach ( var position in geoPositions )
            {
                route.RegisterNewTruckLocation( position.Location, position.Timestamp );
            }


            routes.Add( route );


            //////////////////

            geoPositions = new[]{

                 new GeoPosition<GeoCoordinate>(march2018, new GeoCoordinate(50, 50)),
                 new GeoPosition<GeoCoordinate>(march2018.AddMinutes(5), new GeoCoordinate(51.21709661403662,36.7782883744862374)),
                 new GeoPosition<GeoCoordinate>(march2018.AddMinutes(10), new GeoCoordinate(51.21709661403662,37.778288374486237450)),
                 new GeoPosition<GeoCoordinate>(march2018.AddMinutes(15), new GeoCoordinate(51.21709661403662,38.778288374486237450)),
                 new GeoPosition<GeoCoordinate>(march2018.AddMinutes(20), new GeoCoordinate(51.21709661403662,39.778288374486237450))
            };

            route = TruckRoute.Create();

            foreach ( var position in geoPositions )
            {
                route.RegisterNewTruckLocation( position.Location, position.Timestamp );
            }


            routes.Add( route );


            return routes.ToArray();

        }
    }
   


    


}