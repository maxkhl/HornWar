﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;

namespace Horn_War_II.GameObjects
{
    /// <summary>
    /// Default player class - controls a character with given inputs
    /// </summary>
    [Spawn.SpawnAttribute("Images/NoPreview", false)]
    class Player : Character
    {

        protected GameCam Camera { get; set; }

        public Player(Scenes.GameScene GameScene, GameCam Camera, PhysicEngine PhysicEngine, Character.SkinType Skin)
            : base(GameScene, PhysicEngine, Skin)
        {
            this.Camera = Camera;

            Game.InputManager.OnMWScroll += InputManager_OnMWScroll;
        }

        private void InputManager_OnMWScroll(bool Up)
        {
            if (Up)
                this.GameScene.Map.Camera.ZoomAnimation.Animate(0.2f * this.GameScene.Map.Camera.Zoom, 200, Tools.Easing.EaseFunction.CubicEaseOut);
            else
                this.GameScene.Map.Camera.ZoomAnimation.Animate(-0.2f * this.GameScene.Map.Camera.Zoom, 200, Tools.Easing.EaseFunction.CubicEaseOut);
        }

        MouseState oldMs = Mouse.GetState();
        public override void Update(GameTime gameTime)
        {
            if (Game.InputManager.IsActionDown(InputManager.Action.Up))
                Move(new Vector2(0, -1));

            if (Game.InputManager.IsActionDown(InputManager.Action.Down))
                Move(new Vector2(0, 1));

            if (Game.InputManager.IsActionDown(InputManager.Action.Left))
                Move(new Vector2(-1, 0));

            if (Game.InputManager.IsActionDown(InputManager.Action.Right))
                Move(new Vector2(1, 0));

            if (Game.InputManager.IsActionDown(InputManager.Action.Boost))
                if (this.Texture.AnimationSequences != null && this.Texture.AnimationSequences.ContainsKey("Talk"))
                    this.Texture.Play(true, "Talk", 3);
                //Stop(); //heh

            var ingameMousePosition = Camera.ToWorld(Game.InputManager.MousePosition);
            LookAt = ingameMousePosition;

            if (Game.InputManager.IsActionPressed(InputManager.Action.Stab) && Game.InputManager.MouseActive)
            {
                var Explosion = new Effects.Explosion(GameScene, PhysicEngine);
                Explosion.Position = ingameMousePosition;
            }

            if (Game.InputManager.IsActionPressed(InputManager.Action.DropWeapon) && this.Weapon != null)
                this.Weapon.Detach();

            if (Game.InputManager.IsActionPressed(InputManager.Action.PickUp))
                this.PickUp();

            //if(Game.InputManager.IsActionPressed(InputManager.Action.PickUp))

            /*var ms = Mouse.GetState();

            if (ms.ScrollWheelValue < oldMs.ScrollWheelValue)
                _camera.Zoom += 0.01f;
            if (ms.ScrollWheelValue > oldMs.ScrollWheelValue)
                _camera.Zoom -= 0.01f;

            oldMs = ms;*/

            /*var localRotation = MathHelper.ToDegrees(this.Rotation);
            localRotation -= (int)(localRotation / 360) * 360;
            if (localRotation < 0 && localRotation > -270)
                this.Effects = SpriteEffects.None;
            else
                this.Effects = SpriteEffects.FlipHorizontally;*/

            base.Update(gameTime);
        }
    }
}
