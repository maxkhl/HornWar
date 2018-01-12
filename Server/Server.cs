using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace Horn_War_II.Server
{
    /// <summary>
    /// Used for hosting a gameserver
    /// </summary>
    class Server : Lidgren.Network.NetServer, IGameComponent, IDisposable
    {
        /// <summary>
        /// Game, this server is synchronizing
        /// </summary>
        public HornWarII Game { get; private set; }

        /// <summary>
        /// The clientmanager used by this server
        /// </summary>
        public ClientManager ClientManager { get; private set; }

        /// <summary>
        /// Servers tickrate
        /// </summary>
        public int Tickrate { get; private set; }

        /// <summary>
        /// Contains outgoing messages (debug/console shit)
        /// </summary>
        public System.Collections.Concurrent.ConcurrentQueue<string> Output { get; private set; }

        /// <summary>
        /// Main-thread for this server
        /// </summary>
        private System.Threading.Thread _Thread { get; set; }

        /// <summary>
        /// Timer for messuring the main
        /// </summary>
        private System.Diagnostics.Stopwatch _Stopwatch { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Port">Port, the server should listen to</param>
        /// <param name="Tickrate">Tickrate of the Server (this will set the Tickrate for this Game and all Clients)</param>
        /// <param name="UPnP">Enable UPnP port forwarding</param>
        public Server(HornWarII Game, int Port, int Tickrate, bool UPnP) : base(new NetPeerConfiguration("HornWar2")
        {
            Port = Port,
            EnableUPnP = UPnP, 
            NetworkThreadName = "HornWarLidgrenNetThread",
        })
        {
            this.Game = Game;
            this.Tickrate = Tickrate;
            this.ClientManager = new ClientManager(this);
            this.Output = new System.Collections.Concurrent.ConcurrentQueue<string>();
            this._Thread = new System.Threading.Thread(new System.Threading.ThreadStart(Work));
            this._Thread.IsBackground = true;
            this._Thread.Name = "HornWarServerThread";
            this._Stopwatch = new System.Diagnostics.Stopwatch();
        }

        /// <summary>
        /// Starts up the server and the network threads
        /// </summary>
        public virtual void Initialize()
        {
            this.Start();
            this._Thread.Start();
        }

        /// <summary>
        /// Work-function for the main thread
        /// </summary>
        private void Work()
        {
            while (!_Disposing)
            {
                _Stopwatch.Start();

                //Do work
                HandleIncommingMessages();


                _Stopwatch.Stop();

                //Calculate time and sleep accordingly
                var RemainingTime = 1000 / this.Tickrate - _Stopwatch.ElapsedMilliseconds;
                if (RemainingTime <= 0)
                    Output.Enqueue(String.Format("Cannot keep up with target tickrate. This tick took {0}ms too long.", RemainingTime));
                else
                    System.Threading.Thread.Sleep((int)RemainingTime);

                _Stopwatch.Reset();
            }
        }

        /// <summary>
        /// Handles incomming messages (what did you expect?)
        /// </summary>
        private void HandleIncommingMessages()
        {
            NetIncomingMessage im;
            while ((im = ReadMessage()) != null)
            {
                // External message? - ClientManagers business
                if(im.SenderConnection != null)
                    ClientManager.Process(im);

                // Internal message! Probably debugging or error stuff
                else
                    switch (im.MessageType)
                    {
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.ErrorMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.VerboseDebugMessage:
                            Output.Enqueue(im.ReadString());
                            break;
                        default:
                            Output.Enqueue("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes " + im.DeliveryMethod + "|" + im.SequenceChannel);
                            break;
                    }

                Recycle(im);
            }
        }

        private bool _Disposing { get; set; }
        public void Dispose()
        {
            this.Shutdown("Server being disposed. Bye.");
            _Disposing = true;
        }
    }
}
