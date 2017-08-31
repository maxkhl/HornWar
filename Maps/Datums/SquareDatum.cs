using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Horn_War_II.Maps.Datums
{
    /// <summary>
    /// Defines a datum for a specific area
    /// </summary>
    abstract class SquareDatum : Datum
    {
        /// <summary>
        /// Bounds of this datum
        /// </summary>
        public Rectangle Bounds { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Datum" /> class.
        /// </summary>
        /// <param name="Name">Name of this datum.</param>
        /// <param name="Bounds">Bounds of this datum.</param>
        public SquareDatum(Map Map, string Name, Rectangle Bounds)
            : base(Map, Name)
        {
            this.Bounds = Bounds;
        }

        /// <summary>
        /// Returns a random position inside the datums bounds.
        /// </summary>
        public Vector2 GetRandomPoint(GameTime Time)
        {
            var rand = new Random(Time.TotalGameTime.Milliseconds);
            return new Vector2(
                (float)(rand.NextDouble() * Bounds.Width + Bounds.X),
                (float)(rand.NextDouble() * Bounds.Height + Bounds.Y));
        }
    }
}
