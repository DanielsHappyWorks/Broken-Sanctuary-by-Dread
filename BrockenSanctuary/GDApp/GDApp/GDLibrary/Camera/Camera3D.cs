using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GDApp;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    //Represents the base camera class to which controllers can be attached (to do...)
    public class Camera3D : Actor3D
    {
        #region Fields
        private ProjectionParameters projectionParameters;
        private Viewport viewPort;
        private Vector2 viewportCentre;
        private int drawDepth;
        #endregion

        #region Properties
        public Matrix View
        {
            get
            {
                return Matrix.CreateLookAt(this.Transform3D.Translation,
                    this.Transform3D.Translation + this.Transform3D.Look,
                    this.Transform3D.Up);
            }
        }
        public Matrix Projection
        {
            get
            {
                return this.projectionParameters.Projection;
            }
        }
        public ProjectionParameters ProjectionParameters
        {
            get
            {
                return this.projectionParameters;
            }
            set
            {
                this.projectionParameters = value;
            }
        }
        public Viewport Viewport
        {
            get
            {
                return this.viewPort;
            }
            set
            {
                this.viewPort = value;
                this.viewportCentre = new Vector2(this.viewPort.Width / 2.0f, this.viewPort.Height / 2.0f);
            }
        }

        public Vector2 ViewportCentre
        {
            get
            {
                return this.viewportCentre;
            }
        }
        public int DrawDepth
        {
            get
            {
                return this.drawDepth;
            }
            set
            {
                this.drawDepth = value;
            }
        }

        #endregion

        //creates a default camera3D - we can use this for a fixed camera archetype i.e. one we will clone - see MainApp::InitialiseCameras()
        public Camera3D(string id, ActorType actorType, Viewport viewPort)
            : this(id, actorType, Transform3D.Zero,
            ProjectionParameters.StandardMediumFourThree, viewPort, 0, StatusType.Updated)
        {

        }

        //forward compatibility (since v3.4) for existing code with no drawDepth
        public Camera3D(string id, ActorType actorType,
         Transform3D transform, ProjectionParameters projectionParameters, Viewport viewPort)
            : this(id, actorType, transform, projectionParameters, 
            viewPort, 0, StatusType.Updated)
        {

        }

        //forward compatibility (since v3.4) for existing code with no StatusType
        public Camera3D(string id, ActorType actorType,
            Transform3D transform, ProjectionParameters projectionParameters,
            Viewport viewPort, int drawDepth)
            : this(id, actorType, transform, projectionParameters, 
            viewPort, drawDepth, StatusType.Updated)
        {
        }

        public Camera3D(string id, ActorType actorType,
            Transform3D transform, ProjectionParameters projectionParameters,
            Viewport viewPort, int drawDepth, StatusType statusType)
            : base(id, actorType, transform, statusType)
        {
            this.projectionParameters = projectionParameters;
            this.viewPort = viewPort;
            this.drawDepth = drawDepth;
        }

        public override bool Equals(object obj)
        {
            Camera3D other = obj as Camera3D;

            return Vector3.Equals(this.Transform3D.Translation, other.Transform3D.Translation)
                && Vector3.Equals(this.Transform3D.Look, other.Transform3D.Look)
                    && Vector3.Equals(this.Transform3D.Up, other.Transform3D.Up)
                        && this.ProjectionParameters.Equals(other.ProjectionParameters);
        }
        public override int GetHashCode() //a simple hash code method 
        {
            int hash = 1;
            hash = hash * 31 + this.Transform3D.Translation.GetHashCode();
            hash = hash * 17 + this.Transform3D.Look.GetHashCode();
            hash = hash * 13 + this.Transform3D.Up.GetHashCode();
            hash = hash * 53 + this.ProjectionParameters.GetHashCode();
            return hash;
        }
        public new object Clone()
        {
            return new Camera3D("clone - " + this.ID,
                this.ActorType, (Transform3D)this.Transform3D.Clone(), 
                (ProjectionParameters)this.projectionParameters.Clone(), this.Viewport);
        }
        public override string ToString()
        {
            return this.ID
                + ", Translation: " + MathUtility.Round(this.Transform3D.Translation, 0)
                + ", Look: " + MathUtility.Round(this.Transform3D.Look, 0)
                + ", Up: " + MathUtility.Round(this.Transform3D.Up, 0);

        }
    }
}

