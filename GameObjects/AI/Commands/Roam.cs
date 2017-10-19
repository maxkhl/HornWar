//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Xna.Framework;

//namespace Horn_War_II.GameObjects.AI.Commands
//{
//    /// <summary>
//    /// Lets the AI randomly roam around
//    /// </summary>
//    class Roam : Command
//    {
//        private Rectangle? Area;

//        /// <summary>
//        /// Position, the AI is moving towards
//        /// </summary>
//        public Vector2 TargetPosition { get; set; }

//        /// <summary>
//        /// Next waypoint on the way to the target position
//        /// </summary>
//        public Pathfinding.Node Waypoint { get; set; }

//        /// <summary>
//        /// Constructor
//        /// </summary>
//        /// <param name="Area">Area, the command is valid in. Null if you dont care</param>
//        /// <param name="TargetAI">AI, this command is used for</param>
//        public Roam(Rectangle? Area, AI TargetAI) : base(TargetAI)
//        {
//            this.Area = Area;
//            CalculateNewTarget();
//        }

//        /// <summary>
//        /// Update loop for this command
//        /// </summary>
//        /// <param name="gameTime">Current gametime</param>
//        public override void Update(GameTime gameTime)
//        {
//            if (Waypoint != null && (TargetAI.Character.Position - Waypoint.Position).Length() < TargetDistanceToSucceed)
//                CalculateNextWaypoint();
//            if ((TargetAI.Character.Position - TargetPosition).Length() < TargetDistanceToSucceed)
//                CalculateNewTarget();
//            if (Waypoint == null)
//                CalculateNewTarget();

//        }


//        /// <summary>
//        /// Calculates a new random targetposition
//        /// </summary>
//        public void CalculateNextWaypoint()
//        {
//            Waypoint = this.TargetAI.Game.GetComponent<Pathfinding.PathFinderEngine>()?.FindPath( 
//                this.TargetAI.Character.Position, 
//                this.TargetPosition, 
//                new List<FarseerPhysics.Dynamics.Body>()
//                {
//                    this.TargetAI.Character.Body,
//                    this.TargetAI.Character.Weapon?.Body,
//                });


//            CurrentWaypoint = new Waypoint(
//                Waypoint.Position,
//                false,
//                Character.WalkSpeed.Half,
//                Waypoint.Position == TargetPosition ? true : false);
//        }

//        bool once = false;

//        /// <summary>
//        /// Calculates a new random targetposition
//        /// </summary>
//        public void CalculateNewTarget()
//        {
//            int Timeout = 50;
//            Waypoint = null;

//            Vector2 target = Vector2.Zero;
//            while (Waypoint == null)
//            {
//                if (Area.HasValue)
//                    target = new Vector2(
//                        TargetAI.RandomGenerator.Next(Area.Value.X, Area.Value.Width),
//                        TargetAI.RandomGenerator.Next(Area.Value.Y, Area.Value.Height));
//                else
//                    target = new Vector2(
//                        TargetAI.Character.Position.X + TargetAI.RandomGenerator.Next(-300, 300),
//                        TargetAI.Character.Position.Y + TargetAI.RandomGenerator.Next(-300, 300));

//                //if (!once)
//                //{
//                    Waypoint = this.TargetAI.Game.GetComponent<Pathfinding.PathFinderEngine>()?.FindPath(
//                        this.TargetAI.Character.Position,
//                        target,
//                        new List<FarseerPhysics.Dynamics.Body>()
//                        {
//                        this.TargetAI.Character.Body,
//                        this.TargetAI.Character.Weapon?.Body,
//                        });
//                    once = !once;
//                //}

//                Timeout--;
//                if (Timeout <= 0)
//                    return;
//            }

//            this.TargetPosition = target;



//            CurrentWaypoint = new Waypoint(
//                Waypoint.Position,
//                false,
//                Character.WalkSpeed.Half,
//                Waypoint.Position == TargetPosition ? true : false);
//        }

//        public override void DebugDraw(GameTime gameTime, hTexture Pixel, DebugDrawer debugDrawer)
//        {
//            debugDrawer.DrawLine(this.TargetAI.Character.Position, this.TargetPosition, Color.Blue);
//            debugDrawer.DrawLine(this.TargetAI.Character.Position, this.CurrentWaypoint.Target, Color.Pink);

//            base.DebugDraw(gameTime, Pixel, debugDrawer);
//        }
//    }
//}
