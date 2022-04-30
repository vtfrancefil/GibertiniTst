sing System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GibertiniTst
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cst = new CancellationTokenSource();

            string ip = string.Empty;
            int port = 0;
            string endBytes = string.Empty;

            if (args.Length == 2)
            {
                ip = args[0];
                port = int.Parse(args[1]);
            }
            else
            {
                if (args.Length == 3)
                {
                    ip = args[0];
                    port = int.Parse(args[1]);
                    endBytes = args[2];
                    if (endBytes == "CR")
                    {
                        endBytes = "\r";
                    }
                    if (endBytes == "LF")
                    {
                        endBytes = "\n";
                    }
                    if(endBytes == "CRLF")
                    {
                        endBytes = "\r\n";
                    }
                }
                else
                {
                    Console.WriteLine("ERROR : Please check application start up paramaters, IP and port");
                    Console.Read();
                    return;
                }
            }

            TcpClient tcpClient = new TcpClient();

            try
            {
                tcpClient.Connect(ip, port);
                // Get the stream used to read the message sent by the server.
                NetworkStream networkStream = tcpClient.GetStream();
                // Set a 10 millisecond timeout for reading.
                networkStream.ReadTimeout = 10;
                // Read the server message into a byte buffer.
                byte[] bytes = new byte[1024];
                int byteread = 0;

            using (tcpClient)
            using (networkStream)
            {
                for (; ; )
                {
                    try
                    {
                        networkStream.Write(Encoding.ASCII.GetBytes("B" + endBytes));
                         byteread = networkStream.Read(bytes, 0, 20);
                        //Convert the server's message into a string and display it.
                        string data = Encoding.ASCII.GetString(bytes, 0, byteread);
                        Console.WriteLine("Server sent message: {0}", data);
                    }
                    catch { }

                }
            }


                // while (tcpClient.Connected && networkStream.CanWrite)
                // {
                //     try
                //     {
                //         networkStream.Write(Encoding.ASCII.GetBytes("B" + endBytes));
                //         byteread = networkStream.Read(bytes, 0, 20);
                //         string data = Encoding.ASCII.GetString(bytes, 0, byteread);
                //         Console.WriteLine("Server sent message: {0}", data);
                //     }
                //     catch (Exception) { }
                // }

                // networkStream.Close();
                // tcpClient.Close();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
            }

        }
    }
}
