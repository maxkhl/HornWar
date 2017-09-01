﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Horn_War_II.GameObjects.AI
{
    /// <summary>
    /// Updateable AI-GameObject. This can control a Character-Instance
    /// </summary>
    partial class AI : GameObject
    {
        #region AIOptions Structure
        /// <summary>
        /// AI-Options
        /// </summary>
        public struct AIOptions
        {
            /// <summary>
            /// Gets or sets a value indicating whether [this npc is hostile towards everybody].
            /// </summary>
            public bool DefaultHostile { get; set; }

            /// <summary>
            /// Gets or sets the view distance.
            /// </summary>
            public float ViewDistance { get; set; }

            /// <summary>
            /// Roam-mode will pick random points only inside this area
            /// </summary>
            public Rectangle RoamArea { get; set; }

            /// <summary>
            /// Will enforce roamarea behaviour on any situation - even if in combat, targets will not be chased if outside of the area
            /// </summary>
            public bool ForceRoamArea { get; set; }

            /// <summary>
            /// Defines AI difficulty
            /// </summary>
            public DifficultyType Difficulty { get; set; }

            /// <summary>
            /// NPC difficulty
            /// </summary>
            public enum DifficultyType
            {
                Ez,
                Wtf,
            }
        }
        #endregion

        /// <summary>
        /// Can be used to customize this AI
        /// </summary>
        public AIOptions Options;

        /// <summary>
        /// The character, this AI is controling
        /// </summary>
        public Character Character { get; private set; }

        /// <summary>
        /// State of this NPC
        /// </summary>
        public AIState State
        {
            get
            {
                return _State;
            }
            private set
            {
                _State = value;
                switch(_State)
                {
                    case AIState.Charge:
                        this.Character.Speed = Character.WalkSpeed.Full;
                        break;
                    case AIState.Move:
                        this.Character.Speed = Character.WalkSpeed.Half;
                        break;
                    case AIState.Roam:
                        this.Character.Speed = Character.WalkSpeed.Slow;
                        break;
                }
            }
        }
        private AIState _State;

        /// <summary>
        /// Defines the state of the AI
        /// </summary>
        public enum AIState
        {
            Wait,
            Roam,
            Move,
            Charge,
            Hit,
            Fallback,
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Character">Character, this AI should control</param>
        /// <param name="Options">Options for this AI</param>
        /// <param name="Game">Game, this AI should be registered to</param>
        public AI(Character Character, AIOptions Options, HornWarII Game) 
            : base(Game)
        {
            this.Character = Character;
            this.Character.AI = this;
            this.Options = Options;
            if (this.Options.ViewDistance == 0)
                this.Options.ViewDistance = 6000;

            InitializeThinking();
        }

        /// <summary>
        /// Updates the AI
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            ProcessThinking(gameTime);
            ProcessMovement(gameTime);
            ProcessEffects(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Disposal of the AI
        /// </summary>
        public override void Dispose()
        {
            // Remove AI from character
            this.Character.AI = null;
            DisposeThinking();
            base.Dispose();
        }
    }
}