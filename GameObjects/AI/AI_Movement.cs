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
                    this.Character.LookAt = this.Character.Position - TargetPosition;
                }
                else
                {
                    if (this.AttackTarget != null)
                    {
                        TargetPosition = (this.AttackTarget.Position - this.Character.Position) * 5 + FarseerPhysics.ConvertUnits.ToDisplayUnits(this.AttackTarget.Body.LinearVelocity) - FarseerPhysics.ConvertUnits.ToDisplayUnits(this.Character.Body.LinearVelocity) * FarseerPhysics.ConvertUnits.ToDisplayUnits(this.Character.Body.Mass);

                        this.Character.LookAt = this.AttackTarget.Position;
                    }

                    // Attack curve
                    if (this.Path != null)
                        TargetPosition = (Path.CalculateStep(this.Character.Position, AttackTarget.Position) * 25) - FarseerPhysics.ConvertUnits.ToDisplayUnits(this.Character.Body.LinearVelocity) * FarseerPhysics.ConvertUnits.ToDisplayUnits(this.Character.Body.Mass);
                }

                GoToCalculatedPoint = TargetPosition;
                this.Character.Move(TargetPosition);
            }
        }

        public Tools.Path Path;
        public BodyObject AttackTarget;
        private void Attack(BodyObject Target, float CurveWidth, float CurveStart, float CurveEnd, Tools.Easing.EaseFunction FunctionIn, Tools.Easing.EaseFunction FunctionOut)
        {
            this.AttackTarget = Target;
            this.GoTo = Vector2.Zero;


            // Path shit deacitvated for now - it's harder than I thought
            /*if (Path == null)
            {
                this.AttackTarget = Target;
                this.GoTo = Vector2.Zero;

                Path = new Tools.Path(
                    this.Character.Position,
                    Target.Position,
                    FunctionIn,
                    FunctionOut,
                    Tools.Path.CurveSides.Left,
                    CurveStart,
                    CurveEnd,
                    CurveWidth);
            }*/
        }

        private void Fallback(BodyObject Target)
        {
            this.AttackTarget = null;

            var reverseVector = Vector2.Negate(Target.Position - this.Character.Position);
            reverseVector.Normalize();
            this.GoTo = reverseVector;
            this.GoToLookAt = Target.Position - this.Character.Position;
        }

        private void Move(Vector2 Target)
        {
            this.AttackTarget = null;
            this.Path = null;

            this.GoTo = Target;
        }
    }
}
