using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Horn_War_II.Maps.Datums
{
    /// <summary>
    /// Defines a datum
    /// </summary>
    abstract class Datum : IGameComponent
    {
        /// <summary>
        /// Name of this datum
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Map this datum is attached to
        /// </summary>
        public Map Map { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Datum"/> class.
        /// </summary>
        /// <param name="Name">Name of this datum.</param>
        public Datum(Map Map, string Name)
        {
            this.Map = Map;
            this.Name = Name;
        }

        /// <summary>
        /// Not used. (Use constructor instead)
        /// </summary>
        public void Initialize()
        { }
    }
}
