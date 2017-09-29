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
    class BloodHit : Particle
    {
        public BloodHit(ParticleEngine particleEngine, Vector2 Position, Vector2 Velocity) : base(particleEngine, new ParticleSettings()
        {
            ParticleTexture = new hTexture(particleEngine.Game.Content.Load<Texture2D>("Images/blood_hit"), new Vector2(128), 16, 25),
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
        }
    }
}
