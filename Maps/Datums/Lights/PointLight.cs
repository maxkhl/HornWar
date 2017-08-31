using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Horn_War_II.Maps.Datums.Lights
{
    class PointLight : Light
    {
        /// <summary>
        /// Radius of this pointlight
        /// </summary>
        public float Radius { get; set; }

        public PointLight(Map Map, string Name, float Radius)
            : base(Map, Name)
        {
            this.Radius = Radius;
        }
    }
}
