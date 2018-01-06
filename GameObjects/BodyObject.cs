using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Common.TextureTools;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Common.Decomposition;
using Microsoft.Xna.Framework;
using Horn_War_II.GameObjects.Effects;

namespace Horn_War_II.GameObjects
{
    /// <summary>
    /// A physical gameobject
    /// </summary>
    class BodyObject : SpriteObject
    {
        /// <summary>
        /// Physical world this object is bound to
        /// </summary>
        public PhysicEngine PhysicEngine { get; private set; }

        /// <summary>
        /// Physical body of this world
        /// </summary>
        public Body Body { get; private set; }

        /// <summary>
        /// Hulls of this body for the penumbra shader
        /// </summary>
        public Penumbra.Hull[] Hulls
        {
            get { return _Hulls; }
            private set
            {
                //De-register old hulls
                if(_Hulls != null)
                    foreach(var Hull in _Hulls)
                    {
                        GameScene.PenumbraObject.Hulls.Remove(Hull);
                    }

                _Hulls = value;

                //Register new hulls
                foreach (var Hull in _Hulls)
                {
                    GameScene.PenumbraObject.Hulls.Add(Hull);
                }

            }
        }
        private Penumbra.Hull[] _Hulls;

        /// <summary>
        /// Position of the physical body
        /// </summary>
        public override Vector2 Position
        {
            get
            {
                if (this.Body != null)
                    return ConvertUnits.ToDisplayUnits(Body.Position);
                else
                    return Vector2.Zero;
            }
            set
            {
                if (this.Body != null)
                    Body.Position = ConvertUnits.ToSimUnits(value);
            }
        }

        /// <summary>
        /// Gets the forward vector.
        /// </summary>
        public Vector2 Forward
        {
            get
            {
                return Vector2.Transform(new Vector2(1,0), Matrix.CreateRotationZ(this.Rotation));
            }
        }

        /// <summary>
        /// Rotation of the physical body
        /// </summary>
        public override float Rotation
        {
            get
            {
                if (this.Body != null)
                {
                    return Body.Rotation;
                }
                else
                    return 0f;
            }
            set
            {
                if (this.Body != null)
                    Body.Rotation = Body.Rotation;
            }
        }

        /// <summary>
        /// Gets/sets the dynamic flag of this body
        /// </summary>
        public bool Dynamic
        {
            get
            {
                return Body.BodyType == BodyType.Dynamic;
            }
            set
            {
                if (value)
                    Body.BodyType = BodyType.Dynamic;
                else
                    Body.BodyType = BodyType.Static;
            }
        }

        /// <summary>
        /// Gets or sets the linear and angular damping.
        /// </summary>
        public float Damping
        {
            get
            {
                return ConvertUnits.ToDisplayUnits(this.Body.LinearDamping);
            }
            set
            {
                this.Body.LinearDamping = ConvertUnits.ToSimUnits(value);

                float AngularDampingRatio = 4.6f;
                this.Body.AngularDamping = this.Body.LinearDamping * AngularDampingRatio;
            }
        }

        /// <summary>
        /// Gets or sets the mass.
        /// </summary>
        public float Mass
        {
            get
            {
                return ConvertUnits.ToDisplayUnits(Body.Mass);
            }
            set
            {
                Body.Mass = ConvertUnits.ToSimUnits(value);
            }
        }

        /// <summary>
        /// Material, this BodyObject is made out of
        /// </summary>
        public BOMaterial Material { get; set; }

        /// <summary>
        /// Amount of Health this Object has
        /// </summary>
        public float Health
        {
            get
            {
                return this._Health;
            }
            set
            {
                bool die = false;
                if (this._Health > 0 && value <= 0)
                    die = true;

                this._Health = value;

                if (die)
                    this.Die();
            }
        }

        private float _Health = 100;

        /// <summary>
        /// Amount of Damage this Object took
        /// </summary>
        public float Damage { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="GameScene">GameScene, this module should be registered to</param>
        /// <param name="PhysicEngine">The physicengine module this object exists in</param>
        public BodyObject(Scenes.GameScene GameScene, PhysicEngine PhysicEngine)
            : base(GameScene)
        {
            this.PhysicEngine = PhysicEngine;
            Body = new Body(PhysicEngine.World, ConvertUnits.ToSimUnits(this.Position), ConvertUnits.ToSimUnits(this.Rotation), this);
            Body.CollisionCategories = Category.DynamicGOs;

            // Attach GameObject to Body
            Body.UserData = this;

            // Dynamic by default
            this.Dynamic = true;

            this.Damping = 10f;
            this.Mass = 1f;
            
            this.Material = BOMaterial.Unknown;
        }

        public override void Update(GameTime gameTime)
        {
            //Update hulls
            foreach (var Hull in Hulls)
            {
                Hull.Rotation = Body.Rotation;
                Hull.Position = ConvertUnits.ToDisplayUnits(Body.Position);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Attaches a given shape to this objects body
        /// </summary>
        /// <param name="Shape">FarseerPhysics.Collision.Shapes.Shape to attach</param>
        public Fixture AttachShape(Shape Shape)
        {
            var newFixture = this.Body.CreateFixture(Shape, this);
            newFixture.AfterCollision += AfterCollision;
            return newFixture;
        }

        /// <summary>
        /// Generates multiple shapes that form this texture and attaches them to this body. Will destroy every other fixture first!
        /// This will not work with texture atlas or animations
        /// </summary>
        /// <exception cref="Exception">Cant generate shape from texture. Texture is null</exception>
        public List<Fixture> ShapeFromTexture(TriangulationAlgorithm Algorithm = TriangulationAlgorithm.Bayazit)
        {
            if (this.Texture == null)
                throw new Exception("Cant generate shape from texture. Texture is null");

            var fixtures = new List<Fixture>();

            //Destroy all fixtures
            for (int i = 0; i < Body.FixtureList.Count; i++)
                Body.DestroyFixture(Body.FixtureList[i]);

            int texWidth = 0,
                texHeight = 0;
            
            if (Texture.IsAtlas)
            {
                texWidth = (int)Texture.AtlasTile.X;
                texHeight = (int)Texture.AtlasTile.Y;
            }
            else
            {
                texWidth = Texture.Base.Width;
                texHeight = Texture.Base.Height;
            }


            int texSize = texWidth * texHeight;
            var texData = new uint[texSize];

            if (Texture.IsAtlas)
                Texture.Base.GetData<uint>(0, Texture.GetSourceRectangle(), texData, 0, texSize);
            else
                Texture.Base.GetData<uint>(texData);

            var vertices = PolygonTools.CreatePolygon(texData, texWidth, true);
            //= TextureConverter.DetectVertices(texData, Texture.Width);

            //Scale to size
            vertices.Scale(this.Size / new Vector2(texWidth, texHeight));

            //Center shape
            vertices.Translate((this.Size / 2) * -1);

            //Scale to sim
            Vector2 scale = new Vector2(ConvertUnits.ToSimUnits(1));
            vertices.Scale(scale);

            List<Vertices> vertexList = FarseerPhysics.Common.Decomposition.Triangulate.ConvexPartition(
                vertices,
                Algorithm);

            //Vertices hull = FarseerPhysics.Common.ConvexHull.Melkman.GetConvexHull(vertexList);

            foreach (var vert in vertexList)
            {
                var newFixture = this.AttachShape(new FarseerPhysics.Collision.Shapes.PolygonShape(vert, ConvertUnits.ToSimUnits(1)));
                fixtures.Add(newFixture);
                newFixture.AfterCollision += AfterCollision;
            }

            this.Hulls = this.CreateShaderHulls(this.Body).ToArray();

            return fixtures;
        }


        /// <summary>
        /// Pushes the object with the specified force.
        /// </summary>
        public void Push(Vector2 Force)
        {
            this.Body.LinearVelocity += ConvertUnits.ToSimUnits(Force);
        }
        
        /// <summary>
        /// Rotates the object with the specified force.
        /// </summary>
        public void PushAngular(float Force)
        {
            this.Body.ApplyAngularImpulse(ConvertUnits.ToSimUnits(Force));
        }

        /// <summary>
        /// Stops the object.
        /// </summary>
        public void Stop()
        {
            this.Body.AngularVelocity = 0;
            this.Body.LinearVelocity = Vector2.Zero;
        }

        /// <summary>
        /// Fired on a collision. Override to add own collision behaviour.
        /// </summary>
        public virtual void AfterCollision(Fixture fixtureA, Fixture fixtureB, Contact contact, ContactVelocityConstraint impulse)
        {
            PhysicEngine.AfterCollision(fixtureA, fixtureB, contact, impulse);
        }


        /// <summary>
        /// Gets called when this body is hit by another
        /// </summary>
        public virtual void Hit(BodyObject Contact, Vector2 ContactPoint, bool DamagingImpact, float Damage)
        {
            if (DamagingImpact)
            {
                this.Damage += Damage;
                this.Health -= Damage;
            }

            // Invoke Hit event
            OnHit?.Invoke(Contact, DamagingImpact, Damage);

            // Play hit-effect
            if (DamagingImpact)
                HitEffect(Contact, ContactPoint, DamagingImpact, Damage);
        }

        public virtual void HitEffect(BodyObject Contact, Vector2 ContactPoint, bool DamagingImpact, float Damage)
        {
            //Our material
            switch(this.Material)
            {
                case BOMaterial.Biological:
                    switch(Contact.Material) //Being hit by
                    {
                        case BOMaterial.Metal:
                        case BOMaterial.Wood:
                        case BOMaterial.Stone:
                            new BloodHit(this.GameScene.Map.ParticleEngine, ContactPoint, ConvertUnits.ToDisplayUnits(this.Body.LinearVelocity));
                            break;
                    }
                    break;
                case BOMaterial.Metal:
                case BOMaterial.Stone:

                    switch (Contact.Material) //Being hit by
                    {
                        case BOMaterial.Metal:
                        case BOMaterial.Stone:
                            new Effects.Spark(this.GameScene.Map.ParticleEngine, ContactPoint, ConvertUnits.ToDisplayUnits(this.Body.LinearVelocity));
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// Fired, when this body got hit
        /// </summary>
        public event OnHitHandler OnHit;
        public delegate void OnHitHandler(BodyObject Contact, bool DamagingImpact, float Damage);


        /// <summary>
        /// Occurs when health drops under or at 0 - kills object
        /// </summary>
        private void Die()
        {
            //this.Dispose();
        }

        /// <summary>
        /// Applies torque to the body to rotate it towards the given angle.
        /// </summary>
        /// <param name="targetAngle">Target angle.</param>
        /// <param name="Spring">Spring constant. Higher means faster rotation.</param>
        /// <param name="Damping">Damping coefficient. Lower than 1.0 will overshoot.</param>
        protected void Rotate(float targetAngle, float Spring = 2.0f, float Damping = 1.0f)
        {
            if (Body == null) return;
            var c = (float)(-Math.Sqrt(4 * Body.Inertia * Spring) * Damping);
            var v = Body.AngularVelocity;
            var x = AngleDelta(Body.Rotation, targetAngle);
            var springForce = Spring * x;
            var dampingForce = c * v;
            this.Body.ApplyTorque(ConvertUnits.ToSimUnits(springForce + dampingForce));
        }

        /// <summary>
        /// Modulo that handles negatives properly.
        /// </summary>
        /// <param name="a">Value A.</param>
        /// <param name="b">Value B.</param>
        /// <returns></returns>
        private float Mod(double a, double b)
        {
            return (float)(((a % b) + b) % b);
        }

        /// <summary>
        /// Return the difference between angles, normalized to [-pi, pi].
        /// </summary>
        /// <param name="a">Angle A.</param>
        /// <param name="b">Angle B.</param>
        /// <returns>Delta</returns>
        private float AngleDelta(float a, float b)
        {
            var diff = b - a;
            return (float)(Mod(diff + Math.PI, Math.PI * 2) - Math.PI);
        }

        /// <summary>
        /// Material-type of a bodyobject
        /// </summary>
        public enum BOMaterial
        {
            Unknown,
            Biological,
            Metal,
            Wood,
            Stone
        }



        /// <summary>
        /// Creates a shader hull for this body to be used by penumbra lighting
        /// </summary>
        private List<Penumbra.Hull> CreateShaderHulls(Body body)
        {
            var hulls = new List<Penumbra.Hull>();

            foreach (Fixture f in body.FixtureList)
            {
                // Creating the Hull out of the Shape (respectively Vertices) of the fixtures of the physics body
                var hull = new Penumbra.Hull(((PolygonShape)f.Shape).Vertices);

                // We need to scale the Hull according to our "MetersInPixels-Simulation-Value"
                hull.Scale = new Vector2(FarseerPhysics.ConvertUnits.ToDisplayUnits(1));

                // A Hull of Penumbra is set in Display space but the physics body is set in Simulation space
                // Thats why we need to convert the simulation units of the physics object to the display units
                // of the Hull object
                hull.Position = ConvertUnits.ToDisplayUnits(body.Position);

                // We are adding the new Hull to our physics body hull list
                // This is necessary to update the Hulls in the Update method (see below)
                hulls.Add(hull);
            }

            return hulls;
        }

        /// <summary>
        /// Dispose physical body
        /// </summary>
        public override void Dispose()
        {
            PhysicEngine.World.RemoveBody(this.Body); //this will also dispose the body
            this.Body = null;
            base.Dispose();
        }
    }
}
