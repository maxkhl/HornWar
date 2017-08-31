using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Horn_War_II
{
    abstract class Scene : IDisposable
    {
        /// <summary>
        /// Gets the currently used game
        /// </summary>
        public HornWarII Game
        {
            get
            {
                return SceneManager.Game;
            }
        }

        /// <summary>
        /// Will be called by the scene manager during activation of this scene
        /// </summary>
        public abstract void LoadContent();

        /// <summary>
        /// Will be called by the scene manager during update loop if this scene is active
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Will be called by the scene manager during draw loop if this scene is active
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public abstract void Draw(GameTime gameTime);

        /// <summary>
        /// Dispose method. Will be called if this scene is released from the scene manager.
        /// !This will automatically remove all components from the game! Override and remove "base.Dispose()" to disable this
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
