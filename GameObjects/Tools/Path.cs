using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Horn_War_II.GameObjects.Tools
{
    /// <summary>
    /// Used to define a path in the gameworld
    /// </summary>
    class Path
    {
        public Vector2 Start { get; protected set; }
        public Vector2 End { get; protected set; }

        public Vector2 Position { get; protected set; }
        

        private AI.Pathfinding.MinHeap _PathList;
        private AI.Pathfinding.Node _PathNode;

        private AI.Pathfinding.PathFinderEngine _pfEngine;

        /// <summary>
        /// Constructor
        /// </summary>
        public Path(Vector2 Start, Vector2 End, AI.Pathfinding.PathFinderEngine pfEngine, List<FarseerPhysics.Dynamics.Body> IgnoreObstacle)
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
                return Node;
            }
            else return null;
        }
    }
}
