
namespace GDLibrary
{
    public class ActorIDFilter : IFilter<IActor>
    {
        private string id;
        public ActorIDFilter(string id)
        {
            this.id = id;
        }

        public bool Matches(IActor obj)
        {
            return this.id.Equals(obj.getID());
        }
    }
}
