using System;
using System.Collections.Generic;
using System.Text;

namespace Comilio
{
    public enum SmsStatus
    {
        Scheduled,
        Enqueued,
        Sent,
        Delivering,
        Delivered,
        ErrorGeneric,
        ErrorPrefix,
        ErrorRecipient,
    }
}
