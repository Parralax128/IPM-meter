using Celeste.Mod.IPMmeter.Module;
using Monocle;
using System;
using Microsoft.Xna.Framework;
using static Celeste.Mod.IPMmeter.Meter;
using System.Collections.Generic;

namespace Celeste.Mod.IPMmeter;

public static class DisplayComponent
{
    public enum Alignment
    {
        Left,
        Center,
        Right
    }
    public enum TitlePosition
    {
        None,
        Before,
        After
    }
    public enum UseColorSetting
    {
        RGB,
        Hex
    }


    public static PixelFont Font => Fonts.Get("Renogare");

    static Dictionary<TimeModes, string> DefaultTitles = new()
    {
        { TimeModes.Frames, "IPF" },
        { TimeModes.Seconds, "IPS" },
        { TimeModes.Minutes, "IPM" }
    };
    static Dictionary<TitlePosition, Func<string, string, string>> FormatTitle = new()
    {
        { TitlePosition.None, (number, title) => number },
        { TitlePosition.Before, (number, title) => $"{title}: {number}" },
        { TitlePosition.After, (number, title) => $"{number} {title}" }
    };

    public static Wiggler wiggler;
    static Vector2 wigglerOffset => new(0f, 12f * wiggler.Value * Scale);

    public static string FormatNumber(float number)
    {
        string customTitle = IPMmeterModule.Settings.TextCustomization.CustomCounterTitle;
        string title = string.IsNullOrEmpty(customTitle) ? DefaultTitles[IPMmeterModule.Settings.TimeMode] : customTitle;


        return FormatTitle[ShowTitle].Invoke(MathF.Round(number * TimeMult, DecimalDigits).ToString() ?? "0", title);
    }

    static float _number = 0f;
    public static float Number 
    { 
        private get => _number;
        set
        {
            if(value > _number) wiggler.Start();
            _number = value;
        }
    }
    public static float Interval => IPMmeterModule.Settings.DisplayInterval;
    public static int DecimalDigits => IPMmeterModule.Settings.DecimalPartLength;
    public static Alignment Align => IPMmeterModule.Settings.TextCustomization.Alignment;

    static int red => IPMmeterModule.Settings.TextCustomization.UseColorSetting == UseColorSetting.RGB ?
        IPMmeterModule.Settings.TextCustomization.Red :
        HexToColor(IPMmeterModule.Settings.TextCustomization.HexColor, 0);
    static int green => IPMmeterModule.Settings.TextCustomization.UseColorSetting == UseColorSetting.RGB ?
        IPMmeterModule.Settings.TextCustomization.Green :
        HexToColor(IPMmeterModule.Settings.TextCustomization.HexColor, 2);
    static int blue => IPMmeterModule.Settings.TextCustomization.UseColorSetting == UseColorSetting.RGB ?
        IPMmeterModule.Settings.TextCustomization.Blue :
        HexToColor(IPMmeterModule.Settings.TextCustomization.HexColor, 4);

    static float opacity => IPMmeterModule.Settings.TextCustomization.Opacity / 100f;
    static Color Color => new (red / 255f * opacity, green / 255f * opacity, blue / 255f * opacity, opacity);
    
    public static Color OpaqueColor => new(red/255f, green/255f, blue/255f);
    
    static float GrayScale => (float)(0.299f * red) + (float)(0.587f * green) + (float)(0.115f * blue);
    public static Color OutlineColor => (GrayScale / 255f > 0.5f) ? Color.Black : Color.White;
    
    public static float Scale => IPMmeterModule.Settings.TextCustomization.Size / 100f;
    public static float TimeMult => IPMmeterModule.Settings.TimeMode switch
    {
        TimeModes.Frames => 1/60f,
        TimeModes.Seconds => 1,
        TimeModes.Minutes => 60f,
        _ => throw new Exception("unreachable")
    };
    public static TitlePosition ShowTitle => IPMmeterModule.Settings.TextCustomization.ShowCounterTitle;
    public static Vector2 Justify => Align switch
    {
        Alignment.Center => new Vector2(0.5f, 0f),
        Alignment.Right => new Vector2(1f, 0f),
        Alignment.Left => new Vector2(0f, 0f),
        _ => Vector2.Zero
    };

    public static Vector2 TextPos => 6 * new Vector2(IPMmeterModule.Settings.TextCustomization.PosX, IPMmeterModule.Settings.TextCustomization.PosY);
    
    static bool DrawOutline => IPMmeterModule.Settings.TextCustomization.Outline;

    public static void Render()
    {
        if (!IPMmeterModule.Settings.Enabled)
            return;

        string resultText = FormatNumber(Number);
        DisplayBackground.Render(Font.Get(64f).Measure(resultText).X * Scale);


        if (DrawOutline)
        {
            Font.DrawEdgeOutline(64, resultText, TextPos + wigglerOffset, Justify,
                new Vector2(Scale), OpaqueColor, 4f, OutlineColor, 1f, OutlineColor);
        }
        else
        {
            Font.Draw(64, resultText, TextPos + wigglerOffset, Justify, new Vector2(Scale), Color);
        }

        RoomResultComponent.Render();
    }
}
