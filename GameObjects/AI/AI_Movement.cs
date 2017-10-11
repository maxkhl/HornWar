using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Horn_War_II.GameObjects.AI
{
    partial class AI
    {
        /// <summary>
        /// Currently calculated goto-step (including curves and shit)
        /// </summary>
        public Vector2 GoToCalculatedPoint { get; private set; }

        /// <summary>
        /// Moves character towards this point
        /// </summary>
        public Vector2 GoTo { get; private set; }

        /// <summary>
        /// Lookat while moving towards goto
        /// </summary>
        private Vector2 GoToLookAt
        {
            get
            {
                return _GoToLookAt;
            }
            set
            {
                _GoToLookAt = value;
                Character.LookAt = value;
            }
        }
        private Vector2 _GoToLookAt;

        /// <summary>
        /// Processes the movement-orders for this AI
        /// </summary>
        /// <param name="gameTime"></param>
        private void ProcessMovement(GameTime gameTime)
        {
            var Movement = ActiveCommand?.Update(gameTime);
            if(Movement.HasValue)
                Character.Move(Movement.Value);
        }
    }
}
