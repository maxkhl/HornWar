using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Horn_War_II.GameObjects
{
    /// <summary>
    /// Ingame label that is shown for a specified time
    /// </summary>
    class Label : DrawableObject
    {
        /// <summary>
        /// Defines a label animation
        /// </summary>
        public enum Animation
        {
            None,
            RaiseFade,
            LowerFade
        }

        /// <summary>
        /// Text for this label
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Time in ms until the label disappears
        /// </summary>
        public float Time { get; set; }

        /// <summary>
        /// The labels position
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The labels color
        /// </summary>
        public Color Color { get; set; }

        private float _elapsedTime = 0;

        private Animation _animation;

        private SpriteFont _font;

        private float _transparency = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="Label" /> class.
        /// </summary>
        /// <param name="GameScene">Main GameScene.</param>
        /// <param name="Text">Text for this label.</param>
        /// <param name="Position">Labelposition.</param>
        /// <param name="Time">Time in ms until the label disappears.</param>
        /// <param name="Color">Fontcolor.</param>
        /// <param name="Animation">Animation for this label.</param>
        /// <param name="SpriteFont">Path to a spritefont, leave empty for default.</param>
        public Label(Scenes.GameScene GameScene, string Text, Vector2 Position, float Time, Color Color, Animation Animation = Animation.RaiseFade, string SpriteFont = "")
            : base(GameScene)
        {
            this.Text = Text;
            this.Position = Position;
            this.Time = Time;
            this._animation = Animation;
            this.Color = Color;
            if(SpriteFont == "")
                this._font = Game.Content.Load<SpriteFont>("Fonts/IngameText");
            else
                this._font = Game.Content.Load<SpriteFont>(SpriteFont);
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            switch(_animation)
            {
                case Animation.RaiseFade:
                    this.Position += new Vector2(0, -1.1f);
                    _transparency = (_elapsedTime / Time) * -1 + 1;
                    break;
                case Animation.LowerFade:
                    this.Position += new Vector2(0, 1.1f);
                    _transparency = (_elapsedTime / Time) * -1 + 1;
                    break;
            }


            //Destroy self when time over
            if (_elapsedTime >= Time)
                this.Dispose();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 Center = _font.MeasureString(Text) / 2 + Position;

            Game.SpriteBatch.DrawString(_font, Text, Center, Color * _transparency);
            base.Draw(gameTime);
        }
    }
}
