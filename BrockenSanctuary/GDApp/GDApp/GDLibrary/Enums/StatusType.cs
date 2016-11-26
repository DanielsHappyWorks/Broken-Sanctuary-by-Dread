
namespace GDLibrary
{
    public enum StatusType
    {
        //used for enabling objects for updating and drawing e.g. a model or a camera, or a controller
        Drawn = 1,
        Updated = 2,

        //used for media
        Play = 4,
        Pause = 8,
        Stop = 16,
        Reset = 32

        /*
         * Q. Why do we use powers of 2? will it allow us to do anything different?
         * A. StatusType.Updated | StatusType.Drawn - See ObjectManager::Update() or Draw()
         */ 

    }
}
