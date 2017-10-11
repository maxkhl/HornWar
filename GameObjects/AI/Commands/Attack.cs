using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Horn_War_II.GameObjects.AI.Commands
{
    /// <summary>
    /// Lets the AI attack a target
    /// </summary>
    class Attack : Command
    {

        private Tools.Path Path { get; set; }

        /// <summary>
        /// Character that is being attacked
        /// </summary>
        public Character Target { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="TargetAI">AI, this command is used for</param>
        public Attack(Character TargetCharacter, AI TargetAI) : base(TargetAI)
        {
                this.Target = Target;

                Path = new Tools.Path(
                    TargetAI.Character.Position,
                    Target.Position,
                    Tools.Easing.EaseFunction.BackEaseIn,
                    Tools.Easing.EaseFunction.BackEaseOut,
                    Tools.Path.CurveSides.Left,
                    1,
                    100,
                    100);
        }

        /// <summary>
        /// Update loop for this command
        /// </summary>
        /// <param name="gameTime">Current gametime</param>
        /// <returns>Returns movement vector</returns>
        public override Vector2 Update(GameTime gameTime)
        {

            return Vector2.Zero;
        }
    }
}
