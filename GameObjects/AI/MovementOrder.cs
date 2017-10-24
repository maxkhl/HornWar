using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horn_War_II.GameObjects.AI
{
    /// <summary>
    /// Defines a Waypoint for the AI
    /// </summary>
    struct Waypoint
    {
        /// <summary>
        /// Target of movement 
        /// </summary>
        public Vector2 Target { get; set; }

        /// <summary>
        /// Next waypoint 
        /// </summary>
        public Vector2? PeekNext { get; set; }

        /// <summary>
        /// Use stamina-boost
        /// </summary>
        public bool Boost { get; set; }

        /// <summary>
        /// Speed of the character
        /// </summary>
        public Character.WalkSpeed Speed { get; set; }

        /// <summary>
        /// Character stops at target position?
        /// </summary>
        public bool Stop { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Target">Target of movement</param>
        /// <param name="Boost">Use stamina-boost</param>
        /// <param name="Speed">Speed of the character</param>
        /// <param name="Stop">Character stops at target position?</param>
        public Waypoint(Vector2 Target, bool Boost, Character.WalkSpeed Speed, bool Stop, Vector2? PeekNext)
        {
            this.Target = Target;
            this.Boost = Boost;
            this.Speed = Speed;
            this.Stop = Stop;
            this.PeekNext = PeekNext;
        }

        /// <summary>
        /// Returns a Waypoint that lets the character stand still
        /// </summary>
        public static Waypoint Zero
        {
            get
            {
                return new Waypoint(Vector2.Zero, false, Character.WalkSpeed.Half, true, null);
            }
        }

        public static bool operator ==(Waypoint c1, Waypoint c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(Waypoint c1, Waypoint c2)
        {
            return !c1.Equals(c2);
        }

        public override bool Equals(object obj)
        {
            var other = (Waypoint)obj;

            return this.Target == other.Target &&
                   this.Boost == other.Boost &&
                   this.Speed == other.Speed &&
                   this.Stop == other.Stop;
        }

    }
}
