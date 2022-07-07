namespace OnDiSpotShop.Client.Services.AddressServices
{
    public interface IAddressService
    {
        Task<Address> GetAddress();
        Task<Address> AddOrUpdateAddress(Address address);
    }
}
