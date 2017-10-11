using Horn_War_II.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horn_War_II.GameObjects.Effects
{
    /// <summary>
    /// Dust effect (like dust coming from the ceiling)
    /// Use AttachTo to attach it to a gameObject
    /// </summary>
    class Dust : ParticleSystem.Emitter
    {
        /// <summary>
        /// Creates a new dust-effect
        /// </summary>
        public Dust(GameScene gameScene, ParticleSystem.ParticleEngine particleEngine) : base(gameScene, particleEngine)
        {
            //var pa = new GameObjects.ParticleSystem.Emitter(this, Map.ParticleEngine);
            ParticleDefaultSettings = new GameObjects.ParticleSystem.ParticleSettings()
            {
                ParticleTexture = new hTexture(Game.Content.Load<Texture2D>("Images/Effects/Dust"), new Vector2(64), 24, 15)
                {
                    AtlasRotation = MathHelper.ToRadians(-90),
                },
                Lifetime = 3000,
                RndLinearVelocity = true,
                RndLinearVelocityMin = new Vector2(3, 120),
                RndLinearVelocityMax = new Vector2(-3, 120),
                RndAngularVelocity = true,
                RndAngularVelocityMin = 0,
                RndAngularVelocityMax = 100,

                TextureDirection = GameObjects.ParticleSystem.ParticleSettings.TextureDirections.FollowDirection,
                FullTransparencyAtMS = 0,
                NoTransparencyAtMS = 2500,
            };
            
            EmissionMin = 0;
            EmissionMax = 25;
            EmissionRadius = 2;
            Emission = true;
        }
    }
}
