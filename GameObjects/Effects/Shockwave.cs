using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics;
using FarseerPhysics.Controllers;

namespace Horn_War_II.GameObjects.Effects
{
    /// <summary>
    /// Should create a invisible shockwave that pushes away dynamic objects
    /// </summary>
    class Shockwave : Controller
    {
        public float Size { get; private set; }

        public Vector2 Position { get; set; }

        private float _time;
        private float _lifetime;

        public Shockwave(SpriteObject Object, Vector2 Position, float Size, float Lifetime)
            : base(ControllerType.AbstractForceController)
        {
            this.Position = Position;
            this.Size = Size;
            this.Enabled = true;
            this._lifetime = Lifetime;
        }

        public override void Update(float dt)
        {
            if (!this.Enabled) return;
            
            _time += dt * 1000;
            if (_time >= _lifetime)
                this.Enabled = false;

            ApplyForce(dt, ((_time / _lifetime) * -1 + 1));
        }

        public void ApplyForce(float dt, float strength)
        {

            foreach(var body in this.World.BodyList)
            {
                var delta = this.Position - body.Position;
                if (delta.Length() < Size)
                {
                    delta.Normalize();
                    delta *= -1;

                    var power = (Size - delta.Length()) / Size;
                    body.ApplyForce(delta * power * strength * 0.1f);
                }
            }
        }
    }
}
