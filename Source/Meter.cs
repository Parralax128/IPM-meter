using Celeste.Mod.IPMmeter.CalcMethods;
using Celeste.Mod.IPMmeter.AccountingMethods;
using Celeste.Mod.IPMmeter.Module;
using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using static Celeste.Mod.IPMmeter.Meter;

namespace Celeste.Mod.IPMmeter;

public static class Extensions
{
    public static bool Press(this Options options)
    {
        return options == Options.All || options == Options.OnlyPresses;
    }
    public static bool Release(this Options options)
    {
        return options == Options.All || options == Options.OnlyReleases;
    }
    
}
public class Meter : Entity
{
    public enum CalcMethod
    {
        GapSaving,
        InputSaving,
        Utoog,
        ByRoom
    }
    public enum Options
    {
        All,
        OnlyPresses,
        OnlyReleases,
        None,
    }
    


    public enum GrabOptions
    {
        All,
        OnlyNormal,
        OnlyHoldable,
        None
    }
    
    
    static Accounter Accounter;
    static Calculator Calculator;

    static float displayIntervalTimer = 0f;


    public static int HexToColor(string input, int num)
    {
        int result = 0;
        for(int c=num; c<num+2; c++)
        {
            if (input[c] == '1')
                result += (int)MathF.Pow(16, 1 - (c - num));
            else if (input[c] == '2')
                result += 2*(int)MathF.Pow(16, 1 - (c - num));
            else if (input[c] == '3')
                result += 3*(int)MathF.Pow(16, 1 - (c - num));
            else if (input[c] == '4')
                result += 4*(int)MathF.Pow(16, 1 - (c - num));
            else if (input[c] == '5')
                result += 5*(int)MathF.Pow(16, 1 - (c - num));
            else if (input[c] == '6')
                result += 6*(int)MathF.Pow(16, 1 - (c - num));
            else if (input[c] == '7')
                result += 7*(int)MathF.Pow(16, 1 - (c - num));
            else if (input[c] == '8')
                result += 8*(int)MathF.Pow(16, 1 - (c - num));
            else if (input[c] == '9')
                result += 9*(int)MathF.Pow(16, 1 - (c - num));
            else if (input[c] == 'A' || input[c] == 'a')
                result += 10*(int)MathF.Pow(16, 1 - (c - num));
            else if (input[c] == 'B' || input[c] == 'b')
                result += 11*(int)MathF.Pow(16, 1 - (c - num));
            else if (input[c] == 'C' || input[c] == 'c')
                result += 12*(int)MathF.Pow(16, 1 - (c - num));
            else if (input[c] == 'D' || input[c] == 'd')
                result += 13*(int)MathF.Pow(16, 1 - (c - num));
            else if (input[c] == 'E' || input[c] == 'e')
                result += 14*(int)MathF.Pow(16, 1 - (c - num));
            else if (input[c] == 'F' || input[c] == 'f')
                result += 15*(int)MathF.Pow(16, 1 - (c - num));
        }
        return result;
    }

    public Meter()
    {
        AddTag(Tags.HUD | Tags.Global | Tags.TransitionUpdate | Tags.FrozenUpdate | Tags.Persistent | Tags.PauseUpdate);

        DisplayComponent.wiggler = Wiggler.Create(IPMmeterModule.Settings.DisplayInterval * 0.75f, 1f / IPMmeterModule.Settings.DisplayInterval);
        DisplayComponent.wiggler.StartZero = true;
        DisplayComponent.wiggler.UseRawDeltaTime = true;

        Accounter = new Careless();
        Calculator = Calculators[IPMmeterModule.Settings.CalcMethod];
    }
    public static void UpdateCalcMethod(CalcMethod method) => Calculator = Calculators[method];

    public static readonly Dictionary<CalcMethod, Calculator> Calculators = new()
    {
        { CalcMethod.GapSaving, new GapSaving() },
        { CalcMethod.InputSaving, new InputSaving() },
        { CalcMethod.Utoog, new Utoog() },
        { CalcMethod.ByRoom, new Room() }
    };

    public override void Update()
    {
        if (!IPMmeterModule.Settings.Enabled) return;

        DisplayComponent.wiggler.Update();

        Player player = Scene.Tracker.GetEntity<Player>();
        int inputs = Accounter.Update(IPMmeterModule.Settings, player);
        
        Calculator.Update(inputs);

        if (player != null && player.JustRespawned)
        {
            Calculator.OnRespawn();
            RoomResultComponent.OnPlayerRespawn(player);
        }

        RoomResultComponent.AfterUpdate(inputs, player);
    }


    public override void Render()
    {
        displayIntervalTimer += Engine.DeltaTime;
        if (displayIntervalTimer >= DisplayComponent.Interval)
        {
            DisplayComponent.Number = Calculator.GetDisplayValue();
            displayIntervalTimer = 0f;
        }

        DisplayComponent.Render();
    }
}