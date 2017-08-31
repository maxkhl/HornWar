using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Horn_War_II.Maps.Datums
{    
    /// <summary>
    /// Light datum
    /// </summary>
    class Light : Datum
    {
        /// <summary>
        /// Color of this light
        /// </summary>
        public Color Color = Color.White;

        /// <summary>
        /// Brightness multiplicator
        /// </summary>
        public float Brightness = 1f;

        /// <summary>
        /// Initializes a new instance of the <see cref="Light"/> class.
        /// </summary>
        /// <param name="Name">Name of this datum.</param>
        public Light(Map Map, string Name)
            : base(Map, Name)
        {

        }
    }
}
