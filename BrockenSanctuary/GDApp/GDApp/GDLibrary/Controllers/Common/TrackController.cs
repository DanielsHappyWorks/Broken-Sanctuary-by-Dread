using Microsoft.Xna.Framework;
namespace GDLibrary
{
    //used to cause an object (or camera) to move along a predefined track
    public class TrackController : Controller
    {
        #region Variables
        private Transform3DCurve transform3DCurve;
        private StatusType playState;
        private float elapsedTimeInMs;
        #endregion

        #region Properties
        public Transform3DCurve Transform3DCurve
        {
            get
            {
                return this.transform3DCurve;
            }
            set
            {
                this.transform3DCurve = value;
            }
        }
        public StatusType PlayStateType
        {
            get
            {
                return this.playState;
            }
            set
            {
                this.playState = value;
            }
        }
        #endregion


        public TrackController(string id, 
            ControllerType controllerType,
            Transform3DCurve transform3DCurve, StatusType playState)
            : base(id, controllerType)
        {
            this.transform3DCurve = transform3DCurve;
            this.playState = playState;
            this.elapsedTimeInMs = 0;
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            Actor3D parentActor = actor as Actor3D;

            if (this.playState == StatusType.Play)
                UpdateTrack(gameTime, parentActor);
            else if ((this.playState == StatusType.Reset) || (this.playState == StatusType.Stop))
                this.elapsedTimeInMs = 0;
        }

        private void UpdateTrack(GameTime gameTime, Actor3D parentActor)
        {
            if (parentActor != null)
            {
                this.elapsedTimeInMs += gameTime.ElapsedGameTime.Milliseconds;

                Vector3 translation, look, up;
                this.transform3DCurve.Evalulate(elapsedTimeInMs, 2,
                    out translation, out look, out up);

                parentActor.Transform3D.Translation = translation;
                parentActor.Transform3D.Look = look;
                parentActor.Transform3D.Up = up;
            }
        }

        //add clone...
    }
}

