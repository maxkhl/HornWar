using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Horn_War_II.GameObjects
{
    /// <summary>
    /// Non-player-character this is a AI controlled character
    /// </summary>
    class NPC : Character
    {

        public NPC(AI.AI.AIOptions Options, Scenes.GameScene GameScene, PhysicEngine PhysicEngine, SkinType Skin)
            : base(GameScene, PhysicEngine, Skin)
        {
            // Connects automatically to this character
            new AI.AI(this, Options, GameScene.Game);
        }
    }
}
