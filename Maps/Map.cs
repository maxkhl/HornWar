using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Horn_War_II.GameObjects;
using Horn_War_II.GameObjects.ParticleSystem;
using Microsoft.Xna.Framework;

namespace Horn_War_II.Maps
{
    /// <summary>
    /// Defines a map and all its components
    /// </summary>
    abstract class Map : IDisposable
    {
        /// <summary>
        /// The maps background
        /// </summary>
        public Background Background { get; protected set; }

        /// <summary>
        /// Camera for this map
        /// </summary>
        public GameCam Camera { get; private set; }

        /// <summary>
        /// GameScene, this map is running in
        /// </summary>
        public Scenes.GameScene GameScene { get; private set; }

        /// <summary>
        /// The physic engine used by this map.
        /// </summary>
        /// <value>
        public PhysicEngine PhysicEngine { get; private set; }

        /// <summary>
        /// The particle engine used by this map.
        /// </summary>
        /// <value>
        public ParticleEngine ParticleEngine { get; private set; }

        /// <summary>
        /// List of all the datums in this map
        /// </summary>
        public List<Datums.Datum> Datums { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        public Map(Scenes.GameScene GameScene)
        {
            this.GameScene = GameScene;
            this.Camera = new GameCam(GameScene.Game);
            this.PhysicEngine = new GameObjects.PhysicEngine(GameScene);
            this.ParticleEngine = new ParticleEngine(GameScene, PhysicEngine);

            new GameObjects.DebugDrawer(GameScene, PhysicEngine.World, Camera);
            new GameObjects.HealthbarDrawer(GameScene, Camera);
        }

        /// <summary>
        /// Updates this map
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Disposes this map and all of its components
        /// </summary>
        public virtual void Dispose()
        {
            //Get a copy of all components
            var components = SceneManager.Game.Components.ToList();

            //Undock all components from the game
            SceneManager.Game.Components.Clear();

            //Dispose all disposable components
            foreach (var component in components)
                if (component.GetType().IsAssignableFrom(typeof(IDisposable)))
                    ((IDisposable)component).Dispose();
        }
    }
}
