using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Horn_War_II.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Horn_War_II.Maps
{
    /// <summary>
    /// The goblins cave
    /// - limited space (surrounded by rocks and dirt)
    /// - moveable mushrooms
    /// - darkness
    /// </summary>
    class Cave : Map
    {
        public Cave(Scenes.GameScene GameScene) : base(GameScene)
        {
            GameScene.PenumbraObject.AmbientColor = Color.White * 0.5f;

            // Background
            this.Background = new GameObjects.Background(GameScene, Camera, "Images/Maps/Cave/RockBackground");

            Camera.AllowedArea = new Rectangle(-1750, -900, 3500, 1800);

            // Setup PP Shader
            GameScene.RenderTechnique = Scenes.GameScene.RenderTechniques.Default;
            GameScene.ScreenColor = new Vector4(0.8f, 0.8f, 1.0f, 1.0f);
            GameScene.Brightness = 0.0f;
            GameScene.Contrast = 1.1f;

            // Setup cave border
            var caveBorderTop = new StaticObject(GameScene, PhysicEngine);
            caveBorderTop.Texture = new hTexture(GameScene.Game.Content.Load<Texture2D>("Images/Maps/Cave/CaveTop"));
            caveBorderTop.ShapeFromTexture();
            caveBorderTop.Material = BodyObject.BOMaterial.Stone;
            caveBorderTop.DrawOrder = 200;

            var caveBorderBottom = new StaticObject(GameScene, PhysicEngine);
            caveBorderBottom.Texture = new hTexture(GameScene.Game.Content.Load<Texture2D>("Images/Maps/Cave/CaveBottom"));
            caveBorderBottom.ShapeFromTexture();
            caveBorderBottom.Material = BodyObject.BOMaterial.Stone;
            caveBorderBottom.DrawOrder = 200;

            // Floating rocks
            var rock1 = new GameObjects.Decoration.Rock(GameScene, PhysicEngine, GameObjects.Decoration.Rock.RockType.Rock1);
            rock1.Position = new Microsoft.Xna.Framework.Vector2(800, 120);


            //caveBorder.Position -= caveBorder.Size / 2;

            var goblin1 = new GameObjects.NPC(
                new GameObjects.AI.AI.AIOptions()
                {
                    DefaultHostile = false,
                    Passive = true
                }, 
                GameScene, 
                PhysicEngine, 
                Character.SkinType.Cyborg);
            goblin1.Position = new Vector2(200, 0);


            // Spawn mushrooms
            var mushroom1 = new GameObjects.Decoration.Mushroom(GameScene, PhysicEngine, GameObjects.Decoration.Mushroom.MushroomType.Mushroom1);
            mushroom1.Position = new Microsoft.Xna.Framework.Vector2(500, 820);
            mushroom1.Body.IgnoreCollisionWith(caveBorderTop.Body);
            mushroom1.Body.IgnoreCollisionWith(caveBorderBottom.Body);

            var mushroom2 = new GameObjects.Decoration.Mushroom(GameScene, PhysicEngine, GameObjects.Decoration.Mushroom.MushroomType.Mushroom1);
            mushroom2.Position = new Microsoft.Xna.Framework.Vector2(-1050, 700);
            mushroom2.Body.IgnoreCollisionWith(caveBorderTop.Body);
            mushroom2.Body.IgnoreCollisionWith(caveBorderBottom.Body);

            /*var mushroom3 = new GameObjects.Decoration.Mushroom(GameScene, PhysicEngine, GameObjects.Decoration.Mushroom.MushroomType.Mushroom2);
            mushroom3.Position = new Microsoft.Xna.Framework.Vector2(1500, 750);
            mushroom3.Body.IgnoreCollisionWith(caveBorderTop.Body);
            mushroom3.Body.IgnoreCollisionWith(caveBorderBottom.Body);*/


            // Campfire
            var campfire = new GameObjects.Decoration.CampFire(GameScene, PhysicEngine, ParticleEngine)
            {
                Position = new Vector2(760, 570),
                DrawOrder = 250,
                IsOn = true,
            };
        }

        Random random = new Random();
        public float DustSpawn = -1;
        public override void Update(GameTime gameTime)
        {
            if (DustSpawn < 0)
            {
                new GameObjects.Effects.Dust(this.GameScene, this.ParticleEngine)
                {
                    LocalPosition = new Vector2((float)random.NextDouble() * Camera.AllowedArea.Width + Camera.AllowedArea.X, Camera.AllowedArea.Y),
                    EmitterLifetime = (float)random.NextDouble() * (1000 - 500) + 500,
                };
                DustSpawn = (float)random.NextDouble() * (5000 - 500) + 500;
            }
            DustSpawn -= gameTime.ElapsedGameTime.Milliseconds;

            base.Update(gameTime);
        }
    }
}
