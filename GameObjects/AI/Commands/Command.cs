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
        /// Constructor
        /// </summary>
        /// <param name="TargetAI">AI, this command is used for</param>
        public Command(AI TargetAI)
        {
            this.TargetAI = TargetAI;
        }

        /// <summary>
        /// Update loop for this command
        /// </summary>
        /// <param name="gameTime">Current gametime</param>
        /// <returns>Returns movement vector</returns>
        public abstract Vector2 Update(GameTime gameTime);
    }
}
