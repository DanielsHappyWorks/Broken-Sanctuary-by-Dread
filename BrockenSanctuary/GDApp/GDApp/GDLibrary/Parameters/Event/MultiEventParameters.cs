using System;
using System.Collections.Generic;

namespace GDLibrary
{
    public class MultiEventParameters : EventParameters, ICloneable
    {
        #region Variables
        private GenericList<EventData> eventDataList; //should we be able to add multiple events?
        #endregion

        #region Properties
        public GenericList<EventData> EventDataList
        {
            get
            {
                return this.eventDataList;
            }
            set
            {
                this.eventDataList = value;
            }
        }
        #endregion

        //pickups with no text or description or event generation
        public MultiEventParameters(int value)
            : this(value, null, null)
        {

        }
        public MultiEventParameters(int value, string text, string description)
            : base(value, text, description)
        {
            this.eventDataList = new GenericList<EventData>();
        }

        public override void Publish()
        {
            foreach(EventData eventData in this.eventDataList)
                EventDispatcher.Publish(eventData);
        }
        public new object Clone() //deep copy
        {
            return null;// new MultiEventParameters(this.Value, this.Text, this.Description,
                // (EventData)this.eventData.Clone()); //user-defined class so we need to explicitly call its clone method
        }

        public override bool Equals(object obj)
        {
            MultiEventParameters other = obj as MultiEventParameters;

            return base.Equals(obj) && this.eventDataList == other.EventDataList;
        }

        public override int GetHashCode() //a simple hash code method 
        {
            int hash = base.GetHashCode();
            hash = hash * 59 + this.eventDataList.GetHashCode();
            return hash;
        }
    }
}
