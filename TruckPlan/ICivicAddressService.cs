namespace TruckPlanning
{
    public interface ICivicAddressService //TODO: interface could be moved to inner layer
    {
        Task<CountryDescriptor> GetCountryFromGeoLocationAsync(double latitude, double longitude);
    }
}
