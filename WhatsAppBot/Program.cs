using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using System.Threading;
using BOT.Comands;
using BOT.DataBase;
using WhatsApp.WebElements;
using System.Timers;
// This file should be the glue that brings all "APIs/frameworks" together

namespace BOT {
    class Program {
       
        static Wap wap = new Wap();
        
        private static void CheckConsoleComands() {

            while (true) {
                string input = Console.ReadLine().ToLower();    
                AdminComand.IfComandExecute(input,  "OWNER", wap);
                UserComand.IfComandExecute(input, "OWNER", wap);
            }

        }
        public static bool ClickOnNewMessages() {
            try {
                IWebElement newMsg = wap.GetUnreadMesages();
                newMsg.Click();
                return true;
            }catch {
                return false;
            }
            
        }

        public static void ReadMsgsUsingDataBase() {
            IList<IWebElement> messages =  wap.GetMsgs();
            string group = wap.GetCurrentTargetName();
            // If the group exists in the data base check the last msg
            LastMsg lastConversationMsg = WhatsAppDataBase.GetLastMsgFromConversation(group);
            // If the group/user doesn't exist in the database, create it
            if(lastConversationMsg == null) {
                // Create it
                WhatsAppDataBase.CreateNewGroup(group, wap.GetMsgText(messages[messages.Count - 1]));
            } else {
                for (int i = messages.Count -1; i >= 0; i--) {
                    string msgText = wap.GetMsgText(messages[i]);
                    string msgUser = wap.GetMsgSender(messages[i]);
                    // If the last msg from the group is identical we stop reading the chat
                    if (msgText == lastConversationMsg.text) {
                        string lastMsgTextPula = wap.GetMsgText(messages[messages.Count - 1]);
                        WhatsAppDataBase.UpdateLastMsgInGroup(group, lastMsgTextPula);
                        return;
                    } else {
                        string lastMsgTextPizda = wap.GetMsgText(messages[messages.Count - 1]);
                        WhatsAppDataBase.UpdateLastMsgInGroup(group, lastMsgTextPizda);
                        UserComand.IfComandExecute(msgText, group, wap);
                    }
                }
                string lastMsgText = wap.GetMsgText(messages[messages.Count - 1]);
                WhatsAppDataBase.UpdateLastMsgInGroup(group, lastMsgText);
            }

           
        }
        private static void InitQuestTimer() {
            System.Timers.Timer t = new System.Timers.Timer(TimeSpan.FromSeconds(10).TotalMilliseconds); // set the time (5 min in this case)
            t.AutoReset = true;
            // GIVE QUESST
            //t.Elapsed += new ElapsedEventHandler(GiveQuest);
            t.Start();
        }
        private static void InitComandsFromConsoleThreads() {
            Thread consoleComands = new Thread(CheckConsoleComands);
            consoleComands.Start();
        }
        private static void InitProgram() {
            InitQuestTimer();
            InitComandsFromConsoleThreads();
        }
        //sender = msg[i].GetAttribute("innerHTML"); } catch { return;
        static void Main(string[] args) {
            InitProgram();
            
            while (true) {
                // Check if there is any new msg
                if(ClickOnNewMessages()) {
                    
                    // If there is a new msg, click on the chat and read the msg
                    ReadMsgsUsingDataBase();
                    // Make sure you get to read all the msgs in that group after clicking on it
                    if(wap.GetCurrentTargetName() != "Nimic 2") {
                        wap.AccesTarget("Nimic 2"); //After you read the msgs return to the dummy group
                    }
                }
                System.Threading.Thread.Sleep(5000);
                // If there are no new mesages do nothing
                //Console.WriteLine("No new mesages");
            }

        }


           
            

    }
}



 //* Timer t = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds); // set the time (5 min in this case)
 //           t.AutoReset = true;
 //           t.Elapsed += new System.Timers.ElapsedEventHandler(your_method);
 //           t.Start();
            





       





/*
 static void printMesages(List<Mesage> m)
    {
        for (int i = 0; i < m.Count; i++)
        {
            Console.WriteLine(m[i].msg);
            Console.WriteLine(m[i].repliedTo);
            Console.WriteLine(m[i].sender);
            Console.ForegroundColor = ConsoleColor.Red;
            switch (m[i].msgType)
            {

                case EMsgType.TEXT:
                    Console.WriteLine(EMsgType.TEXT.ToString());
                    break;
                case EMsgType.IMAGE:
                    Console.WriteLine(EMsgType.IMAGE.ToString());
                    break;
                case EMsgType.VIDEO:
                    Console.WriteLine(EMsgType.VIDEO.ToString());
                    break;
                case EMsgType.REPLY:
                    Console.WriteLine(EMsgType.REPLY.ToString());
                    break;
                case EMsgType.SOUND:
                    Console.WriteLine(EMsgType.SOUND.ToString());
                    break;
            }
            Console.ResetColor();
            Console.WriteLine();
        }
    }

    */