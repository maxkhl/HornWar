using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Horn_War_II.GameObjects
{
    /// <summary>
    /// Displays health-bars of characters ingame
    /// </summary>
    class HealthbarDrawer : DrawableObject, IDrawable
    {
        GameCam _camera;
        SpriteFont _font;

        hTexture pixel;

        /// <summary>
        /// Width of the healthbars
        /// </summary>
        public float HealthbarWidth { get; set; }

        /// <summary>
        /// Height of the healthbars
        /// </summary>
        public float HealthbarHeight { get; set; }

        /// <summary>
        /// Offset of the healthbar relative to characters center
        /// </summary>
        public Vector2 HealthbarOffset { get; set; }

        /// <summary>
        /// Transparency of the healthbar
        /// </summary>
        public float HealthbarTransparency { get; set; }

        /// <summary>
        /// Transparency of the healthbars background
        /// </summary>
        public float HealthbarBackgroundTransparency { get; set; }

        public HealthbarDrawer(Scenes.GameScene GameScene, GameCam Camera)
            : base(GameScene)
        {
            _camera = Camera;
            _font = Game.Content.Load<SpriteFont>("Fonts/IngameSmall");

            pixel = new hTexture(new Texture2D(Game.GraphicsDevice, 1, 1));
            pixel.Base.SetData<Color>(new Color[1] { Color.White });

            this.DrawOrder = 99999;
            this.Visible = true;


            HealthbarWidth = 70f;
            HealthbarHeight = 10f;
            HealthbarOffset = new Vector2(-HealthbarWidth / 2, -60);
            HealthbarTransparency = 0.7f;
            HealthbarBackgroundTransparency = 0.3f;

        }



        public override void Draw(GameTime gameTime)
        {
            foreach(Character comp in Game.Components.Where(x => x is Character))
            {

                var healthbarPosition = comp.Position + HealthbarOffset;
                var healthbarBounds = new Rectangle(
                    (int)healthbarPosition.X, 
                    (int)healthbarPosition.Y, 
                    (int)(HealthbarWidth * (comp.Health / 100)), 
                    (int)HealthbarHeight);

                var healthbarBackgroundEdgeSize = 2f;
                var healthbarBackground = new Rectangle(
                    (int)(healthbarPosition.X - healthbarBackgroundEdgeSize), 
                    (int)(healthbarPosition.Y - healthbarBackgroundEdgeSize), 
                    (int)(HealthbarWidth + healthbarBackgroundEdgeSize * 2), 
                    (int)(HealthbarHeight + healthbarBackgroundEdgeSize * 2));

                // Background
                pixel.Draw(gameTime,
                       healthbarBackground,
                       Color.Black * HealthbarBackgroundTransparency,
                       0,
                       Vector2.Zero,
                       SpriteEffects.None,
                       0);

                // The bar itself
                pixel.Draw(gameTime,
                       healthbarBounds,
                       Color.Red * HealthbarTransparency,
                       0,
                       Vector2.Zero,
                       SpriteEffects.None,
                       0);

            }

            base.Draw(gameTime);
        }
    }
}
