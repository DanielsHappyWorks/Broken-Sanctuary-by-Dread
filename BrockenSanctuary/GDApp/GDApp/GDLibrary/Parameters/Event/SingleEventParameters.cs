using System;

namespace GDLibrary
{
    public class SingleEventParameters : EventParameters, ICloneable
    {
        #region Variables
        private EventData eventData; //should we be able to add multiple events?
        #endregion

        #region Properties
        public EventData EventData
        {
            get
            {
                return this.eventData;
            }
            set
            {
                this.eventData = value;
            }
        }
        #endregion

        //pickups with no text or description or event generation
        public SingleEventParameters(int value)
            : base(value)
        {

        }
        public SingleEventParameters(int value, string text, string description, EventData eventData)
            : base(value, text, description)
        {
            this.eventData = eventData;
        }

        public override void Publish()
        {
            EventDispatcher.Publish(this.eventData);
        }

        public new object Clone() //deep copy
        {
            return new SingleEventParameters(this.Value, this.Text, this.Description,
                (EventData)this.eventData.Clone()); //user-defined class so we need to explicitly call its clone method
        }

        public override bool Equals(object obj)
        {
            SingleEventParameters other = obj as SingleEventParameters;
            return base.Equals(obj) && this.eventData == other.EventData;
        }

        public override int GetHashCode() //a simple hash code method 
        {
            int hash = base.GetHashCode();
            hash = hash * 59 + this.eventData.GetHashCode();
            return hash;
        }
    }
}
