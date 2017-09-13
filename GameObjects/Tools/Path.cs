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

        public Vector2 Target { get; private set; }

        /// <summary>
        /// Contains all the waypoints of this path
        /// </summary>
        public Vector2[] Waypoints { get; private set; }

        private int CurrentWaypoint = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Source">Source of the path</param>
        /// <param name="Target">Target of the path</param>
        /// <param name="FunctionIn">Easing function for engaging the curve</param>
        /// <param name="FunctionOut">Easing function for ending the curve</param>
        /// <param name="CurveSide">Curve side of the easing function</param>
        public Path(Vector2 Source, Vector2 Target, Tools.Easing.EaseFunction FunctionIn, Tools.Easing.EaseFunction FunctionOut, CurveSides CurveSide)
        {
            this.Source = Source;
            this.Target = Target;

            var CurveStart = (Source - Target).Length();
            GeneratePath(Source, Target, FunctionIn, FunctionOut, CurveSide, CurveStart, 0, CurveStart * 0.5f, 1);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Source">Source of the path</param>
        /// <param name="Target">Target of the path</param>
        /// <param name="FunctionIn">Easing function for engaging the curve</param>
        /// <param name="FunctionOut">Easing function for ending the curve</param>
        /// <param name="CurveSide">Curve side of the easing function</param>
        /// <param name="CurveStart">Distance to target, the curve should start at</param>
        /// <param name="CurveEnd">Distance to target, the curve should end a</param>
        /// <param name="CurveWidth">Width of the curve</param>
        public Path(Vector2 Source, Vector2 Target, Tools.Easing.EaseFunction FunctionIn, Tools.Easing.EaseFunction FunctionOut, CurveSides CurveSide, float CurveStart, float CurveEnd, float CurveWidth)
        {
            this.Source = Source;
            this.Target = Target;

            GeneratePath(Source, Target, FunctionIn, FunctionOut, CurveSide, CurveStart, CurveEnd, CurveWidth, 1);
        }


        private void GeneratePath(Vector2 Source, Vector2 Target, Tools.Easing.EaseFunction FunctionIn, Tools.Easing.EaseFunction FunctionOut, CurveSides CurveSide, float CurveStart, float CurveEnd, float CurveWidth, float Resolution)
        {
            var TargetDistance = (Source - Target).Length();

            var CurveSpan = CurveStart - CurveEnd;

            var ForwardVector = (Target - Source);
            ForwardVector.Normalize();

            var PathSize = (int)(TargetDistance / Resolution);

            this.Waypoints = new Vector2[PathSize];

            var Waypoint = Source;

            for (int i = 0; i < PathSize; i++)
            {
                var CurrentDistance = i * Resolution;

                var addWaypoint = ForwardVector * Resolution;

                Waypoint += addWaypoint;

                // Only curve the path when distance between start and end
                if (CurrentDistance < CurveStart && CurrentDistance > CurveEnd)
                {
                    var firstHalf = CurrentDistance - CurveEnd < CurveSpan / 2;
                    float s = 0;

                    if (firstHalf)
                        s = Tools.Easing.Ease(
                            FunctionIn,
                            CurrentDistance - CurveEnd,
                            0,
                            CurveWidth,
                            CurveSpan / 2);
                    else
                    {
                        if(CurrentDistance == 800)
                        { }
                        s = Tools.Easing.Ease(
                            FunctionOut,
                            (CurrentDistance - CurveEnd) - CurveSpan / 2,
                            0,
                            CurveWidth,
                            CurveSpan / 2);
                        s = (s - CurveWidth) * -1;
                    }


                    var SideVector = new Vector2(0, s);
                    SideVector = RotateVector2(SideVector, Math.Atan2(ForwardVector.Y, ForwardVector.X));

                    // Get the vector pointing left or right from the forward one
                    var ForwardRightVector = new Vector2(ForwardVector.Y, -ForwardVector.X);
                    var ForwardLeftVector = -ForwardRightVector;

                    // Add the specified vector to the target position
                    addWaypoint = SideVector;
                    //addWaypoint.Normalize();
                    //addWaypoint *= Resolution;
                }


                this.Waypoints[i] = Waypoint + addWaypoint;
            }
        }



        /// <summary>
        /// Calculates the next step for this path
        /// </summary>
        /// <param name="Position">Current position</param>
        /// <param name="Target">Target position</param>
        /// <returns>Next step in path</returns>
        public Vector2 CalculateStep(Vector2 Position, Vector2 Target)
        {
            if(Waypoints == null)
                return Vector2.Zero;

            var TargetOffset = Target - this.Target;

            var WaypointDistance = 0f;
            var Waypoint = Vector2.Zero;
            while (WaypointDistance < 1 && Waypoints != null)
            {
                Waypoint = Waypoints[CurrentWaypoint] + TargetOffset;

                WaypointDistance = (Waypoint - Position).Length();


                if (WaypointDistance < 50)
                {
                    CurrentWaypoint++;

                    // Fire event when target reached
                    if (CurrentWaypoint >= Waypoints.Length)
                    {
                        Waypoints = null;
                        CurrentWaypoint = 0;
                        GoToArrived?.Invoke(Target);
                    }
                }
            }

            return (Waypoint - Position);
        }

        /// <summary>
        /// Calculates the next step for this path (simulation)
        /// </summary>
        /// <param name="Position">Current position</param>
        /// <param name="Target">Target position</param>
        /// <returns>Next step in path</returns>
        public Vector2 CalculateStepSim(Vector2 Position, Vector2 Target, int Waypoint)
        {
            // Save
            var OldWaypoint = CurrentWaypoint;
            var OldWaypointList = Waypoints;


            CurrentWaypoint = Waypoint;
            var Result = CalculateStep(Position, Target);

            // Restore
            CurrentWaypoint = OldWaypoint;
            Waypoints = OldWaypointList;

            return Result;
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
