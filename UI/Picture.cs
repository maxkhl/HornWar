using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Horn_War_II.UI
{
    class Picture : UiObject
    {
        public Picture(Scenes.MenuScene MenuScene, GameObjects.PhysicEngine PhysicEngine, Texture2D Texture)
            : base(MenuScene, PhysicEngine)
        {
            this.Texture = new hTexture(Texture);

            //this.ShapeFromTexture();
        }


    }
}
