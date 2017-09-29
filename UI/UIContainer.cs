using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horn_War_II.UI
{
    /// <summary>
    /// Used to pack ui elements together
    /// </summary>
    class UIContainer : UiObject
    {
        public List<UiObject> Children { get; private set; }

        public override bool Overlay
        {
            get
            {
                return base.Overlay;
            }
            set
            {
                base.Overlay = value;

                foreach (var Child in Children)
                    Child.Overlay = value;
            }
        }

        public override Microsoft.Xna.Framework.Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                var move = base.Position - value;

                foreach(var Child in Children)
                    Child.Position += move;

                base.Position = value;
            }
        }

        /// <summary>
        /// Returns the bounds of the entire container
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                var retBnds = new Rectangle();
                foreach(var child in this.Children)
                {
                    if (child.Position.X < retBnds.X)
                        retBnds.X = (int)child.Position.X;
                    if (child.Position.Y < retBnds.Y)
                        retBnds.X = (int)child.Position.X;

                    if (child.Position.X + child.Width > retBnds.X + retBnds.Width)
                        retBnds.Width = (int)(child.Position.X + child.Width - retBnds.X);

                    if (child.Position.Y + child.Height > retBnds.Y + retBnds.Height)
                        retBnds.Width = (int)(child.Position.Y + child.Height - retBnds.Y);
                }
                return retBnds;
            }
        }

        public UIContainer(Scenes.GameScene GameScene, GameObjects.PhysicEngine PhysicEngine)
            : base(GameScene, PhysicEngine)
        {
            this.Children = new List<UiObject>();
        }
        public UIContainer(Scenes.GameScene GameScene, GameObjects.PhysicEngine PhysicEngine, List<UiObject> Children)
            : base(GameScene, PhysicEngine)
        {
            this.Children = new List<UiObject>(Children);
        }
        public UIContainer(Scenes.GameScene GameScene, GameObjects.PhysicEngine PhysicEngine, UiObject[] Children)
            : base(GameScene, PhysicEngine)
        {
            this.Children = new List<UiObject>(Children);
        }
    }
}
