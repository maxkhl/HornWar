using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;

namespace Horn_War_II.GameObjects.Decoration
{
    /// <summary>
    /// A moveable mushroom object. Can be placed in a cave or wherever else you like. I don't tell you what to do.
    /// </summary>
    class Mushroom : BodyObject
    {
        /// <summary>
        /// Different types of asteroids
        /// </summary>
        public enum MushroomType
        {
            Mushroom1,
            Mushroom2,
        }

        /// <summary>
        /// Type of this Mushroom
        /// </summary>
        public MushroomType Type { get; private set; }

        /// <summary>
        /// Current joint of this mushroom
        /// </summary>
        public RevoluteJoint Joint { get; private set; }

        /// <summary>
        /// Static Axle for the joint
        /// </summary>
        private Body Axle { get; set; }

        /// <summary>
        /// Position of the mushroom (anchored to the world using a revolute joint)
        /// </summary>
        public override Microsoft.Xna.Framework.Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                //base.Position = value;

                if(Axle == null)
                {
                    Axle = BodyFactory.CreateCircle(PhysicEngine.World, ConvertUnits.ToSimUnits(0.1f), 1);
                    Axle.BodyType = BodyType.Static;
                    
                }
                Axle.Position = ConvertUnits.ToSimUnits(value);

                if (Joint != null)
                    this.PhysicEngine.World.RemoveJoint(Joint);

                Joint = JointFactory.CreateRevoluteJoint(
                    PhysicEngine.World,
                    this.Body,
                    Axle,
                    ConvertUnits.ToSimUnits(new Vector2(0, 0)));
                Joint.LocalAnchorA = ConvertUnits.ToSimUnits(new Vector2(this.Size.X / 2, this.Size.Y / 2));;
                Joint.LocalAnchorB = Vector2.Zero;

                Joint.LowerLimit = MathHelper.ToRadians(-30);
                Joint.UpperLimit = MathHelper.ToRadians(30);
                Joint.LimitEnabled = true;
            }
        }

        public Mushroom(Scenes.GameScene GameScene, PhysicEngine PhysicEngine, MushroomType Type)
            : base(GameScene, PhysicEngine)
        {
            this.Type = Type;
            this.Body.IgnoreGravity = true;

            switch(Type)
            {
                case MushroomType.Mushroom1:
                    this.Texture = new hTexture(Game.Content.Load<Texture2D>("Images/Maps/Cave/Mushroom1"));
                    break;
                case MushroomType.Mushroom2:
                    this.Texture = new hTexture(Game.Content.Load<Texture2D>("Images/Maps/Cave/Mushroom2"));
                    break;
            }

            this.ShapeFromTexture();
        }

        public override void Update(GameTime gameTime)
        {
            // This tries to keep the mushroom upright
            this.Rotate(0);

            base.Update(gameTime);
        }
    }
}
