using System;


namespace Celeste.Mod.IPMmeter.Module;

public class IPMmeterModule : EverestModule
{
    public static IPMmeterModule Instance { get;  set; }

    public override Type SettingsType => typeof(IPMmeterSettings);
    public static IPMmeterSettings Settings => (IPMmeterSettings)Instance._Settings;

    public override Type SessionType => typeof(IPMmeterSession);
    public static IPMmeterSession Session => (IPMmeterSession)Instance._Session;

    public override Type SaveDataType => typeof(IPMmeterSaveData);
    public static IPMmeterSaveData SaveData => (IPMmeterSaveData)Instance._SaveData;

    public IPMmeterModule()
    {
        Instance = this;
        Logger.SetLogLevel(nameof(IPMmeterModule), LogLevel.Verbose);
    }

    static void OnLoadingThread(Level level)
    {
        level.Add(new Meter());
    }
    public override void Load() 
    {
        Everest.Events.LevelLoader.OnLoadingThread += OnLoadingThread;
    }

    public override void Unload() 
    {
        Everest.Events.LevelLoader.OnLoadingThread -= OnLoadingThread;
    }
}