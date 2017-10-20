using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using FarseerPhysics;

namespace Horn_War_II.GameObjects.ParticleSystem
{
    /// <summary>
    /// Emits particles regularly to a specified engine
    /// </summary>
    class Emitter : GameObject
    {

        /// <summary>
        /// Gets or sets settings for newly spawned particles
        /// </summary>
        public ParticleSettings ParticleDefaultSettings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Emitter"/> is automaticaly emitting particles.
        /// </summary>
        public bool Emission { get; set; }

        /// <summary>
        /// Gets or sets the minimum amount of particles to emit per second.
        /// </summary>
        public int EmissionMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of particles to emit per second.
        /// </summary>
        public int EmissionMax { get; set; }

        /// <summary>
        /// Maximum radius, particles are allowed to be emitted
        /// </summary>
        public float EmissionRadius { get; set; }

        /// <summary>
        /// Random instance used by this emitter and all of its particles
        /// </summary>
        public Random Random { get; private set; }

        /// <summary>
        /// Lifetime this emitter will be alive for. Disposes itself once it reaches zero. Value is ms. Negative means infinite
        /// </summary>
        public float EmitterLifetime
        {
            get
            {
                return _EmitterLifetime;
            }
            set
            {
                if (_EmitterLifetime > 0 && value <= 0)
                    this.Dispose();

                _EmitterLifetime = value;
            }
        }
        private float _EmitterLifetime = -1;

        public ParticleEngine ParticleEngine { get; private set; }

        /// <summary>
        /// Position that specifies where particles spawn
        /// </summary>
        public Vector2 Position { get; set; }

        public SpriteObject AttachedTo { get; set; }
        public Vector2 LocalPosition { get; set; }

        public Emitter(Scenes.GameScene GameScene, ParticleEngine ParticleEngine) : base(GameScene.Game)
        {
            this.ParticleEngine = ParticleEngine;
            Random = new Random();
        }


        private float SecondTick = 0;
        private int EmissionTick = 0;
        public override void Update(GameTime gameTime)
        {
            // Count down lifetime. Will dispose itself when 0 is reached
            if (EmitterLifetime > 0)
                EmitterLifetime -= gameTime.ElapsedGameTime.Milliseconds;

            this.Position = LocalPosition;
            if (AttachedTo != null)
                this.Position += AttachedTo.Position;

            if (this.Emission)
            {
                var EmissionsPerSecond = EmissionMax - EmissionMin;
                if (EmissionsPerSecond < 0)
                    throw new Exception("Emitters minimum emission is bigger than its maximum emission value");

                SecondTick += gameTime.ElapsedGameTime.Milliseconds;

                var EmissionsThatShouldHaveHappenedTillNowBitch = (int)(SecondTick / 1000 * EmissionsPerSecond);

                var MissingEmissions = EmissionsThatShouldHaveHappenedTillNowBitch - EmissionTick;

                if(MissingEmissions > 0)
                {
                    Emit(MissingEmissions);
                    EmissionTick += MissingEmissions;
                }

                var settings = this.ParticleDefaultSettings;
                settings.StartPosition = this.Position;
                this.ParticleDefaultSettings = settings;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Emits a specified amount of particles.
        /// </summary>
        /// <param name="Amount">The amount of particles to emit.</param>
        public void Emit(int Amount)
        {
            for(int i = 0; i < Amount; i++)
            {
                var settings = this.ParticleDefaultSettings;
                settings.StartPosition
                    = this.ParticleDefaultSettings.StartPosition + new Vector2((float)ParticleEngine.Random.NextDouble() * 2 - 1, (float)ParticleEngine.Random.NextDouble() * 2 - 1) * EmissionRadius;
                
                ParticleEngine.Spawn(new Particle(ParticleEngine, settings));
            }
        }

    }
}
