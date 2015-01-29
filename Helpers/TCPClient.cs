using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RCS.K2.NFLN.Helpers
{
    public class TcpMessageReceivedEventArgs
    {
        public TcpClient RemoteClient;
        public string Message;
    }

    public class TcpFeedbackReceivedEventArgs
    {
        public TcpClient RemoteClient;
        public string Message;
        public string Id;
    }

    public delegate void TcpMessageReceivedDlgt(object sender, TcpMessageReceivedEventArgs args);
    public delegate void TcpFeedbackReceivedDlgt(object sender, TcpFeedbackReceivedEventArgs args);

    public static class NetHelper
    {
        public static string GetLocalIP()
        {
            var ipAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            var index = 0;
            while (ipAddresses[index].ToString() == "127.0.0.1" || ipAddresses[index].ToString().Contains(":"))
            {
                index++;
            }
            return ipAddresses[index].ToString();
        }
    }


    public class TcpClients : IDisposable
    {

        private readonly TcpListener _tcpListener;
        private Thread _listenThread;
        private readonly ObservableCollection<TcpClient> _clientList;

        public bool KeepListening;

        public event TcpMessageReceivedDlgt TcpMessageReceived;
        public event TcpFeedbackReceivedDlgt TcpFeedbackReceived;

        public TcpClients(int listenPort)
        {
            _clientList = new ObservableCollection<TcpClient>();

            _tcpListener = new TcpListener(IPAddress.Any, listenPort);


        }

        public void Dispose()
        {
            _tcpListener.Stop();

            foreach (var client in _clientList)
            {
                client.Close();
            }
        }

        public ObservableCollection<TcpClient> Clients
        {
            get
            {
                return _clientList;
            }
        }

        public void StartListening()
        {
            _listenThread = new Thread(ListenForClients);
            _listenThread.Start();
        }

        private void ListenForClients()
        {
            _tcpListener.Start();

            while (true)
            {
                //blocks until a client has connected to the server
                TcpClient client;
                try
                {
                    client = _tcpListener.AcceptTcpClient();
                }
                catch (Exception)
                {
                    break;
                }

                //create a thread to handle communication
                //with connected client
                var clientThread = new Thread(HandleClientComm);
                clientThread.Start(client);
            }
        }


        private void HandleClientComm(object client)
        {
            var tcpClient = (TcpClient)client;

            _clientList.Add(tcpClient);

            NetworkStream clientStream = tcpClient.GetStream();

            var message = new byte[4096];

            while (true)
            {
                int bytesRead;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    break;
                }

                //message has successfully been received
                var encoder = new UTF8Encoding();
                string messageString = encoder.GetString(message, 0, bytesRead);

                foreach (string theMessage in messageString.Split('\0'))
                {

                    Console.WriteLine(theMessage);
                    if (TcpMessageReceived != null)
                    {
                        var args = new TcpMessageReceivedEventArgs
                        {
                            RemoteClient = tcpClient,
                            Message = theMessage,
                        };
                        TcpMessageReceived(this, args);

                    }
                }

            }
            tcpClient.Close();
        }
    }
}