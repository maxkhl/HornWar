using System;
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
    /// Sword. Rotateable
    /// </summary>
    class Sword : Weapon
    {

        public Sword(Scenes.GameScene GameScene, PhysicEngine PhysicEngine)
            : base(GameScene, PhysicEngine)
        {
            this.Texture = new hTexture(Game.Content.Load<Texture2D>("Images/Sword"));
            this.ShapeFromTexture();
            this.DrawOrder = -10;
            this.Damping = 0;
            this.Material = BOMaterial.Metal;

            this.Mass /= 2;
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

        }
    }
}
