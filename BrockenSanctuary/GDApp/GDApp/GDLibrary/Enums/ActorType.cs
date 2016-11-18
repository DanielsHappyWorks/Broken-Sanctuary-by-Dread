namespace GDLibrary
{
    public enum ActorType : sbyte
    {
        Props, //interact
        Pickup, //ammo
        Player,
        Decorator, //building
        Camera,
        Zone, //invisible and triggers event
        Helper,
        CollidableProp,
        CollidableGround,
        CollidableCamera
    }
}
