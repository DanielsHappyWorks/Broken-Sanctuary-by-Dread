using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class RailController : TargetController
    {
        #region Fields
        private RailParameters railParameters;
        private bool bFirstUpdate = true;
        private float lerpSpeed;
        private Vector3 oldCameraToTarget;
        #endregion

        #region Properties
        public RailParameters RailParameters
        {
            get
            {
                return this.railParameters;
            }
            set
            {
                this.railParameters = value;
            }
        }
        public bool FirstUpdate
        {
            get
            {
                return this.bFirstUpdate;
            }
            set
            {
                this.bFirstUpdate = value;
            }
        }
        #endregion

        public RailController(string id, ControllerType controllerType, IActor targetActor, 
                                RailParameters railParameters, float lerpSpeed)
            : base(id, controllerType, targetActor)
        {
            this.railParameters = railParameters;
            this.lerpSpeed = lerpSpeed;
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            Actor3D parentActor = actor as Actor3D;
            DrawnActor3D targetDrawnActor = this.TargetActor as DrawnActor3D;

            if(this.bFirstUpdate)
            {
                //set the initial position of the camera
                parentActor.Transform3D.Translation = railParameters.MidPoint;
                this.bFirstUpdate = false;
            }

            //get look vector to target
            Vector3 cameraToTarget = MathUtility.GetNormalizedObjectToTargetVector(parentActor.Transform3D, targetDrawnActor.Transform3D);

            //new position for camera if it is positioned between start and the end points of the rail
            Vector3 projectedCameraPosition = parentActor.Transform3D.Translation
                + Vector3.Dot(cameraToTarget, railParameters.Look) * railParameters.Look * gameTime.ElapsedGameTime.Milliseconds;

            //do not allow the camera to move outside the rail
            if (railParameters.InsideRail(projectedCameraPosition))
                parentActor.Transform3D.Translation = projectedCameraPosition;

            //set the camera to look at the object
            parentActor.Transform3D.Look = cameraToTarget;

            //set the camera to look at the target object
            parentActor.Transform3D.Look = Vector3.Lerp(this.oldCameraToTarget, cameraToTarget, lerpSpeed);

            //store old values for lerp
            this.oldCameraToTarget = cameraToTarget;

        }


        //add clone...
    }
}
