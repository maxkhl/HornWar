using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Horn_War_II.GameObjects.ParticleSystem
{
    struct ParticleSettings
    {
        /// <summary>
        /// Spawn position of the particle
        /// </summary>
        public Vector2 StartPosition;

        /// <summary>
        /// The particles texture
        /// </summary>
        public hTexture ParticleTexture;

        public Color ParticleColor;

        public TextureDirections TextureDirection;
        public enum TextureDirections
        {
            FollowRotation,
            FollowDirection,
        }

        public string AnimationSequence;
        public bool AnimationRepeat;

        /// <summary>
        /// The angular velocity of the particle
        /// </summary>
        public float AngularVelocity;

        public bool RndAngularVelocity;
        public float RndAngularVelocityMin;
        public float RndAngularVelocityMax;

        /// <summary>
        /// The linear velocity of the particle
        /// </summary>
        public Vector2 LinearVelocity;

        public bool RndLinearVelocity;
        public Vector2 RndLinearVelocityMin;
        public Vector2 RndLinearVelocityMax;


        /// <summary>
        /// Lifetime of the particle in MS
        /// </summary>
        public float Lifetime;

        public bool RndLifetime;
        public float RndLifetimeMin;
        public float RndLifetimeMax;


        public float LinearDamping;
        public float AngularDamping;

        /// <summary>
        /// MS, the particle should reach full transparency
        /// </summary>
        public float FullTransparencyAtMS;

        /// <summary>
        /// MS, the particle should be fully transparent at
        /// </summary>
        public float NoTransparencyAtMS;

        /// <summary>
        /// DrawOrder for this particle
        /// </summary>
        public int DrawOrder;
    }
}
