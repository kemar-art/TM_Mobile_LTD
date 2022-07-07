namespace OnDiSpotShop.Client.Services.AddressServices
{
    public interface IAddressService
    {
        Task<ServiceResponse<Address>> GetAddress();
        Task<ServiceResponse<Address>> AddOrUpdateAddress(Address address);
    }
}
