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
        /// Tells the character to hurry and move faster
        /// </summary>
        private bool Boost { get; set; }

        /// <summary>
        /// Processes the movement-orders for this AI
        /// </summary>
        /// <param name="gameTime"></param>
        private void ProcessMovement(GameTime gameTime)
        {
            this.Character.Boost = this.Boost;

            if (this.State != AIState.Wait)
            {

                this.Boost = true;

                var TargetPosition = this.Character.Position;

                // Normal -straight- movement
                if (this.GoTo != Vector2.Zero)
                {
                    TargetPosition = this.GoTo;
                }
                else
                {
                    // Attack curve
                    if (this.Path != null)
                        TargetPosition = Path.CalculateStep(this.Character.Position, AttackTarget.Position);
                }

                GoToCalculatedPoint = TargetPosition;
                this.Character.Move(TargetPosition);
            }
        }

        public Tools.Path Path;
        public BodyObject AttackTarget;
        private void Attack(BodyObject Target, float Distance, Tools.Easing.EaseFunction Function)
        {
            this.AttackTarget = Target;
            this.GoTo = Vector2.Zero;

            Path = new Tools.Path(
                this.Character.Position,
                Function,
                Tools.Path.CurveSides.Left,
                900,
                0,
                70);
        }

        private void Move(Vector2 Target)
        {
            this.AttackTarget = null;
            this.Path = null;

            this.GoTo = Target;
        }
    }
}
