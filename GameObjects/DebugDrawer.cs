using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Horn_War_II.GameObjects
{
    /// <summary>
    /// Used to debug the physics engine and other stuff
    /// </summary>
    [Spawn.SpawnAttribute("Images/Preview/BackgroundWorker", false)]
    class DebugDrawer : DrawableObject, IDrawable
    {
        Tools.DebugView _physicsDebug;
        GameCam _camera;
        SpriteFont _debugFont;

        hTexture Pixel;

        public DebugDrawer(Scenes.GameScene GameScene, FarseerPhysics.Dynamics.World World, GameCam Camera)
            : base(GameScene)
        {
            _camera = Camera;
            _debugFont = Game.Content.Load<SpriteFont>("DebugFont");
            _physicsDebug = new Tools.DebugView(World);
            _physicsDebug.LoadContent(Game.GraphicsDevice, Game.Content);
            _physicsDebug.AppendFlags(FarseerPhysics.DebugViewFlags.Shape);
            _physicsDebug.AppendFlags(FarseerPhysics.DebugViewFlags.PolygonPoints);

            Pixel = new hTexture(new Texture2D(Game.GraphicsDevice, 1, 1));
            Pixel.Base.SetData<Color>(new Color[1] { Color.White });

            this.DrawOrder = 99999;

            this.Visible = false;
#if DEBUG
            this.Visible = true;
            this.Overlay = true;
#endif
        }

        /// <summary>
        /// GameObject Draw
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            foreach (var comp in Game.Components)
            {
                if (typeof(SpriteObject).IsAssignableFrom(comp.GetType()))
                {
                    var drawComp = (SpriteObject)comp;
                    if (!(typeof(BodyObject).IsAssignableFrom(comp.GetType()) && ((BodyObject)drawComp).Body.JointList != null && ((BodyObject)drawComp).Body.JointList.Joint.BodyA != ((BodyObject)drawComp).Body))
                    {
                        Game.SpriteBatch.DrawString(_debugFont, drawComp.Position.ToString(), drawComp.Position, Color.Red);

                        if (typeof(BodyObject).IsAssignableFrom(comp.GetType()))
                        {
                            Game.SpriteBatch.DrawString(_debugFont, ((BodyObject)drawComp).Body.LinearVelocity.ToString(), drawComp.Position + drawComp.Size + new Vector2(0, 12), Color.Lime);
                            Game.SpriteBatch.DrawString(_debugFont, MathHelper.ToDegrees(((BodyObject)drawComp).Body.Rotation).ToString(), drawComp.Position + drawComp.Size + new Vector2(0, 24), Color.Yellow);
                        }
                    }
                    
                }

                if (typeof(AI.AI).IsAssignableFrom(comp.GetType()))
                {
                    var ai = (AI.AI)comp;

                    if (ai.GoTo != null)
                    {
                        var forw = ai.GoTo;
                        forw.Normalize();

                        Pixel.Draw(gameTime,
                            new Rectangle(
                                (int)ai.Character.Position.X,
                                (int)ai.Character.Position.Y,
                                (int)(ai.GoTo - ai.Character.Position).Length(),
                                2),
                            Color.Yellow,
                            (float)Math.Atan2(forw.Y, forw.X),
                            Vector2.Zero,
                            SpriteEffects.None,
                            0);
                    }

                    if (ai.Path != null && ai.AttackTarget != null)
                    {
                        var Points = ai.Path.GetCurve(100);

                        foreach (var point in ai.Path.Points)
                        {
                            DrawLine(point + new Vector2(-4, -4), point + new Vector2(4, 4), 1, Color.Yellow);
                            DrawLine(point + new Vector2(-4, 4), point + new Vector2(4, -4), 1, Color.Yellow);
                        }

                        

                        for (int i = 0; i < Points.Length - 1; i++)
                        {
                            DrawLine(Points[i], Points[i + 1], 1, Color.Red);
                        }
                    }
                }
            }
            Matrix proj = Matrix.CreateOrthographicOffCenter(0f, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height, 0f, 0f, 1f);
            _physicsDebug.RenderDebugData(proj, _camera.View);
        }
        
        private void DrawLine(Vector2 start, Vector2 end, int width, Color color)
        {
            //start = this.GameScene.Map.Camera.ToScreen(start);
            //end = this.GameScene.Map.Camera.ToScreen(end);

            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);
                Game.SpriteBatch.Draw(Pixel.Base,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    width), //width of line, change this to make thicker line
                null,
                color, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }
    }
}
