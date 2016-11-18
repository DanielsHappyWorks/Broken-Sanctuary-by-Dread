
using GDApp;
using Microsoft.Xna.Framework;
namespace GDLibrary
{
    public class Controller : IController
    {
        #region Variables
        public static Main game;
        private string id;
        private ControllerType controllerType;
        #endregion

        #region Properties
        public string ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
        public ControllerType ControllerType
        {
            get
            {
                return this.controllerType;
            }
            set
            {
                this.controllerType = value;
            }
        }
        #endregion

        public Controller(string id, ControllerType controllerType)
        {
            this.id = id;
            this.controllerType = controllerType;
        }

        public virtual void Update(GameTime gameTime, IActor actor)
        {
            //does nothing so no point in child classes calling this.
        }

        public virtual string GetID()
        {
            return this.ID;
        }
    }
}
