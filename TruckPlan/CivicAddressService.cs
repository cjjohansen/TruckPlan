using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace TruckPlanning
{


    public record CountryDescriptor(string CountryCode, string CountryName );


    public record CivicAddressServiceConfiguration(string API_KEY, string BaseURL);

    public class CivicAddressService : ICivicAddressService
    {
        CivicAddressServiceConfiguration _configuration;

        private JsonSerializerSettings _serialiserSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        public CivicAddressService(HttpClient httpClient, CivicAddressServiceConfiguration configuration ) {
            HttpClient = httpClient;
            _configuration = configuration;
        }

        public HttpClient HttpClient { private get; init; }

        public async Task<CountryDescriptor> GetCountryFromGeoLocationAsync( double latitude, double longitude )
        {

            string url = _configuration.BaseURL;

            var queryBuilder = new UriBuilder( url );
                                                         
            string latitudeAsString = latitude.ToString( "0.00000000000000", System.Globalization.CultureInfo.InvariantCulture );
            string longitudeAsString = longitude.ToString( "0.00000000000000", System.Globalization.CultureInfo.InvariantCulture );

            queryBuilder.Query = $"lat={latitudeAsString}&lon={longitudeAsString}&apiKey={_configuration.API_KEY}";

            var urlWithQuery = queryBuilder.ToString();

            HttpClient.DefaultRequestHeaders.Accept
            .Add( new MediaTypeWithQualityHeaderValue( "application/json" ) );

            HttpResponseMessage response = await HttpClient.GetAsync( urlWithQuery );

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            GeoResponse? geoResponse = null;

            try
            {
                geoResponse = JsonConvert.DeserializeObject<GeoResponse>( responseBody, _serialiserSettings );
            }
            catch(Exception ex )
            {
                throw new ApplicationException( "Could not Deserialize geo response", ex );
            }

            var feature = geoResponse?.Features.FirstOrDefault();

            if(feature == null)
            {
                throw new ApplicationException( "Internal error geo Response could not be identified");
            }

            var countryDescriptor = new CountryDescriptor( feature.Properties.CountryCode.ToString(), feature.Properties.Country.ToString());

            return countryDescriptor;
        }
    }
}
