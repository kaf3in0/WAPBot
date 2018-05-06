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
        static QuestDataBase q = new QuestDataBase();
        static Wap wap = new Wap();
        static void printQuest(Quest q) {
                Console.WriteLine("ID: {0}\nENABLED: {1}\nCLASA: {2}\nRARITATE: {3}\nTITLU: {4}\nTEXT: {5}\nTIMER: {6}\nXP: {7}\nPUNCTE: {8}\nSTATUS: {9}",
                    q.id, q.enabled, q.clasa, q.raritate, q.titlu, q.text, q.timpOre, q.xp, q.puncte, q.status);
        }
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


        private static void ReadMsgs() {
            // Get the msgs from whatsapp
            IList<IWebElement> messages =  wap.GetMsgs();
            // Check if the msg is a comand or not
            foreach(var msg in messages) {
                // I dont have to check if i've read the msgs if i only read it once when I get a new msg
                string msgText = wap.GetMsgText(msg);
                if (msgText == null) { // If the msg list BROKE just do this function again
                    ReadMsgs();
                }
                string msgSender = wap.GetMsgSender(msg);
                if(msgSender == null) {
                    ReadMsgs();// If the msg list BROKE just do this function again
                }
                // Instead check if the msg is the last one that we read... from the database
                try { IWebElement star = msg.FindElement(By.XPath(".//span[contains(@data-icon,'star')]")); }
                catch {
                    wap.StarMsg(msg);
                    System.Threading.Thread.Sleep(2000);
                    Console.WriteLine("{0}: {1}", msgSender, msgText);
                    UserComand.IfComandExecute(msgText, msgSender, wap);
                    AdminComand.IfComandExecute(msgText, msgSender, wap);
                }

                
            }
            // If it is a comand execute it acordingly; Comands class should do the rest
            
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
                    ReadMsgs();
                    // Make sure you get to read all the msgs in that group after clicking on it
                    if(wap.GetCurrentTargetName() != "Nimic") {
                        wap.AccesTarget("Nimic"); //After you read the msgs return to the dummy group
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