using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientServerApp
{

    public class Client : IDisposable
    {
        private readonly TcpClient client;
        private readonly IPAddress ipAddress;
        private readonly int port;
        private bool _disposed;



        public Client(IPAddress ipAddress, int port = 3000, int timeout = 2000)
        {
            client = new TcpClient
            {
                ReceiveTimeout = timeout
            };
            this.ipAddress = ipAddress;
            this.port = port;
        }

        /// <summary>
        /// Connect to the server
        /// </summary>
        public bool Connect(out string replyMessage)
        {
            try
            {
                if (client.Connected == false)
                {
                    client.Connect(new IPEndPoint(ipAddress, port));
                }
                DateTime dt = DateTime.Now;
                string dtStr = dt.ToString();
                replyMessage = $"{dtStr} Client: Connect to the external server IP: {ipAddress} Port: {port}!";
                return true;
            }
            catch (SocketException)
            {

                DateTime dt = DateTime.Now;
                string dtStr = dt.ToString();
                replyMessage = $"{dtStr} Client: failed connection to the external server IP: {ipAddress} Port: {port}!";
                return false;
            }
            catch (ArgumentException e)
            {
                replyMessage = e.Message;
                return false;
            }
           
        }

        public bool SendMessage(string msg, out string message)
        {
            try
            {

                if (client.Connected == true)
                {
                    NetworkStream clientStream = client.GetStream();
                    var encoder = new ASCIIEncoding();
                    var buffer = encoder.GetBytes(msg);
                    try
                    {
                        clientStream.Write(buffer, 0, buffer.Length);
                    }
                    catch
                    {
                        throw new ArgumentException("The system can not write to network stream.");
                    }
                    if (clientStream.CanWrite)
                    {
                        clientStream.Flush();
                    }
                    else
                    {
                        throw new ArgumentException("The system can not write to network stream.");
                    }
                    // Buffer to store the response bytes.
                    var data = new byte[4096];
                    try
                    {
                        int bytes = clientStream.Read(data, 0, data.Length);
                        message = Encoding.ASCII.GetString(data, 0, bytes);
                    }
                    catch (IOException)
                    {
                        throw new ArgumentException("A message from external server isn't received.");
                    }


                    return true;
                }
                else
                {
                    message = "";
                    return false;


                }
            }
            catch (SocketException)
            {

                DateTime dt = DateTime.Now;
                string dtStr = dt.ToString();
                message = $"{dtStr} Client: failed connection to the external server!";
                return false;
            }
            catch (ArgumentException e)
            {
                message = e.Message;
                return false;
            }
            

        }

        private void Close()
        {
            if (client.Connected)
            {
                client.Close();
            }
            //Dispose(false);
            //client.Dispose();
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                Close();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }






    }
}
