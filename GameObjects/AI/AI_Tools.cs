using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Horn_War_II.GameObjects.AI
{
    /// <summary>
    /// Tool functions for the AI
    /// </summary>
    partial class AI
    {
        /// <summary>
        /// Calculates the angle of a position and direction
        /// </summary>
        /// <param name="Forward">Forward-Vector</param>
        /// <param name="Position">Position</param>
        /// <returns></returns>
        private float Angle(Vector2 Forward, Vector2 Position)
        {
            return (float)Math.Atan2(Forward.Y - Position.Y, Forward.X - Position.X);
        }
    }
}
