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
    class UIPopup : UIContainer
    {
        public UiButton Button1 { get; private set; }
        public UiButton Button2 { get; private set; }

        public UIPopup(Scenes.GameScene GameScene, GameObjects.PhysicEngine PhysicEngine, string Text, string TextButton1, string TextButton2)
            : base(GameScene, PhysicEngine)
        {
            this.Children.Add(new UI.UiLabel(GameScene, PhysicEngine, Text)
            {
                Overlay = true,
                Position = new Vector2(),
            });
            Button1 = new UI.UiButton(GameScene, PhysicEngine, TextButton1)
            {
                Overlay = true,
                Position = new Vector2(0, 100),
            };
            this.Children.Add(Button1);

            if (TextButton2 != "")
            {
                Button2 = new UI.UiButton(GameScene, PhysicEngine, TextButton2)
                {
                    Overlay = true,
                    Position = new Vector2(0, 200),
                };
                this.Children.Add(Button2);
            }
        }
    }
}
