using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Horn_War_II.GameObjects
{
    class DebugDrawer : DrawableObject, IDrawable
    {
        Tools.DebugView _physicsDebug;
        GameCam _camera;
        SpriteFont _debugFont;

        public DebugDrawer(Scenes.GameScene GameScene, FarseerPhysics.Dynamics.World World, GameCam Camera)
            : base(GameScene)
        {
            _camera = Camera;
            _debugFont = Game.Content.Load<SpriteFont>("DebugFont");
            _physicsDebug = new Tools.DebugView(World);
            _physicsDebug.LoadContent(Game.GraphicsDevice, Game.Content);
            _physicsDebug.AppendFlags(FarseerPhysics.DebugViewFlags.Shape);
            _physicsDebug.AppendFlags(FarseerPhysics.DebugViewFlags.PolygonPoints);

            this.DrawOrder = 99999;

            this.Visible = false;
#if DEBUG
            this.Visible = true;
#endif
        }

        /// <summary>
        /// GameObject Draw
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            foreach(var comp in Game.Components)
            {
                if (typeof(SpriteObject).IsAssignableFrom(comp.GetType()))
                {
                    var drawComp = (SpriteObject)comp;
                    Game.SpriteBatch.DrawString(_debugFont, drawComp.Position.ToString(), drawComp.Position + drawComp.Size, Color.Red);

                    if (typeof(BodyObject).IsAssignableFrom(comp.GetType()))
                    {
                        Game.SpriteBatch.DrawString(_debugFont, ((BodyObject)drawComp).Body.LinearVelocity.ToString(), drawComp.Position + drawComp.Size + new Vector2(0, 12), Color.Lime);
                        Game.SpriteBatch.DrawString(_debugFont, MathHelper.ToDegrees(((BodyObject)drawComp).Body.Rotation).ToString(), drawComp.Position + drawComp.Size + new Vector2(0, 24), Color.Yellow);
                    }
                    
                }
            }
            Matrix proj = Matrix.CreateOrthographicOffCenter(0f, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height, 0f, 0f, 1f);
            _physicsDebug.RenderDebugData(proj, _camera.View);
        }
    }
}
