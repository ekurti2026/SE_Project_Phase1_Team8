using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyShareDAL.Context;
using KeyShareDAL.Interfaces;
using KeyShareDAL.Repositories;

namespace KeyShareDAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly KeyShareContext _context;

        public IUserRepository Users { get; }
        public IVehicleRepository Vehicles { get; }
        public IBookingRepository Bookings { get; }
        public IReviewRepository Reviews { get; }
        public IMessageRepository Messages { get; }
        public IPaymentRepository Payments { get; }
        public INotificationRepository Notifications { get; }
        public IAvailabilityCalendarRepository AvailabilityCalendars { get; }

        public UnitOfWork(KeyShareContext context)
        {
            _context = context;
            Users = new UserRepository(context);
            Vehicles = new VehicleRepository(context);
            Bookings = new BookingRepository(context);
            Reviews = new ReviewRepository(context);
            Messages = new MessageRepository(context);
            Payments = new PaymentRepository(context);
            Notifications = new NotificationRepository(context);
            AvailabilityCalendars = new AvailabilityCalendarRepository(context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}