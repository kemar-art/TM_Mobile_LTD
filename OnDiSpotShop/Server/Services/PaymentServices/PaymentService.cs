using Stripe;
using Stripe.Checkout;

namespace OnDiSpotShop.Server.Services.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartService cartService;
        private readonly IAuthService authService;
        private readonly IOrderService orderService;

    
        const string secret = "whsec_1daa30845c47f497857854ccf348d7d8a951d2c432339317a2de50aaf763ada0";

        public PaymentService(ICartService cartService, IAuthService authService, IOrderService orderService)
        {
            StripeConfiguration.ApiKey = "sk_test_51LGqLWDS5s5cwn5d1fHIRhXCy9jUp2edhrTQniyKRO8eXu6ilHiEymDsZc3GjdKdPfyxe5Eg6r6q6GUAi4iFOz8s00U582WZ0D";
            this.cartService = cartService;
            this.authService = authService;
            this.orderService = orderService;


        }
        public async Task<Session> CreateCheckoutSession()
        {
            var products = (await cartService.GetDbCartProducts()).Data;
            var lineItems = new List<SessionLineItemOptions>();
            products.ForEach(product => lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = product.Price * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Name,
                        Images = new List<string> { product.ImageUrl }
                    }
                },
                Quantity = product.Quantity
            }));

            var options = new SessionCreateOptions
            {
                CustomerEmail = authService.GetUserEmail(),
                ShippingAddressCollection = new SessionShippingAddressCollectionOptions
                {
                    AllowedCountries = new List<string> { "JM" }
                },
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:7151/order-success",
                CancelUrl = "https://localhost:7151/cart"
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return session;
        }

        public async Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request)
        {
            var json = await new StreamReader(request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                        json,
                        request.Headers["Stripe-Signature"],
                        secret
                    );

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    var user = await authService.GetUserByEmail(session.CustomerEmail);
                    await orderService.PlaceOrder(user.Id);
                }

                return new ServiceResponse<bool> { Data = true };
            }
            catch (StripeException e)
            {
                return new ServiceResponse<bool> { Data = false, Success = false, Message = e.Message };
            }
        }
    }
}
