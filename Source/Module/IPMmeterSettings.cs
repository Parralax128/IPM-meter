using Celeste.Mod.Entities;
using Celeste.Mod.IPMmeter;
using Monocle;
using static Celeste.Mod.IPMmeter.DisplayComponent;
using static Celeste.Mod.IPMmeter.Meter;

namespace Celeste.Mod.IPMmeter.Module;

public class IPMmeterSettings : EverestModuleSettings
{

    [SettingName("enabled")]
    public bool Enabled { get; set; } = true;


    CalcMethod _calcMethod = CalcMethod.GapSaving;
    [SettingName("calcMethod")]
    [SettingSubText("calcMethodSubtext")]
    public CalcMethod CalcMethod 
    { 
        get => _calcMethod;
        set
        {
            _calcMethod = value;
            Meter.UpdateCalcMethod(value);
        }
    }


    public CarelessAccountingSubMenu CarelessAccounting { get; set; } = new();
    [SettingSubMenu]
    public class CarelessAccountingSubMenu
    {
        public Options Keyboard { get; set; } = Options.OnlyPresses;
        public Options Gamepad { get; set; } = Options.OnlyPresses;
        public Options Analog { get; set; } = Options.All;
        public Options Mouse { get; set; } = Options.OnlyPresses;
    }


    public AccountingInputsSubMenu VanillaAccounting { get; set; } = new AccountingInputsSubMenu();
    [SettingSubMenu]
    public class AccountingInputsSubMenu
    {
        [SettingName("Dash")]
        public Options Dash { get; set; } = Options.OnlyPresses;

        [SettingName("Demo")]
        public Options DemoDash { get; set; } = Options.OnlyPresses;

        [SettingName("Jump")]
        public Options Jump { get; set; } = Options.OnlyPresses;

        [SettingName("Dirs")]
        public Options Directions { get; set; } = Options.OnlyPresses;

        [SettingName("GrabPress")]
        [SettingSubText("GrabPressSubtext")]
        public GrabOptions GrabPress { get; set; } = GrabOptions.All;

        [SettingName("GrabRelease")]
        [SettingSubText("GrabReleaseSubtext")]
        public GrabOptions GrabRelease { get; set; } = GrabOptions.All;
    }

     float BufferSizeRaw = 2.5f;
    [SettingName("bufferSize")]
    [SettingSubText("bufferSizeSubtext")]
    [SettingInGame(false)]
    [SettingNumberInput(false, 5)]
    public float BufferSize
    {
        get => BufferSizeRaw;
        set => BufferSizeRaw = Calc.Clamp(value, 0.1f, 60f);
    }

     float RefreshIntervalRaw = 0.3f;
    [SettingName("refreshInterval")]
    [SettingSubText("refreshIntervalSubtext")]
    [SettingInGame(false)]
    [SettingNumberInput(false)]
    public float DisplayInterval
    {
        get => RefreshIntervalRaw;
        set
        {
            RefreshIntervalRaw = Calc.Clamp(value, 1f/60f, 60f);
            wiggler = Wiggler.Create(RefreshIntervalRaw * 0.75f, 1f / RefreshIntervalRaw);
        }
    }

    [SettingName("timeMode")]
    [SettingSubText("timeModeSubtext")]
    public TimeModes TimeMode { get; set; } = TimeModes.Seconds;

    [SettingName("decimalPartLength")]
    [SettingRange(0, 5)]
    public int DecimalPartLength { get; set; } = 2;

    public TextParametersSubMenu TextCustomization { get; set; } = new TextParametersSubMenu();
    [SettingSubMenu]
    public class TextParametersSubMenu
    {
        [SettingName("posX")]
        [SettingRange(-10, 330, true)]
        public int PosX { get; set; } = 2;

        [SettingName("posY")]
        [SettingRange(-10, 194, true)]
        public int PosY { get; set; } = 155;

        [SettingName("alignment")]
        public Alignment Alignment { get; set; } = Alignment.Left;

        [SettingName("size")]
        [SettingSubText("SizeSubtext")]
        [SettingRange(1, 500, true)]
        public int Size { get; set; } = 100;

        [SettingName("useColorSetting")]
        public UseColorSetting UseColorSetting { get; set; } = UseColorSetting.RGB;

        [SettingName("r")]
        [SettingRange(0, 255, true)]
        public int Red { get; set; } = 255;

        [SettingName("g")]
        [SettingRange(0, 255, true)]
        public int Green { get; set; } = 255;

        [SettingName("b")]
        [SettingRange(0, 255, true)]
        public int Blue { get; set; } = 255;

        [SettingName("hexColor")]
        [SettingMinLength(6)]
        [SettingMaxLength(6)]
        public string HexColor { get; set; } = "FFFFFF";

        [SettingName("outline")]
        [SettingSubText("OutlineSubtext")]
        public bool Outline { get; set; } = true;

        [SettingName("opacity")]
        [SettingRange(0, 100, true)]
        public int Opacity { get; set; } = 100;


        [SettingName("showCounterTitleMod")]
        public TitlePosition ShowCounterTitle { get; set; } = TitlePosition.None;

        [SettingName("customCounterTitle")]
        [SettingSubText("customCounterTitleSubtext")]
        [SettingMinLength(0)]
        [SettingMaxLength(100)]
        public string CustomCounterTitle { get; set; } = "";
    }

    [SettingName("showBG")]
    [SettingSubText("ShowBGSubtext")]
    public bool ShowBG { get; set; } = true;
    public BGSettings BackGroundSettings { get; set; } = new BGSettings();
    [SettingSubMenu]

    public class BGSettings
    {
         float PeakLifeTimeRaw = 2f;
        [SettingName("peakLifetime")]
        [SettingSubText("peakLifetimeSubtext")]
        [SettingInGame(false)]
        [SettingNumberInput(false, 5)]
        public float PeakLifeTime
        {
            get => PeakLifeTimeRaw;
            set => PeakLifeTimeRaw = Calc.Clamp(value, 0f, 10f);
        }

        [SettingName("peakDecreaseSpeed")]
        [SettingSubText("peakDecreaseSpeedSubtext")]
        [SettingRange(1, 100, true)]
        public int PeakDecreaseSpeed { get; set; } = 20;
    }


    [SettingName("showResultRoomCPM")]
    [SettingSubText("showResultRoomCPMSubtext")]
    public bool ShowResultRoom { get; set; } = true;
    public RoomResult ResultRoomValueSettings { get; set; } = new RoomResult();
    [SettingSubMenu]

    public class RoomResult
    {
        [SettingName("resultSize")]
        [SettingSubText("resultSizeSubtext")]
        [SettingRange(1, 500, true)]
        public int ResultSize { get; set; } = 140;


         float DurationRaw = 4f;
        [SettingName("duration")]
        [SettingSubText("durationSubtext")]
        [SettingInGame(false)]
        [SettingNumberInput(false, 5)]
        public float Duration
        {
            get => DurationRaw;
            set => DurationRaw = Calc.Clamp(value, 1f, 10f);
        }

        [SettingName("copyTextColor")]
        public bool CopyTextParams { get; set; } = false;
    }
}