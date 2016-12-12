/*
Function: 		Encapsulates the transformation and World matrix specific parameters for any entity that can have a position e.g. a player, a prop, a camera
Author: 		NMCG
Version:		1.0
Date Updated:	1/10/16
Bugs:			None
Fixes:			None
*/

using System;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class Transform3D : ICloneable
    {
        public static Transform3D Zero = new Transform3D(Vector3.Zero,
                                Vector3.Zero, Vector3.One, -Vector3.UnitZ, Vector3.UnitY);

        #region Fields
        private Vector3 translation, rotation, scale;
        private Vector3 look, up;
        private Matrix world;
        private bool isDirty;
        private Transform3D originalTransform3D;

        //used when moving primitive objects to check for CD/CR
        private Vector3 translateIncrement;
        private float rotateIncrement;
        #endregion

        #region Properties
        public Matrix Orientation
        {
            get
            {
                return Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X))
                        * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))
                            * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z));
            }
        }
        public Matrix World
        {
            set
            {
                this.world = value;
            }
            get
            {
                if (this.isDirty)
                {
                    this.world = Matrix.Identity * Matrix.CreateScale(scale)
                                    * Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X))
                                        * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))
                                            * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z))
                                                * Matrix.CreateTranslation(translation);
                    this.isDirty = false;
                }
                return this.world;
            }
        }
        public Vector3 Translation
        {
            get
            {
                return this.translation;
            }
            set
            {
                this.translation = value;
                this.isDirty = true;
            }
        }
        public Vector3 Rotation
        {
            get
            {
                return this.rotation;
            }
            set
            {
                this.rotation = value;
                this.isDirty = true;
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

        public Vector3 Target
        {
            get
            {
                return this.translation + this.look;
            }
        }
        public Vector3 Up
        {
            get
            {
                return this.up;
            }
            set
            {
                this.up = value;
            }
        }
        public Vector3 Look
        {
            get
            {
                return this.look;
            }
            set
            {
                this.look = value;
            }
        }
        public Vector3 Right
        {
            get
            {
                return Vector3.Normalize(Vector3.Cross(this.look, this.up));
            }
        }
        public Transform3D OriginalTransform3D
        {
            get
            {
                return this.originalTransform3D;
            }
        }
      
        public Vector3 TranslateIncrement
        {
            get
            {
                return translateIncrement;
            }
            set
            {
                translateIncrement = value;
            }
        }
        public float TranslateIncrementY //used by 1st person camera to constrain vertical movement
        {
            get
            {
                return translateIncrement.Y;
            }
            set
            {
                translateIncrement.Y = value;
            }
        }

        public float RotateIncrement
        {
            get
            {
                return rotateIncrement;
            }
            set
            {
                rotateIncrement = value;
            }
        }
        #endregion

        //used by the camera
        public Transform3D(Vector3 translation, Vector3 look, Vector3 up)
            : this(translation, Vector3.Zero, Vector3.One, look, up)
        {

        }

        //used by zone objects
        public Transform3D(Vector3 translation,  Vector3 scale)
            : this(translation, Vector3.Zero, scale, Vector3.UnitX, Vector3.UnitY)
        {

        }

        //used by drawn objects
        public Transform3D(Vector3 translation, Vector3 rotation, Vector3 scale, Vector3 look, Vector3 up)
        {
            this.Translation = translation;
            this.Rotation = rotation;
            this.Scale = scale;

            this.Look = Vector3.Normalize(look);
            this.Up = Vector3.Normalize(up);

            this.originalTransform3D = (Transform3D)this.Clone();
        }

        public void Reset()
        {
            this.translation = this.originalTransform3D.Translation;
            this.rotation = this.originalTransform3D.Rotation;
            this.scale = this.originalTransform3D.Scale;
            this.look = this.originalTransform3D.Look;
            this.up = this.originalTransform3D.Up;
        }

        public override bool Equals(object obj)
        {
            Transform3D other = obj as Transform3D;

            return Vector3.Equals(this.translation, other.Translation)
                && Vector3.Equals(this.rotation, other.Rotation)
                    && Vector3.Equals(this.scale, other.Scale)
                        && Vector3.Equals(this.look, other.Look)
                         && Vector3.Equals(this.up, other.Up);
        }

        public override int GetHashCode() //a simple hash code method 
        {
            int hash = 1;
            hash = hash * 31 + this.translation.GetHashCode();
            hash = hash * 17 + this.look.GetHashCode();
            hash = hash * 13 + this.up.GetHashCode();
            return hash;
        }


        public void RotateBy(Vector3 rotateBy)
        {
            this.rotation = this.OriginalTransform3D.Rotation + rotateBy;

            //update the look and up - RADIANS!!!!
            Matrix rot = Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(this.rotation.X),
                MathHelper.ToRadians(this.rotation.Y), MathHelper.ToRadians(this.rotation.Z));

            this.look = Vector3.Transform(this.originalTransform3D.Look, rot);
            this.up = Vector3.Transform(this.originalTransform3D.Up, rot);

            this.isDirty = true;
        }

        public void RotateAroundYBy(float magnitude)
        {
            this.rotation.Y += magnitude;
            this.look = Vector3.Normalize(Vector3.Transform(this.originalTransform3D.Look,
                Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))));

            this.isDirty = true;
        }
        public void Rotate(Vector3 rotateBy)
        {
            this.rotation += rotateBy;

            //update the look and up - RADIANS!!!!
            Matrix rot = Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(this.rotation.X),
                MathHelper.ToRadians(this.rotation.Y), MathHelper.ToRadians(this.rotation.Z));

            this.look = Vector3.Transform(this.OriginalTransform3D.Look, rot);
            this.up = Vector3.Transform(this.OriginalTransform3D.Up, rot);

            this.isDirty = true;
        }

        public void TranslateBy(Vector3 translateBy)
        {
            TranslateBy(translateBy, 1);
        }

        public void TranslateBy(Vector3 translateBy, float speedMultiplier)
        {
            this.translation += (translateBy * speedMultiplier);
            this.isDirty = true;
        }

        public void ScaleTo(Vector3 scale)
        {
            this.scale = scale;
            this.isDirty = true;
        }

        public void ScaleBy(Vector3 scaleBy)
        {
            this.scale *= scaleBy;
            this.isDirty = true;
        }

        public object Clone() //deep copy - Vector3 are structures (i.e. value types) and so MemberwiseClone() will copy by value and effectively make a deep copy
        {
            return this.MemberwiseClone();
        }
    }
}
