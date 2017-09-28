using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics;
using FarseerPhysics.Factories;

namespace Horn_War_II.GameObjects
{    
    /// <summary>
    /// A weapon gameobject that can be attached to a player
    /// </summary>
    abstract class Weapon : BodyObject
    {
        /// <summary>
        /// Contains the character that is currently holding this weapon (null if none)
        /// </summary>
        public Character Holder
        { get; private set; }

        public FarseerPhysics.Dynamics.Joints.RevoluteJoint AttachmentJoint { get; protected set; }

        public bool IsFixed
        {
            get
            {
                return AttachmentJoint.LimitEnabled;
            }
            set
            {
                AttachmentJoint.LimitEnabled = value;
                if (value && Holder != null)
                    this.Rotation = Holder.Rotation;
                AttachmentJoint.LowerLimit = MathHelper.ToRadians(-2);
                AttachmentJoint.UpperLimit = MathHelper.ToRadians(2);
            }
        }

        public Weapon(Scenes.GameScene GameScene, PhysicEngine PhysicEngine)
            : base(GameScene, PhysicEngine)
        {
        }

        /// <summary>
        /// Attaches the weapon to the specified character.
        /// </summary>
        /// <param name="Character">Receiver of the weapon.</param>
        public virtual void Attach(Character Character)
        {
            Holder = Character;
            Holder.Weapon = this;

            Holder.Body.RemoveDontCollideWith(this.Body);
        }

        /// <summary>
        /// Try to detach this weapon
        /// </summary>
        public bool TryDetach { get; private set; }

        /// <summary>
        /// Detaches the weapon from the current holder.
        /// </summary>
        public virtual void Detach()
        {
			if(Holder != null)
			{
                Holder.Body.DontCollideWith(this.Body);

                //Clear holder
                Holder.Weapon = null;
                Holder = null;

                //Destroy joint
                this.PhysicEngine.World.RemoveJoint(AttachmentJoint);
				AttachmentJoint = null;
                
			}
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public delegate void onHitCharacterHandler(Character Target);
        public event onHitCharacterHandler onHitCharacter;

        public override void Hit(BodyObject Contact, bool DamagingImpact, float Damage)
        {
            if(DamagingImpact && typeof(Character).IsAssignableFrom(Contact.GetType()))
            {
                if (onHitCharacter != null)
                    onHitCharacter((Character)Contact);
            }

            base.Hit(Contact, DamagingImpact, Damage);
        }
    }
}
