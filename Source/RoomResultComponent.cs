using Celeste.Mod.IPMmeter.CalcMethods;
using Celeste.Mod.IPMmeter.Module;
using FMOD;
using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static Celeste.Mod.IPMmeter.DisplayComponent;

namespace Celeste.Mod.IPMmeter;

static class RoomResultComponent
{
    public class RoomResult
    {
        public float timer;
        public Vector2 position;
        public string text;

        public RoomResult(float number)
        {
            timer = 0f;
            position = TextPos;
            text = FormatNumber(number);
        }
    }

    public static bool Enabled => IPMmeterModule.Settings.ShowResultRoom;
    static float ShowDuration => IPMmeterModule.Settings.ResultRoomValueSettings.Duration;
    
    static SimpleCurve positionCurve => new SimpleCurve(TextPos,
        new Vector2(TextPos.X, TextPos.Y - 84f*Scale * (IPMmeterModule.Settings.ResultRoomValueSettings.ResultSize / 140f)),
        new Vector2(TextPos.X, TextPos.Y - 60f*Scale));
     
    static SimpleCurve opacityCurve = new SimpleCurve(Vector2.Zero, new Vector2(2f, 0f), new Vector2(1.3f, 2f));
    static SimpleCurve scaleCurve = new SimpleCurve(Vector2.UnitX, 
        new Vector2(2f, IPMmeterModule.Settings.ResultRoomValueSettings.ResultSize / 100f),
        new Vector2(1f, ((IPMmeterModule.Settings.ResultRoomValueSettings.ResultSize / 50f) + 1f) / 3f));

    static List<RoomResult> Results = new();

    static CalcMethods.Room Calculator = new();
    static Vector2? prevRespawnPoint;

    public static void AfterUpdate(int inputs, Player player)
    {
        Calculator.Update(inputs);

        if (player == null) return;
        if (player.level.Session.RespawnPoint != prevRespawnPoint)
        {
            // spawn point changed
            Results.Add(new RoomResult(Calculator.GetDisplayValue()));
            Calculator.OnRespawn();
        }

        prevRespawnPoint = player.level.Session.RespawnPoint;


        if (Results.Count > 0)
        {
            for (int c = Results.Count - 1; c >= 0; c--)
            {
                Results[c].timer += Engine.DeltaTime;
                if (Results[c].timer > ShowDuration)
                {
                    Results.RemoveAt(c);
                }
            }
        }
    }
    public static void OnPlayerRespawn(Player player)
    {
        Calculator.OnRespawn();
    }
    public static void Render()
    {
        if (Results.Count == 0 || !Enabled) return;

        foreach(RoomResult result in Results)
        {
            float progress = result.timer / ShowDuration;
            result.position = positionCurve.GetPoint(Ease.SineInOut(progress));


            float alpha = opacityCurve.GetPoint(Ease.SineInOut(progress)).Y;
            
            Font.Draw(64, result.text, result.position, Justify,
                new Vector2(Scale * scaleCurve.GetPoint(Ease.SineInOut(progress)).Y),
                (IPMmeterModule.Settings.ResultRoomValueSettings.CopyTextParams ? OpaqueColor : Color.LightBlue) * alpha);

        }
    }
    
}
