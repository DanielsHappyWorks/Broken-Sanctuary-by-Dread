using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GDApp;

namespace GDLibrary
{
    public class EventDispatcher : GameComponent
    {
        private static Stack<EventData> stack;
        private static HashSet<EventData> uniqueSet;

        //a delegate is basically a list - the list contains a pointer to a function - this function pointer comes from the object wishing to be notified when the event occurs.
        public delegate void CameraEventHandler(EventData eventData);
        public delegate void PickupEventHandler(EventData eventData);

        //an event is either null (not yet happened) or non-null - when the event occurs the delegate reads through its list and calls all the listening functions
        public event CameraEventHandler CameraChanged;
        public event PickupEventHandler PickupChanged;

        public EventDispatcher(Main game, int initialSize)
            : base(game)
        {
            stack = new Stack<EventData>(initialSize);
            uniqueSet = new HashSet<EventData>(new EventDataEqualityComparer());
        }

        public static void Publish(EventData eventData)
        {
            //this prevents the same event being added multiple times within a single update e.g. 10x bell ring sounds
            if (!uniqueSet.Contains(eventData))
            {
                stack.Push(eventData);
                uniqueSet.Add(eventData);
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < stack.Count; i++)
                Process(stack.Pop());

            stack.Clear();
            uniqueSet.Clear();

            base.Update(gameTime);
        }

        private void Process(EventData eventData)
        {
            //Switch - See https://msdn.microsoft.com/en-us/library/06tc147t.aspx
            //one case for each category type
            switch (eventData.EventCategoryType)
            {
                case EventCategoryType.Camera:
                    OnCamera(eventData);
                    break;

                case EventCategoryType.Pickup:
                    OnPickup(eventData);
                    break;

                //add a case to handle the On...() method for each type

                default:
                    break;
            }
        }

        //called when a pickup is collected
        private void OnPickup(EventData eventData)
        {
            //non-null if an object has subscribed to this event
            if (PickupChanged != null)
                PickupChanged(eventData);
        }

      
        //called when a camera event needs to be generated
        protected virtual void OnCamera(EventData eventData)
        {
            if (CameraChanged != null)
                CameraChanged(eventData);
        }
    }
}
