
namespace GDLibrary
{
    public class ActorIDFilter : IFilter<Actor>
    {
        private string id;
        public ActorIDFilter(string id)
        {
            this.id = id;
        }

        public bool Matches(Actor obj)
        {
            return this.id.Equals(obj.GetID());
        }
    }
}
