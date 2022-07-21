using API.Errors;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Order = Core.Entities.OrderAggregate.Order;

namespace API.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly string _webhookSecret;
        private readonly ILogger<IPaymentService> _logger;
        public PaymentsController(IPaymentService paymentService, ILogger<IPaymentService> logger, IConfiguration configuration)
        {
            _webhookSecret = configuration.GetSection("StripeSettings:WebHookSecret").Value;
            _logger = logger;
            _paymentService = paymentService;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePamentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            return basket == null ? BadRequest(new ApiResponse(400, "Problem with your basket")) : basket;
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var jason = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(jason, Request.Headers["Stripe-Signature"], _webhookSecret);
            PaymentIntent intent;
            Order order;

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Succeeded: ", intent.Id);
                    order = await _paymentService.UpdateOrderPaymentSuceeded(intent.Id);
                    _logger.LogInformation("Order updated to payment received: ", order.Id);
                    break;

                case "payment_intent.payment_failed":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Failed: ", intent.Id);
                    order = await _paymentService.UpdateOrderPaymentFailed(intent.Id);
                    _logger.LogInformation("Order updated to payment failed: ", order.Id);
                    break;
            }

            return new EmptyResult();
        }
    }
}