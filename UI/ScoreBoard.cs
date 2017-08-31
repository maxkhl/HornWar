using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Horn_War_II.UI
{
    class ScoreBoard : UIContainer
    {
        GameObjects.Character Char1;
        GameObjects.Character Char2;
        UiLabel Score1;
        UiLabel Score2;
        public ScoreBoard(Scenes.GameScene GameScene, GameObjects.PhysicEngine PhysicEngine, GameObjects.Character Char1, GameObjects.Character Char2)
            : base(GameScene, PhysicEngine)
        {
            var FontCaption = Game.Content.Load<SpriteFont>("Fonts/IngameCaption");
            var FontText = Game.Content.Load<SpriteFont>("Fonts/IngameText");

            this.Char1 = Char1;
            this.Char2 = Char2;
            
            Score1 = new UiLabel(GameScene, PhysicEngine, "0") { Position = new Vector2(0, 22), Font = FontText };
            Score2 = new UiLabel(GameScene, PhysicEngine, "0") { Position = new Vector2(0, 82), Font = FontText };

            this.Children.AddRange(new UiObject[] {
                new UiLabel(GameScene, PhysicEngine, "Player 1") { Position = new Vector2(0,0), Font = FontCaption },
                Score1,
                new UiLabel(GameScene, PhysicEngine, "NPC 1") { Position = new Vector2(0,60), Font = FontCaption },
                Score2,
            });

            this.Position = new Vector2(0,0);
            this.Overlay = true;
        }

        public override void Update(GameTime gameTime)
        {
            Score1.Text = Char2.Damage.ToString();
            Score2.Text = Char1.Damage.ToString();

            
            base.Update(gameTime);
        }
    }
}
