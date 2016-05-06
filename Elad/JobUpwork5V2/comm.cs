using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ClientServerApp
{

    public class Server
    {
        public static IPAddress ExternalIPAddress;
        public static int ExternalTimeout;
        public static int ExternalPort;
        private int commandcounter = 0;
        private readonly TcpListener tcpListener;
        private int connectedClients;
        public string frequenza_TX_attivo;
        public Client externalClient;
        public int Id { get; set; }
        public static int IdSer;
        public static int TX_attivo;

        public static void SetExternalServerData(IPAddress ipAddress, int port, int timeOut)
        {
            ExternalIPAddress = ipAddress;
            ExternalTimeout = timeOut;
            ExternalPort = port;
        }
        

        private bool ConnectExternalServer(out string replyMessage)
        {

            try
            {
                externalClient = null;
                externalClient = new Client(ExternalIPAddress, ExternalPort, ExternalTimeout);
                externalClient.Connect();
                //IdSer= IdSer+1;
                replyMessage = $"external server connected! Id  {Id}";
                // replyMessage = externalClient.SendMessage(msg);
                //      externalClient.Dispose();
                return true;
            }
            catch (SocketException)
            {

                DateTime dt = DateTime.Now;
                string dtStr = dt.ToString();
                replyMessage = $"{dtStr} Server {Id}: failed connection to the external server!";
            }
            catch (ArgumentException e)
            {
                replyMessage = e.Message;
                return false;
            }
            return false;

        }


        private bool SendMessageToExternalServer(string msg, out string replyMessage)
        {
            try
            {
                bool ok = externalClient.SendMessage(msg, out replyMessage);
                if (ok == false)
                {
                    ConnectExternalServer(out replyMessage);
                    SendMessageToExternalServer(msg, out replyMessage);
                }
                return true;
            }
            catch (SocketException)
            {

                DateTime dt = DateTime.Now;
                string dtStr = dt.ToString();
                replyMessage = $"{dtStr} Server{Id}: failed connection to the external server!";
            }
            catch (ArgumentException e)
            {
                replyMessage = e.Message;
                return false;
            }
            return false;
        }
    }
}
      


     
