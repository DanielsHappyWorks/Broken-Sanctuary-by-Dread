namespace GDLibrary
{
    public class EventData
    {
        #region Fields
        private EventActionType eventType;
        private EventCategoryType eventCategoryType;
        private object sender;
        private string id;
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
        public object Sender
        {
            get
            {
                return this.sender;
            }
            set
            {
                this.sender = value;
            }
        }
        public EventActionType EventType
        {
            get
            {
                return this.eventType;
            }
            set
            {
                this.eventType = value;
            }
        }
        public EventCategoryType EventCategoryType
        {
            get
            {
                return this.eventCategoryType;
            }
            set
            {
                this.eventCategoryType = value;
            }
        }
        #endregion

        public EventData(string id, object sender, EventActionType eventType, EventCategoryType eventCategoryType)
        {
            this.id = id;                           //id of sender
            this.sender = sender;                   //object reference of sender
            this.eventType = eventType;             //is it play, mute, volume, zone?   
            this.eventCategoryType = eventCategoryType; //where did it originate? ui, menu, video
        }

        public object Clone() //deep copy
        {
            return this.MemberwiseClone(); //all primitive types or structs so use MemberwiseClone();
        }


        public override bool Equals(object obj)
        {
            EventData other = obj as EventData;
            return this.id.Equals(other) && this.sender == other.Sender && this.eventType == other.EventType && this.eventCategoryType == other.EventCategoryType;
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 11 + this.id.GetHashCode();
            hash = hash * 31 + this.sender.GetHashCode();
            hash = hash * 47 + this.eventType.GetHashCode();
            hash = hash * 79 + this.eventCategoryType.GetHashCode();
            return hash;
        }
    }
}
