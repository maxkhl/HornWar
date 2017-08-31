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
