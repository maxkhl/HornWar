using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horn_War_II.Server
{
    /// <summary>
    /// Manages the Connection 
    /// </summary>
    class ClientManager
    {
        /// <summary>
        /// Clients ordered by their connection
        /// </summary>
        public ConcurrentDictionary<NetConnection, RemoteClient> Clients { get; private set; }

        /// <summary>
        /// The Server that uses this clientmanager
        /// </summary>
        public Server Server { get; private set; }

        public ClientManager(Server Server)
        {
            Clients = new ConcurrentDictionary<NetConnection, RemoteClient>();
            this.Server = Server;
        }

        public void ProcessAsync(NetIncomingMessage im)
        {
            var task = new Task(delegate { WorkIM(im); });
            task.Start();
        }

        /// <summary>
        /// Works a single incoming message
        /// </summary>
        private void WorkIM(NetIncomingMessage im)
        {
            // Create client if doesnt exist already
            if (!Clients.ContainsKey(im.SenderConnection))
                CreateClient(im.SenderConnection);

            // Get the client of this IM
            var Client = Clients[im.SenderConnection];

            switch (im.MessageType)
            {
                case NetIncomingMessageType.DebugMessage:
                case NetIncomingMessageType.ErrorMessage:
                case NetIncomingMessageType.WarningMessage:
                case NetIncomingMessageType.VerboseDebugMessage:
                    Server.Output.Enqueue(im.ReadString());
                    break;
                default:
                    Server.Output.Enqueue("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes " + im.DeliveryMethod + "|" + im.SequenceChannel);
                    break;
            }

            Server.Recycle(im); //Cleanup IM
        }

        /// <summary>
        /// Creates a new client to a given connection that is handled by this manager
        /// </summary>
        private void CreateClient(NetConnection connection)
        {
            var newClient = new RemoteClient();
            this.Clients.AddOrUpdate(
              connection,
              newClient,
              (key, oldValue) => newClient);



        }
    }
}
