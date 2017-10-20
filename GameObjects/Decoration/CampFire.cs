using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Horn_War_II.GameObjects.Decoration
{
    /// <summary>
    /// A immovable/burning campfire
    /// </summary>
    class CampFire : BodyObject
    {
        /// <summary>
        /// Fire effect of this campfire
        /// </summary>
        public Effects.Fire FireEffect { get; private set; }

        /// <summary>
        /// Turns the fire on or off
        /// </summary>
        public bool IsOn
        {
            get
            {
                return _IsOn;
            }
            set
            {
                if (value)
                {
                    Texture.AtlasFrame = 1;
                    FireEffect.Enabled = true;
                }
                else
                {
                    Texture.AtlasFrame = 0;
                    FireEffect.Enabled = false;
                }

                _IsOn = value;
            }
        }
        private bool _IsOn = false;

        public CampFire(Scenes.GameScene GameScene, PhysicEngine PhysicEngine, ParticleSystem.ParticleEngine ParticleEngine)
            : base(GameScene, PhysicEngine)
        {
            this.Body.IsStatic = true;
            this.Material = BOMaterial.Wood;
            
            this.Texture = new hTexture(Game.Content.Load<Texture2D>("Images/CampFire"), new Microsoft.Xna.Framework.Vector2(82, 37), 2);
            this.Texture.AtlasFrame = 0;

            FireEffect = new GameObjects.Effects.Fire(GameScene, ParticleEngine, 10, 5)
            {
                AttachedTo = this,
                LocalPosition = new Microsoft.Xna.Framework.Vector2(0, -10),
            };

            IsOn = false;

            this.ShapeFromTexture();
        }
    }
}
