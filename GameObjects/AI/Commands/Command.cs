using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horn_War_II.GameObjects.AI.Commands
{
    /// <summary>
    /// Abstract base class for AI commands
    /// Commands are self-processing orders the AI can execute (like evade, roam or attack)
    /// </summary>
    abstract class Command
    {
        /// <summary>
        /// AI, this command is used for
        /// </summary>
        public AI TargetAI { get; set; }

        /// <summary>
        /// The current Waypoint for this command
        /// </summary>
        public Waypoint Waypoint { get; protected set; }

        /// <summary>
        /// Distance to waypoint to succeed
        /// </summary>
        public float TargetDistanceToSucceed { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="TargetAI">AI, this command is used for</param>
        public Command(AI TargetAI)
        {
            this.TargetAI = TargetAI;
            TargetDistanceToSucceed = 15;
        }

        /// <summary>
        /// Update loop for this command
        /// </summary>
        /// <param name="gameTime">Current gametime</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Requests a Waypoint, null if not available
        /// </summary>
        /// <returns>New Waypoint</returns>
        public abstract Waypoint? RequestWaypoint();

        /// <summary>
        /// Debug drawing
        /// </summary>
        /// <param name="gameTime">Current gametime</param>
        /// <param name="Pixel">Pixel texture for drawing</param>
        /// <param name="debugDrawer">Debug drawer that called this method</param>
        public virtual void DebugDraw(GameTime gameTime, hTexture Pixel, DebugDrawer debugDrawer) { }
    }
}
