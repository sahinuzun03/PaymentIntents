using Microsoft.AspNetCore.Mvc;
using PaymentIntents.Models;
using Stripe;

namespace PaymentIntents.Controllers
{
    public class PaymentIntentController : Controller
    {
        private readonly IConfiguration _config;
        public PaymentIntentController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult PaymentPage()
        {
            return View("Payment");
        }

        [HttpPost]
        public async Task<IActionResult> Payment([FromBody]PaymentData paymentData)
        {
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];


            var options = new PaymentIntentCreateOptions
            {
                Amount = 1000000,
                Currency = "usd",
                Description = "Test Value",
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                PaymentMethod = paymentData.PaymentMethod,
                SetupFutureUsage = "off_session",
                ReceiptEmail = "sahinuzun03@gmail.com",
            };



            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            return Json(new { clientSecret = paymentIntent.ClientSecret });
        }
    }
}
