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
    internal class ServerClass
    {
        IPAddress iPAddress;
        int port = 2000;
        TcpListener mTcpListener;
        List<TcpClient> mClients;

        public ServerClass()
        {
            mClients = new List<TcpClient>();
        }

        public bool KeepRunning { get; set; }
        public async Task StartListening()
        {
            iPAddress = IPAddress.Any;
            if (iPAddress == null)
            {
                iPAddress = IPAddress.Any;
            }

            if (port <= 0)
            {
                port = 2000;
            }

            Console.WriteLine($"IP Address: {iPAddress.ToString()} Port: {port.ToString()}");

            mTcpListener = new TcpListener(iPAddress, port);

            try
            {
                mTcpListener.Start();
                KeepRunning = true;
                Console.WriteLine($"Server Started");
                while (KeepRunning)
                {
                    var returnByAccept = await mTcpListener.AcceptTcpClientAsync();
                    Console.WriteLine($"Client connected: {returnByAccept.Client.RemoteEndPoint} Count: {mClients.Count+1}");

                    ClientCare(returnByAccept);

                    mClients.Add(returnByAccept);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public async Task SendToAll(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            try
            {
                byte[] buffMgs = Encoding.UTF8.GetBytes(message);
                foreach(TcpClient c in mClients)
                {
                    c.GetStream().WriteAsync(buffMgs, 0, buffMgs.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public async Task ClientCare(TcpClient tcpClient)
        {
            try
            {
                NetworkStream networkStream = tcpClient.GetStream();
                StreamReader streamReader = new StreamReader(networkStream);

                char[] buff = new char[1024];

                Console.WriteLine("Ready to Read");

                while (KeepRunning)
                {
                    int read = await streamReader.ReadAsync(buff, 0, buff.Length);

                    Console.WriteLine($"Returned {read}");

                    if (read == 0)
                    {
                        RemoveClient(tcpClient);
                        Console.WriteLine($"Client disconnected {tcpClient.ToString()}");
                        break;
                    }
                    string reveivedText = new string(buff, 0, buff.Length);
                    SendToAll(reveivedText);
                    Console.WriteLine($"Received {reveivedText}");
                    Array.Clear(buff, 0, buff.Length);

                }
            }
            catch (Exception)
            {
                RemoveClient(tcpClient);
                throw;
            }
        }

        private void RemoveClient(TcpClient tcpClient)
        {
            if (mClients.Contains(tcpClient))
            {
                mClients.Remove(tcpClient);
                Console.WriteLine($"Client removed, count: {mClients.Count}");
            }
        }
    }
}
