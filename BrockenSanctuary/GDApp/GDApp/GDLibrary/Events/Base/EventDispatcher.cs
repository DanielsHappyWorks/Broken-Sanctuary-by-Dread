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
        public delegate void MenuEventHandler(EventData eventData);
        public delegate void SoundEventHandler(EventData eventData);
        public delegate void OpacityEventHandler(EventData eventData);
        public delegate void VideoEventHandler(EventData eventData);
        public delegate void TextRenderEventHandler(EventData eventData);
        public delegate void InteractionEventHandler(EventData eventData);

        //an event is either null (not yet happened) or non-null - when the event occurs the delegate reads through its list and calls all the listening functions
        public event CameraEventHandler CameraChanged;
        public event PickupEventHandler PickupChanged;
        public event MenuEventHandler MenuChanged;
        public event SoundEventHandler SoundChanged;
        public event OpacityEventHandler OpacityChanged;
        public event VideoEventHandler VideoChanged;
        public event TextRenderEventHandler TextRenderChanged;
        public event InteractionEventHandler Interaction;

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

                case EventCategoryType.MainMenu:
                    OnMenu(eventData);
                    break;

                case EventCategoryType.Sound:
                    OnSound(eventData);
                    break;

                case EventCategoryType.OpacityChange:
                    OnOpacity(eventData);
                    break;

                case EventCategoryType.Video:
                    OnVideo(eventData);
                    break;

                case EventCategoryType.TextRender:
                    OnTextRender(eventData);
                    break;
                case EventCategoryType.Interaction:
                    OnInteraction(eventData);
                    break;

                //add a case to handle the On...() method for each type

                default:
                    break;
            }
        }

        //called when a video event needs to be generated e.g. play, pause, restart
        protected virtual void OnVideo(EventData eventData)
        {
            if (VideoChanged != null)
                VideoChanged(eventData);
        }

        //called when a text renderer event needs to be generated e.g. alarm in sector 2
        protected virtual void OnTextRender(EventData eventData)
        {
            //non-null if an object has subscribed to this event
            if (TextRenderChanged != null)
                TextRenderChanged(eventData);
        }

        //called when an object changes its opacity level
        protected virtual void OnOpacity(EventData eventData)
        {
            //non-null if an object has subscribed to this event
            if (OpacityChanged != null)
                OpacityChanged(eventData);

        }
        //called when a menu change is requested
        protected virtual void OnMenu(EventData eventData)
        {
            //non-null if an object has subscribed to this event
            if (MenuChanged != null)
                MenuChanged(eventData);
        }

        //called when a sound change is requested
        protected virtual void OnSound(EventData eventData)
        {
            //non-null if an object has subscribed to this event
            if (SoundChanged != null)
                SoundChanged(eventData);
        }

        //called when a pickup is collected
        protected virtual void OnPickup(EventData eventData)
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
        protected virtual void OnInteraction(EventData eventData)
        {
            if (Interaction != null)
                Interaction(eventData);
        }
    }
}
