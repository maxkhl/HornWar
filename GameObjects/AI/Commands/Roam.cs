using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Horn_War_II.GameObjects.AI.Commands
{
    /// <summary>
    /// Lets the AI randomly roam around
    /// </summary>
    class Roam : Command
    {
        private Rectangle? Area;

        /// <summary>
        /// Position, the AI is moving towards
        /// </summary>
        public Vector2 TargetPosition { get; set; }

        private float TargetDistanceToSucceed = 5;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Area">Area, the command is valid in. Null if you dont care</param>
        /// <param name="TargetAI">AI, this command is used for</param>
        public Roam(Rectangle? Area, AI TargetAI) : base(TargetAI)
        {
            this.Area = Area;
        }

        /// <summary>
        /// Update loop for this command
        /// </summary>
        /// <param name="gameTime">Current gametime</param>
        /// <returns>Returns movement vector</returns>
        public override Vector2 Update(GameTime gameTime)
        {
            if ((TargetAI.Character.Position - TargetPosition).Length() < TargetDistanceToSucceed)
                CalculateNewTarget();

            return TargetPosition;
        }

        /// <summary>
        /// Calculates a new random targetposition
        /// </summary>
        public void CalculateNewTarget()
        {
            int Timeout = 10;
            bool clearPath = false;

            Vector2 Movement = Vector2.Zero;

            while (!clearPath)
            {
                if (Area.HasValue)
                    Movement = new Vector2(
                        TargetAI.RandomGenerator.Next(Area.Value.X, Area.Value.Width),
                        TargetAI.RandomGenerator.Next(Area.Value.Y, Area.Value.Height));
                else
                    Movement = new Vector2(
                        TargetAI.Character.Position.X + TargetAI.RandomGenerator.Next(-300, 300),
                        TargetAI.Character.Position.Y + TargetAI.RandomGenerator.Next(-300, 300));

                var hits = TargetAI.Character.PhysicEngine.World.RayCast(
                    FarseerPhysics.ConvertUnits.ToSimUnits(TargetAI.Character.Position),
                    FarseerPhysics.ConvertUnits.ToSimUnits(Movement));

                bool Obstacle = false;
                foreach (var fixture in hits)
                    if (fixture.Body.UserData != this && fixture.Body.UserData != TargetAI.Character.Weapon)
                        Obstacle = true;

                clearPath = !Obstacle;

                Timeout--;
                if (Timeout <= 0)
                    clearPath = true;
            }
        }
    }
}
