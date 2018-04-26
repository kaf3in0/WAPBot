using System;
using OpenQA.Selenium;
using System.Collections.Generic;
using WhatsApp.WebElements;
// rr comand : folosesti comanda !quest sa iti trimita toate questurile; fiecare quest cu mesajul lui
    // dai reply cu rr la questul pe care doresti sa il termini
// TODO: Use a dictionary for implementing comands
namespace BOT {
    namespace Comands {
        public static class UserComand {         
            static Dictionary<string, Action<string[], string, Wap>> comand = new Dictionary<string, Action<string[], string, Wap>>() {
                { "!help"           ,   help},              // Trimite tote comenzile ACCESIBILE
                { "!quest"          ,   quest},             // Trimite iformatiile despre questurile active
                { "!rr"             ,   requestReward},     // Cere rewardurile pentru un quest
                { "!requestreward"  ,   requestReward}
            };  
            public static void requestReward(string[] args,string user, Wap wap) {
                Console.WriteLine("TODO Logic for the rr comand");
            }
            public static void quest(string[] args,string user, Wap wap) {

            }
            public static void help(string[] args, string user, Wap wap) {
                var keyList = new List<string>(comand.Keys);
                string msgBuilder = "";
                foreach(var key in keyList) {
                    msgBuilder += key + "\n";
                }
                wap.SendMsgTo(msgBuilder, "Nimic");
            }

            public static void IfComandExecute(string input, string user, Wap wap) {

                Action<string[], string, Wap> function_to_execute = null;
                // Split 2 times, 1 time for the comand, 1 time for the user and the rest is the msg
                string[] inputMain = input.Split(new string[] { " " }, 2, StringSplitOptions.None); // Overload the function so it only splits a number of times
                if (comand.ContainsKey(inputMain[0])) { // The first element should be the comand
                    comand.TryGetValue(inputMain[0], out function_to_execute);
                    function_to_execute(inputMain, user, wap);
                }
            }
        }


        public static class AdminComand {
            // Comands dictionary
            // NOTE: Daca vrei o fucntie care returneaza ceva, foloseste: Func<param1, param2, returnType>; Func<returnType>
            static Dictionary<string, Action<string[], string, Wap>> comand = new Dictionary<string, Action<string[], string, Wap>>() {
                { "!target"     ,   target },           // Acceseaza chatul persoanei
                { "!helpadmin"  ,   helpadmin},         // Trimite toate comenzile de ADMIN
                { "!sendmsgto"  ,   sendMsgTo},         // Trimite mesaj persoanei, accesand chatul
                { "!addgroup"   ,   addGroup}
            };  

            public static void addGroup(string[] args, string user, Wap wap) {
                Console.WriteLine("TODO Logic for the addgroup comand");
            }
            public static void helpadmin(string[] args, string user, Wap wap) {
                var keyList = new List<string>(comand.Keys);   // Get all the keys in the dictionary
                foreach(var key in keyList) {
                    Console.WriteLine(key);
                }
            }
            public static void target(string[] args,string user, Wap wap) {
                try { wap.AccesTarget(args[1]); }
                catch { Console.WriteLine("Wrong comand"); }
                Console.WriteLine("TODO Logic for the target comand");
            }
            public static void sendMsgTo(string[] args,string user, Wap wap) {
                try { wap.SendMsgTo(args[2], args[1]); }
                catch { Console.WriteLine("Wrong comand"); } 
            }


            public static void IfComandExecute(string input, string user, Wap wap) {
                if (!HasAcces(user)) {
                    Console.WriteLine(user + " does not have acces");
                    return; // Check if the user demanding to use the admin comands has acces
                }
                string[] arguments = new string[7];
                Action<string[], string, Wap> function_to_execute = null;
                // Split 2 times, 1 time for the comand, 1 time for the user and the rest is the msg
                string[] inputMain = input.Split(new string[] { " " }, 3, StringSplitOptions.None); // Overload the function so it only splits a number of times
                if (comand.ContainsKey(inputMain[0])) { // The first element should be the comand
                    comand.TryGetValue(inputMain[0], out function_to_execute);
                    function_to_execute(inputMain, user, wap);
                }
            }

            public static bool HasAcces(string user) {
                //TODO: FROM database get user rank(admin, user, owner);
                string rank = "OWNER";  
                if(user == rank) {
                    return true;
                } else return false;
                // TODO: After implementing the data base
                // TODO: Functie care returneaza daca userul care a scris comanda are acces sau nu la comenzile de admin
            }
        }
    }
}
