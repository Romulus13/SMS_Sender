using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class MessageRepository
    {
        public BulkInsertStatus InsertMessage(List<MessageDB> messages)
        {

            BulkInsertStatus status = BulkInsertStatus.FAILURE;
            if (messages == null)
            {
                return status; 
            }
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlEXPRESS"].ToString()))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (SqlCommand cmd = new SqlCommand("dbo.[insertMessages]", connection, transaction))
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("RECIPIENT");
                        dt.Columns.Add("SMS_FILENAME");
                        dt.Columns.Add("CELLPHONE");
                        dt.Columns.Add("TIME_SENT",System.Type.GetType("System.DateTime"));
                        foreach (MessageDB msg in messages)
                        {
                            ///lazy loading,first we check if recipient exists, then we check if recipient has a name and cellphone number
                            if (msg.Recipient != null && !String.IsNullOrEmpty(msg.Recipient.CellPhone) && !String.IsNullOrEmpty(msg.Recipient.FullName) )
                                dt.Rows.Add(msg.Recipient.FullName, msg.FileName, msg.Recipient.CellPhone, msg.DateSent);
                        }
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MessagesToInput", dt);
                        int numOfInserted =  cmd.ExecuteNonQuery();
                        if (numOfInserted == 0)
                            status = BulkInsertStatus.NONE_INSERTED;
                        else if (numOfInserted > 0)
                            status = BulkInsertStatus.SUCCESS;
                    }

                    transaction.Commit();
                    
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ///explicitly put status back to failure in case we made there was error in commit
                    status = BulkInsertStatus.FAILURE;
                    ///TODO log errors
                    throw new CustomDbException(ex.Message, ex, "Error in inserting recipient from database");
                }
            }
            
            return status;
        }


    }
}
