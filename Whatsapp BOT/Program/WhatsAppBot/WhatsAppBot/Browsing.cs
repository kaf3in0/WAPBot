using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
// TODO: Clasa asta ar trebui sa se ocupe de accesari, trimitere de mesaje, INTERACTIUNI, NAVIGATIE
namespace WhatsAppBot
{
    
    public enum EMsgType { TEXT, REPLY, IMAGE, VIDEO, SOUND, NOTFOUND };
    public class Mesage
    {
        public Mesage(EMsgType msgType, string msginfo, string sender, string msg, string repliedTo)
        {
            this.msginfo = msginfo;
            this.sender = sender;
            this.msg = msg;
            this.repliedTo = repliedTo;
            this.msgType = msgType;
        }
        public string sender { get; private set; }
        public string msg { get; private set; }
        public string msginfo { get; private set; }
        public string repliedTo { get; private set; }
        public string target { get; private set; }
        public EMsgType msgType;
    }

    public static class COMAND
    {
        public static string targetcmd = "!target";
        public static string sendmsgtocmd = "!sendmsgto";
        public static string helpcmd = "!help";
    }

    public static class Act
    {
        public static void hover(this IWebElement webElement, IWebDriver driver)
        {
            new Actions(driver).MoveToElement(webElement).Perform();
        }
    }
    public class Browsing
    {
        private void Timer(int miliseconds)
        {
            System.Threading.Thread.Sleep(miliseconds);
        }
        IWebDriver driver;
        public string GetCurrentTargetName()
        {//  //*[@id = 'main']//span[contains(@title,'')]
            string targetname = driver.FindElement(By.XPath("//*[@id = 'main']//div[contains(@class,'1WBXd')]//span[@title]")).Text;
            return targetname;
        }
        // Constructor
        public Browsing()
        {
            driver = new ChromeDriver("C:\\Users\\Edward\\Documents\\Desktop\\VS COD");
            driver.Url = "https://web.whatsapp.com/";
        }
        public void StarMsg(IWebElement element)
        {
            try
            {
                Act.hover(element, driver);
            }
            catch { Console.Write("Hover failed"); return; }    // If anything fails, most likely means that a broke the xpath, so stop wasting time
            try
            {
                element.FindElement(By.XPath(".//div[@data-js-context-icon='true']")).Click(); // Click dropdown
                driver.FindElement(By.XPath(".//div[@class='_3lSL5 _2dGjP _1vu-E'][contains(text(),'Star message')]")).Click();    //Click starmsg
            }
            catch
            {
                Console.WriteLine("Failed to star msg");
            }

        }
        public void accesTarget(string target)
        {
            IWebElement searchTarget = driver.FindElement(By.XPath("//label[@class='_2MSJr']"));
            searchTarget.SendKeys(target + Keys.Enter);
        }
        public IWebElement getChat()
        {
            IWebElement chat = driver.FindElement(By.XPath("//div[@class='_2S1VP copyable-text selectable-text']"));
            return chat;
        }
        public void sendMsgTo(string msg, string target, int count = 1)
        {
            accesTarget(target);
            IWebElement chat = getChat();
            for (int i = 0; i < count; i++) {
                chat.SendKeys(msg + Keys.Enter);
                System.Threading.Thread.Sleep(600);
            }
        }
        private EMsgType getMsgType(IWebElement msgextended)
        {

            try {
                IWebElement soundmsg = msgextended.FindElement(By.XPath(".//input[contains(@type,'range')]"));
                return EMsgType.SOUND;
            }
            catch { /* Just keep checking */ }
            // Verifica daca poti primi elementul specific al TIPULUI de mesaj
            // ex: UN mesaj de tip imagine va avea elementul specific pt imagine, dc nu atunci nu este imagine
            try {
                IWebElement imagemsg = msgextended.FindElement(By.XPath(".//img"));
                return EMsgType.IMAGE;              //TODO: Also dowload the image with this path
            }
            catch { /* Just keep checking */ }

            ////*[@id='main']//div[contains(@class,'message')]//div[contains(@class,'Y9G3K')]
            try
            {
                IWebElement videomsg = msgextended.FindElement(By.XPath(".//div[contains(@class,'video')]"));
                return EMsgType.VIDEO;
            }
            catch { /* Just keep checking */ }

            try {
                IWebElement replymsg = msgextended.FindElement(By.XPath(".//span[contains(@class,'quoted')]"));
                return EMsgType.REPLY;
            }
            catch { /* Just keep checking */ }
            return EMsgType.TEXT;
        }

        // Target must be accesed before using this
        public IList<IWebElement> GetMsgs()
        {
            return driver.FindElements(By.XPath("//*[@id='main']//div[contains(@class,'message')]"));
        }
        private static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
        private string formatSender(string sender)
        {
            string newSender = "";
            string buffer = "";
            int end = sender.IndexOf(": \"><div class") - 1;
            if (end > 0)
            {
                while (sender[end] != ' ' || sender[end - 1] != ']')
                {
                    buffer += sender[end];
                    end--;
                }
                newSender = ReverseString(buffer);
            }
            return newSender;
        }
        private void IsMsgComand(string msg, string s, string savtarget)
        {
            string sender = formatSender(s);    // The sender is the xpath, cause the .Text function does not give all the detail
            //TODO: use a foreach to iterate through all comands instead
            if(msg.Contains(COMAND.helpcmd))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Help Comand detected");
                Console.WriteLine(sender);
                Console.ResetColor();
                sendMsgTo("Nope, just testing it with text. :(", sender);
                accesTarget(savtarget); // Acces the target we left from
            }
        }
        // Target must be accesed before using this
        public void ReadMsgs(string target)
        {

            IList<IWebElement> msg = GetMsgs();             
            for (int i = msg.Count - 5; i < msg.Count; i++) // Check for the last 5 msgs
            {
                string strmsg = "";
                try { strmsg = msg[i].Text; }
                catch { return; }

                string sender = "";
                try { sender = msg[i].GetAttribute("innerHTML"); }
                catch { return; }
                // Check if the message is already stared, if it is then just ignore it
                try { IWebElement star = msg[i].FindElement(By.XPath(".//span[contains(@data-icon,'star')]")); }
                catch   // if the mesage is not stared already, star it
                {
                    try { StarMsg(msg[i]); }
                    catch { return; } // If this hapens it means that the xpath was broken by a newly sent message, so just stop and try again with a refreshed xpath
                    IsMsgComand(strmsg, sender, target);
                }              
            }

            
        }
    }
}








/*
 * public static void Main()
{
    System.Timers.Timer aTimer = new System.Timers.Timer();
    aTimer.Elapsed+=new ElapsedEventHandler(OnTimedEvent);
    aTimer.Interval=5000;
    aTimer.Enabled=true;

    Console.WriteLine("Press \'q\' to quit the sample.");
    while(Console.Read()!='q');
}

 // Specify what you want to happen when the Elapsed event is raised.
 private static void OnTimedEvent(object source, ElapsedEventArgs e)
 {
     Console.WriteLine("Hello World!");
 }
 */
