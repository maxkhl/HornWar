using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Horn_War_II.GameObjects.Decoration
{
    /// <summary>
    /// A immovable/floating rock
    /// </summary>
    class Rock : BodyObject
    {
        /// <summary>
        /// Different types of rocks
        /// </summary>
        public enum RockType
        {
            Rock1,
        }

        /// <summary>
        /// Type of this rock
        /// </summary>
        public RockType Type { get; private set; }

        public Rock(Scenes.GameScene GameScene, PhysicEngine PhysicEngine, RockType Type)
            : base(GameScene, PhysicEngine)
        {
            this.Type = Type;
            this.Body.IsStatic = true;
            this.Material = BOMaterial.Stone;

            switch(Type)
            {
                case RockType.Rock1:
                    this.Texture = new hTexture(Game.Content.Load<Texture2D>("Images/Maps/Cave/Rock1"));
                    break;
            }

            this.ShapeFromTexture();
        }
    }
}
