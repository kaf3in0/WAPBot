using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;


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
        public static class QuestDataBase {

            // If modifying these scopes, delete your previously saved credentials
            // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
            static SheetsService service;
            static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
            static string ApplicationName = "QUEST_DATA_BASE";
            static UserCredential GetCredential() {
                UserCredential credential;

                using (var stream =
                    new FileStream("client_secret.json", FileMode.Open, FileAccess.Read)) {
                    string credPath = System.Environment.GetFolderPath(
                        System.Environment.SpecialFolder.Personal);
                    credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;

                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                return credential;
            }
            private static void InitService() {
                // Create Google Sheets API service.
                service = new SheetsService(new BaseClientService.Initializer() {
                    HttpClientInitializer = GetCredential(),
                    ApplicationName = ApplicationName,
                });
            }
            public static List<Quest> GetDataBase() {
                List<Quest> quests = new List<Quest>();
                InitService();



                // Define request parameters.
                String spreadsheetId = "1Stf8I-k9s1R-EC89r1y-S0_tUggl7y2a33aMMj858f0";
                String range = "Foaie1";   // The entire page (Sheet1)
                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);


                ValueRange response = request.Execute();
                IList<IList<Object>> values = response.Values;
                if (values != null && values.Count > 0) {

                    for (int row = 2; row < values.Count; row++) {
                        Quest quest = new Quest();
                        quest.clasa = values[row][0].ToString();

                        if (values[row][1].ToString() == "FALSE")
                            quest.enabled = false;
                        else quest.enabled = true;
                        quest.id = values[row][2].ToString();
                        quest.raritate = values[row][3].ToString();
                        quest.titlu = values[row][4].ToString();
                        quest.text = values[row][5].ToString();
                        quest.xp = values[row][6].ToString();
                        quest.puncte = values[row][7].ToString();
                        quest.status = values[row][8].ToString();
                        quest.timpOre = values[row][9].ToString();
                        quests.Add(quest);
                    }
                } else {
                    Console.WriteLine("No data found.");
                }

                return quests;
            }

        }
    }





}
