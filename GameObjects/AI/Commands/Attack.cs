//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Xna.Framework;

//namespace Horn_War_II.GameObjects.AI.Commands
//{
//    /// <summary>
//    /// Lets the AI attack a target
//    /// </summary>
//    class Attack : Command
//    {

//        /// <summary>
//        /// Character that is being attacked
//        /// </summary>
//        public Character Target { get; private set; }

//        /// <summary>
//        /// Path for this Attack command
//        /// </summary>
//        public Tools.Path Path { get; private set; }

//        /// <summary>
//        /// Constructor
//        /// </summary>
//        /// <param name="TargetAI">AI, this command is used for</param>
//        public Attack(Character TargetCharacter, AI TargetAI) : base(TargetAI)
//        {
//            this.Target = Target;

//            if (TargetAI.Character.Weapon is Weapons.Horn)
//                Path = new Tools.Path(new Vector2[2] { TargetAI.Character.Position, TargetCharacter.Position }, true);
//        }

//        /// <summary>
//        /// Update loop for this command
//        /// </summary>
//        /// <param name="gameTime">Current gametime</param>
//        public override void Update(GameTime gameTime)
//        {   
            
//        }

//        public override Waypoint? RequestWaypoint()
//        {
//            return Waypoint.Zero;
//        }




//        /// <summary>
//        /// Generates a curved path based on the given parameters
//        /// </summary>
//        /// <param name="Source">Source of the path</param>
//        /// <param name="Target">Target of the path</param>
//        /// <param name="FunctionIn">Easing function first half</param>
//        /// <param name="FunctionOut">Easing function last half</param>
//        /// <param name="LeftCurve">Is it a left curve? Otherwise right</param>
//        /// <param name="CurveStart">Distance to target, when the courve should start</param>
//        /// <param name="CurveEnd">Distance to target, when the courve should end</param>
//        /// <param name="CurveWidth">Width of the curve</param>
//        /// <param name="Resolution">Resolution (1 = 1 pixel per waypoint)</param>
//        /// <returns>Calculated path as array (can be used with path-class)</returns>
//        private Vector2[] GeneratePath(Vector2 Source, Vector2 Target, Tools.Easing.EaseFunction FunctionIn, Tools.Easing.EaseFunction FunctionOut, bool LeftCurve, float CurveStart, float CurveEnd, float CurveWidth, float Resolution)
//        {

//            var TargetDistance = (Source - Target).Length();

//            var CurveSpan = CurveStart - CurveEnd;

//            var ForwardVector = (Target - Source);
//            ForwardVector.Normalize();

//            var PathSize = (int)(TargetDistance / Resolution);

//            var Waypoints = new Vector2[PathSize];

//            var Waypoint = Source;

//            for (int i = 0; i < PathSize; i++)
//            {
//                var CurrentDistance = i * Resolution;

//                var addWaypoint = ForwardVector * Resolution;

//                Waypoint += addWaypoint;

//                // Only curve the path when distance between start and end
//                if (CurrentDistance < CurveStart && CurrentDistance > CurveEnd)
//                {
//                    var firstHalf = CurrentDistance - CurveEnd < CurveSpan / 2;
//                    float s = 0;

//                    if (firstHalf)
//                        s = Tools.Easing.Ease(
//                            FunctionIn,
//                            CurrentDistance - CurveEnd,
//                            0,
//                            CurveWidth,
//                            CurveSpan / 2);
//                    else
//                    {
//                        if (CurrentDistance == 800)
//                        { }
//                        s = Tools.Easing.Ease(
//                            FunctionOut,
//                            (CurrentDistance - CurveEnd) - CurveSpan / 2,
//                            0,
//                            CurveWidth,
//                            CurveSpan / 2);
//                        s = (s - CurveWidth) * -1;
//                    }


//                    var SideVector = new Vector2(0, s);
//                    SideVector = RotateVector2(SideVector, Math.Atan2(ForwardVector.Y, ForwardVector.X));

//                    // Get the vector pointing left or right from the forward one
//                    var ForwardRightVector = new Vector2(ForwardVector.Y, -ForwardVector.X);
//                    var ForwardLeftVector = -ForwardRightVector;

//                    // Add the specified vector to the target position
//                    addWaypoint = SideVector;
//                    //addWaypoint.Normalize();
//                    //addWaypoint *= Resolution;
//                }


//                Waypoints[i] = Waypoint + addWaypoint;
//            }
//            return Waypoints;
//        }

//        /// <summary>
//        /// Rotates a 2-dimensional vector around the given rad
//        /// </summary>
//        private Vector2 RotateVector2(Vector2 vec, double radians)
//        {
//            Vector2 result = new Vector2();
//            result.X = (float)(vec.X * Math.Cos(radians) - vec.Y * Math.Sin(radians));
//            result.Y = (float)(vec.X * Math.Sin(radians) + vec.Y * Math.Cos(radians));
//            return result;
//        }
//    }
//}
