using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


// TODO: Aici ar trebui sa se execute logica pentru rularea BOTului
namespace WhatsAppBot
{
    class Program
    {
        static string targetcmd = "!target";
        static string sendmsgtocmd = "!sendmsgto";
        static string helpcmd = "!help";
        
  
        static void Main(string[] args)
        {
            Browsing browse = new Browsing();
            while (true)
            {
                string input = Console.ReadLine();
                AdminComand(ref browse, input);
                browse.accesTarget(input);
                while(true)
                    browse.GetLastMsg(input);
            }
            
        }
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
            if (comand.Contains(targetcmd))
            {
                //Remove the comand name before looking for contacts/groups/target
                string target = comand.Remove(comand.IndexOf(targetcmd), targetcmd.Length + 1);//+1 Because there is a space in front
                Console.WriteLine(target);
                browse.accesTarget(target);
            }
            else if (comand.Contains(sendmsgtocmd)) // Ex: !sendmsgto ana are mere -Nimic -5
            {
                bool skipWhile = false;

                string buffer = comand.Remove(comand.IndexOf(sendmsgtocmd), sendmsgtocmd.Length + 1);//+1 Because there is a space in front
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
        }
    }


}
