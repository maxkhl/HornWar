using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Controllers;

namespace Horn_War_II.GameObjects.Effects
{
    /// <summary>
    /// Explosion with a physical push
    /// </summary>
    class Explosion : SpriteObject
    {
        private PhysicEngine _physicEngine;
        private Shockwave _expController;

        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
                _expController.Position = ConvertUnits.ToSimUnits(value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Explosion"/> class.
        /// </summary>
        /// <param name="GameScene">Main GameScene.</param>
        /// <param name="PhysicEngine">The physic engine.</param>
        public Explosion(Scenes.GameScene GameScene, PhysicEngine PhysicEngine)
            : base(GameScene)
        {
            this._physicEngine = PhysicEngine;

            this.Texture = new hTexture(
                Game.Content.Load<Texture2D>("Images/Effects/Explosion1"), // Texture
                new Vector2(125), // Atlas tilesize
                11, // Frames
                28); // FPS

            this.Size = new Vector2(125);

            this.Texture.AnimationStopped += Texture_AnimationStopped;
            this.Texture.Play(false);


            _expController = new Shockwave(this, ConvertUnits.ToSimUnits(this.Position), 5, 1000);
            _physicEngine.World.AddController(_expController);
        }

        void Texture_AnimationStopped(hTexture sender)
        {
            _physicEngine.World.RemoveController(_expController);
            this.Dispose();
        }
    }
}
