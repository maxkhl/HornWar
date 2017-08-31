using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Horn_War_II.Maps.Datums
{
    /// <summary>
    /// Defines a spawn location
    /// </summary>
    class Spawn : SquareDatum
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Spawn"/> class.
        /// </summary>
        /// <param name="Name">Name of this datum.</param>
        /// <param name="Bounds">Bounds of this datum.</param>
        public Spawn(Map Map, string Name, Rectangle Bounds)
            : base(Map, Name, Bounds)
        {

        }

        /// <summary>
        /// Attempts to get a spawn point in this area. Returns false if there are any problems/spawning is not possible
        /// </summary>
        public bool TryGetSpawnPoint(GameTime Time, out Vector2 Point)
        {
            Point = this.GetRandomPoint(Time);
            return true; // TODO: Collision- and spawncamp-check
        }
    }
}
