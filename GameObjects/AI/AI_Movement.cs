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
        private Vector2 _PositionAtCurrentMovementOrder;
        private Vector2 _VelocityAtCurrentMovementOrder;

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
                if (newWP.HasValue)
                {
                    CurrentMovementOrder = newWP.Value;
                    _VelocityAtCurrentMovementOrder = FarseerPhysics.ConvertUnits.ToDisplayUnits(this.Character.Body.LinearVelocity);
                    _PositionAtCurrentMovementOrder = this.Character.Position;
                }
            }

            if (CurrentMovementOrder != Waypoint.Zero)
            {
                Character.Boost = CurrentMovementOrder.Boost;
                Character.Speed = CurrentMovementOrder.Speed;

                var direction = CurrentMovementOrder.Target - this.Character.Position;

                var DistanceBetweenPoints = (CurrentMovementOrder.Target - _PositionAtCurrentMovementOrder).Length();
                var DistanceToTarget = (CurrentMovementOrder.Target - this.Character.Position).Length();

                if(DistanceBetweenPoints > 0)
                {

                    var target = Vector2.Hermite(
                        _PositionAtCurrentMovementOrder,
                        _PositionAtCurrentMovementOrder + (_VelocityAtCurrentMovementOrder *
                          FarseerPhysics.ConvertUnits.ToDisplayUnits(this.Character.Body.Mass) * -1),
                        CurrentMovementOrder.Target,
                        (CurrentMovementOrder.PeekNext.HasValue ? CurrentMovementOrder.PeekNext.Value : CurrentMovementOrder.Target),
                        DistanceToTarget / DistanceBetweenPoints);

                    target -= this.Character.Position;
                    target.Normalize();

                    /*target +=
                        (FarseerPhysics.ConvertUnits.ToDisplayUnits(this.Character.Body.LinearVelocity)) *
                        FarseerPhysics.ConvertUnits.ToDisplayUnits(this.Character.Body.Mass) * -1;*/

                    Character.Move(target);

                }
            }
        }

        private void DebugDrawMovement(GameTime gameTime, hTexture Pixel, DebugDrawer debugDrawer)
        {
            if (CurrentMovementOrder != Waypoint.Zero)
            {
                Vector2 position = _PositionAtCurrentMovementOrder;
                for (float p = 1; p >= 0; p -= 0.05f)
                {
                    var target = Vector2.Hermite(
                        _PositionAtCurrentMovementOrder,
                        _PositionAtCurrentMovementOrder + ( _VelocityAtCurrentMovementOrder *
                          FarseerPhysics.ConvertUnits.ToDisplayUnits(this.Character.Body.Mass) * -1),
                        CurrentMovementOrder.Target,
                        (CurrentMovementOrder.PeekNext.HasValue ? CurrentMovementOrder.PeekNext.Value : CurrentMovementOrder.Target),
                        p);

                    debugDrawer.DrawLine(position, target, Color.Yellow, 1);

                    position = target;
                }
            }
        }
    }
}
