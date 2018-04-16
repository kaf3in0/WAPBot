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

    public static class Act
    {
        public static void hover(this IWebElement webElement, IWebDriver driver)
        {
            new Actions(driver).MoveToElement(webElement).Perform();
        }
    }
    public class Browsing
    {
        public void Timer(int miliseconds)
        {
            System.Threading.Thread.Sleep(miliseconds);
        }
        IWebDriver driver;
        public string getTargetName()
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
            Act.hover(element, driver);
            try
            {
                element.FindElement(By.XPath(".//div[@data-js-context-icon='true']")).Click(); // Click dropdown
                Timer(300);
                driver.FindElement(By.XPath(".//div[@class='_3lSL5 _2dGjP _1vu-E'][contains(text(),'Star message')]")).Click();    //Click starmsg
            }
            catch
            {
                Console.WriteLine("Failed to star msg");
            }
            
        }
        //private void ReadMsg(IWebElement element)
        //{
        //}
        // Gets you to the group chat or friend conversation specified
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
            for (int i = 0; i < count; i++){
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


        public void GetLastMsg(string target)
        {

            IList<IWebElement> msg = driver.FindElements(By.XPath("//*[@id='main']//div[contains(@class,'message')]"));
            IWebElement lastmsg = msg[msg.Count - 1];
            try
            {
                IWebElement star = lastmsg.FindElement(By.XPath(".//span[contains(@data-icon,'star')]"));
            }
            catch
            {
                try { Console.WriteLine(lastmsg.Text); }
                catch { }
                StarMsg(lastmsg);
            }
            
            
            
        }
    }
}