using System;

namespace GDLibrary
{
    public class EventParameters : ICloneable
    {
        #region Variables
        private int value;
        private string text, description;
        #endregion

        #region Properties
        public int Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }
        #endregion

        //pickups with no text or description or event generation
        public EventParameters(int value)
            : this(value, null, null)
        {
        }

        public EventParameters(int value, string text, string description)
        {
            this.value = value;
            this.text = text;
            this.description = description;
        }

        //overridden in single and multi event to publish the event notification
        public virtual void PublishEvent()
        {
        }


        public object Clone() //deep copy of simple C#, XNA or struct types using MemberwiseClone()
        {
            return this.MemberwiseClone(); 
        }

        public override bool Equals(object obj)
        {
            EventParameters other = obj as EventParameters;

            return this.value == other.Value
                && this.text == other.Text
                    && this.description == other.Description;
        }

        public override int GetHashCode() //a simple hash code method 
        {
            int hash = 1;
            hash = hash * 31 + this.value.GetHashCode();
            hash = hash * 17 + this.text.GetHashCode();
            hash = hash * 13 + this.description.GetHashCode();
            return hash;
        }
    }
}
