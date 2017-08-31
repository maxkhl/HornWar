using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.TextureTools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Horn_War_II.GameObjects
{
    /// <summary>
    /// A static, not moving, object
    /// </summary>
    class StaticObject : BodyObject
    {
        public StaticObject(Scenes.GameScene GameScene, PhysicEngine PhysicEngine)
            : base(GameScene, PhysicEngine)
        {
            //Static
            this.Dynamic = false;
            Body.CollisionCategories = FarseerPhysics.Dynamics.Category.StaticGOs;
        }
    }
}
