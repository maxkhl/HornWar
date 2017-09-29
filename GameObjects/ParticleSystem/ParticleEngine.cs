using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Horn_War_II.GameObjects.ParticleSystem
{
    /// <summary>
    /// Takes care of handling and drawing particles
    /// </summary>
    class ParticleEngine : DrawableObject
    {
        /// <summary>
        /// A list of all living particles.
        /// </summary>
        public List<Particle> Particles { get; private set; }

        /// <summary>
        /// Provides access to the physicengine for the particles
        /// </summary>
        public PhysicEngine PhysicEngine { get; private set; }

        /// <summary>
        /// Provides random numbers for the particles
        /// </summary>
        public Random Random { get; private set; }

        public ParticleEngine(Scenes.GameScene gameScene, PhysicEngine PhysicEngine)
            : base(gameScene)
        {
            this.PhysicEngine = PhysicEngine;
            Particles = new List<Particle>();
            Random = new Random();
            this.DrawOrder = 200;
        }

        /// <summary>
        /// Spawns a new particle on this engine
        /// </summary>
        public void Spawn(Particle newParticle)
        {
            Particles.Add(newParticle);
        }

        public void Remove(Particle oldParticle)
        {
            Particles.Remove(oldParticle);
        }


        /// <summary>
        /// Removes all particles that are alive right now.
        /// </summary>
        public void Clear()
        {
            Particles.Clear();
        }

        /// <summary>
        /// Updates all living particles
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < Particles.Count; i++)
            {
                Particles[i].Update(gameTime);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws all living particles
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            for (int i = 0; i < Particles.Count; i++)
                Particles[i].Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
