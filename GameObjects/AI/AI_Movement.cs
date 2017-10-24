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

        public Waypoint CurrentMovementOrder { get; protected set; }

        /// <summary>
        /// Processes the movement-orders for this AI
        /// </summary>
        /// <param name="gameTime"></param>
        private void ProcessMovement(GameTime gameTime)
        {
            if (ActiveCommand != null)
            {
                ActiveCommand.Update(gameTime);

                var newWP = ActiveCommand.RequestWaypoint();
                if(newWP.HasValue)
                    CurrentMovementOrder = newWP.Value;
            }

            
            Character.Boost = CurrentMovementOrder.Boost;
            Character.Speed = CurrentMovementOrder.Speed;

            var direction = CurrentMovementOrder.Target - this.Character.Position;


            var target = Vector2.Hermite(
                this.Character.Position,
                this.Character.Position + ( FarseerPhysics.ConvertUnits.ToDisplayUnits(this.Character.Body.LinearVelocity) *
                                            FarseerPhysics.ConvertUnits.ToDisplayUnits(this.Character.Body.Mass) * -1 ),
                CurrentMovementOrder.Target,
                (CurrentMovementOrder.PeekNext.HasValue ? CurrentMovementOrder.PeekNext.Value : CurrentMovementOrder.Target),
                1.0f);

            target -= this.Character.Position;

            /*target +=
                (FarseerPhysics.ConvertUnits.ToDisplayUnits(this.Character.Body.LinearVelocity)) *
                FarseerPhysics.ConvertUnits.ToDisplayUnits(this.Character.Body.Mass) * -1;*/

            Character.Move(target);
            
        }
    }
}
