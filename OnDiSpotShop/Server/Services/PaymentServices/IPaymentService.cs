﻿using Stripe.Checkout;

namespace OnDiSpotShop.Server.Services.PaymentServices
{
    public interface IPaymentService
    {
        Task<Session> CreateCheckoutSession();
        Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request);
    }
}
