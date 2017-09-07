using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Horn_War_II.GameObjects.Tools
{
    /// <summary>
    /// Used to define and calculate a path in the gameworld
    /// </summary>
    class Path
    {

        /// <summary>
        /// Maximum allowed distance, the goto-command is allowed to leave the direct path sideways
        /// </summary>
        private float CurveWidth { get; set; }

        /// <summary>
        /// Distance where the goto-curve function starts
        /// </summary>
        private float CurveStart { get; set; }

        /// <summary>
        /// Distance where the goto-curve function ends
        /// </summary>
        private float CurveEnd { get; set; }

        /// <summary>
        /// Gets the span (length) of the curve
        /// </summary>
        private float CurveSpan
        {
            get
            {
                return CurveStart - CurveEnd;
            }
        }

        /// <summary>
        /// Left-sided goto-curve?
        /// </summary>
        private CurveSides CurveSide { get; set; }

        /// <summary>
        /// Side, a curve should go
        /// </summary>
        public enum CurveSides
        {
            Left,
            Right
        }

        /// <summary>
        /// Distance, a path needs in order to be called successful
        /// </summary>
        private float FinishDistance { get; set; }

        /// <summary>
        /// Easing function for the path
        /// </summary>
        private Tools.Easing.EaseFunction Function { get; set; }

        /// <summary>
        /// GoToArrived-Handler
        /// </summary>
        /// <param name="GoToPoint">Target point, the character arrived at</param>
        public delegate void GoToArrivedHandler(Vector2 GoToPoint);

        /// <summary>
        /// Called when the GoTo-Point was reached by the ai's character
        /// </summary>
        public event GoToArrivedHandler GoToArrived;

        public Vector2 Source { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Source">Source of the path</param>
        /// <param name="Target">Target of the path</param>
        /// <param name="Function">Easing function of the path</param>
        /// <param name="CurveSide">Curve side of the easing function</param>
        public Path(Vector2 Source, Vector2 Target, Tools.Easing.EaseFunction Function, CurveSides CurveSide)
        {
            this.Source = Source;
            this.Function = Function;
            this.CurveEnd = 0;
            this.CurveStart = (Source - Target).Length();
            this.CurveWidth = this.CurveStart * 0.5f; //Half the distance is curve width as default
            this.CurveSide = CurveSide;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Source">Source of the path</param>
        /// <param name="Target">Target of the path</param>
        /// <param name="Function">Easing function of the path</param>
        /// <param name="CurveSide">Curve side of the easing function</param>
        /// <param name="CurveStart">Distance to target, the curve should start at</param>
        /// <param name="CurveEnd">Distance to target, the curve should end a</param>
        /// <param name="CurveWidth">Width of the curve</param>
        public Path(Vector2 Source, Tools.Easing.EaseFunction Function, CurveSides CurveSide, float CurveStart, float CurveEnd, float CurveWidth)
        {
            this.Source = Source;
            this.Function = Function;
            this.CurveEnd = CurveEnd;
            this.CurveStart = CurveStart;
            this.CurveWidth = CurveWidth;
            this.CurveSide = CurveSide;
        }




        /// <summary>
        /// Calculates the next step for this path
        /// </summary>
        /// <param name="Position">Current position</param>
        /// <param name="Target">Target position</param>
        /// <returns>Next step in path</returns>
        public Vector2 CalculateStep(Vector2 Position, Vector2 Target)
        {

            var TargetDistance = (Target - Position).Length();

            var ForwardVector = (Target - Source);
            ForwardVector.Normalize();


            var TargetPosition = ForwardVector;

            // Only curve the path when distance between start and end
            if (TargetDistance < CurveStart && TargetDistance > CurveEnd)
            {
                // Get the path offset, using the given easing function

                var firstHalf = TargetDistance - CurveEnd > CurveSpan / 2;
                float s = 0;

                if (firstHalf)
                    s = Tools.Easing.Ease(
                        Easing.EaseFunction.BounceEaseIn,
                        CurveSpan - (TargetDistance - CurveEnd),
                        0,
                        CurveWidth,
                        CurveSpan / 2);
                else
                {
                    s = Tools.Easing.Ease(
                        Easing.EaseFunction.BounceEaseIn,
                        CurveSpan - (TargetDistance - CurveEnd) + CurveSpan / 2,
                        0,
                        CurveWidth,
                        CurveSpan);
                    s = (s - CurveWidth) * -1;
                }

                //if (!firstHalf)
                //    s -= CurveWidth;

                var SideVector = new Vector2(0, s);
                SideVector = RotateVector2(SideVector, Math.Atan2(ForwardVector.Y, ForwardVector.X));

                // Get the vector pointing left or right from the forward one
                var ForwardRightVector = new Vector2(ForwardVector.Y, -ForwardVector.X);
                var ForwardLeftVector = -ForwardRightVector;

                // Add the specified vector to the target position
                TargetPosition += SideVector;

            }


            // Fire event when target reached
            if (TargetDistance < (FinishDistance <= 0 ? 50 : FinishDistance))
                GoToArrived?.Invoke(Target);

            return TargetPosition;
        }

        private Vector2 RotateVector2(Vector2 vec, double radians)
        {
            Vector2 result = new Vector2();
            result.X = (float)(vec.X * Math.Cos(radians) - vec.Y * Math.Sin(radians));
            result.Y = (float)(vec.X * Math.Sin(radians) + vec.Y * Math.Cos(radians));
            return result;
        }
    }
}
