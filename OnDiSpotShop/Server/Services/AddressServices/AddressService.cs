namespace OnDiSpotShop.Client.Services.AddressServices
{
    public class AddressService : IAddressService
    {
        private readonly DataContext context;
        private readonly IAuthService authService;

        public async Task<ServiceResponse<Address>> AddOrUpdateAddress(Address address)
        {
            var response = new ServiceResponse<Address>();
            var dbAddress = (await GetAddress()).Data;
            if (dbAddress == null)
            {
                address.UserId = authService.GetUserId();
                context.Addresses.Add(address);
                response.Data = address;
            }
            else
            {
                dbAddress.FirstName = address.FirstName;
                dbAddress.LastName = address.LastName;
                dbAddress.Parish = address.Parish;
                dbAddress.Country = address.Country;
                dbAddress.City = address.City;
                dbAddress.Cedex = address.Cedex;
                dbAddress.Street = address.Street;
                response.Data = dbAddress;
            }

            await context.SaveChangesAsync();

            return response;
        }

        public async Task<ServiceResponse<Address>> GetAddress()
        {
            int userId = authService.GetUserId();
            var address = await context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId);
            return new ServiceResponse<Address> { Data = address };
        }
    }
}

