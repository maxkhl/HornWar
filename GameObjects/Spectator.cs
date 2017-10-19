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

        //Random random = new Random();
        public override void DebugDraw(GameTime gameTime, hTexture Pixel, DebugDrawer debugDrawer)
        {
            /*float max = 500,
                  min = -500;

            for (int i = 0; i < 5; i++)
            {
                var target = new Vector2(
                    this.Position.X + (float)random.NextDouble() * (max - min) + min,
                    this.Position.Y + (float)random.NextDouble() * (max - min) + min);

                bool hit = false;
                Game.GetComponent<PhysicEngine>().World.RayCast((f, p, n, fr) =>
                    {
                        hit = true;
                        return 0;
                    },
                    FarseerPhysics.ConvertUnits.ToSimUnits(target),
                    FarseerPhysics.ConvertUnits.ToSimUnits(Position));

                var color = !hit ? Color.White : Color.Red;

                var forw = target - this.Position;
                forw.Normalize();
                Pixel.Draw(gameTime,
                    new Rectangle(
                        (int)this.Position.X,
                        (int)this.Position.Y,
                        (int)(target - this.Position).Length(),
                        2),
                    color,
                    (float)Math.Atan2(forw.Y, forw.X),
                    Vector2.Zero,
                    SpriteEffects.None,
                    0);

            }*/
            base.DebugDraw(gameTime, Pixel, debugDrawer);
        }

        public override void Dispose()
        {
            Game.InputManager.OnMWScroll -= InputManager_OnMWScroll;
            base.Dispose();
        }
    }
}
