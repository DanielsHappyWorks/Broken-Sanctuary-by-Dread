
namespace GDLibrary
{
    public enum EventActionType : sbyte
    {
        //sent by menu, audio, video
        OnPlay,
        OnStop,
        OnPause,
        OnExit,
        OnRestart,
        OnVolumeUp,
        OnVolumeDown,
        OnMute,
        OnClick,
        OnHover,

        //sounds - 2D
        OnPlay2DCue,
        OnPause2DCue,
        OnResume2DCue,
        OnStop2DCue,

        //sounds - 3D
        OnPlay3DCue,
        OnPause3DCue,
        OnResume3DCue,
        OnStop3DCue,
        OnStopAll3DCues,

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

        //transparency changes in objects
        OnAlpha,

        //all other events...
        OnTextRender,

        //pickups
        PickUpKey,
        WinGame,
        LoseGame,
        PhoneInteraction,
        KettleInteraction,
        MicrowaveInteraction,
        ChairInteraction,
        ToasterInteraction,
        FridgeInteraction,
        OvenInteraction,
        ClockInteraction,
        DoorInteraction,
        PaintingGoodInteraction,
        PaintingBadInteraction,
        PaintingOtherInteraction,
        SinkInteraction,
        MirrorInteraction,
        ToiletInteraction,
        RadioInteraction,
        TVInteraction,
        RemoteInteraction,
        VinoInteraction,

        //all other events...

    }
}
