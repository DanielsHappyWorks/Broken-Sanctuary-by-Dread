using System.Collections.Generic;

namespace GDLibrary
{
    //used by the EventDispatcher to compare to events in the HashSet - remember that HashSets allow us to quickly prevent something from being added to a list/stack twice
    public class EventDataEqualityComparer : IEqualityComparer<EventData>
    {

        public bool Equals(EventData e1, EventData e2)
        {
            return e1.ID.Equals(e2.ID)
                && e1.EventType.Equals(e2.EventType)
                    && e1.EventCategoryType.Equals(e2.EventCategoryType)
                        && (e1.Sender as Actor).GetID().Equals(e2.Sender as Actor);

        }

        public int GetHashCode(EventData e)
        {
            return e.GetHashCode();
        }
    }
}
