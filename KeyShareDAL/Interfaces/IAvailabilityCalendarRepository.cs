using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyshareDL.Entities;
using KeyShareDL.Entities;

namespace KeyShareDAL.Interfaces
{
    public interface IAvailabilityCalendarRepository
    {
        AvailabilityCalendar? GetCalendarByVehicle(int vehicleId);
        AvailabilityCalendar AddCalendar(AvailabilityCalendar calendar);
        AvailabilityCalendar UpdateCalendar(AvailabilityCalendar calendar);
        AvailabilityCalendar DeleteCalendar(AvailabilityCalendar calendar);
    }
}