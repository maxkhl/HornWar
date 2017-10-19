using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Horn_War_II.GameObjects.AI.Pathfinding
{
    /// <summary>
    /// Original Author: Roy Triesscheijn (http://www.roy-t.nl)
    /// Class used to provide pathfinding capabilities in the game
    /// </summary>
    class PathFinderEngine : GameObject
    {
        private PhysicEngine _PhysicEngine;

        public PathFinderEngine(Scenes.GameScene GameScene, PhysicEngine PhysicEngine) : base(GameScene.Game)
        {
            this._PhysicEngine = PhysicEngine;
        }

        /// <summary>
        /// Reverse-searches for a path between two points
        /// </summary>
        /// <param name="Start">Start position</param>
        /// <param name="End">Target position</param>
        /// <param name="Exclude">Bodys to exclude during check</param>
        /// <returns>Next waypoint on the list</returns>
        public MinHeap FindPathReversed(Vector2 Start, Vector2 End, List<Body> Exclude)
        {
            //note we just flip start and end here so you don't have to.            
            return FindPath(End, Start, Exclude);
        }

        //private List<Vector2> _HitPoints = new List<Vector2>();
        private float _MinDistanceToObstacle = 50f;
        private MinHeap _MiniHeapDebugDraw = null;

        /// <summary>
        /// Searches for a path between two points
        /// </summary>
        /// <param name="Start">Start position</param>
        /// <param name="End">Target position</param>
        /// <param name="Exclude">Bodys to exclude during check</param>
        /// <returns>Next waypoint on the list</returns>
        public MinHeap FindPath(Vector2 Start, Vector2 End, List<Body> Exclude)
        {
            _DebugSurrounding.Clear();
            //_HitPoints.Clear();

            Node startNode = new Node(Start, 0, 0, null);
            Node endNode = new Node(End, 0, 0, null);

            MinHeap openList = new MinHeap();
            openList.Add(startNode);

            _MiniHeapDebugDraw = openList;

            bool hit = false;
            _PhysicEngine.World.RayCast((f, p, n, fr) =>
            {
                if(!Exclude.Contains(f.Body))
                {
                    hit = true;
                    return 0;
                }
                else
                {
                    return -1;
                }
            },
                FarseerPhysics.ConvertUnits.ToSimUnits(Start),
                FarseerPhysics.ConvertUnits.ToSimUnits(End));


            if (!hit)
            {
                openList = new MinHeap();
                openList.Add(endNode);
                endNode.next = startNode;
                return openList;
            }




            Dictionary<Vector2, bool> brWorld = new Dictionary<Vector2, bool>();

            brWorld.Add(Start, true);
            brWorld.Add(End, true);

            Int64 overflow = 10000;
            while (openList.HasNext())
            {
                if (overflow <= 0) return null;
                //if (MaxJumpCount > 0 && Jumps >= MaxJumpCount) return null;

                Node current = openList.ExtractFirst();

                if ((current.Position - End).Length() < 100)
                {
                    return openList;
                }

                var surroundings = GetSurrounding(current.Position, End, Exclude);

                //Add new nodes in our network
                foreach (var node in surroundings)
                    if (!brWorld.ContainsKey(node.Position))
                        brWorld.Add(node.Position, false);

                for (int i = 0; i < surroundings.Length; i++)
                {
                    SurroundingNode surr = surroundings[i];
                    Vector2 brWorldIdx = surr.Position;

                    if (brWorld.ContainsKey(brWorldIdx) && brWorld[brWorldIdx] == false)
                    {
                        brWorld[brWorldIdx] = true;
                        int pathCost = current.pathCost + surr.Cost;
                        int cost = pathCost + (int)(surr.Position - End).Length();
                        Node node = new Node(surr.Position, cost, pathCost, current);
                        openList.Add(node);
                        overflow--;
                    }
                }
            }
            return null; //no path found
        }

        /// <summary>
        /// Surrounding node class
        /// </summary>
        class SurroundingNode
        {
            public SurroundingNode(Vector2 Position)
            {
                this.Position = Position;
                Cost = 0;
            }

            public Vector2 Position;
            public int Cost;

            public override bool Equals(object obj)
            {
                return this.Position.Equals(((SurroundingNode)obj).Position);
            }
        }

        /// <summary>
        /// Returns neighbour nodes
        /// </summary>
        private SurroundingNode[] GetSurrounding(Vector2 Position, Vector2 Target, List<Body> Exclude)
        {
            List<SurroundingNode> Surrounding = new List<SurroundingNode>();

            // Make a direct cast and see if we can skip a ton of nodes
            var testCast = TubeRayCast(Position, Target, Exclude, _MinDistanceToObstacle);
            if(!testCast)
            {
                // This means we have a direct connection to the target so we add that node and get the fuck out
                Surrounding.Add(new SurroundingNode(Target));
                return Surrounding.ToArray();
            }
            /*else if((testCast.Item2 - Position).Length() > _MinDistanceToObstacle * 2) // We have hit an obstacle in between so we figure out if it is far enough away. Maybe we can skip some way without using the heavier raycasting
            {
                // Now we figure out how far we can move without hitting the obstacle
                var distanceVec = Position - testCast.Item2;
                var distance = distanceVec.Length();

                var directionVec = distanceVec;
                directionVec.Normalize();

                var directionVecMinDist = directionVec * _MinDistanceToObstacle;

                // This is the point far away enough to meet our min distance requirement before the obstacle
                var targetVec = testCast.Item2 - directionVecMinDist;

                // We add this point as the next node and let the fancy algorithm take over to avoid it in our next loops
                Surrounding.Add(new SurroundingNode(targetVec));
                return Surrounding.ToArray();
            }*/


            var degreeToTarget = MathHelper.ToDegrees((float)Math.Atan2(Target.Y - Position.Y, Target.X - Position.X)) * -1;

            // Generate surrounding points
            // Loop over 180 degrees (half a circle btw)
            for (float r = degreeToTarget; r >= degreeToTarget - 180; r -= 30)
            {
                // Two loops per degree, one for positive degree and the other for negative degrees. So we dont check clockwise but we look towards the target and
                // move outwards as long as we dont find a path
                for (int x = -1; x <= 1; x += 2)
                {
                    var rayLength = RotateVector2(
                                        new Vector2(_MinDistanceToObstacle, 0),
                                        MathHelper.ToRadians(r * x));

                    var targetVec = Position + rayLength;

                    var rayResult = TubeRayCast(Position, targetVec, Exclude, _MinDistanceToObstacle);

                    // No hit. Now we check of there is enough room for us
                    if (!rayResult)
                    {
                        var targetVecSpaceCheck = Position + rayLength * 2;
                        rayResult = TubeRayCast(Position, targetVecSpaceCheck, Exclude, _MinDistanceToObstacle);
                        // No hit again! This means this point is save to travel to
                        if (!rayResult)
                        {
                            Surrounding.Add(new SurroundingNode(targetVec));
                            //return Surrounding.ToArray();
                        }
                    }
                }
                if(Surrounding.Count != 0)
                    return Surrounding.ToArray();
            }

            


            /*foreach (var hPoint in _HitPoints)
                if ((hPoint - Position).Length() < _MinDistanceToObstacle)
                    Surrounding.Clear();*/

            return Surrounding.ToArray();
        }


        private bool TubeRayCast(Vector2 From, Vector2 To, List<Body> Exclude, float Width)
        {
            //var degreeToTarget = MathHelper.ToDegrees((float)Math.Atan2(From.Y - To.Y, From.X - To.X)) * -1;
            //var degreeToTargetRight = degreeToTarget + 90;

            //var directionVec = new Vector2(
            //    (float)Math.Cos(degreeToTarget),
            //    -(float)Math.Sin(degreeToTarget));

            var ForwardVector = From - To;
            ForwardVector.Normalize();

            var SideVector = new Vector2(0, 1);
            SideVector = RotateVector2(SideVector, Math.Atan2(ForwardVector.Y, ForwardVector.X));

            // Get the vector pointing left or right from the forward one
            var ForwardRightVector = new Vector2(ForwardVector.Y, -ForwardVector.X);
            var ForwardLeftVector = -ForwardRightVector;


            var halfWidth = Width / 2;
            var ray1 = RayCast(From + (ForwardRightVector * halfWidth), To + (ForwardRightVector * halfWidth), Exclude);
            var ray2 = RayCast(From + (ForwardLeftVector * halfWidth), To + (ForwardLeftVector * halfWidth), Exclude);

            return ray1.Item1 || ray2.Item1;
        }

        private Tuple<bool, Vector2> RayCast(Vector2 From, Vector2 To, List<Body> Exclude)
        {
            bool hit = false;
            var pos = Vector2.Zero;

            _PhysicEngine.World.RayCast((f, p, n, fr) =>
            {
                if (!Exclude.Contains(f.Body))
                {
                    //_HitPoints.Add(FarseerPhysics.ConvertUnits.ToDisplayUnits(p));
                    pos = FarseerPhysics.ConvertUnits.ToDisplayUnits(p);
                    hit = true;
                    return 0;
                }
                else
                {
                    return -1;
                }
            },
            FarseerPhysics.ConvertUnits.ToSimUnits(From),
            FarseerPhysics.ConvertUnits.ToSimUnits(To));

            if (!_DebugSurrounding.ContainsKey(From))
                _DebugSurrounding.Add(From, new List<Tuple<Vector2, bool>>());
            
            _DebugSurrounding[From].Add(new Tuple<Vector2, bool>(To, hit));

            return new Tuple<bool, Vector2>(hit, pos);
        }

        Dictionary<Vector2, List<Tuple<Vector2, bool>>> _DebugSurrounding = new Dictionary<Vector2, List<Tuple<Vector2, bool>>>();

        /// <summary>
        /// Drawing the node-network
        /// </summary>
        public override void DebugDraw(GameTime gameTime, hTexture Pixel, DebugDrawer debugDrawer)
        {
            foreach (var networkKVP in _DebugSurrounding)
            { 
                var source = networkKVP.Key;
                
                foreach(var target in networkKVP.Value)
                {
                    debugDrawer.DrawLine(source, target.Item1, target.Item2 ? Color.Silver : Color.Red, 1);
                }
            }

            if(_MiniHeapDebugDraw != null)
            {
                Node oldNode = null;
                var node = _MiniHeapDebugDraw.ExtractFirst();
                while(node != null)
                {
                    if(oldNode != null)
                        debugDrawer.DrawLine(oldNode.Position, node.Position, Color.Pink, 2);

                    oldNode = node;
                    node = node.next;
                }

            }


            base.DebugDraw(gameTime, Pixel, debugDrawer);
        }


        /// <summary>
        /// Rotates a 2-dimensional vector around the given rad
        /// </summary>
        private Vector2 RotateVector2(Vector2 vec, double radians)
        {
            Vector2 result = new Vector2();
            result.X = (float)(vec.X * Math.Cos(radians) - vec.Y * Math.Sin(radians));
            result.Y = (float)(vec.X * Math.Sin(radians) + vec.Y * Math.Cos(radians));
            return result;
        }
    }
}
