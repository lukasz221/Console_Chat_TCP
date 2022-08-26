using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ConsoleConsoleChat
{
    internal class ClientClass
    {
        IPAddress iPAddress;
        int iPPort = 2000;
        TcpClient mClient;
        string ip = "127.0.0.1";

        public ClientClass()
        {
            iPAddress = null;
            iPPort = 2000;
            mClient = null;
        }

        public IPAddress ServerIPAddress
        {
            get { return iPAddress; }
        }

        public int ServerPort
        {
            get { return iPPort; }
        }

        public bool SetServerIPAddress(string ip)
        {
            IPAddress ipaddr = null;

            if (!IPAddress.TryParse(ip, out ipaddr))
            {
                Console.WriteLine("Invalid server IP.");
                return false;
            }
            iPAddress = ipaddr;

            return true;
        }

        public async Task ConnectToServer()
        {
            SetServerIPAddress(ip);
            if (mClient == null)
            {
                mClient = new TcpClient();  
            }

            try
            {
                await mClient.ConnectAsync(iPAddress, iPPort);
                Console.WriteLine($"Connected to server IP: {iPAddress} Port: {iPPort}");
                ReadData(mClient);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public async Task ReadData(TcpClient mClient)
        {
            try
            {
                StreamReader streamReader = new StreamReader(mClient.GetStream());
                char[] buffer = new char[1024];
                int readBytesCount = 0;

                while (true)
                {
                    readBytesCount = await streamReader.ReadAsync(buffer, 0, buffer.Length);

                    if (readBytesCount <= 0)
                    {
                        Console.WriteLine("Disconnected.");
                        mClient.Close();
                        break;
                    }
                    Console.WriteLine($"Received bytes: {readBytesCount} Message: {new string(buffer)}");
                    Array.Clear(buffer, 0, readBytesCount);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public async Task SendToServer(string mgs)
        {
            if (string.IsNullOrEmpty(mgs))
            {
                Console.WriteLine("Empty string");
                return;
            }
            if (mClient != null)
            {
                StreamWriter streamWriter = new StreamWriter(mClient.GetStream());
                streamWriter.AutoFlush = true;

                await streamWriter.WriteLineAsync(mgs);
                Console.WriteLine("Data send");
            }
        }
    }
}
