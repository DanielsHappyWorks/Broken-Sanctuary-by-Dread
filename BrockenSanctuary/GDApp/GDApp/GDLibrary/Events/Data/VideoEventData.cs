
namespace GDLibrary
{
    public class VideoEventData : EventData
    {
        #region Variables
        private string name; //video to use
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return name;
            }
        }
        #endregion

        //used when we wish to specify a camera to be set as active
        public VideoEventData(string id, object sender, EventActionType eventActionType, EventCategoryType eventCategoryType, string name)
            : base(id, sender, eventActionType, eventCategoryType)
        {
            this.name = name;       //e.g. target video name to play/pause/restart
        }

        public override string ToString()
        {
            return base.ToString() + "VideoEventData - Name: " + this.name;
        }

        //add GetHashCode and Equals
        public override bool Equals(object obj)
        {
            VideoEventData other = obj as VideoEventData;
            return base.Equals(obj) && this.name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 11 + this.name.GetHashCode();
            hash = hash * 47 + base.GetHashCode();
            return hash;
        }
    }
}
