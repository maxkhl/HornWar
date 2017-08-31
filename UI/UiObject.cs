using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Horn_War_II.UI
{    
    /// <summary>
    /// GameObjects used for the UI framework. Nais
    /// </summary>
    class UiObject : GameObjects.StaticObject
    {
        public Scenes.GameScene GameScene { get; set; }
        public UiObject(Scenes.GameScene GameScene, GameObjects.PhysicEngine PhysicEngine)
            : base(GameScene, PhysicEngine)
        {
            this.GameScene = GameScene;
        }

        public delegate void MouseOverHandler(UiObject sender);
        public delegate void MouseClickHandler(UiObject sender);

        /// <summary>
        /// Occurs every frame the mouse hovers over this object.
        /// </summary>
        public event MouseOverHandler OnMouseOver;

        /// <summary>
        /// Occurs every time the mouse is clicked over this object.
        /// </summary>
        public event MouseOverHandler OnMouseClick;

        public override void Update(GameTime gameTime)
        {
            if(Game.InputManager.MouseActive)
            {
                if (OnMouseOver != null || OnMouseClick != null)
                {
                    var mousePos = GameScene.Map.Camera.ToWorld(Game.InputManager.MousePosition);
                    var surface = new Rectangle(
                                      (int)(Position.X - Size.X / 2), (int)(Position.Y - Size.Y / 2),
                                      (int)Size.X, (int)Size.Y);
                    if ( ( OnMouseOver != null || OnMouseClick != null ) && 
                        surface.Contains(mousePos))
                    {
                        if (OnMouseOver != null)
                            OnMouseOver(this);

                        if (OnMouseClick != null && Game.InputManager.MouseClicked)
                            OnMouseClick(this);
                    }

                }

            }
            base.Update(gameTime);
        }

    }
}
