using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class MessageDB
    {
        public DateTime DateSent { get; set; }

        public RecipientDB Recipient { get; set; }

        public String FileName { get; set; }

        public String SMSFilePath { get; set; }

        public Int64 Id { get; set; }
    }
}
