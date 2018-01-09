using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Horn_War_II.Server
{
    /// <summary>
    /// Used for hosting a gameserver
    /// </summary>
    class Gameserver : Lidgren.Network.NetServer, IGameComponent, IUpdateable, IDisposable
    {

        public Game Game { get; }

        /// <summary>
        /// Triggers updating of the Gameserver
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Update Order of this component. Should be the highest for the gameserver
        /// </summary>
        public int UpdateOrder { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Port">Port, the server should listen to</param>
        /// <param name="Tickrate">Tickrate of the Server (this will set the Tickrate for this Game and all Clients)</param>
        /// <param name="UPnP">Enable UPnP port forwarding</param>
        public Gameserver(int Port, int Tickrate, bool UPnP) : base(new Lidgren.Network.NetPeerConfiguration("HornWar2")
        {
            Port = Port,
            EnableUPnP = UPnP,            
        })
        {
            this.Start();
            this.Game.TargetElapsedTime = TimeSpan.FromMilliseconds(1000 / Tickrate);
        }
        
        /// <summary>
        /// Updates the GameServer
        /// </summary>
        /// <param name="gameTime">Current GameTime</param>
        public void Update(GameTime gameTime)
        {

        }


        public void Dispose()
        {

        }


        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public virtual void Initialize()
        {

        }

        protected virtual void OnEnabledChanged(object sender, EventArgs args)
        {
            EnabledChanged(sender, args);
        }
        protected virtual void OnUpdateOrderChanged(object sender, EventArgs args)
        {
            UpdateOrderChanged(sender, args);
        }
    }
}
