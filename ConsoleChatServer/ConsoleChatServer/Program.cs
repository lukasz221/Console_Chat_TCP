using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChatServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServerClass server = new ServerClass();
            server.StartListening();
            while (true)
            {
                string mgs = Console.ReadLine();
                server.SendToAll(mgs);
            }
        }
    }
}
