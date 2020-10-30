using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using brightcast.Entities;
using brightcast.Helpers;
using brightcast.Models.Checkout;
using brightcast.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace brightcast.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MembershipController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IUserProfileService _userProfileService;
        private readonly IMembershipService _membershipService;
        private readonly IBusinessService _businessService;

        public MembershipController(
            IUserProfileService userProfileService,
            IMembershipService membershipService,
            IBusinessService businessService,
            IOptions<AppSettings> appSettings
        )
        {
            _userProfileService = userProfileService;
            _membershipService = membershipService;
            _businessService = businessService;
            _appSettings = appSettings.Value;
        }


        [HttpPost("checkout/create")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CreateCheckoutSessionRequest req)
        {
            int userId;

            try
            {
                userId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
            }
            catch (Exception)
            {
                return BadRequest("User not found");
            }

            var userProfile = _userProfileService.GetAllByUserId(userId)
                .FirstOrDefault(x => x.Default && x.Deleted == 0);

            if (userProfile == null || userProfile.Id == 0)
                return NotFound(
                    new
                    {
                        message = "UserProfile Not Found"
                    });


            var options = new SessionCreateOptions
            {
                // See https://stripe.com/docs/api/checkout/sessions/create
                // for additional parameters to pass.
                // {CHECKOUT_SESSION_ID} is a string literal; do not change it!
                // the actual Session ID is returned in the query parameter when your customer
                // is redirected to the success page.

                SuccessUrl = $"{_appSettings.UiBaseUrl}/pages/main/checkout/success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{_appSettings.UiBaseUrl}/pages/main/checkout/canceled",
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                Mode = "subscription",
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = req.PriceId,
                        Quantity = 1,
                    },
                },
            };
            var stripeClient = new StripeClient(_appSettings.StripeApiKey);
            var service = new SessionService(stripeClient);
            try
            {
                var session = await service.CreateAsync(options);

                _membershipService.CreateCheckoutSession(new CheckoutSession()
                {
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "API",
                    Deleted = 0,
                    StripeCheckoutSessionId = session.Id,
                    PaymentStatus = session.PaymentStatus,
                    Mode = session.Mode,
                    StripeCustomerId = session.CustomerId,
                    StripeSubscriptionId = session.SubscriptionId,
                    UserProfileId = userProfile.Id,
                });

                return Ok(new CreateCheckoutSessionResponse
                {
                    SessionId = session.Id,
                });
            }
            catch (StripeException e)
            {
                Console.WriteLine(e.StripeError.Message);
                return BadRequest( e.StripeError.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            Event stripeEvent;
            try
            {
                var webhookSecret = _appSettings.StripeWebhookSecret;
                stripeEvent = EventUtility.ParseEvent(
                    json);

                Console.WriteLine($"Webhook notification with type: {stripeEvent.Type} found for {stripeEvent.Id}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Something failed {e}");
                return BadRequest();
            }

            var session = stripeEvent.Data.Object as Session;

            switch (stripeEvent.Type)
            {
                case Events.CheckoutSessionCompleted:
                    // Payment is successful and the subscription is created.
                    // You should provision the subscription.

                    

                    if ( session != null && session.PaymentStatus == "paid")
                    {
                        var dbSession = _membershipService.GetSessionById(session.Id);
                        dbSession.PaymentStatus = "paid";
                        dbSession.StripeCustomerId = session.CustomerId;
                        dbSession.StripeSubscriptionId = session.SubscriptionId;
                        dbSession.UpdatedAt = DateTime.UtcNow;
                        dbSession.UpdatedBy = "webhook";
                        _membershipService.UpdateCheckoutSession(dbSession);
                        _businessService.UpdateMembership(dbSession.UserProfileId, "premium");
                    }

                    break;
                case  Events.InvoicePaid:
                    //send email with confirmation
                    if (session != null && session.PaymentStatus == "paid")
                    {
                        var dbSession = _membershipService.GetSessionById(session.Id);
                        dbSession.PaymentStatus = "paid";
                        dbSession.StripeCustomerId = session.CustomerId;
                        dbSession.StripeSubscriptionId = session.SubscriptionId;
                        dbSession.UpdatedAt = DateTime.UtcNow;
                        dbSession.UpdatedBy = "webhook";
                        _membershipService.UpdateCheckoutSession(dbSession);
                        _businessService.UpdateMembership(dbSession.UserProfileId, "premium");
                    }
                    break;
                case Events.InvoicePaymentFailed:
                    if ( session != null && session.PaymentStatus == "unpaid")
                    {
                        var dbSession = _membershipService.GetSessionById(session.Id);
                        dbSession.PaymentStatus = "unpaid";
                        dbSession.StripeCustomerId = session.CustomerId;
                        dbSession.StripeSubscriptionId = session.SubscriptionId;
                        dbSession.UpdatedAt = DateTime.UtcNow;
                        dbSession.UpdatedBy = "webhook";
                        _membershipService.UpdateCheckoutSession(dbSession);
                        _businessService.UpdateMembership(dbSession.UserProfileId, "free");
                    }
                    break;
            }

            return Ok();
        }
    }
}
