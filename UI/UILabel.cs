using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Horn_War_II.UI
{
    class UiLabel : UiObject
    {
        /// <summary>
        /// Displaytext
        /// </summary>
        public string Text 
        { 
            get
            {
                return _Text;
            }
            set
            {
                if (value != null && value != _Text)
                    _TextSize = Font.MeasureString(value);
                _Text = value;
            }
        }
        private string _Text;
        private Vector2 _TextSize;

        /// <summary>
        /// Gets or sets a value indicating whether the text is centered.
        /// </summary>
        public bool Centered { get; set; }

        /// <summary>
        /// Gets or sets the color of the font.
        /// </summary>
        public Color FontColor { get; set; }

        public SpriteFont Font
        {
            get
            {
                if (_Font == null)
                    return GameScene.DefaultFont;
                else
                    return _Font;
            }
            set
            {
                _Font = value;
                this.Text = this.Text;
            }
        }
        private SpriteFont _Font = null;

        public UiLabel(Scenes.GameScene GameScene, GameObjects.PhysicEngine PhysicEngine, string Text)
            : base(GameScene, PhysicEngine)
        {
            this.FontColor = Color.White;
            this.Text = Text;
        }

        public UiLabel(Scenes.GameScene GameScene, GameObjects.PhysicEngine PhysicEngine, string Text, SpriteFont Font)
            : base(GameScene, PhysicEngine)
        {
            this.FontColor = Color.White;
            this.Font = Font;
            this.Text = Text;
        }


        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //Draw Text after baseDraw so Texture attribute can serve as a background for the label (f.e. used for buttons)
            base.Draw(gameTime);
            Game.SpriteBatch.DrawString(Font, Text, this.Position - (Centered ? this._TextSize / 2 : Vector2.Zero), this.FontColor);
        }
    }
}
