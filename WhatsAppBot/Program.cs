using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using System.Timers;
using BOT.Comands;
using BOT.DataBase;
using WhatsApp.WebElements;
using System.Threading;
// This file should be the glue that brings all "APIs/frameworks" together

namespace BOT {

    class Groups {
        // //span[@class='OUeyt']
        // Since we check all new mesages that we get I dont think we need to limit the groups
                //we iterate through, unless i dont want people outside the group to acces it
    }
    class Program {
        static Wap wap = new Wap();
        static List<Quest> quests = QuestDataBase.GetDataBase();
        static void printQuests() {
            foreach (var q in quests) {
                Console.WriteLine("{0}: {1}: {2} {3} {4} {5} {6} {7} {8} {9}",
                    q.clasa, q.enabled, q.id, q.raritate, q.titlu, q.text, q.xp, q.puncte, q.status, q.timpOre);
            }
        }
        private static void your_method(object sender, ElapsedEventArgs e) {
            printQuests();
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
        private static string GetMsgText(IWebElement msg) {
            try {
                string m = msg.FindElement(By.XPath(".//span[contains(@class,'selectable-text')]")).Text;
                return m;
            } catch { return "FALSE"; }
        }
        private static string GetMsgSender(IWebElement msg) {
            try {
                string buffer = msg.GetAttribute("innerHTML");
                string sender = wap.FormatSender(buffer);
                return sender;
            }
            catch { return "NONE"; }
            
        }
        private static void ReadMsgs() {
            // Get the msgs from whatsapp
            IList<IWebElement> messages =  wap.GetMsgs();
            // Check if the msg is a comand or not
            foreach(var msg in messages) {
                // I dont have to check if i've read the msgs if i only read it once when I get a new msg
                string msgText = GetMsgText(msg);
                if (msgText == "FALSE") { // If the msg list BROKE just do this function again
                    ReadMsgs();
                }
                string msgSender = GetMsgSender(msg);
                if(msgSender == "NONE") {
                    ReadMsgs();         // If the msg list BROKE just do this function again
                }
                Console.WriteLine("{0}: {1}", msgSender, msgText);
                UserComand.IfComandExecute(msgText, msgSender, wap);
                AdminComand.IfComandExecute(msgText, msgSender, wap);
            }
            // If it is a comand execute it acordingly; Comands class should do the rest
            
        }
        //sender = msg[i].GetAttribute("innerHTML"); } catch { return;
        static void Main(string[] args) {
                Thread consoleComands = new Thread(CheckConsoleComands);
                consoleComands.Start();
            while (true) {
                // Check if there is any new msg
                if(ClickOnNewMessages()) {
                    // If there is a new msg, click on the chat and read the msg
                    ReadMsgs();
                    // Make sure you get to read all the msgs in that group after clicking on it
                }
                System.Threading.Thread.Sleep(5000);
                // If there are no new mesages do nothing
                //Console.WriteLine("No new mesages");
            }

        }


    }
}


/*
 * Timer t = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds); // set the time (5 min in this case)
            t.AutoReset = true;
            t.Elapsed += new System.Timers.ElapsedEventHandler(your_method);
            t.Start();
            */





       





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

    static void AdminComand(ref Browsing browse, string comand)
    {
        if (comand.Contains(COMAND.targetcmd))
        {
            //Remove the comand name before looking for contacts/groups/target
            string target = comand.Remove(comand.IndexOf(COMAND.targetcmd), COMAND.targetcmd.Length + 1);//+1 Because there is a space in front
            Console.WriteLine(target);
            browse.accesTarget(target);
        }
        else if (comand.Contains(COMAND.sendmsgtocmd)) // Ex: !sendmsgto ana are mere -Nimic -5
        {
            bool skipWhile = false;

            string buffer = comand.Remove(comand.IndexOf(COMAND.sendmsgtocmd), COMAND.sendmsgtocmd.Length + 1);//+1 Because there is a space in front
            Console.WriteLine();
            int space1 = buffer.IndexOf('-');                 // Find first -
            int space2 = buffer.IndexOf('-', space1 + 1);       // Find second -, using space1+1 as the starting position
            if (space2 == -1)       // The function return -1 if substring not found
            {
                space2 = buffer.IndexOf(' ', space1 + 1);
                skipWhile = true;
            }
            if (space2 == -1)
            {
                space2 = buffer.Length - space1;
                skipWhile = true;
            }
            string msg = buffer.Substring(0, space1);           // space1 in cazul asta este marimea substringului
            string target = buffer.Substring(space1 + 1, space2 - space1 - 1);

            space2++;
            int times = 1;
            if (skipWhile == false)
            {
                while (buffer[space2] == ' ' || buffer[space2] > '9' || buffer[space2] < '1') { space2++; }
                times = buffer[space2] - 48;
            }
            Console.WriteLine(space2);
            //Console.WriteLine(buffer[space2]);
            Console.WriteLine(times);
            browse.sendMsgTo(msg, target, times);
        }
    }*/

