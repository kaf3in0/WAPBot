﻿using System;
using System.Collections.Generic;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;


namespace WhatsApp {
    namespace WebElements {


        public enum EMsgType { TEXT, REPLY, IMAGE, VIDEO, SOUND, NOTFOUND };
        public class Mesage {
            public Mesage(EMsgType msgType, string msginfo, string sender, string msg, string repliedTo) {
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
        public static class Act {
            public static void hover(this IWebElement webElement, IWebDriver driver) {
                new Actions(driver).MoveToElement(webElement).Perform();
            }
        }


        public class Wap {
            IWebDriver driver;
            // Constructor
            public Wap() {
                driver = new ChromeDriver("C:\\Users\\Edward\\Documents\\Desktop\\Whatsapp BOT\\WebDriver");
                driver.Url = "https://web.whatsapp.com/";
            }
            public IWebDriver GetDriver() {
                return driver;  // This cant fail
            }
            public IWebElement GetUnreadMesages() {
                try {
                    IList<IWebElement> buffer = driver.FindElements(By.XPath("//span[@class='OUeyt']"));
                    Act.hover(buffer[buffer.Count - 1], driver);
                    // After hovering overing it, the element moves, so update that
                    IList<IWebElement> privateMsg = driver.FindElements(By.XPath("//span[@class='OUeyt']"));
                    return privateMsg[privateMsg.Count - 1];
                } catch {
                    Console.WriteLine("Private mesage not found");
                    return null;
                }
            }

            public string GetCurrentTargetName() {//  //*[@id = 'main']//span[contains(@title,'')]
                try {
                    string targetname = driver.FindElement(By.XPath("//*[@id = 'main']//div[contains(@class,'1WBXd')]//span[@title]")).Text;
                    return targetname;
                } catch {
                    Console.WriteLine("Current target name not found");
                    return null;
                }
            }
             public IWebElement GetChatBar() {
                try {
                    IWebElement chat = driver.FindElement(By.XPath("//div[@class='_2S1VP copyable-text selectable-text']"));
                    return chat;
                } catch {
                    Console.WriteLine("Chat bar not found");
                    return null;
                }

            }
            public bool AccesTarget(string target) {
                try {
                    IWebElement searchTarget = driver.FindElement(By.XPath("//label[@class='_2MSJr']"));
                    searchTarget.SendKeys(target + Keys.Enter);
                    return true;
                } catch {
                    Console.WriteLine("Search target bar not found");
                    return false;
                }
            }
            public bool SendMsgTo(string msg, string target, int count = 1) {
                AccesTarget(target);
                try {
                    IWebElement chat = GetChatBar();
                    for (int index = 0; index < count; index++) {
                        // Format text so it can allow new lines in whatsapp
                        if (msg.Contains("\n")) {
                            for (int i = 0; i < msg.Length; i++) {
                                if (msg[i] == '\n')
                                    chat.SendKeys(Keys.Shift + Keys.Enter);
                                else
                                    chat.SendKeys(msg[i].ToString());
                            }
                            chat.SendKeys(Keys.Enter);
                        } else chat.SendKeys(msg + Keys.Enter);
                        System.Threading.Thread.Sleep(600);
                    }
                    return true;
                } catch {
                    Console.WriteLine("Failed to send msg");
                    return false;
                }
            }

            // Target must be accesed before using this
            public IList<IWebElement> GetMsgs() {
                try {
                    return driver.FindElements(By.XPath("//*[@id='main']//div[contains(@class,'message')]"));
                } catch {
                    Console.WriteLine("Mesages not found");
                    return null;
                }
            }
            // Usless for now
            private EMsgType GetMsgType(IWebElement msgextended) {

                try {
                    IWebElement soundmsg = msgextended.FindElement(By.XPath(".//input[contains(@type,'range')]"));
                    return EMsgType.SOUND;
                } catch { /* Just keep checking */ }
                // Verifica daca poti primi elementul specific al TIPULUI de mesaj
                // ex: UN mesaj de tip imagine va avea elementul specific pt imagine, dc nu atunci nu este imagine
                try {
                    IWebElement imagemsg = msgextended.FindElement(By.XPath(".//img"));
                    return EMsgType.IMAGE;              //TODO: Also dowload the image with this path
                } catch { /* Just keep checking */ }

                ////*[@id='main']//div[contains(@class,'message')]//div[contains(@class,'Y9G3K')]
                try {
                    IWebElement videomsg = msgextended.FindElement(By.XPath(".//div[contains(@class,'video')]"));
                    return EMsgType.VIDEO;
                } catch { /* Just keep checking */ }

                try {
                    IWebElement replymsg = msgextended.FindElement(By.XPath(".//span[contains(@class,'quoted')]"));
                    return EMsgType.REPLY;
                } catch { /* Just keep checking */ }
                return EMsgType.TEXT;
            }

            // TODO: Change the star method, it doesn't work, use data base instead
            public void StarMsg(IWebElement element) {
                try {
                    Act.hover(element, driver);
                } catch { Console.Write("Hover failed"); return; }    // If anything fails, most likely means that a broke the xpath, so stop wasting time
                try {
                    element.FindElement(By.XPath(".//div[@data-js-context-icon='true']")).Click(); // Click dropdown
                    driver.FindElement(By.XPath(".//div[@class='_3lSL5 _2dGjP _1vu-E'][contains(text(),'Star message')]")).Click();    //Click starmsg
                } catch {
                    Console.WriteLine("Failed to star msg");
                }
            }
            public Point GetElementPosition(IWebElement element) {
                Point position = element.Location;
                Console.WriteLine("X: {0}, Y: {1}",position.X, position.Y);
                return position;
            }
            public string GetMsgText(IWebElement msg) {
                try {
                    string m = msg.FindElement(By.XPath(".//span[contains(@class,'selectable-text')]")).Text;
                    return m;
                } catch { return null; }
            }

            public string GetMsgSender(IWebElement msg) {
                try {
                    string buffer = msg.GetAttribute("innerHTML");
                    string sender = FormatSender(buffer);
                    return sender;
                } catch { return null; }
            }

            // Ugly shit
            private static string ReverseString(string str) {
                char[] arr = str.ToCharArray();
                Array.Reverse(arr);
                return new string(arr);
            }
            public string FormatSender(string sender) {
                string newSender = "", buffer = "";
                int end = sender.IndexOf(": \"><div class") - 1;
                if (end > 0) {
                    while (sender[end] != ' ' || sender[end - 1] != ']') {
                        buffer += sender[end];
                        end--;
                    }
                    newSender = ReverseString(buffer);
                }
                return newSender;
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
