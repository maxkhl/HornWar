using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Horn_War_II.GameObjects.AI.Pathfinding
{
    /// <summary>
    /// Used to define a path in the gameworld
    /// </summary>
    class Path
    {
        public Vector2 Start { get; protected set; }
        public Vector2 End { get; protected set; }

        public Vector2 Position { get; protected set; }
        

        private MinHeap _PathList;
        private Node _PathNode;

        private PathFinderEngine _pfEngine;

        public bool LastWaypoint { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Path(Vector2 Start, Vector2 End, PathFinderEngine pfEngine, List<FarseerPhysics.Dynamics.Body> IgnoreObstacle)
        {
            this.Start = Start;
            this.Position = Start;
            this.End = End;
            this._pfEngine = pfEngine;

            _PathList = _pfEngine.FindPathReversed(
                this.Position,
                this.End,
                IgnoreObstacle);

            if (_PathList != null && _PathList.HasNext())
                _PathNode = _PathList.ExtractFirst();
        }

        public Vector2? Next()
        {
            if (_PathNode != null)
            {
                var Node = _PathNode.Position;
                Position = Node;
                _PathNode = _PathNode.next;

                if (_PathNode == null)
                    LastWaypoint = true;
                return Node;
            }
            else return null;
        }


        public Vector2? PeekNext()
        {
            if (_PathNode != null && _PathNode.next != null)
            {
                var Node = _PathNode.next.Position;
                return Node;
            }
            else return null;
        }
    }
}
