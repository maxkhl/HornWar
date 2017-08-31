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
        UI.Picture Header;
        public Scenes.MenuScene MenuScene;

        public MainMenu(Scenes.MenuScene MenuScene)
            : base(MenuScene)
        {
            this.MenuScene = MenuScene;

            // Background
            //this.Background = new GameObjects.Background(MenuScene, Camera, "Images/Maps/Cave/RockBackground");

            // Setup PP Shader
            GameScene.RenderTechnique = Scenes.GameScene.RenderTechniques.Default;
            GameScene.ScreenColor = new Vector4(1.0f, 1.0f, 1.0f, 1.0f); //Keep true colors in menu
            GameScene.Contrast = 1.0f;


#if DEBUG
            GameScene.Brightness = -1.0f;
            GameScene.BrightnessAnimation.Animate(1f, 1000, GameObjects.Tools.Easing.EaseFunction.CubicEaseIn);
            this.Camera.Zoom = 0f;
            this.Camera.ZoomAnimation.Animate(1f, 1000, GameObjects.Tools.Easing.EaseFunction.CubicEaseOut);

            BuildMainMenu();
#else
            GameScene.Brightness = -1.0f;
            GameScene.BrightnessAnimation.Animate(1f, 8000, GameObjects.Tools.Easing.EaseFunction.CubicEaseIn);
            this.Camera.Zoom = 0f;
            this.Camera.ZoomAnimation.Animate(0.9f, 500, GameObjects.Tools.Easing.EaseFunction.CubicEaseOut);

            this.Camera.ZoomAnimation.OnAnimationDone += ZoomAnimation_OnAnimationDone2;

            //BuildNewGameMenu();
            BuildDisclaimerMenu();
#endif


        }
        
        void ZoomAnimation_OnAnimationDone2(GameObjects.Tools.Animation sender)
        {
            sender.OnAnimationDone -= ZoomAnimation_OnAnimationDone2;
            this.Camera.ZoomAnimation.Animate(0.3f, 13000, GameObjects.Tools.Easing.EaseFunction.CubicEaseInOut);
            this.Camera.ZoomAnimation.OnAnimationDone += ZoomAnimation_OnAnimationDone3;
        }
        void ZoomAnimation_OnAnimationDone3(GameObjects.Tools.Animation sender)
        {
            sender.OnAnimationDone -= ZoomAnimation_OnAnimationDone3;
            GameScene.BrightnessAnimation.Animate(-1f, 1000, GameObjects.Tools.Easing.EaseFunction.CubicEaseOut);
            this.Camera.ZoomAnimation.Animate(-1.2f, 1000, GameObjects.Tools.Easing.EaseFunction.CubicEaseOut);
            this.Camera.ZoomAnimation.OnAnimationDone += ZoomAnimation_OnAnimationDone4;
        }
        void ZoomAnimation_OnAnimationDone4(GameObjects.Tools.Animation sender)
        {
            sender.OnAnimationDone -= ZoomAnimation_OnAnimationDone4;
            ClearMenu();
            BuildMainMenu();
            GameScene.BrightnessAnimation.Animate(1f, 1000, GameObjects.Tools.Easing.EaseFunction.CubicEaseOut);
            this.Camera.ZoomAnimation.Animate(0.8f, 1000, GameObjects.Tools.Easing.EaseFunction.CubicEaseOut);
        }

        /// <summary>
        /// Should clear the current menu and all the other shit to get a blank screen
        /// </summary>
        public void ClearMenu()
        {
            //Get a copy of all components
            var components = SceneManager.Game.Components.ToList();
            
            //Dispose all needed components
            foreach (var component in components)
            {
                if (typeof(GameObjects.SpriteObject).IsAssignableFrom(component.GetType())) // Remove all texture components (usualy gamestuff)
                {
                    if (component.GetType().IsAssignableFrom(typeof(IDisposable)))
                        ((IDisposable)component).Dispose();
                    SceneManager.Game.Components.Remove(component);
                }
            }
        }

        public void BuildDisclaimerMenu()
        {
            var line1 = new UI.UiLabel(MenuScene, PhysicEngine, "This is a early development build.", MenuScene.CaptionFont) { Centered = true };

            var AssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
            var line2 = new UI.UiLabel(MenuScene, PhysicEngine, String.Format("Version: {0}", AssemblyName.Version.ToString()), MenuScene.CaptionFont) { Centered = true };
            line2.Position = line2.Position + new Vector2(0, 40);

            var line3 = new UI.UiLabel(MenuScene, PhysicEngine, "Add me on Steam to report bugs or ask questions.", MenuScene.CaptionFont) { Centered = true };
            line3.Position = line3.Position + new Vector2(0, 100);
            


        }

        public void BuildNewGameMenu()
        {

            var frame1BG = new UI.Picture(MenuScene, PhysicEngine, SceneManager.Game.Content.Load<Texture2D>("Images/Menu/PreviewCave")) 
            { Position = new Vector2(-175, -120) };
            var frame1 = new UI.Picture(MenuScene, PhysicEngine, SceneManager.Game.Content.Load<Texture2D>("Images/Menu/MainMenuFrame1")) 
            { Position = new Vector2(-180, -120) };
            frame1.OnMouseClick += frame1_OnMouseClick;


            var frame2BG = new UI.Picture(MenuScene, PhysicEngine, SceneManager.Game.Content.Load<Texture2D>("Images/Menu/PreviewSpace")) 
            { Position = new Vector2(-5, -120) };
            var frame2 = new UI.Picture(MenuScene, PhysicEngine, SceneManager.Game.Content.Load<Texture2D>("Images/Menu/MainMenuFrame1"))
            { Position = new Vector2(0, -120) };
            frame2.OnMouseClick += frame2_OnMouseClick;
            

            var frame3 = new UI.Picture(MenuScene, PhysicEngine, SceneManager.Game.Content.Load<Texture2D>("Images/Menu/MainMenuFrame1"));
            frame3.Position = frame3.Position + new Vector2(180, -120);
        }

        void frame1_OnMouseClick(UI.UiObject sender)
        {

            sender.OnMouseClick -= frame1_OnMouseClick;

            // Zoom out
            GameScene.BrightnessAnimation.Animate(-1f, 1000, GameObjects.Tools.Easing.EaseFunction.CubicEaseOut);
            this.Camera.ZoomAnimation.Animate(-0.8f, 1000, GameObjects.Tools.Easing.EaseFunction.CubicEaseOut);
            this.Camera.ZoomAnimation.OnAnimationDone += ZoomAnimation_OnAnimationDone6;
            GameScene.BrightnessAnimation.OnAnimationDone += ZoomAnimation_OnAnimationDone6;
        }

        void ZoomAnimation_OnAnimationDone6(GameObjects.Tools.Animation sender)
        {
            sender.OnAnimationDone -= ZoomAnimation_OnAnimationDone6;

            if (this.Camera.ZoomAnimation.Active == false && GameScene.BrightnessAnimation.Active == false)
            {
                MenuScene.Game.SceneManager.ActiveScene = new Scenes.GameScene(Scenes.GameScene.GameSceneMap.Cave);
            }
        }

        void frame2_OnMouseClick(UI.UiObject sender)
        {

            sender.OnMouseClick -= frame2_OnMouseClick;

            // Zoom out
            GameScene.BrightnessAnimation.Animate(-1f, 1000, GameObjects.Tools.Easing.EaseFunction.CubicEaseOut);
            this.Camera.ZoomAnimation.Animate(-0.8f, 1000, GameObjects.Tools.Easing.EaseFunction.CubicEaseOut);
            this.Camera.ZoomAnimation.OnAnimationDone += ZoomAnimation_OnAnimationDone7;
            GameScene.BrightnessAnimation.OnAnimationDone += ZoomAnimation_OnAnimationDone7;
        }

        void ZoomAnimation_OnAnimationDone7(GameObjects.Tools.Animation sender)
        {
            sender.OnAnimationDone -= ZoomAnimation_OnAnimationDone7;

            if (this.Camera.ZoomAnimation.Active == false && GameScene.BrightnessAnimation.Active == false)
            {
                MenuScene.Game.SceneManager.ActiveScene = new Scenes.GameScene(Scenes.GameScene.GameSceneMap.Space);
            }
        }

        public void BuildMainMenu()
        {
            float yoff = -120;
            Header = new UI.Picture(MenuScene, PhysicEngine, SceneManager.Game.Content.Load<Texture2D>("Images/Menu/Heading"));
            Header.Position = Header.Position + new Vector2(0, 0 + yoff);

            float ybtnoff = 0;
            var button = new UI.UiButton(MenuScene, PhysicEngine, "New Game");
            button.Background = UI.UiButton.Backgrounds.Wood1;

            button.OnMouseClick += button_OnMouseClick;
            ybtnoff += 70;

            button = new UI.UiButton(MenuScene, PhysicEngine, "Multiplayer");
            button.Background = UI.UiButton.Backgrounds.Wood2;
            button.Position = button.Position + new Vector2(0, ybtnoff);
            ybtnoff += 70;

            button = new UI.UiButton(MenuScene, PhysicEngine, "Options");
            button.Background = UI.UiButton.Backgrounds.Wood3;
            button.Position = button.Position + new Vector2(0, ybtnoff);
            ybtnoff += 70;

            button = new UI.UiButton(MenuScene, PhysicEngine, "Quit");
            button.Background = UI.UiButton.Backgrounds.Wood4;
            button.Position = button.Position + new Vector2(0, ybtnoff);
            //button.OnMouseClick += delegate { this.GameScene.Game.Exit(); };
            ybtnoff += 70;

            //button.Position = button.Position + new Vector2(0, 274 + yoff);

            //var label = new UI.UiLabel(MenuScene, PhysicEngine, "Test asdsa wdqdqwdwq qw d wqwdq ");
            //label.Position = button.Position + new Vector2(0, 274 + yoff);

            // Spawn NPCs
            var npcGoblin1 = new GameObjects.NPC(GameScene, PhysicEngine, Character.SkinType.Goblin);
            npcGoblin1.Position = new Microsoft.Xna.Framework.Vector2(-500, 0);
            npcGoblin1.DrawOrder = -5;
            npcGoblin1.RoamArea = new Rectangle(0, 0, 400, 400); // Makes sure the goblin roams around the visible area
            npcGoblin1.ForceRoamArea = true;
            var npcGoblin1Horn = new GameObjects.Weapons.Horn(GameScene, PhysicEngine);
            npcGoblin1Horn.Attach(npcGoblin1);

            npcGoblin1 = new GameObjects.NPC(GameScene, PhysicEngine, Character.SkinType.Goblin);
            npcGoblin1.Position = new Microsoft.Xna.Framework.Vector2(500, 0);
            npcGoblin1.DrawOrder = -5;
            npcGoblin1.RoamArea = new Rectangle(0, 0, 400, 400); // Makes sure the goblin roams around the visible area
            npcGoblin1.ForceRoamArea = true;
            npcGoblin1Horn = new GameObjects.Weapons.Horn(GameScene, PhysicEngine);
            npcGoblin1Horn.Attach(npcGoblin1);

            /*npcGoblin1 = new GameObjects.NPC(GameScene, PhysicEngine, Character.SkinType.Goblin);
            npcGoblin1.Position = new Microsoft.Xna.Framework.Vector2(0, -500);
            npcGoblin1.DrawOrder = -5;
            npcGoblin1.RoamArea = Camera.Visible; // Makes sure the goblin roams around the visible area
            npcGoblin1Horn = new GameObjects.Weapons.Horn(GameScene, PhysicEngine);
            npcGoblin1Horn.Attach(npcGoblin1);*/

            //npcGoblin1.DefaultHostile = true;
        }

        void button_OnMouseClick(UI.UiObject sender)
        {
            sender.OnMouseClick -= button_OnMouseClick;

            // Zoom out
            GameScene.BrightnessAnimation.Animate(-1f, 1000, GameObjects.Tools.Easing.EaseFunction.CubicEaseOut);
            this.Camera.ZoomAnimation.Animate(-0.8f, 1000, GameObjects.Tools.Easing.EaseFunction.CubicEaseOut);
            this.Camera.ZoomAnimation.OnAnimationDone += ZoomAnimation_OnAnimationDone;
            GameScene.BrightnessAnimation.OnAnimationDone += ZoomAnimation_OnAnimationDone;
        }

        void ZoomAnimation_OnAnimationDone(GameObjects.Tools.Animation sender)
        {
            sender.OnAnimationDone -= ZoomAnimation_OnAnimationDone;

            if (this.Camera.ZoomAnimation.Active == false && GameScene.BrightnessAnimation.Active == false)
            {
                ClearMenu();
                BuildNewGameMenu();
                GameScene.BrightnessAnimation.Animate(1f, 1000, GameObjects.Tools.Easing.EaseFunction.CubicEaseOut);
                this.Camera.ZoomAnimation.Animate(0.8f, 1000, GameObjects.Tools.Easing.EaseFunction.CubicEaseOut);
            }
        }
    }
}
