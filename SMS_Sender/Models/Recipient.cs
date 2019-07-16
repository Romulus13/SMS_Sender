using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;

namespace SMS_Sender.Models
{
    public class Recipient
    {

        public Recipient() { }
        public Recipient(RecipientDB rec)
        {
            if (rec != null)
            {
                this.Id = rec.Id;
                this.FullName = rec.FullName;
                this.CellPhone = rec.CellPhone;
            }
        }

        public Int64 Id { get; set; }
        public String CellPhone { get; set; }

        public string FullName { get; set; }

        public bool? SendSMS { get; set; }

        public RecipientDB MapToRecipientDB()
        {
            RecipientDB recDb = new RecipientDB();
            recDb.Id = this.Id;
            recDb.FullName = this.FullName;
            recDb.CellPhone = this.CellPhone;

            return recDb;
        }
    }
}