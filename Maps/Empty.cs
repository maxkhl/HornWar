using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Horn_War_II.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Horn_War_II.Maps
{
    /// <summary>
    /// Empty map
    /// </summary>
    class Empty : Map
    {
        public Empty(Scenes.GameScene GameScene)
            : base(GameScene)
        {
            //Do nothing
        }
    }
}
