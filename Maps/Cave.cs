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
    /// - eh
    /// </summary>
    class Cave : Map
    {
        public Cave(Scenes.GameScene GameScene) : base(GameScene)
        {
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

            var caveBorderBottom = new StaticObject(GameScene, PhysicEngine);
            caveBorderBottom.Texture = new hTexture(GameScene.Game.Content.Load<Texture2D>("Images/Maps/Cave/CaveBottom"));
            caveBorderBottom.ShapeFromTexture();



            //caveBorder.Position -= caveBorder.Size / 2;

            // Spawn mushrooms
            var mushroom1 = new GameObjects.Decoration.Mushroom(GameScene, PhysicEngine, GameObjects.Decoration.Mushroom.MushroomType.Mushroom1);
            mushroom1.Position = new Microsoft.Xna.Framework.Vector2(500, 820);
            mushroom1.Body.IgnoreCollisionWith(caveBorderTop.Body);
            mushroom1.Body.IgnoreCollisionWith(caveBorderBottom.Body);

            var mushroom2 = new GameObjects.Decoration.Mushroom(GameScene, PhysicEngine, GameObjects.Decoration.Mushroom.MushroomType.Mushroom1);
            mushroom2.Position = new Microsoft.Xna.Framework.Vector2(-1050, 700);
            mushroom2.Body.IgnoreCollisionWith(caveBorderTop.Body);
            mushroom2.Body.IgnoreCollisionWith(caveBorderBottom.Body);

            var mushroom3 = new GameObjects.Decoration.Mushroom(GameScene, PhysicEngine, GameObjects.Decoration.Mushroom.MushroomType.Mushroom2);
            mushroom3.Position = new Microsoft.Xna.Framework.Vector2(1500, 750);
            mushroom3.Body.IgnoreCollisionWith(caveBorderTop.Body);
            mushroom3.Body.IgnoreCollisionWith(caveBorderBottom.Body);      
        }
    }
}
