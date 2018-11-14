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
    /// Lightning effect
    /// </summary>
    class Lightning : Particle
    {
        public Penumbra.Light Light { get; private set; }

        private Penumbra.PenumbraComponent _PenumbraObject;

        public Lightning(ParticleEngine particleEngine, Vector2 Position) : base(particleEngine, new ParticleSettings()
        {
            ParticleTexture = new hTexture(particleEngine.Game.Content.Load<Texture2D>("Images/Effects/Lightning"), new Vector2(512), 4, 30),
            Lifetime = 500,
            StartPosition = Position,
            FullTransparencyAtMS = 10,
            NoTransparencyAtMS = 110
        })
        {
            //this.Texture.Play(false);

            //Lets the particle become alive
            particleEngine.Spawn(this);

            Light = new Penumbra.PointLight()
            {
                Scale = new Vector2(1800),
                Intensity = 0.7f,
                Radius = 600,
                Color = Color.White,
            };
            _PenumbraObject = particleEngine.GameScene.PenumbraObject;
            _PenumbraObject.Lights.Add(Light);
        }

        float _LightAlive = 134;
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
