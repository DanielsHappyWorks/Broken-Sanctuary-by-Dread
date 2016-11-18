
namespace GDLibrary
{
    public class CameraEventData : EventData
    {
        #region Fields
        private string cameraLayout;
        private string cameraID;
        #endregion

        #region Properties
        public string CameraID
        {
            get
            {
                return this.cameraID;
            }
            set
            {
                this.cameraID = value;
            }
        }
        public string CameraLayout
        {
            get
            {
                return this.cameraLayout;
            }
            set
            {
                this.cameraLayout = value;
            }
        }
        #endregion

        public CameraEventData(string id, object sender, EventActionType eventType, 
                                EventCategoryType eventCategoryType, string cameraLayout, string cameraID)
            : base(id, sender, eventType, eventCategoryType)
        {
            this.cameraLayout = cameraLayout;   //e.g. fullscreen?
            this.cameraID = cameraID;           //e.g. first person collidable
        }

        //add GetHashCode and Equals
        public override bool Equals(object obj)
        {
            CameraEventData other = obj as CameraEventData;
            return base.Equals(obj) && this.cameraLayout == other.CameraLayout && this.cameraID == other.CameraID;
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 11 + this.cameraLayout.GetHashCode();
            hash = hash * 31 + this.cameraID.GetHashCode();
            hash = hash * 47 + base.GetHashCode();
            return hash;
        }
    }
}
