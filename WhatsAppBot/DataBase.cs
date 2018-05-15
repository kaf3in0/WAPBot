using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System;

namespace BOT {
    namespace DataBase {
        public class Quest {
            public int id;
            public string type = "";
            public string rarity = "";
            public string status = "";
            public bool isEnabled;
            public string title = "";
            public string text = "";
            public int timeInHours;  // TIME IN HOURS
            public int sectCoins;
            public int sectPoints;
        }
        public class LastMsg {
            public string text;
            public string userPhoneNumber;
            public string date;
            public string time;
        }
        
        public static class WhatsAppDataBase {
             public static void UpdateLastMsgInGroup(string group, string newLastMesage) {
                string connectionString = ConfigurationManager.ConnectionStrings["WhatsAppBot.Properties.Settings.DataBaseBotConnectionString"].ConnectionString;
                string query =
                @"UPDATE Groups SET Groups.LastMesageText = @last_msg WHERE Groups.Name = @group_name;";
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection)) {
                        cmd.Parameters.AddWithValue("@last_msg", newLastMesage);
                        cmd.Parameters.AddWithValue("@group_name", group);
                        cmd.ExecuteNonQuery();
                        GetLastMsgFromConversation(group);
                    }
                }

             }
            public static string GetUserPhoneNumberById(int id) {
                string connectionString = ConfigurationManager.ConnectionStrings["WhatsAppBot.Properties.Settings.DataBaseBotConnectionString"].ConnectionString;
                string query =
                @"
                SELECT Users.PhoneNumber FROM Users WHERE Users.Id = @user_id;
                ";
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    connection.Open();
                    using(SqlCommand cmd = new SqlCommand(query, connection)) {
                        cmd.Parameters.AddWithValue("@user_id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader()) {
                            if (!reader.HasRows) return null;
                            reader.Read();
                            return reader["PhoneNumber"].ToString();
                        }
                    }
                }
            }
            public static void CreateNewGroup(string groupName, string lastMesage) {
                string connectionString = ConfigurationManager.ConnectionStrings["WhatsAppBot.Properties.Settings.DataBaseBotConnectionString"].ConnectionString;
                string query =
                @"
                INSERT INTO Groups(
                Groups.Name, 
                Groups.LastMesageUserId, 
                Groups.LastMesageText, 
                Groups.LastMesageDate, 
                Groups.LastMesageTime
                )
                VALUES (@group_name, 7, @last_Mesage, 'NULL', 'NULL');
                ";
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection)) {
                        cmd.Parameters.AddWithValue("@group_name", groupName);
                        cmd.Parameters.AddWithValue("@last_Mesage", lastMesage);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            public static LastMsg GetLastMsgFromConversation(string gorupName) {
                string connectionString = ConfigurationManager.ConnectionStrings["WhatsAppBot.Properties.Settings.DataBaseBotConnectionString"].ConnectionString;
                string query =
                @"
                SELECT * FROM Groups WHERE Groups.Name = @group_name
                ";
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection)) {
                        cmd.Parameters.AddWithValue("@group_name", gorupName);
                        using (SqlDataReader reader = cmd.ExecuteReader()) {
                            if (!reader.HasRows) { return null; }
                            reader.Read();
                            int userId = (int)reader["LastMesageUserId"];


                            LastMsg lastMsg = new LastMsg();
                            lastMsg.userPhoneNumber = GetUserPhoneNumberById(userId);
                            lastMsg.date = (string)reader["LastMesageDate"];
                            lastMsg.time = (string)reader["LastMesageTime"];
                            lastMsg.text = (string)reader["LastMesageText"];
                            return lastMsg;
                            
                        }
                    }
                }
            }
        }
        public static class QuestDataBase {

           
            private static bool QuestExists(int id, string connectionString) {
                string query =
                @"
                SELECT COUNT(*) FROM Quests WHERE Quests.Id = @quest_id
                ";
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection)) {
                        cmd.Parameters.AddWithValue("@quest_id", id);
                        if ((int)cmd.ExecuteScalar() > 0) {
                            return true;
                        } else return false;
                    }
                }
            }
            public static Quest GetQuestById(int id) {
                string connectionString = ConfigurationManager.ConnectionStrings["WhatsAppBot.Properties.Settings.DataBaseBotConnectionString"].ConnectionString;
                Quest q = new Quest();
                string query = @"SELECT * FROM Quests WHERE Quests.Id = @id";
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection)) {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader()) {
                            if (!reader.HasRows) { return null; }
                            reader.Read();

                            q.id = (int)reader["Id"];
                            q.type = (string)reader["TypeId"];
                            q.rarity = (string)reader["RarityId"];
                            if (!reader.IsDBNull(3))
                                q.status = (string)reader["StatusId"];
                            else q.status = null;
                            q.isEnabled = (bool)reader["IsEnabled"];
                            q.title = (string)reader["Title"];
                            q.text = (string)reader["Text"];
                            q.timeInHours = (int)reader["TimeInHours"];
                            q.sectCoins = (int)reader["SectCoins"];
                            q.sectPoints = (int)reader["SectPoints"];
                        } 
                    }
                }

                return q;
            }
            
        }
    }
}
