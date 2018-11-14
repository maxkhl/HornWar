using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Horn_War_II.GameObjects.Tools
{
    /// <summary>
    /// Used for drawing a single bezier curve
    /// </summary>
    class BezierCurve : DrawableGameComponent
    {
        /// <summary>
        /// Points of the curve
        /// </summary>
        public Vector2[] Points
        {
            get
            {
                return _Points;
            }
            private set
            {
                _Points = value;
            }
        }
        private Vector2[] _Points;

        private SpriteBatch _spriteBatch;

        /// <summary>
        /// Creates a new Beziercurve
        /// </summary>
        /// <param name="game">Game the curve will be drawn in</param>
        public BezierCurve(Game game, SpriteBatch spriteBatch) : base(game)
        {
            _spriteBatch = spriteBatch;
            _Points = new Vector2[0];
        }

        /// <summary>
        /// Adds a new point to the curve
        /// </summary>
        /// <param name="newPoint"></param>
        public void AddPoint(Vector2 newPoint)
        {
            Array.Resize(ref _Points, Points.Length + 1);
            _Points[Points.Length - 1] = newPoint;
        }

        /// <summary>
        /// Returns a specific point on the curve
        /// </summary>
        /// <param name="x">Percentual position on the curve (1-100)</param>
        /// <returns>Absolute point on the curve</returns>
        public Vector2 GetPointOnCurve(float x)
        {
            if (Points.Length == 0) return Vector2.Zero;

            Vector2 Target = Points[0];

            List<Vector2> curvePoints = new List<Vector2>(Points);
            while (curvePoints.Count != 1)
            {
                var newCurvePoints = new List<Vector2>();
                for (int i = 0; i < curvePoints.Count - 1; i++)
                {
                    var condensedPoint = (curvePoints[i + 1] - curvePoints[i]) * (x / 100) + curvePoints[i];
                    newCurvePoints.Add(condensedPoint);
                }
                curvePoints = newCurvePoints;
            }

            //for(int i = 0; i < Points.Length - 1; i++)
            //{
            //    var vecA = Points[i];
            //    var vecB = Points[i + 1];

            //    var localVec = Points[i + 1] - Points[i];
            //    Target += localVec * ( x / 100 ) * .5f;
            //}

            return curvePoints[0];
        }

        /// <summary>
        /// Returns a list of points along the curve
        /// </summary>
        /// <param name="Resolution">Resolution of the list (f.e. 100 = 100 points along the curve)</param>
        /// <returns>List of points on the curve</returns>
        public Vector2[] GetCurve(int Resolution)
        {
            if (Points.Length == 0) return new Vector2[0];

            List<Vector2> vecList = new List<Vector2>((int)Resolution);
            float step = 100 / (float)Resolution;
            for (int i = 0; i < Resolution + 1; i++)
            {

                vecList.Add(GetPointOnCurve(step * i));
            }
            return vecList.ToArray();
        }
        

        
    }
}
