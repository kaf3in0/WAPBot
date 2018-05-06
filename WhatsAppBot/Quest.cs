using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System;

namespace BOT {
    namespace DataBase {
        public class Quest {
            public string clasa = "";
            public bool enabled;
            public string id = "";
            public string raritate = "";
            public string titlu = "";
            public string text = "";
            public string xp = "";
            public string puncte = "";
            public string status = "";
            public string timpOre = "";   // TIME IN HOURS
        }

        public class QuestDataBase {
            string connectionString = "";
            SqlConnection connection;
            // Constructor
            public QuestDataBase() {
                connectionString = ConfigurationManager.ConnectionStrings["WhatsAppBot.Properties.Settings.DataBaseBotConnectionString"].ConnectionString;
                plm();
            }
            public void plm() {
                //using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Quests", connection))
                //using (connection = new SqlConnection(connectionString))
                //{
                //    SqlDataReader dataReader = 
                //}


                using(SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();      // I dont think this is necessay. i think it auto opens

                    using(SqlCommand cmd = new SqlCommand("SELECT * FROM Quests", connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {

                                    for(int i = 0; i<reader.FieldCount; i++)
                                        Console.WriteLine(reader.GetName(i));
                                    //do something
                                }
                            }
                        } // reader closed and disposed up here

                    } // command disposed here

                } //connection closed and disposed here

            }
        }
    }





}
