using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyShareDAL.Interfaces;

namespace KeyShareDAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IVehicleRepository Vehicles { get; }
        IBookingRepository Bookings { get; }
        IReviewRepository Reviews { get; }
        IMessageRepository Messages { get; }
        IPaymentRepository Payments { get; }
        INotificationRepository Notifications { get; }
        IAvailabilityCalendarRepository AvailabilityCalendars { get; }

        void Save();
    }
}