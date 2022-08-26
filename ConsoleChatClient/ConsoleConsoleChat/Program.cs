using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ConsoleConsoleChat
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ClientClass clientClass = new ClientClass();

            clientClass.ConnectToServer();

            while (true)
            {
                string mgs = Console.ReadLine();
                clientClass.SendToServer(mgs);
            }
        }
    }
}
