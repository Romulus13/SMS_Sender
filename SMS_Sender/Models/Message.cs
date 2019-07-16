using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using DataLayer;
namespace SMS_Sender.Models
{
    public class Message
    {
        public Message()
        {
            this.SMSFilePath = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["SMS_save_Folder"].ToString();

        }
        public DateTime DateSent { get; set; }

        public Recipient Recipient { get; set; }

        public String FileName { get; set; }

        public String SMSFilePath { get; set; }

        public String Content { get; set; }

        public Int64 Id { get; set; }
        public MessageDB MapToMessageDB()
        {
            MessageDB msgDb = new MessageDB();
            msgDb.Id = this.Id;
            if (this.Recipient != null)
            {
                msgDb.Recipient = this.Recipient.MapToRecipientDB();
            }
            
            msgDb.SMSFilePath = this.SMSFilePath;
            msgDb.DateSent = this.DateSent;
            msgDb.FileName = this.FileName;

            return msgDb;
        }
        /// <summary>
        /// This method simulates the process of sending a SMS message.
        /// </summary>
        internal bool SendMessage()
        {
            try
            {
                if (this.Recipient != null)
                {
                    this.FileName = "demo_" + this.Recipient.CellPhone.Replace(' ', '_') + ".txt";
                }
                using (StreamWriter sw = File.AppendText(this.SMSFilePath + "\\" + this.FileName))
                {
                    this.DateSent = DateTime.Now;
                    sw.WriteLine(this.DateSent);
                    sw.WriteLine(this.Content);
                    sw.WriteLine("----------------------------------------------------------");
                }
            }
            catch (Exception)
            {
                ///TODO: log exceptions
                ///However we will still pass on throwing them, instead we are sending false as a signal for delivery failure
                return false;
            }

            return true;
        }
        /// <summary>
        /// Determine to what degree have we managed to send the SMS messages. The barebones version handles just a few statuses.
        /// This method can be upgraded to handle the exact numbers ( messages sent compared to messages delivered)
        /// </summary>
        /// <param name="sendStatuses"></param>
        /// <returns></returns>
        public static BulkDeliveryStatus DetermineMessageDeliveryStatus(Dictionary<Message,bool> sendStatuses)
        {
            ///lets presume all messages were not delivered
            BulkDeliveryStatus status = BulkDeliveryStatus.FAILURE;
            ///we will use counter variables because that means we only have to loop through the dictionary once.
            ///and the code is more understandable
            //////fior simplicity's sake we could use Enumerable.All method but then we looping through the dictionary multiple times unless we create a more complex Predicate...
            int deliveredCnt = 0;
            int undeliveredCnt = 0;
            foreach (var sendStatusMsg in sendStatuses.Values)
            {
                if (sendStatusMsg == false)
                {
                    undeliveredCnt++;
                }
                else
                {
                    deliveredCnt++;
                }
            }

            if (undeliveredCnt >0  && deliveredCnt > 0)
            {
                status = BulkDeliveryStatus.PARTIAL;
            }
            else if (deliveredCnt == sendStatuses.Count && undeliveredCnt == 0)
            {
                status = BulkDeliveryStatus.SUCCESS;
            }
            return status;
        }


        public static SMSResponse MessageToDeliver(BulkDeliveryStatus dlvStat, BulkInsertStatus insStat)
        {
            SMSResponse resp = new SMSResponse();
            string msg = string.Empty;
            if (dlvStat == BulkDeliveryStatus.SUCCESS && insStat == BulkInsertStatus.SUCCESS)
            {
                resp.Message = "Poruke poslane i zabilježene!";
                resp.Status = 200;
            }
            else if (dlvStat == BulkDeliveryStatus.SUCCESS && insStat != BulkInsertStatus.SUCCESS)
            {
                resp.Message = "Poruke poslane ali njihovo slanje nije zabilježeno!";
                resp.Status = 202;
            }
            else if(dlvStat == BulkDeliveryStatus.PARTIAL)
            {
                resp.Message = "Nisu sve poruke poslane!";
                resp.Status = 500;
            }
            else {

                resp.Message = "Poruke nisu poslane!";
                resp.Status = 500;
                
            }
            return resp;
        }
    }
}