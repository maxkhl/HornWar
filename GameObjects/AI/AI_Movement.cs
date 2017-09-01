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
        /// Target position the AI is moving towards
        /// </summary>
        public Vector2 GoTo
        {
            get
            {
                return _GoTo;
            }
            set
            {
                _GoTo = value;
                Character.LookAt = value;
                GoToInitDistance = (Character.Position - _GoTo).Length();
            }
        }
        private Vector2 _GoTo;

        /// <summary>
        /// Maximum allowed distance, the goto-command is allowed to leave the direct path sideways
        /// </summary>
        private float GoToCurveSize { get; set; }

        /// <summary>
        /// Initial distance when the goto-command was ordered
        /// </summary>
        private float GoToInitDistance { get; set; }

        /// <summary>
        /// Distance, a GoTo command needs to be successful
        /// </summary>
        private float GoToFinishDistance { get; set; }

        /// <summary>
        /// Easing function for the GoTo-command. Determins a path to travel
        /// </summary>
        private Tools.Easing.EaseFunction GoToFunction { get; set; }

        /// <summary>
        /// GoToArrived-Handler
        /// </summary>
        /// <param name="GoToPoint">Target point, the character arrived at</param>
        public delegate void GoToArrivedHandler(Vector2 GoToPoint);

        /// <summary>
        /// Called when the GoTo-Point was reached by the ai's character
        /// </summary>
        public event GoToArrivedHandler GoToArrived;

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
                var TargetPosition = this.GoTo - this.Character.Position + (FarseerPhysics.ConvertUnits.ToDisplayUnits(this.Character.Body.LinearVelocity) * -1);
                var TargetDistance = (this.Character.Position - TargetPosition).Length();

                var s = Tools.Easing.Ease(GoToFunction, TargetDistance - GoToFinishDistance, -GoToCurveSize, GoToCurveSize, this.GoToInitDistance);
               // TargetPosition += ( * Vector2.(Vector2.UnitY * s, Matrix.Create));

                this.Character.Move(TargetPosition);
            }

            if ((this.Character.Position - this.GoTo).Length() < (GoToFinishDistance <= 0 ? 50 : GoToFinishDistance))
            {
                GoToFinishDistance = 50;
                GoToArrived?.Invoke(this.GoTo);
            }
        }

        private void Move(Vector2 Position, float Distance, Tools.Easing.EaseFunction Function)
        {
            GoTo = Position;
            GoToFinishDistance = Distance;
            GoToFunction = Function;
            GoToCurveSize = 80;
        }
    }
}
