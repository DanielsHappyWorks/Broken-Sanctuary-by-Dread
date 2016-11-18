
namespace GDLibrary
{
    public enum EventActionType : sbyte
    {
        //sent by menu, audio, video
        OnPlay,
        OnExit,
        OnStop,
        OnPause,
        OnRestart,
        OnVolumeUp,
        OnVolumeDown,
        OnMute,
        OnClick,
        OnHover,

        //zones
        OnZoneEnter,
        OnZoneExit,

        //camera
        OnCameraChanged,

        //player or  nonplayer
        OnLoseHealth,
        OnGainHealth,
        OnLose,
        OnWin,

        //pickups
        OnPickup,

        //all other events...

    }
}
