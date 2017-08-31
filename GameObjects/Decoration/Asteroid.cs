using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Horn_War_II.GameObjects.Decoration
{
    /// <summary>
    /// A moveable/floating asteroid
    /// </summary>
    class Asteroid : BodyObject
    {
        /// <summary>
        /// Different types of asteroids
        /// </summary>
        public enum AsteroidType
        {
            Asteroid1,
            Asteroid2,
            Asteroid3,
            Asteroid4
        }

        /// <summary>
        /// Type of this asteroid
        /// </summary>
        public AsteroidType Type { get; private set; }

        public Asteroid(Scenes.GameScene GameScene, PhysicEngine PhysicEngine, AsteroidType Type)
            : base(GameScene, PhysicEngine)
        {
            this.Type = Type;
            this.Body.IgnoreGravity = true;

            switch(Type)
            {
                case AsteroidType.Asteroid1:
                    this.Texture = new hTexture(Game.Content.Load<Texture2D>("Images/Maps/Space/Asteroid1"));
                    break;
                case AsteroidType.Asteroid2:
                    this.Texture = new hTexture(Game.Content.Load<Texture2D>("Images/Maps/Space/Asteroid2"));
                    break;
                case AsteroidType.Asteroid3:
                    this.Texture = new hTexture(Game.Content.Load<Texture2D>("Images/Maps/Space/Asteroid3"));
                    break;
                case AsteroidType.Asteroid4:
                    this.Texture = new hTexture(Game.Content.Load<Texture2D>("Images/Maps/Space/Asteroid4"));
                    break;
            }

            this.ShapeFromTexture();
        }
    }
}
