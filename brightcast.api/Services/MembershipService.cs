using System;
using System.Linq;
using brightcast.Entities;
using brightcast.Helpers;

namespace brightcast.Services
{
    public interface IMembershipService
    {
        CheckoutSession CreateCheckoutSession(CheckoutSession session);
        void UpdateCheckoutSession(CheckoutSession session);
        CheckoutSession GetSessionById(string sessionId);


    }

    public class MembershipService : IMembershipService
    {
        private DataContext _context;

        public MembershipService(DataContext context)
        {
            _context = context;
        }


        public CheckoutSession CreateCheckoutSession(CheckoutSession session)
        {
            session.CreatedAt = DateTime.UtcNow;
            session.CreatedBy = "API";

            session.Deleted = 0;

            _context.CheckoutSessions.Add(session);
            _context.SaveChanges();

            return session;
        }

        public void UpdateCheckoutSession(CheckoutSession session)
        {

            _context.CheckoutSessions.Update(session);
            _context.SaveChanges();
            }

        public CheckoutSession GetSessionById(string sessionId)
        {
            return _context.CheckoutSessions.Where(x => x.StripeCheckoutSessionId == sessionId)
                .SingleOrDefault(x => x.Deleted == 0);
        }
    }
}