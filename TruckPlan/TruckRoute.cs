using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckPlanning
{
    public class TruckRoute
    {

        private List<GeoPosition<GeoCoordinate>> _positions = new List<GeoPosition<GeoCoordinate>>(); //Consider using hashset or dictionary


        protected TruckRoute() {
        
        }

        public static TruckRoute Create()
        {
            return new TruckRoute();
        }



        public IReadOnlyCollection<GeoPosition<GeoCoordinate>> RoutePositions { get { return _positions.ToImmutableArray(); } }

       

        public TruckArrivedAtLocation RegisterNewTruckLocation(GeoCoordinate position, DateTimeOffset utcTimestamp )
        {
            var geoPosition = new GeoPosition<GeoCoordinate>(utcTimestamp, position);

            uint nextSequense = (uint) _positions.Count() + 1;

            _positions.Add(geoPosition);

            return new TruckArrivedAtLocation(Guid.NewGuid(), geoPosition, nextSequense, utcTimestamp);
        }

        


        public record TruckArrivedAtLocation (Guid Id, GeoPosition<GeoCoordinate> GeoPosition, uint SequenceNumber , DateTimeOffset UtcTimestamp );


    }


    public class DistanceCalculator
    {
        private readonly ICivicAddressService _civicAddressService;

        public DistanceCalculator(ICivicAddressService civicAddressService)
        {
            _civicAddressService = civicAddressService;
        }


        public double GetDistanceDrivenAt( IList<GeoPosition<GeoCoordinate>> positions,  MonthInYear monthInYear )
        {
            var positionsInTime = positions.Where( p => 
            p.Timestamp.Year == monthInYear.Year 
            && p.Timestamp.Month == monthInYear.Month
            ).OrderBy( p => p.Timestamp ).ToList();

            var numberOfPositions = positionsInTime.Count;

            if ( !( numberOfPositions > 1 ) )
                return 0;


            var counter = 0;
            double distance = 0;

            var initialPosition = positionsInTime[ 0 ];

            while ( counter < numberOfPositions - 1 )
            {

                var position = positionsInTime[ counter ];
                var nextPosition = positionsInTime[ counter + 1 ];

                var deltaDistance = nextPosition.Location.GetDistanceTo( position.Location );

                distance = distance + deltaDistance;

                counter++;
            }

            return distance;

        }

        public async Task<double> GetDistanceDrivenInCountryAsync( List<GeoPosition<GeoCoordinate>> positions, MonthInYear monthInYear , string countryCode )
        {


            var positionsInCountry = new List<GeoPosition<GeoCoordinate>>();

            foreach ( var position in positions )
            {

                // We are making http request in loop. This is not optimal.
                var country = await _civicAddressService.GetCountryFromGeoLocationAsync( position.Location.Latitude, position.Location.Longitude );

                if ( country.CountryCode == countryCode )
                {
                    positionsInCountry.Add( position );
                }

            }

            return GetDistanceDrivenAt(positionsInCountry, monthInYear);

        }

    }

    
}
