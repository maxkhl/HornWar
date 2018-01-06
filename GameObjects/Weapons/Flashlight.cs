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
    /// Flashlight 
    /// </summary>
    class Flashlight : Weapon
    {

        Penumbra.Spotlight _Light;

        public Flashlight(Scenes.GameScene GameScene, PhysicEngine PhysicEngine)
            : base(GameScene, PhysicEngine)
        {
            this.Texture = new hTexture(Game.Content.Load<Texture2D>("Images/Flashlight"));
            this.ShapeFromTexture();
            this.DrawOrder = -10;
            this.Material = BOMaterial.Metal;
            this.Mass = 0.01f;

            _Light = new Penumbra.Spotlight();
            GameScene.PenumbraObject.Lights.Add(_Light);

            _Light.Scale = Vector2.One * 580;
            _Light.Color = Color.LightYellow;

            /*emitter = new ParticleEmitter(GameScene, PhysicEngine);
            emitter.AttachedTo = this;
            emitter.EmissionMax = 2;
            emitter.Emission = true;
            emitter.ParticleDefaultSettings = new ParticleEmitter.ParticleSettings()
            {
                ParticleTexture = new hTexture(Game.Content.Load<Texture2D>("Images/Effects/Explosion1"), new Vector2(125), 11, 28),
                AnimationRepeat = true,
                ParticleColor = Color.White,
                Lifetime = 10000,
                TextureDirection = ParticleEmitter.ParticleSettings.TextureDirections.FollowDirection,
            };*/
        }
        //ParticleEmitter emitter;

        public override void Update(GameTime gameTime)
        {
            _Light.Position = this.Position + Vector2.Transform(Vector2.UnitX * 50, Matrix.CreateRotationZ(this.Rotation));


            _Light.Rotation = this.Rotation;
            base.Update(gameTime);
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
            AttachmentJoint.LocalAnchorA = ConvertUnits.ToSimUnits(new Vector2(-35, 0));
            AttachmentJoint.LocalAnchorB = ConvertUnits.ToSimUnits(new Vector2(0, 0));
            this.IsFixed = true;

            this.DrawOrder = Character.DrawOrder - 1;

            // Move to weapon to the character to make sure its not stuck anywhere
            this.Position = Character.Position;
        }
    }
}
