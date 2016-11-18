using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class ThirdPersonController : TargetController
    {
        #region Fields
        private float elevationAngle, distance, scrollSpeedDistanceMultiplier, scrollSpeedElevationMultiplier;

        //used to dampen camera movement
        private Vector3 oldTranslation;
        private Vector3 oldCameraToTarget;
        private float lerpSpeed;
        #endregion

        #region Properties
        public float LerpSpeed
        {
            get
            {
                return lerpSpeed;
            }
            set
            {

                //lerp speed should be in the range >0 and <=1
                lerpSpeed = (value > 0) && (lerpSpeed <= 1) ? value : 0.1f;
            }
        }

        public float ScrollSpeedDistanceMultiplier
        {
            get
            {
                return scrollSpeedDistanceMultiplier;
            }
            set
            {
                //distanceScrollMultiplier should not be lower than 0
                scrollSpeedDistanceMultiplier = (value > 0) ? value : 1;
            }
        }

        public float ScrollSpeedElevationMultiplier
        {
            get
            {
                return scrollSpeedElevationMultiplier;
            }
            set
            {
                //scrollSpeedElevationMulitplier should not be lower than 0
                scrollSpeedElevationMultiplier = (value > 0) ? value : 1;
            }
        }

        public float Distance
        {
            get
            {
                return distance;
            }
            set
            {
                //distance should not be lower than 0
                distance = (value > 0) ? value : 1;
            }
        }
        public float ElevationAngle
        {
            get
            {
                return elevationAngle;
            }
            set
            {
                elevationAngle = value % 360;
                elevationAngle = MathHelper.ToRadians(elevationAngle);
            }
        }
        #endregion

        public ThirdPersonController(string id, ControllerType controllerType, Actor targetActor,
            float distance, float scrollSpeedDistanceMultiplier, float elevationAngle, 
                float scrollSpeedElevationMultiplier, float lerpSpeed)
            : base(id, controllerType, targetActor)
        {
            //call properties to set validation on distance and radian conversion
            this.Distance = distance;

            //allows us to control distance and elevation from the mouse scroll wheel
            this.ScrollSpeedDistanceMultiplier = scrollSpeedDistanceMultiplier;
            this.ScrollSpeedElevationMultiplier = scrollSpeedElevationMultiplier;
            //notice that we pass the incoming angle through the property to convert it to radians
            this.ElevationAngle = elevationAngle;

            //dampen camera movement
            this.lerpSpeed = lerpSpeed; 
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            Actor3D parentActor = actor as Actor3D;
            Actor3D targetActor = this.TargetActor as Actor3D;

            UpdateFromScrollWheel(gameTime);

            UpdateParent(gameTime, parentActor, targetActor);

        }

        private void UpdateParent(GameTime gameTime, Actor3D parentActor, Actor3D targetActor)
        {
            //rotate the target look around the target right to get a vector pointing away from the target at a specified elevation
            Vector3 cameraToTarget = Vector3.Transform(targetActor.Transform3D.Look,
                Matrix.CreateFromAxisAngle(targetActor.Transform3D.Right, elevationAngle));

            //normalize to give unit length, otherwise distance from camera to target will vary over time
            cameraToTarget.Normalize();

            //set the position of the camera to be a set distance from target and at certain elevation angle
            parentActor.Transform3D.Translation = Vector3.Lerp(this.oldTranslation, cameraToTarget * distance + targetActor.Transform3D.Translation, lerpSpeed);

            //set the camera to look at the target object
            parentActor.Transform3D.Look = Vector3.Lerp(this.oldCameraToTarget, cameraToTarget, lerpSpeed);

            //store old values for lerp
            this.oldTranslation = parentActor.Transform3D.Translation;
            this.oldCameraToTarget = -cameraToTarget;
        }

        private void UpdateFromScrollWheel(GameTime gameTime)
        {
            float scrollWheelDelta = -game.MouseManager.GetDeltaFromScrollWheel() * gameTime.ElapsedGameTime.Milliseconds;
            this.Distance += this.scrollSpeedDistanceMultiplier * scrollWheelDelta;
        //    this.ElevationAngle += this.scrollSpeedElevationMultiplier * scrollWheelDelta;
        }

        //add clone...
    }
}
