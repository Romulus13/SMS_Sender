using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS_Sender.Models
{
    public enum BulkDeliveryStatus
    {
        SUCCESS = 1,
        PARTIAL = 2,
        FAILURE = 3
    }
}