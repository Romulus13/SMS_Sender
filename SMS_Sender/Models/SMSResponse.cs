using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS_Sender.Models
{
    public class SMSResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }

        public SMSResponse(int status, string msg)
        {
            this.Message = msg;
            this.Status = status;
        }
        public SMSResponse() { }
    }
}