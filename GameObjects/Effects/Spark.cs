using Horn_War_II.GameObjects.ParticleSystem;
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
    /// Blood splatter effect
    /// </summary>
    class Spark : Particle
    {
        public Penumbra.Light Light { get; private set; }

        private Penumbra.PenumbraComponent _PenumbraObject;

        public Spark(ParticleEngine particleEngine, Vector2 Position, Vector2 Velocity) : base(particleEngine, new ParticleSettings()
        {
            ParticleTexture = new hTexture(particleEngine.Game.Content.Load<Texture2D>("Images/Spark"), new Vector2(128), 16, 25),
            Lifetime = 500,
            RndAngularVelocity = true,
            RndAngularVelocityMin = -100,
            RndAngularVelocityMax = 100,
            RndLinearVelocity = true,
            RndLinearVelocityMin = Velocity,
            RndLinearVelocityMax = Velocity,
            StartPosition = Position,
            FullTransparencyAtMS = 100,
        })
        {
            //Lets the particle become alive
            particleEngine.Spawn(this);

            Light = new Penumbra.PointLight()
            {
                Scale = new Vector2(800),
                Intensity = 0.1f,
                Radius = 60,
                Color = Color.Yellow,
            };
            _PenumbraObject = particleEngine.GameScene.PenumbraObject;
            _PenumbraObject.Lights.Add(Light);
        }

        float _LightAlive = 80;
        public override void Update(GameTime gameTime)
        {
            Light.Position = FarseerPhysics.ConvertUnits.ToDisplayUnits(this.Body.Position);

            if(_LightAlive <= 0)
                _PenumbraObject.Lights.Remove(Light);

            _LightAlive -= gameTime.ElapsedGameTime.Milliseconds;
            base.Update(gameTime);
        }

        public override void Dispose()
        {
            _PenumbraObject.Lights.Remove(Light);
            base.Dispose();
        }
    }
}
