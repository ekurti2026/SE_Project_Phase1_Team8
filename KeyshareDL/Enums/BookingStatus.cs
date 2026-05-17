using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyshareDL.Enums
{
    public enum BookingStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        Cancelled = 3,
        Confirmed = 4,
        PaymentPending = 5,
        PaymentFailed = 6,
        InProgress = 7,
        Completed = 8,
        Reviewed = 9
    }
}
