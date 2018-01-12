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

        public void Process(NetIncomingMessage im)
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
                // Redirect to client
                case NetIncomingMessageType.ConnectionApproval:
                case NetIncomingMessageType.Data:
                    var task = new Task(delegate { Client.Process(im); });
                    task.Start();
                    break;
                // Connections and Disconnections
                case NetIncomingMessageType.StatusChanged:
                    NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();
                    switch (status)
                    {
                        case NetConnectionStatus.RespondedAwaitingApproval:
                            Server.Output.Enqueue("Incomming connection from " + im.SenderConnection.RemoteEndPoint.Address.ToString());
                            break;
                        case NetConnectionStatus.RespondedConnect:
                        case NetConnectionStatus.Connected:
                            //Do nothing for now
                            break;
                        case NetConnectionStatus.Disconnecting:

                            break;
                    }
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
            var newClient = new RemoteClient(this, connection);
            this.Clients.AddOrUpdate(
              connection,
              newClient,
              (key, oldValue) => newClient);



        }
    }
}
