using Microsoft.Xna.Framework;
using System;

namespace GDLibrary
{
    /*
     * Allow the user to pass in offsets for a 3D curve so that platforms can use the same curve but
     * operate "out of sync" by the offsets specified. 
     */
    public class Transform3DCurveOffsets : ICloneable
    {
        public static Transform3DCurveOffsets Zero = new Transform3DCurveOffsets(Vector3.Zero, Vector3.One, 0, 0);

        #region Fields
        private Vector3 position, scale;
        private float rotation;
        private float timeInSecs;
        #endregion

        #region Properties
        public Vector3 Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
            }
        }
        public Vector3 Scale
        {
            get
            {
                return this.scale;
            }
            set
            {
                this.scale = value;
            }
        }
        public float Rotation
        {
            get
            {
                return this.rotation;
            }
            set
            {
                this.rotation = value;
            }
        }
        public float TimeInSecs
        {
            get
            {
                return this.timeInSecs;
            }
            set
            {
                this.timeInSecs = value;
            }
        }
        public float TimeInMs
        {
            get
            {
                return this.timeInSecs * 1000;
            }
        }
        #endregion

        public Transform3DCurveOffsets(Vector3 position, Vector3 scale, float rotation, float timeInSecs)
        {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
            this.timeInSecs = timeInSecs;
        }
        public object Clone()
        {
            return this.MemberwiseClone(); //simple C# or XNA types so use MemberwiseClone()
        }
    }

    //Represents a 3D point on a camera curve (i.e. position, look, and up) at a specified time in seconds
    public class Transform3DCurve
    {
        #region Variables
        private Curve3D translationCurve, lookCurve, upCurve;
        #endregion

        public Transform3DCurve(CurveLoopType curveLoopType)
        {
            this.translationCurve = new Curve3D(curveLoopType);
            this.lookCurve = new Curve3D(curveLoopType);
            this.upCurve = new Curve3D(curveLoopType);
        }
        public void Add(Vector3 translation, Vector3 look, Vector3 up, float timeInSecs)
        {
            this.translationCurve.Add(translation, timeInSecs);
            this.lookCurve.Add(look, timeInSecs);
            this.upCurve.Add(up, timeInSecs);
        }
        public void Clear()
        {
            this.translationCurve.Clear();
            this.lookCurve.Clear();
            this.upCurve.Clear();
        }

        //add clone...

        //See https://msdn.microsoft.com/en-us/library/t3c3bfhx.aspx for information on using the out keyword
        public void Evalulate(float timeInSecs, int precision,
            out Vector3 translation, out Vector3 look, out Vector3 up)
        {
            translation = this.translationCurve.Evaluate(timeInSecs, precision);
            look = this.lookCurve.Evaluate(timeInSecs, precision);
            up = this.upCurve.Evaluate(timeInSecs, precision);
        }
    }
}
