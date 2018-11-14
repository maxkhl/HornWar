using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Horn_War_II.GameObjects.Decoration
{
    /// <summary>
    /// A randomly moving firefly
    /// </summary>
    class Firefly : BodyObject
    {
        static Random _Random;
        Penumbra.PointLight _Light;
        Penumbra.PointLight _LightGlow;

        /// <summary>
        /// Area, the firefly is alowed to move inside
        /// </summary>
        public Rectangle AllowedArea
        {
            get
            {
                return _AllowedArea;
            }
            set
            {
                _AllowedArea = value;
            }
        }
        public Rectangle _AllowedArea = Rectangle.Empty;

        public Firefly(Scenes.GameScene GameScene, PhysicEngine PhysicEngine)
            : base(GameScene, PhysicEngine)
        {
            this.Body.IgnoreGravity = true;

            this.Texture = new hTexture(Game.Content.Load<Texture2D>("Images/Firefly"));
            this.ShapeFromTexture(false);


            if(_Random == null)
                _Random = new Random();

            _Light = new Penumbra.PointLight();
            GameScene.PenumbraObject.Lights.Add(_Light);

            _Light.Scale = Vector2.One * 880;
            _Light.Color = Color.LightBlue * 0.6f;

            _LightGlow = new Penumbra.PointLight();
            GameScene.PenumbraObject.Lights.Add(_LightGlow);

            _LightGlow.Scale = Vector2.One * 140;
            _LightGlow.Color = Color.LightBlue * 0.6f;
        }

        Vector2 _TargetPosition;
        public override void Update(GameTime gameTime)
        {
            _Light.Position = this.Position + Vector2.UnitX * 5;
            _LightGlow.Position = this.Position + Vector2.UnitX * 5;

            if (_TargetPosition == Vector2.Zero)
                _TargetPosition = this.Position;

            if ((_TargetPosition - this.Position).Length() < 1 &&
                (_TargetPosition - this.Position).Length() > -1)
            {
                _TargetPosition = this.Position + new Vector2(
                    ((float)_Random.NextDouble() - 0.5f) * 2,
                    ((float)_Random.NextDouble() - 0.5f) * 2) * 150;

                // Limit movement to area if given
                if (AllowedArea != Rectangle.Empty)
                {
                    _TargetPosition.X = MathHelper.Clamp(_TargetPosition.X, AllowedArea.X, AllowedArea.X + AllowedArea.Width);
                    _TargetPosition.Y = MathHelper.Clamp(_TargetPosition.Y, AllowedArea.Y, AllowedArea.Y + AllowedArea.Height);
                }

                /*while (GameScene.Map.PhysicEngine.World.RayCast(this.Position, _TargetPosition).Count > 0)
                {
                    _TargetPosition = this.Position + new Vector2(
                        (float)_Random.NextDouble() - 0.5f * 2,
                        (float)_Random.NextDouble() - 0.5f * 2) * 50;
                }*/
            }
            else
            {
                this.Push(new Vector2(
                    MathHelper.Clamp(_TargetPosition.X - this.Position.X, -1, 1),
                    MathHelper.Clamp(_TargetPosition.Y - this.Position.Y, -1, 1)) + this.Body.LinearVelocity * -1);
            }
            base.Update(gameTime);
        }
    }
}
