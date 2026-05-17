using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyShareDAL.Context;
using KeyShareDAL.Interfaces;
using KeyshareDL.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeyShareDAL.Repositories
{
    public class AvailabilityCalendarRepository : IAvailabilityCalendarRepository
    {
        private readonly KeyShareContext _context;

        public AvailabilityCalendarRepository(KeyShareContext context)
        {
            _context = context;
        }

        public AvailabilityCalendar? GetCalendarByVehicle(int vehicleId)
        {
            return _context.AvailabilityCalendars
                .Include(a => a.Vehicle)
                .FirstOrDefault(a => a.VehicleId == vehicleId);
        }

        public AvailabilityCalendar AddCalendar(AvailabilityCalendar calendar)
        {
            _context.AvailabilityCalendars.Add(calendar);
            return calendar;
        }

        public AvailabilityCalendar UpdateCalendar(AvailabilityCalendar calendar)
        {
            _context.AvailabilityCalendars.Update(calendar);
            return calendar;
        }

        public AvailabilityCalendar DeleteCalendar(AvailabilityCalendar calendar)
        {
            _context.AvailabilityCalendars.Remove(calendar);
            return calendar;
        }
    }
}