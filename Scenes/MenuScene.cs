using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Horn_War_II.Scenes
{
    class MenuScene : GameScene
    {
        public MenuScene() : base(GameSceneMap.None)
        { }

        public SpriteFont DefaultFont { get; set; }
        public SpriteFont CaptionFont { get; set; }

        public override void LoadGame()
        {
            DefaultFont = Game.Content.Load<SpriteFont>("MenuFont");
            CaptionFont = Game.Content.Load<SpriteFont>("MenuCaption");
            this.Map = new Maps.MainMenu(this);
            // The base would load ingame stuff. We dont want that here
            //base.LoadGame();
        }

        public override void Update(GameTime gameTime)
        {
            Game.InputManager.IsActionPressed(InputManager.Action.Escape);
            base.Update(gameTime);
        }
    }
}
