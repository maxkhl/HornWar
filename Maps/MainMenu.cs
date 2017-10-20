using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Horn_War_II.GameObjects;

namespace Horn_War_II.Maps
{
    class MainMenu : Map
    {
        public Scenes.MenuScene MenuScene;

        public GameObjects.BrowserObject BrowserObject { get; set; }

        public MainMenu(Scenes.MenuScene MenuScene)
            : base(MenuScene)
        {
            this.MenuScene = MenuScene;

            // Setup PP Shader
            GameScene.RenderTechnique = Scenes.GameScene.RenderTechniques.Default;
            GameScene.ScreenColor = new Vector4(1.0f, 1.0f, 1.0f, 1.0f); //Keep true colors in menu
            GameScene.Contrast = 1.0f;
            GameScene.Brightness = -1;

            BrowserObject = new BrowserObject(
                MenuScene, 
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI\\MainMenu.html"),
                this);

            BrowserObject.Overlay = false;
            BrowserObject.AutoCenter = true;
            BrowserObject.Resize();



        }

        bool StartGame = false;
        public void startgame()
        {
            StartGame = true;
        }

        bool MenuInitialized = false;
        public override void Update(GameTime gameTime)
        {
            if(BrowserObject.Loaded && !MenuInitialized)
            {
                this.Camera.Zoom = 0.0001f;
                this.Camera.ZoomAnimation.Animate(.8f, 2000, GameObjects.Tools.Easing.EaseFunction.CircEaseOut);
                this.GameScene.BrightnessAnimation.Animate(1.0f, 2000, GameObjects.Tools.Easing.EaseFunction.CircEaseOut);



                // Spawn NPCs
                var npcGoblin1 = new GameObjects.NPC(new GameObjects.AI.AI.AIOptions()
                {
                    RoamArea = new Rectangle(0, 0, 400, 400), // Makes sure the goblin roams around the visible area
                    ForceRoamArea = true
                },
                GameScene, PhysicEngine, Character.SkinType.Goblin);
                npcGoblin1.Position = new Microsoft.Xna.Framework.Vector2(-500, 0);
                npcGoblin1.DrawOrder = -5;
                var npcGoblin1Horn = new GameObjects.Weapons.Horn(GameScene, PhysicEngine);
                npcGoblin1Horn.Attach(npcGoblin1);

                npcGoblin1 = new GameObjects.NPC(new GameObjects.AI.AI.AIOptions()
                {
                    RoamArea = new Rectangle(0, 0, 400, 400), // Makes sure the goblin roams around the visible area
                    ForceRoamArea = true
                },
                GameScene, PhysicEngine, Character.SkinType.Goblin);
                npcGoblin1.Position = new Microsoft.Xna.Framework.Vector2(500, 0);
                npcGoblin1.DrawOrder = -5;
                npcGoblin1Horn = new GameObjects.Weapons.Horn(GameScene, PhysicEngine);
                npcGoblin1Horn.Attach(npcGoblin1);

                MenuInitialized = true;
            }

            if(StartGame)
                GameScene.Game.SceneManager.ActiveScene = new Scenes.GameScene(Scenes.GameScene.GameSceneMap.Cave);
        }
    }
}
