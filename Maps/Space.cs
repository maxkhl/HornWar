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
    /// The space map
    /// - Fankey scifi
    /// - unlimited space
    /// - asteroids
    /// </summary>
    class Space : Map
    {
        public Space(Scenes.GameScene GameScene)
            : base(GameScene)
        {
            // Background
            this.Background = new GameObjects.Background(GameScene, Camera, "Images/Maps/Space/SpaceBackground");

            //Camera.AllowedArea = new Rectangle(-1750, -900, 3500, 1800);

            // Setup PP Shader
            GameScene.RenderTechnique = Scenes.GameScene.RenderTechniques.Default;
            GameScene.ScreenColor = new Vector4(0.8f, 0.8f, 1.0f, 1.0f);
            GameScene.Brightness = 0.0f;
            GameScene.Contrast = 1.1f;



            //caveBorder.Position -= caveBorder.Size / 2;

            // Spawn asteroids
            var asteroid1 = new GameObjects.Decoration.Asteroid(GameScene, PhysicEngine, GameObjects.Decoration.Asteroid.AsteroidType.Asteroid1);
            asteroid1.Position = new Microsoft.Xna.Framework.Vector2(100, 320);

            var asteroid2 = new GameObjects.Decoration.Asteroid(GameScene, PhysicEngine, GameObjects.Decoration.Asteroid.AsteroidType.Asteroid2);
            asteroid2.Position = new Microsoft.Xna.Framework.Vector2(-300, 720);

            var asteroid3 = new GameObjects.Decoration.Asteroid(GameScene, PhysicEngine, GameObjects.Decoration.Asteroid.AsteroidType.Asteroid3);
            asteroid3.Position = new Microsoft.Xna.Framework.Vector2(50, -300);

            var asteroid4 = new GameObjects.Decoration.Asteroid(GameScene, PhysicEngine, GameObjects.Decoration.Asteroid.AsteroidType.Asteroid4);
            asteroid4.Position = new Microsoft.Xna.Framework.Vector2(500, 820);
        }
    }
}
