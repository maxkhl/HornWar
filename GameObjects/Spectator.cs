using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Horn_War_II.GameObjects
{
    /// <summary>
    /// Spectator class - basically something to control spectating and a object, the camera can follow around
    /// </summary>
    class Spectator : SpriteObject
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Spectator(Scenes.GameScene GameScene) 
            : base(GameScene)
        {
            Game.InputManager.OnMWScroll += InputManager_OnMWScroll;
        }

        private void InputManager_OnMWScroll(bool Up)
        {
            if(Up)
                this.GameScene.Map.Camera.ZoomAnimation.Animate(0.3f * this.GameScene.Map.Camera.Zoom, 200, Tools.Easing.EaseFunction.CubicEaseOut);
            else
                this.GameScene.Map.Camera.ZoomAnimation.Animate(-0.3f * this.GameScene.Map.Camera.Zoom, 200, Tools.Easing.EaseFunction.CubicEaseOut);
        }

        public override void Update(GameTime gameTime)
        {
            if (Game.InputManager.MousePressed)
                this.Position = this.GameScene.Map.Camera.ToWorld(Game.InputManager.MousePosition);

            if (Game.InputManager.IsActionDown(InputManager.Action.Up))
                this.Position += new Vector2(0, -1);

            if (Game.InputManager.IsActionDown(InputManager.Action.Down))
                this.Position += new Vector2(0, 1);

            if (Game.InputManager.IsActionDown(InputManager.Action.Left))
                this.Position += new Vector2(-1, 0);

            if (Game.InputManager.IsActionDown(InputManager.Action.Right))
                this.Position += new Vector2(1, 0);



            base.Update(gameTime);
        }

        public override void Dispose()
        {
            Game.InputManager.OnMWScroll -= InputManager_OnMWScroll;
            base.Dispose();
        }
    }
}
