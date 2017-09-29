using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Horn_War_II.UI
{
    class UiButton : UiLabel
    {
        public enum Backgrounds
        {
            Wood1 = 0,
            Wood2 = 1,
            Wood3 = 2,
            Wood4 = 3
        }

        public Backgrounds Background
        {
            get
            {
                return (Backgrounds)Texture.AtlasFrame;
            }
            set
            {
                Texture.AtlasFrame = (int)value;
            }
        }

        GameObjects.Tools.Animation posAnim;

        public UiButton(Scenes.GameScene GameScene, GameObjects.PhysicEngine PhysicEngine, string Text)
            : base(GameScene, PhysicEngine, Text)
        {
            this.Centered = true;

            this.Texture = new hTexture(
                GameScene.Game.Content.Load<Texture2D>("Images/Menu/MainMenuButtons"), 
                new Vector2(277, 57), 
                4);
            this.Texture.AtlasFrame = 0;

            this.OnMouseOver += UiButton_OnMouseOver;

            posAnim = new GameObjects.Tools.Animation(this.GetType().GetProperty("Position"), this);
            //posAnim.Animate(new Vector2(100, 300), 20000, GameObjects.Tools.Easing.EaseFunction.CircEaseOut);
            //this.ShapeFromTexture();
        }

        public override void Update(GameTime gameTime)
        {
            posAnim.Update(gameTime);
            this.Color = Color.White;
            base.Update(gameTime);
        }

        void UiButton_OnMouseOver(UiObject sender)
        {
            this.Color = Color.LightYellow;
        }
    }
}
