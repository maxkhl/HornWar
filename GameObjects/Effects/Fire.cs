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
        public Penumbra.Light Light { get; private set; }

        public override Vector2 Position
        {
            get => base.Position;
            set
            {
                base.Position = value;
                Light.Position = value + new Vector2(0, -20);
                _DefaultLightPosition = value + new Vector2(0, -20);
            }
        }

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

            Light = new Penumbra.PointLight()
            {
                Scale = new Vector2(Radius * Intensity * 10),
                Color = Color.Yellow,
                Position = this.Position,
                ShadowType = Penumbra.ShadowType.Occluded,
            };
            _DefaultLightScale = Light.Scale;
            _TargetLightScale = Light.Scale;
            gameScene.PenumbraObject.Lights.Add(Light);
        }
        
        private int _TargetGreen = 255;
        private Vector2 _DefaultLightScale;
        private Vector2 _TargetLightScale;

        private Vector2 _DefaultLightPosition;
        private Vector2 _TargetLightPosition;

        public override void Update(GameTime gameTime)
        {
            //Randomize Light Movement

            if (_TargetLightScale == Light.Scale)
            {
                _TargetLightScale = _DefaultLightScale + new Vector2(50) * (float)(this.Random.NextDouble() - 0.5 * 2);
            }
            else
            {
                float step = 3;
                Light.Scale += new Vector2(
                    MathHelper.Clamp(_TargetLightScale.X - Light.Scale.X, -step, step),
                    MathHelper.Clamp(_TargetLightScale.Y - Light.Scale.Y, -step, step));
            }

            if (_TargetLightPosition == Light.Position)
            {
                _TargetLightPosition = _DefaultLightPosition + new Vector2(5) * (float)(this.Random.NextDouble() - 0.5 * 2) + new Vector2(0, -20);
            }
            else
            {
                float step = 3;
                Light.Position += new Vector2(
                    MathHelper.Clamp(_TargetLightPosition.X - Light.Position.X, -step, step),
                    MathHelper.Clamp(_TargetLightPosition.Y - Light.Position.Y, -step, step));
            }


            if (_TargetGreen == Light.Color.G)
            {
                _TargetGreen = 80 + (int)(60 * this.Random.NextDouble());
            }
            else
                Light.Color = new Color(255, Light.Color.G + MathHelper.Clamp(_TargetGreen - Light.Color.G, -1, 1), 0);



            base.Update(gameTime);
        }
    }
}
