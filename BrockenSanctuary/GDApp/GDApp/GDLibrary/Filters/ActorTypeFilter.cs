
namespace GDLibrary
{
    public class ActorTypeFilter : IFilter<IActor>
    {
        private ActorType actorType;
        public ActorTypeFilter(ActorType actorType)
        {
            this.actorType = actorType;
        }

        public bool Matches(IActor obj)
        {
            return (this.actorType == obj.GetActorType());
        }
    }
}
