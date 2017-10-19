using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using FarseerPhysics.Collision.Shapes;

namespace Horn_War_II.GameObjects
{
    /// <summary>
    /// This GameObject manages the physics engine
    /// </summary>
    [Spawn.SpawnAttribute("Images/Preview/BackgroundWorker", false)]
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
        /// Damage has to be higher than this to cause actual damage
        /// </summary>
        public float DamageBorder { get; set; }

        /// <summary>
        /// Global damagemultiplicator for factoring damage dealt to every object
        /// </summary>
        public float DamageMultiplicator { get; set; }

        /// <summary>
        /// Global damagemultiplicator for factoring damage dealt to every object by weapons
        /// </summary>
        public float DamageWeaponMultiplicator { get; set; }

        /// <summary>
        /// Alters the speed of the physics simulation (1 = normal speed)
        /// </summary>
        public float SimulationSpeedMultiplicator { get; set; }

        /// <summary>
        /// Returns all corners of every STATIC physical object in the game (used for pathfinding)
        /// </summary>
        public List<Vector2> GetCorners(List<Body> Exclude)
        {
            List<Vector2> Corners = new List<Vector2>();

            foreach (var body in this.World.BodyList)
            {
                if(body.IsStatic && !Exclude.Contains(body))
                    for (int i = 0; i < body.FixtureList.Count; i++)
                        Corners.AddRange(PointsOfFixture(body, i));
            }

            return Corners.Distinct().ToList();
        }

        public static Vector2 Rotate(Vector2 Position, Vector2 Origin, float angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new Vector2
            {
                X =
                    (int)
                    (cosTheta * (Position.X - Origin.X) -
                    sinTheta * (Position.Y - Origin.Y) + Origin.X),
                Y =
                    (int)
                    (sinTheta * (Position.X - Origin.X) +
                    cosTheta * (Position.Y - Origin.Y) + Origin.Y)
            };
        }
        public static Vector2 PositionOfFixture(Body obj, int Number)
        {
            switch (obj.FixtureList[Number].Shape.ShapeType)
            {
                case (ShapeType.Circle):
                    Vector2 pos = ((CircleShape)obj.FixtureList[Number].Shape).Position;
                    return Rotate(ConvertUnits.ToDisplayUnits(pos), obj.Position, obj.Rotation);

                case (ShapeType.Polygon):
                    PolygonShape shape = (PolygonShape)obj.FixtureList[Number].Shape;
                    List<Vector2> vecs = new List<Vector2>();
                    Vector2 total = Vector2.Zero;
                    foreach (Vector2 v in shape.Vertices)
                    {
                        vecs.Add(ConvertUnits.ToDisplayUnits(v));
                        total += vecs[vecs.Count - 1];
                    }
                    Vector2 position = total / (vecs.Count);
                    return obj.Position + Rotate(position, new Vector2(0, 0), MathHelper.ToDegrees(obj.Rotation));

            }
            throw new ArgumentException("Couldn't grab ShapeType");
        }

        public static Vector2[] PointsOfFixture(Body obj, int Number)
        {
            switch (obj.FixtureList[Number].Shape.ShapeType)
            {
                case (ShapeType.Polygon):
                    PolygonShape shape = (PolygonShape)obj.FixtureList[Number].Shape;
                    List<Vector2> vecs = new List<Vector2>();
                    Vector2 total = Vector2.Zero;
                    foreach (Vector2 v in shape.Vertices)
                    {
                        vecs.Add(Rotate(ConvertUnits.ToDisplayUnits(v), new Vector2(0, 0), MathHelper.ToDegrees(obj.Rotation)));
                        total += vecs[vecs.Count - 1];
                    }
                    return vecs.ToArray();

            }
            return new Vector2[0];
            //throw new ArgumentException("Couldn't grab ShapeType");
        }



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
            //var debugView = new Tools.DebugView(this.World);
            //debugView.Enabled = false;

            UpdateOrder = -200;

            DamageBorder = 2;
            DamageMultiplicator = 1f;
            DamageWeaponMultiplicator = 2;
            SimulationSpeedMultiplicator = 1;


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

            /*if (typeof(ParticleSystem.Particle).IsAssignableFrom(contact.FixtureA.Body.UserData.GetType()))
                ((ParticleSystem.Particle)contact.FixtureA.Body.UserData).Collision(contact.FixtureB.Body, contactPoint);


            if (typeof(ParticleSystem.Particle).IsAssignableFrom(contact.FixtureB.Body.UserData.GetType()))
                ((ParticleSystem.Particle)contact.FixtureB.Body.UserData).Collision(contact.FixtureA.Body, contactPoint);*/


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

            var DamagingStrike = Damage > DamageBorder;

            // Apply global multiplicator
            Damage *= DamageMultiplicator;

            Color Color = Color.Aquamarine;
            if(typeof(Weapon).IsAssignableFrom(Attacker.GetType()))
            {
                Damage *= DamageWeaponMultiplicator;
                Color = Color.Red;
            }
            /*else if (Damage > DamageBorder)
                new Label(GameScene, Math.Round(Damage).ToString(), Attacker.Position, 2000, Color, Label.Animation.RaiseFade, "Fonts/DamageText");

            if(Damaged != null && Damage > DamageBorder)
                new Label(GameScene, Math.Round(Damage).ToString(), Damaged.Position, 2000, Color, Label.Animation.RaiseFade, "Fonts/DamageText");*/

            

            BodyObjectA.Hit(BodyObjectB, contactPoint, Damaged == BodyObjectA && DamagingStrike, Damage);
            BodyObjectB.Hit(BodyObjectA, contactPoint, Damaged == BodyObjectB && DamagingStrike, Damage);

        }

        /// <summary>
        /// Update the world
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (Game.InputManager.IsActionPressed(InputManager.Action.TogglePhysics))
            {
                this.World.Enabled = !this.World.Enabled;
            }

            
            this.World.Step(0.033333f * SimulationSpeedMultiplicator);

            base.Update(gameTime);
        }
    }
}
