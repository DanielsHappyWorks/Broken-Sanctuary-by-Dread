
namespace GDLibrary
{
    //extended by cameras that follow a target e.g. ThirdPersonController or RailController
    public class TargetController : Controller
    {
        #region Fields
        private IActor targetActor;
        #endregion

        #region Properties
        public IActor TargetActor
        {
            get
            {
                return targetActor;
            }
            set
            {
                targetActor = value;
            }
        }

        #endregion

        public TargetController(string id, ControllerType controllerType, IActor targetActor)
            : base(id, controllerType)
        {
            this.targetActor = targetActor;
        }

        //add clone...
    }
}
