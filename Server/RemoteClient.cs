using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Horn_War_II.Server
{
    /// <summary>
    /// Handles a single client on the server
    /// </summary>
    class RemoteClient
    {
        /// <summary>
        /// ClientManager that is handling this remoteClient
        /// </summary>
        public ClientManager ClientManager { get; private set; }

        /// <summary>
        /// Connection, this client is using
        /// </summary>
        public NetConnection Connection { get; private set; }

        public RemoteClient(ClientManager ClientManager, NetConnection Connection)
        {
            this.ClientManager = ClientManager;
            this.Connection = Connection;
        }

        /// <summary>
        /// Processes a incoming message for this client (thread-safe)
        /// </summary>
        public void Process(NetIncomingMessage im)
        {
            switch (im.MessageType)
            {
                case NetIncomingMessageType.ConnectionApproval:
                    ApproveOrDenyConnection(im);
                    break;
                case NetIncomingMessageType.Data:

                    break;
            }
        }

        /// <summary>
        /// Tries to figure out if a connection from a client should be approved or denied and does so
        /// </summary>
        public void ApproveOrDenyConnection(NetIncomingMessage im)
        {
            //im.SenderConnection.Deny("User with the same name already online");
            im.SenderConnection.Approve();
        }

        /// <summary>
        /// Disconnects the client and/or disposes it from the manager
        /// </summary>
        public void Disconnect()
        {
            //Disconnect if connected
            if (this.Connection.Status == NetConnectionStatus.Connected)
                this.Connection.Disconnect("Disconnected by server");

            //Try to remove from clientlist
            while(ClientManager.Clients.TryRemove(this.Connection, out var result) != true)
            {
                System.Threading.Thread.Sleep(1);
            }
        }
    }
}
