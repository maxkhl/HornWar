using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horn_War_II.GameObjects.AI.Pathfinding
{
    /// <summary>
    /// Author: Roy Triesscheijn (http://www.roy-t.nl)
    /// Class defining BreadCrumbs used in path finding to mark our routes
    /// </summary>
    public class Node
    {
        public Vector2 Position;
        public int cost;
        public int pathCost;
        public Node next;
        public Node nextListElem;

        public Node(Vector2 Position, int cost, int pathCost, Node next)
        {
            this.Position = Position;
            this.cost = cost;
            this.pathCost = pathCost;
            this.next = next;
        }
    }
}
