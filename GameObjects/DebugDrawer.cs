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
#endif
            this.Visible = false;
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
                    if (!(typeof(BodyObject).IsAssignableFrom(comp.GetType()) && ((BodyObject)drawComp).Body.JointList != null && ((BodyObject)drawComp).Body.JointList.Joint.BodyA != ((BodyObject)drawComp).Body))
                    {
                        Game.SpriteBatch.DrawString(_debugFont, drawComp.Position.ToString(), drawComp.Position + drawComp.Size, Color.Red);

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
                        var distance = (ai.Character.Position - ai.AttackTarget.Position).Length();

                        var forward = ai.Character.Position - ai.AttackTarget.Position;
                        forward.Normalize();
                        var oldPos = ai.Character.Position;
                        var timeout = 5000;
                        for(int i = 0; i < ai.Path.Waypoints.Length; i++)
                        {
                            timeout--;                                

                            var target = oldPos;

                            target += ai.Path.CalculateStepSim(oldPos, ai.AttackTarget.Position, i);

                            var forw = oldPos - target;
                            forw.Normalize();

                            Pixel.Draw(gameTime,
                                new Rectangle(
                                    (int)oldPos.X,
                                    (int)oldPos.Y,
                                    (int)(target - oldPos).Length(),
                                    2),
                                Color.Yellow,
                                (float)Math.Atan2(forw.Y, forw.X),
                                Vector2.Zero,
                                SpriteEffects.None,
                                0);
                            oldPos = target;
                        }
                    }

                    
                }
            }
            Matrix proj = Matrix.CreateOrthographicOffCenter(0f, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height, 0f, 0f, 1f);
            _physicsDebug.RenderDebugData(proj, _camera.View);
        }
    }
}
