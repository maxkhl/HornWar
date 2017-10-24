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
    class Follow : Command
    {
        /// <summary>
        /// Position, the AI is moving towards
        /// </summary>
        public SpriteObject Target { get; set; }
        public Pathfinding.Path Path { get; set; }

        private Vector2 _TargetPosition;

        /// <summary>
        /// Constructor
        /// </summary>
        public Follow(SpriteObject Target, AI TargetAI) : base(TargetAI)
        {
            this.Target = Target;
            this._TargetPosition = Target.Position;

            Path = new Pathfinding.Path(
                this.TargetAI.Character.Position,
                Target.Position,
                TargetAI.Game.GetComponent<Pathfinding.PathFinderEngine>(),
                new List<FarseerPhysics.Dynamics.Body>()
                {
                    TargetAI.Character.Body,
                    TargetAI.Character.Weapon?.Body,
                });
        }

        /// <summary>
        /// Update loop for this command
        /// </summary>
        /// <param name="gameTime">Current gametime</param>
        public override void Update(GameTime gameTime)
        {
            // Refresh path when target moves too far away
            if((Path.End - Target.Position).Length() > 50)
                Path = new Pathfinding.Path(
                    this.TargetAI.Character.Position,
                    Target.Position,
                    TargetAI.Game.GetComponent<Pathfinding.PathFinderEngine>(),
                    new List<FarseerPhysics.Dynamics.Body>()
                    {
                        TargetAI.Character.Body,
                        TargetAI.Character.Weapon?.Body,
                    });
        }

        public override Waypoint? RequestWaypoint()
        {
            if((TargetAI.Character.Position - Path.Position).Length() < this.TargetDistanceToSucceed)
            {
                var newTarget = Path.Next();
                if(newTarget.HasValue)
                {
                    return new Waypoint(
                        newTarget.Value,
                        false,
                        Character.WalkSpeed.Full,
                        Path.LastWaypoint,
                        Path.PeekNext()
                        );
                }
            }
            return null;
        }

        public override void DebugDraw(GameTime gameTime, hTexture Pixel, DebugDrawer debugDrawer)
        {
            debugDrawer.DrawLine(this.TargetAI.Character.Position, this.Target.Position, Color.Blue);
            debugDrawer.DrawLine(this.TargetAI.Character.Position, this.Path.Position, Color.Pink);

            base.DebugDraw(gameTime, Pixel, debugDrawer);
        }
    }
}
