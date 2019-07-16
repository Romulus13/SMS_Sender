
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace DataLayer
{
    public class RecipientRepository
    {


        public List<RecipientDB> GetAllRecipients()
        {
            List<RecipientDB> recipients = new List<RecipientDB>();
            string commandText = @"SELECT  [ID],[FULLNAME],[CELLPHONE] FROM [Demo].[dbo].[RECIPIENTS] ";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlEXPRESS"].ToString()))
                {

                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            RecipientDB rec;

                            // Call Read before accessing data.
                            while (reader.Read())
                            {
                                rec = new RecipientDB();
                                rec.FullName = CommonFunctions.CheckReaderElementExists(reader, "FullName");
                                rec.CellPhone = CommonFunctions.CheckReaderElementExists(reader, "Cellphone"); ;

                                long id = 0;
                                if (Int64.TryParse(CommonFunctions.CheckReaderElementExists(reader, "Id"), out id))
                                {
                                    rec.Id = id;
                                }

                                recipients.Add(rec);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ///TODO log errors
                throw new CustomDbException(ex.Message, ex, "Error in fetching receivers from database");
            }
            return recipients;
        }

        public RecipientDB InsertRecipient(RecipientDB recipient)
        {
            if (recipient == null)
            {
                return recipient;
            }
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlEXPRESS"].ToString()))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (SqlCommand cmd = new SqlCommand("dbo.[insertRecipients]", connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@FULLNAME", SqlDbType.VarChar).Value = recipient.FullName;
                        cmd.Parameters.Add("@CELLPHONE", SqlDbType.VarChar).Value = recipient.CellPhone;
                        cmd.Parameters.Add("@INSERTED_ID", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        
                        cmd.ExecuteNonQuery();
                        recipient.Id = (long)cmd.Parameters["@INSERTED_ID"].Value;
                    }
          
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ///TODO log errors
                    throw new CustomDbException(ex.Message, ex, "Error in inserting receiver from database");
                }
            }
            return recipient;
        }


    }
}
