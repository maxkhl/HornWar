using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Horn_War_II.Scenes
{
    /// <summary>
    /// This scene runs the game
    /// </summary>
    partial class GameScene : Scene
    {
        /// <summary>
        /// The current map
        /// </summary>
        public Maps.Map Map { get; set; }

        /// <summary>
        /// Default shader
        /// </summary>
        public Effect Shader { get; set; }

        /// <summary>
        /// Game Components that should be drawn in front of everything
        /// </summary>
        public List<IDrawable> OverlayLayer { get; set; }
        
        public SpriteFont DefaultFont { get; set; }

        public GameObjects.Tools.Animation BrightnessAnimation { get; private set; }
        public GameObjects.Tools.Animation ContrastAnimation { get; private set; }

        public enum GameSceneMap
        {
            Cave,
            Space,
            None,
        }

        private GameSceneMap _loadMap;

        public GameScene(GameSceneMap Map) : base()
        {
            _loadMap = Map;
            OverlayLayer = new List<IDrawable>();
            DefaultFont = Game.Content.Load<SpriteFont>("MenuFont");
            BrightnessAnimation = new GameObjects.Tools.Animation(this.GetType().GetProperty("Brightness"), this);
            ContrastAnimation = new GameObjects.Tools.Animation(this.GetType().GetProperty("Contrast"), this);            
        }

        /// <summary>
        /// Load GameObjects to games Components
        /// </summary>
        public override void LoadContent()
        {
            CreateRenderTarget(
                Game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                Game.GraphicsDevice.PresentationParameters.BackBufferHeight);

            Game.Window.ClientSizeChanged += Window_ClientSizeChanged;

            LoadGame();

            Shader = SceneManager.Game.Content.Load<Effect>("Shader/Game/Default");
            PostShader = SceneManager.Game.Content.Load<Effect>("Shader/Game/PostProcessing");
            
        }

        /// <summary>
        /// Part of the loading process. Loads ingame logic of the scene.
        /// </summary>
        public virtual void LoadGame()
        {

            switch(_loadMap)
            {
                case GameSceneMap.Cave:
                    Map = new Maps.Cave(this);


                    // Spawn player
                    var player = new GameObjects.Player(this, Map.Camera, Map.PhysicEngine, GameObjects.Character.SkinType.Goblin);
                    player.Position = new Microsoft.Xna.Framework.Vector2(-500, 50);

                    // Arm player
                    var weapon = new GameObjects.Weapons.Sword(this, Map.PhysicEngine);
                    weapon.Attach(player);

                    // Tell camera to follow player
                    Map.Camera.FollowGO = player;

                    // Spawn NPCs
                    var npcGoblin1 = new GameObjects.NPC(new GameObjects.AI.AI.AIOptions(), this, Map.PhysicEngine, GameObjects.Character.SkinType.Goblin);
                    npcGoblin1.Position = new Microsoft.Xna.Framework.Vector2(500, 50);
                    var npcGoblin1Horn = new GameObjects.Weapons.Horn(this, Map.PhysicEngine);
                    npcGoblin1Horn.Attach(npcGoblin1);
                    var scoreBoard = new UI.ScoreBoard(this, Map.PhysicEngine, player, npcGoblin1);
                    break;
                case GameSceneMap.Space:
                    Map = new Maps.Space(this);


                    // Spawn player
                    var playera = new GameObjects.Player(this, Map.Camera, Map.PhysicEngine, GameObjects.Character.SkinType.Cyborg);
                    playera.Position = new Microsoft.Xna.Framework.Vector2(-500, 50);

                    // Arm player
                    var weapona = new GameObjects.Weapons.Sword(this, Map.PhysicEngine);
                    weapona.Attach(playera);

                    // Tell camera to follow player
                    Map.Camera.FollowGO = playera;

                    // Spawn NPCs
                    var npcCyborg1 = new GameObjects.NPC(new GameObjects.AI.AI.AIOptions(), this, Map.PhysicEngine, GameObjects.Character.SkinType.Cyborg);
                    npcCyborg1.Position = new Microsoft.Xna.Framework.Vector2(500, 50);
                    var npcGoblin1Horna = new GameObjects.Weapons.Horn(this, Map.PhysicEngine);
                    npcGoblin1Horna.Attach(npcCyborg1);
                    var scoreBoarda = new UI.ScoreBoard(this, Map.PhysicEngine, playera, npcCyborg1);
                    break;
            }




        }

        /// <summary>
        /// Draws the GameScene
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {            
            SceneManager.Game.GraphicsDevice.SetRenderTarget(RenderTarget);
            SceneManager.Game.GraphicsDevice.Clear(Color.Black);

            //Background-Drawcall
            if(Map != null && Map.Background != null)
                Map.Background.Draw(gameTime);


            //Main-Drawcalls
            SceneManager.Game.SpriteBatch.Begin(transformMatrix: Map.Camera.View, effect: Shader);
            SceneManager.Game.DrawComponents(gameTime);
            SceneManager.Game.SpriteBatch.End();

            //Overlay-Drawcalls
            SceneManager.Game.SpriteBatch.Begin(transformMatrix: Matrix.Identity, effect: Shader);
            foreach (var dComponent in OverlayLayer)
                dComponent.Draw(gameTime);
            SceneManager.Game.SpriteBatch.End();

            SceneManager.Game.GraphicsDevice.SetRenderTarget(null);

            //Post processing
            SceneManager.Game.SpriteBatch.Begin(effect: PostShader);

            //Set frame parameters
            SetPostShaderParameters();

            //Draw rendertarget
            SceneManager.Game.SpriteBatch.Draw((Texture2D)RenderTarget,
                new Vector2(0, 0),
                new Rectangle(0, 0, RenderTarget.Width, RenderTarget.Height),
                Color.White);

            SceneManager.Game.SpriteBatch.End();
        }



        public override void Update(GameTime gameTime)
        {
            BrightnessAnimation.Update(gameTime);
            ContrastAnimation.Update(gameTime);
        }

        /// <summary>
        /// Dipose everything
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }


        protected void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            CreateRenderTarget(
                Game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                Game.GraphicsDevice.PresentationParameters.BackBufferHeight);

            if (Map != null && Map.Camera != null)
                Map.Camera.Refresh();
        }
    }
}
