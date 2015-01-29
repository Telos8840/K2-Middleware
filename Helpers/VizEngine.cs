using System;
using System.Threading;
using System.Net.Sockets;
using System.ComponentModel;
using Elysium.Notifications;
using RCS.Interop.Viz.Engine;

namespace RCS.K2.NFLN.Helpers
{
    public class VizEngine : INotifyPropertyChanged
    {
        #region private variables

        private string _strStatus;
        private bool? _connectedNull;
        private Renderer _engine;
        private string _scene;
        private bool _cons;
        private const string _Invoke = @"-1 RENDERER*SCRIPT INVOKE ";

        #endregion

        #region public properties

        public event PropertyChangedEventHandler PropertyChanged;
        public bool? ConnectedNull { get { return _connectedNull; } set { _connectedNull = value; OnPropertyChange("ConnectedNull"); } }
        public String Status { get { return _strStatus; } set { _strStatus = value; OnPropertyChange("Status"); } }
        public bool Connected { get { if (_engine == null) return false; return _engine.Connected; } }
        public bool Reconnect { get { return _engine.AutoReconnect; } set { _engine.AutoReconnect = value; } }
        public bool Cons { get { return _cons; } set { _cons = value; OnPropertyChange("Cons"); } }
        public int Port
        {
            get { return _engine.Port; }
            set
            {
                if (Connected)
                    DisconnectFromEngine();
                _engine.Port = (value > 0) ? value : _engine.Port;

            }
        }
        public string Scene { get { return _scene; } set { _scene = (!String.IsNullOrEmpty(value)) ? value : _scene; } }
        public string IP
        {
            get { return _engine.HostnameOrIP; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    if (value.ToLower().Equals("localhost")) value = "127.0.0.1";
                    if (Connected)
                        DisconnectFromEngine();

                    _engine.HostnameOrIP = value;
                }

            }
        }

        public Renderer Engine { get { return _engine; } set { if (_engine == value) return; _engine = value; OnPropertyChange("Engine"); } }

        #endregion

        #region public members
        //Constructor
        public VizEngine() { ConnectedNull = false; }

        public VizEngine(string ip, int port)
        {
            if (ip.ToLower().Equals("localhost"))
                ip = "127.0.0.1";
            _engine = new Renderer(ip, port);
            _engine.EngineStatus += GetEngineStatus;
            ConnectedNull = false;
            _engine.ReceiveTimeout = 500;
        }

        public VizEngine(string ip, int port, bool autoReconnect)
        {
            if (ip.ToLower().Equals("localhost"))
                ip = "127.0.0.1";
            _engine = new Renderer(ip, port);
            _engine.AutoReconnect = autoReconnect;
            _engine.EngineStatus += GetEngineStatus;
            ConnectedNull = false;
            _engine.ReceiveTimeout = 500;
        }

        //Event to be thrown to an outside class
        public delegate void EngineStatusDelegate(string status);
        public event EngineStatusDelegate EngineStatus;

        //Connect to Engine
        public string ConnectToEngine(string ip, int port, bool reconnect)
        {
            String errorMessage = String.Empty;
            Console.WriteLine("CONNECT TO ENGINE: " + ip + ":" + port + " reconnect?" + reconnect);
            if (String.Compare(ip.ToLower(), "localhost") == 0)
                ip = "127.0.0.1";

            //_engine = new Renderer(ip, port);
            //_engine.EngineStatus += GetEngineStatus;           

            if (_engine.HostnameOrIP != IP || _engine.Port != Port)
            {
                _engine = new Renderer(IP, Port);
                _engine.EngineStatus += GetEngineStatus;
            }

            //Three attemps to connect to engine
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    if (_engine != null && _engine.Connected)
                        DisconnectFromEngine();

                    _engine.Connect();
                    _engine.AutoReconnect = reconnect;

                    //Give the object time to connect
                    Thread.Sleep(500);

                    //Could not connect
                    if (_engine.Connected == false)
                        errorMessage = "Application was unable to connect to engine";
                }
                catch (SocketException e)
                {
                    Console.WriteLine("ERROR: " + e.ToString());
                    errorMessage = e.ToString();
                    if (i < 3)
                        continue;
                }

                if (i == 2 && !Connected)
                {
                    //NotificationManager.Push("Viz Connection", "Cannot establish a connection to Viz");
                }
            }

            return errorMessage;
        }

        //Connect to Engine No Param
        public string ConnectToEngine()
        {
            String errorMessage = String.Empty;
            Console.WriteLine("CONNECT TO ENGINE: " + Engine.HostnameOrIP + ":" + Engine.Port);
            //  if ( String.Compare( ip.ToLower(), "localhost" ) == 0 )
            //	  ip = "127.0.0.1";

            if (_engine.HostnameOrIP != IP || _engine.Port != Port)
            {
                _engine = new Renderer(IP, Port);
                _engine.EngineStatus += GetEngineStatus;
            }

            //Three attemps to connect to engine
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    if (_engine != null && _engine.Connected)
                        DisconnectFromEngine();

                    _engine.Connect();
                    _engine.AutoReconnect = true;

                    //Give the object time to connect
                    Thread.Sleep(500);

                    //Could not connect
                    if (_engine.Connected == false)
                        errorMessage = "Application was unable to connect to engine";
                }
                catch (SocketException e)
                {
                    Console.WriteLine("ERROR: " + e.ToString());
                    errorMessage = e.ToString();
                    if (i < 3)
                        continue;
                }

                if (i == 2 && !Connected)
                {
                    //NotificationManager.Push("Viz Connection", "Cannot establish a connection to Viz");
                }
            }
            return errorMessage;
        }

        //Disconnect from Engine
        public bool DisconnectFromEngine()
        {
            try
            {
                if (_engine.Connected)
                {
                    _engine.AutoReconnect = false;
                    _engine.Disconnect();
                }

                ConnectedNull = false;

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.ToString());
            }

            return false;
        }

        //Send Viz Command and get response
        public string SendToEngineResponse(string command)
        {
            try
            {
                string response = _engine.SendToEngineResponse(command);
                //Viz Request failed
                if (String.Compare(response.Substring(0, 2), "-1") == 0)
                    return String.Empty;
                //Ignore first two characters from string
                return response.Substring(2, response.Length - 2);
            }
            catch (SocketException e)
            {
                Console.WriteLine("ERROR: " + e.ToString());
                if (EngineStatus != null)
                    EngineStatus("Disconnected");
            }

            return String.Empty;
        }

        //Send Viz Command 
        public void SendToEngine(string command)
        {
            try
            {
                if (this.Connected)
                {
                    _engine.SendToEngine(command);
                    //_engine.SendToEngine( ReplaceBadVizChars(command ));
                    Console.WriteLine(command);
                    Console.WriteLine("");
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("ERROR: " + e.ToString());
                if (EngineStatus != null)
                    EngineStatus("Disconnected");
            }
        }

        //Invoke a script in the engine
        public void Invoke(string script)
        {
            if (this.Connected)
                this.SendToEngine(_Invoke + script);
        }


        //Event Handler for Connection Status
        private void GetEngineStatus(Renderer sender, TCPSocket.Status currentStatus)
        {
            Status = currentStatus.ToString();
            if (EngineStatus != null)
            {
                EngineStatus(currentStatus.ToString());

                if (currentStatus.ToString() == "Connected")
                    ConnectedNull = true;
                else if (currentStatus.ToString() == "Connecting" || (Reconnect == true && currentStatus.ToString() == "UnableToEstablishConnection"))
                    ConnectedNull = null;
                else
                    ConnectedNull = false;
            }

            Console.WriteLine("ENGINE STATUS: " + currentStatus.ToString());
        }

        #endregion

        #region private methods

        /// <summary>
        /// Searches Viz commands for characters that do not exist in the font, which would
        /// result in erroneous characters being displayed
        /// </summary>
        /// <param name="inString">The command to be investigated</param>
        /// <returns>The command with missing characters replaced with valid characters/strings</returns>
        private static string ReplaceBadVizChars(string inString)
        {
            return inString.Replace("…", "...").Replace("’", "'").Replace("–", "-").Replace("‘", "'").Replace("“", "\"").Replace("”", "\"");
        }

        private void OnPropertyChange(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}
