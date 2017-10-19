using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Horn_War_II.GameObjects.AI
{
    /// <summary>
    /// Handles visible stuff, the AI does. Like changing the character to a red color if taunted and shit
    /// </summary>
    partial class AI
    {
        /// <summary>
        /// Processes the effects for this AI
        /// </summary>
        /// <param name="gameTime"></param>
        private void ProcessEffects(GameTime gameTime)
        {
            if (ActiveCommand == null) return;

            switch (this.ActiveCommand.GetType().Name)
            {
                case "Attack":
                    this.Character.Color = new Color(255, 180, 180);
                    break;
                case "Roam":

                    break;
            }
        }
    }
}
