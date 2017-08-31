using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;


namespace Horn_War_II.GameObjects
{
    /// <summary>
    /// This GameObject manages the physics engine
    /// </summary>
    class PhysicEngine : GameObject
    {
        /// <summary>
        /// Gravity of this world
        /// </summary>
        public Vector2 Gravity
        {
            get
            {
                return _Gravity;
            }
            set
            {
                _Gravity = value;
                if (this.World != null)
                    this.World.Gravity = _Gravity;
            }
        }
        private Vector2 _Gravity = Vector2.Zero;

        /// <summary>
        /// Initialized physical world
        /// </summary>
        public FarseerPhysics.Dynamics.World World { get; private set; }

        private Scenes.GameScene GameScene;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Game">Game, this module should be registered to</param>
        public PhysicEngine(Scenes.GameScene GameScene)
            : base(GameScene.Game)
        {
            this.GameScene = GameScene;
            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);
            this.World = new FarseerPhysics.Dynamics.World(Gravity);
            var debugView = new Tools.DebugView(this.World);
            //debugView.

        }

        /// <summary>
        /// Called by all BodyObjects whenever a collision happens
        /// </summary>
        public void AfterCollision(Fixture fixtureA, Fixture fixtureB, Contact contact, ContactVelocityConstraint impulse)
        {
            Vector2 normal;
            FarseerPhysics.Common.FixedArray2<Vector2> WorldPoints;
            contact.GetWorldManifold(out normal, out WorldPoints);

            var contactPoint = ConvertUnits.ToDisplayUnits(WorldPoints[0]);

            if (typeof(ParticleEmitter.Particle).IsAssignableFrom(contact.FixtureA.Body.UserData.GetType()))
                ((ParticleEmitter.Particle)contact.FixtureA.Body.UserData).Collision(contact.FixtureB.Body, contactPoint);


            if (typeof(ParticleEmitter.Particle).IsAssignableFrom(contact.FixtureB.Body.UserData.GetType()))
                ((ParticleEmitter.Particle)contact.FixtureB.Body.UserData).Collision(contact.FixtureA.Body, contactPoint);


            if (!(typeof(BodyObject).IsAssignableFrom(contact.FixtureA.Body.UserData.GetType()) &&
               typeof(BodyObject).IsAssignableFrom(contact.FixtureB.Body.UserData.GetType())))
                return;

            var BodyObjectA = (BodyObject)contact.FixtureA.Body.UserData;
            var BodyObjectB = (BodyObject)contact.FixtureB.Body.UserData;

            if (BodyObjectA == null || BodyObjectB == null)
                return;



            float maxImpulse = 0f;
            for (int i = 0; i < contact.Manifold.PointCount; i++)
                maxImpulse = Math.Max(maxImpulse, contact.Manifold.Points[i].NormalImpulse);

            float Damage = maxImpulse * 300;
            BodyObject Attacker = null;
            BodyObject Damaged = null;
            if (contact.Manifold.Points[0].NormalImpulse > contact.Manifold.Points[1].NormalImpulse)
            {
                Attacker = BodyObjectA;
                Damaged = BodyObjectB;
            }
            else if (contact.Manifold.Points[0].NormalImpulse < contact.Manifold.Points[1].NormalImpulse)
            {
                Attacker = BodyObjectB;
                Damaged = BodyObjectA;
            }

            if(Attacker == null || Damaged == null)
                return;

            Color Color = Color.Aquamarine;
            if(typeof(Weapon).IsAssignableFrom(Attacker.GetType()))
            {
                Damage *= 2;
                Color = Color.Red;
            }
            else if (Damage > 2)
                new Label(GameScene, Math.Round(Damage / 20 * 1000).ToString(), Attacker.Position, 2000, Color);

            if(Damaged != null && Damage > 2)
                new Label(GameScene, Math.Round(Damage / 20 * 1000).ToString(), Damaged.Position, 2000, Color);



            BodyObjectA.Hit(BodyObjectB, Damaged == BodyObjectA && Damage > 2, Damage);
            BodyObjectB.Hit(BodyObjectA, Damaged == BodyObjectB && Damage > 2, Damage);
        }

        /// <summary>
        /// Update the world
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            this.World.Step(0.033333f);
            base.Update(gameTime);
        }
    }
}
