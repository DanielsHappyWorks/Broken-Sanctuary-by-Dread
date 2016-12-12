
using Microsoft.Xna.Framework.Audio;

namespace GDLibrary
{
    public class SoundEventData : EventData
    {
        #region Fields
        private string cueName;
        private AudioEmitter audioEmitter;
        private AudioStopOptions audioStopOptions;
        #endregion

        #region Properties
        public string CueName
        {
            get
            {
                return this.cueName;
            }
            set
            {
                this.cueName = value;
            }
        }

        public AudioEmitter AudioEmitter
        {
            get
            {
                return this.audioEmitter;
            }
            set
            {
                this.audioEmitter = value;
            }
        }
        public AudioStopOptions AudioStopOptions
        {
            get
            {
                return this.audioStopOptions;
            }
            set
            {
                this.audioStopOptions = value;
            }
        }
        #endregion

        //use when you want to play, pause, resume a 2D OR 3D sound
        public SoundEventData(string id, object sender, EventActionType eventType,
            EventCategoryType eventCategoryType, string cueName)
            : this(id, sender, eventType, eventCategoryType, cueName, AudioStopOptions.Immediate)
        {

        }

        //use when you want to stop a 2D OR 3D sound
        public SoundEventData(string id, object sender, EventActionType eventType,
            EventCategoryType eventCategoryType, string cueName, AudioStopOptions audioStopOptions)
            : this(id, sender, eventType, eventCategoryType, cueName, audioStopOptions, null)
        {

        }

        //use when you want to play a 3D sound
        public SoundEventData(string id, object sender, EventActionType eventType,
          EventCategoryType eventCategoryType, string cueName, AudioEmitter audioEmitter)
          : this(id, sender, eventType, eventCategoryType, cueName, AudioStopOptions.Immediate, audioEmitter)
        {

        }

        //never used - notice it is private
        private SoundEventData(string id, object sender, EventActionType eventType,
            EventCategoryType eventCategoryType, string cueName, AudioStopOptions audioStopOptions, AudioEmitter audioEmitter)
            : base(id, sender, eventType, eventCategoryType)
        {
            this.cueName = cueName;   //"boing
            this.audioStopOptions = audioStopOptions;
            this.audioEmitter = audioEmitter;
        }


        //add GetHashCode and Equals
        public override bool Equals(object obj)
        {
            SoundEventData other = obj as SoundEventData;
            return base.Equals(obj) && this.cueName == other.CueName;
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 11 + this.CueName.GetHashCode();
            hash = hash * 47 + base.GetHashCode();
            return hash;
        }
    }
}
