using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;

namespace Horn_War_II.GameObjects
{
    /// <summary>
    /// Tehesttt
    /// </summary>
    class TestBody : BodyObject
    {
        public TestBody(Scenes.GameScene GameScene, PhysicEngine PhysicEngine)
            : base(GameScene, PhysicEngine)
        {
            this.Texture = new hTexture(SceneManager.Game.Content.Load<Texture2D>("Images/Cyborg"));
            this.Color = Color.Aqua;
            this.Size *= 1.5f;
            this.ShapeFromTexture();
        }
    }
}
