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
    /// Fire effect to make something burn
    /// Use AttachTo to attach it to a gameObject
    /// </summary>
    class Fire : ParticleSystem.Emitter
    {
        /// <summary>
        /// Creates a new fire-effect
        /// </summary>
        /// <param name="Intensity">Intensity of the fire, corresponds with hardware usage</param>
        /// <param name="Radius">Radius of the fire, turn this up if you want to cover a bigger object </param>
        public Fire(GameScene gameScene, ParticleSystem.ParticleEngine particleEngine, int Intensity = 20, float Radius = 5) : base(gameScene, particleEngine)
        {
            //var pa = new GameObjects.ParticleSystem.Emitter(this, Map.ParticleEngine);
            ParticleDefaultSettings = new GameObjects.ParticleSystem.ParticleSettings()
            {
                ParticleTexture = new hTexture(Game.Content.Load<Texture2D>("Images/fire"), new Vector2(125), 11, 15),
                Lifetime = 2000,
                RndLinearVelocity = true,
                RndLinearVelocityMin = new Vector2(30, -70),
                RndLinearVelocityMax = new Vector2(-30, -30),
                RndAngularVelocity = true,
                RndAngularVelocityMin = 0,
                RndAngularVelocityMax = 100,
                ParticleColor = Color.White * 0.8f,

                TextureDirection = GameObjects.ParticleSystem.ParticleSettings.TextureDirections.FollowRotation,
                FullTransparencyAtMS = 250,
                NoTransparencyAtMS = 1500,
                DrawOrder = 200,
            };

            EmissionMin = 0;
            EmissionMax = Intensity;
            EmissionRadius = Radius;
            Emission = true;
        }
    }
}
