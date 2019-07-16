using SMS_Sender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
namespace SMS_Sender.Controllers
{
    public class HomeController : Controller
    {

        //not the best place for repository fields, however for simplicity's sake I've put them here
        private RecipientRepository recRep;
        private MessageRepository msgRep;
        public HomeController()
        {
            recRep = new RecipientRepository();
            msgRep = new MessageRepository();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public JsonResult GetRecipients()
        {
            List<Recipient> recipients = new List<Recipient>();
            List<RecipientDB> recipientsDB = this.recRep.GetAllRecipients();
            foreach (RecipientDB rec in recipientsDB)
            {
                Recipient mappedRec = new Recipient(rec);
                recipients.Add(mappedRec);
            }
            return Json(recipients, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddRecipient(Recipient toAdd)
        {

            RecipientDB addedRec = this.recRep.InsertRecipient(toAdd.MapToRecipientDB());

            return Json(addedRec, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult SendMessages(List<Message> toSend)
        {
            if (toSend == null)
            {
                return Json(new SMSResponse() { Status =404, Message = "Nema poruka za poslati!" }) ;
            }
            SMSResponse response = new SMSResponse();
            Dictionary<Message, bool> sendStatus = new Dictionary<Message, bool>();
            try
            {
                foreach (Message msg in toSend)
                {
                    sendStatus.Add(msg,msg.SendMessage());
                }
                
                BulkInsertStatus insStatus = this.msgRep.InsertMessage(toSend.Select(x=>x.MapToMessageDB()).ToList());
                BulkDeliveryStatus delivStatus = Message.DetermineMessageDeliveryStatus(sendStatus);
                response = Message.MessageToDeliver( delivStatus, insStatus);
            }
            catch(Exception ex)
            {
                ///TODO log exception
                return Json(new SMSResponse(500, "Error in saving messages"));
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }


    }
}