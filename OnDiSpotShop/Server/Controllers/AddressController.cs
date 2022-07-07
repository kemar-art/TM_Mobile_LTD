using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnDiSpotShop.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService addressService;

        public AddressController(IAddressService addressService)
        {
            this.addressService = addressService;
        }
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<Address>>> GetAddress()
        {
            return await addressService.GetAddress();
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<Address>>> AddOrUpdateAddress(Address address)
        {
            return await addressService.AddOrUpdateAddress(address);
        }
    }
}
