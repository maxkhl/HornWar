using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using FarseerPhysics;
using Microsoft.Xna.Framework.Graphics;

namespace Horn_War_II.GameObjects.ParticleSystem
{
    /// <summary>
    /// A single particle spawned by the emitter
    /// </summary>
    class Particle : IDisposable
    {
        public ParticleEngine Engine { get; set; }
        public FarseerPhysics.Dynamics.Body Body { get; private set; }
        public ParticleSettings Settings { get; private set; }

        public float TotalLifetime { get; set; }
        public float Lifetime { get; set; }

        public hTexture Texture { get; set; }

        public Color Color { get; set; }

        private ParticleSettings.TextureDirections TextureDirection { get; set; }

        public Particle(ParticleEngine Engine, ParticleSettings Settings)
        {
            this.Settings = Settings;
            this.TextureDirection = Settings.TextureDirection;


            this.Engine = Engine;




            if (Settings.ParticleTexture != null && Settings.ParticleTexture.IsAnimation)
            {
                this.Texture = Settings.ParticleTexture.Copy(); // Animations need to be copied in order to get individual animations
                if (Settings.AnimationSequence == null)
                    this.Texture.Play(Settings.AnimationRepeat);
                else
                    this.Texture.Play(Settings.AnimationRepeat, Settings.AnimationSequence);
            }
            else
            {
                this.Texture = Settings.ParticleTexture;
            }



            this.Color = Settings.ParticleColor;
            if (this.Color == Color.Transparent)
                this.Color = Color.White;

            if (Settings.RndLifetime)
                TotalLifetime =
                    ((float)Engine.Random.NextDouble() * (Settings.RndLifetimeMax - Settings.RndLifetimeMin)) + Settings.RndLifetimeMin;
            else
                TotalLifetime = Settings.Lifetime;

            this.Lifetime = 0;

            this.Body = BodyFactory.CreateRectangle(
                Engine.PhysicEngine.World,
                ConvertUnits.ToSimUnits(0.5f),
                ConvertUnits.ToSimUnits(0.5f),
                ConvertUnits.ToSimUnits(1f),
                this);

            this.Body.IsStatic = false;
            //this.Body.CollidesWith = FarseerPhysics.Dynamics.Category.StaticGOs;
            this.Body.CollidesWith = FarseerPhysics.Dynamics.Category.None;

            // If emitter attached to physical object
            //if (Emitter.AttachedTo != null && typeof(BodyObject).IsAssignableFrom(Emitter.AttachedTo.GetType()))
            //    this.Body.IgnoreCollisionWith(((BodyObject)Emitter.AttachedTo).Body);

            this.Body.CollisionCategories = FarseerPhysics.Dynamics.Category.Particles;

            this.Body.Restitution = 0.1f;
            this.Body.Friction = 0.1f;
            this.Body.Position = ConvertUnits.ToSimUnits(Settings.StartPosition);

            this.Body.LinearDamping = Settings.LinearDamping;
            this.Body.AngularDamping = Settings.AngularDamping;

            if (Settings.RndAngularVelocity)
                this.Body.AngularVelocity =
                    ConvertUnits.ToSimUnits(((float)Engine.Random.NextDouble() * (Settings.RndAngularVelocityMax - Settings.RndAngularVelocityMin)) + Settings.RndAngularVelocityMin);
            else
                this.Body.AngularVelocity = Settings.AngularVelocity;

            if (Settings.RndLinearVelocity)
                this.Body.LinearVelocity = ConvertUnits.ToSimUnits(new Vector2(
                    ((float)Engine.Random.NextDouble() * (Settings.RndLinearVelocityMax.X - Settings.RndLinearVelocityMin.X)) + Settings.RndLinearVelocityMin.X,
                    ((float)Engine.Random.NextDouble() * (Settings.RndLinearVelocityMax.Y - Settings.RndLinearVelocityMin.Y)) + Settings.RndLinearVelocityMin.Y));
            else
                this.Body.LinearVelocity = Settings.LinearVelocity;

            if (Settings.FullTransparencyAtMS != 0)
                Transparency = 0f;
        }

        private float Transparency = 1f;

        public void Update(GameTime gameTime)
        {
            this.Lifetime += gameTime.ElapsedGameTime.Milliseconds;

            if (Settings.FullTransparencyAtMS != 0)
                Transparency = MathHelper.Clamp(this.Lifetime / Settings.FullTransparencyAtMS, 0, 1);

            if (Settings.NoTransparencyAtMS != 0 && this.Lifetime > Settings.NoTransparencyAtMS)
                Transparency = MathHelper.Clamp(((this.Lifetime - Settings.NoTransparencyAtMS) / (this.TotalLifetime - Settings.NoTransparencyAtMS) - 1) * -1, 0, 1);

            if (this.Lifetime > this.TotalLifetime)
                this.Dispose();
        }

        public void Draw(GameTime gameTime)
        {
            if (this.Texture == null) return;

            var position = ConvertUnits.ToDisplayUnits(this.Body.Position);
            //position.X += this.Texture.Width / 4;
            //position.Y -= this.Texture.Height / 4;

            this.Texture.Draw(gameTime,
                new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    this.Texture.Width,
                    this.Texture.Height),
                this.Color * Transparency,
                TextureDirection == ParticleSettings.TextureDirections.FollowDirection ?
                (float)Math.Atan2(this.Body.LinearVelocity.Y, this.Body.LinearVelocity.X) :
                this.Body.Rotation,
                new Vector2(this.Texture.Width, this.Texture.Height) / 2,
                SpriteEffects.None, 99999);
        }

        public void Collision(FarseerPhysics.Dynamics.Body Object, Vector2 Position)
        {
            this.Dispose();
        }

        public bool Disposing { get; private set; }
        public void Dispose()
        {
            this.Body.Dispose();
            this.Engine.Remove(this);
            this.Texture.Dispose(false);
            this.Disposing = true;
        }
    }
}
