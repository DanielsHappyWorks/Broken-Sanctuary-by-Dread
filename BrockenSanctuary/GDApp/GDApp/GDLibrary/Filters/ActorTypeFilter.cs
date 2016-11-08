
namespace GDLibrary
{
    public class ActorTypeFilter : IFilter<Actor>
    {
        private ActorType actorType;
        public ActorTypeFilter(ActorType actorType)
        {
            this.actorType = actorType;
        }

        public bool Matches(Actor obj)
        {
            return (this.actorType == obj.ActorType);
        }
    }
}
