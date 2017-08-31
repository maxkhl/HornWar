﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics;
using FarseerPhysics.Factories;

namespace Horn_War_II.GameObjects.Weapons
{
    /// <summary>
    /// Flail. Swingable
    /// </summary>
    class Flail : Weapon
    {
        public FlailHead FlailHead { get; set; }

        public Flail(Scenes.GameScene GameScene, PhysicEngine PhysicEngine)
            : base(GameScene, PhysicEngine)
        {
            this.Texture = new hTexture(Game.Content.Load<Texture2D>("Images/Flail"));
            this.ShapeFromTexture();
            this.DrawOrder = -10;
            this.Damping = 0;

            this.Mass /= 2;

            this.FlailHead = new FlailHead(GameScene, PhysicEngine);
            this.FlailHead.Attach(this);
        }

        /// <summary>
        /// Attaches the weapon to the specified character.
        /// </summary>
        /// <param name="Character">Receiver of the weapon.</param>
        public override void Attach(Character Character)
        {
            base.Attach(Character);
            AttachmentJoint = JointFactory.CreateRevoluteJoint(
                PhysicEngine.World,
                this.Body,
                Character.Body,
                ConvertUnits.ToSimUnits(new Vector2(25, 68)));
            AttachmentJoint.LocalAnchorA = ConvertUnits.ToSimUnits(new Vector2(-37, 0));
            AttachmentJoint.LocalAnchorB = ConvertUnits.ToSimUnits(new Vector2(0, 0));
            //RevoluteJoint.LimitEnabled = true;
            //RevoluteJoint.LowerLimit = MathHelper.ToRadians(-2);
            //RevoluteJoint.UpperLimit = MathHelper.ToRadians(2);

            this.FlailHead.Attach(Character);
        }

        public override void Dispose()
        {
            if (this.FlailHead != null) FlailHead.Dispose();

            base.Dispose();
        }
    }

    class FlailHead : Weapon
    {

        public FlailHead(Scenes.GameScene GameScene, PhysicEngine PhysicEngine)
            : base(GameScene, PhysicEngine)
        {
            this.Texture = new hTexture(Game.Content.Load<Texture2D>("Images/FlailHead"));
            this.ShapeFromTexture();
            this.DrawOrder = -10;
            this.Damping = 0;

            this.Mass *= 3;
        }
        public void Attach(Flail Flail)
        {
            var RopeJoint = JointFactory.CreateRopeJoint(
                PhysicEngine.World,
                this.Body,
                Flail.Body,
                ConvertUnits.ToSimUnits(new Vector2(-14, 0)),
                ConvertUnits.ToSimUnits(new Vector2(38, 0)));
            RopeJoint.MaxLength = ConvertUnits.ToSimUnits(20);
            //RevoluteJoint.LimitEnabled = true;
            //RevoluteJoint.LowerLimit = MathHelper.ToRadians(-2);
            //RevoluteJoint.UpperLimit = MathHelper.ToRadians(2);
        }
    }
}
