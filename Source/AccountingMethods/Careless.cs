using Celeste.Mod.IPMmeter.Module;
using Microsoft.Xna.Framework.Input;
using Monocle;
using System;
using static Monocle.MInput;
using static Monocle.MInput.MouseData;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.IPMmeter.AccountingMethods;

class Careless : Accounter
{
    static Keys[] keys = Enum.GetValues<Keys>();

    static GamePadData Gamepad => GamePads[Input.Gamepad];
    static Buttons[] padButtons = Enum.GetValues<Buttons>();
    static MouseButtons[] mouseButtons = Enum.GetValues<MouseButtons>();

    public override int Update(IPMmeterSettings allSettings, Player player)
    {
        var settings = allSettings.CarelessAccounting;

        int inputs = 0;

        // keyboard
        foreach(Keys key in keys)
        {
            bool prev = MInput.Keyboard.PreviousState[key] == KeyState.Down;
            bool current = MInput.Keyboard.CurrentState[key] == KeyState.Down;

            inputs += GetIncrement(prev, current, settings.Keyboard);
        }

        // mouse
        foreach (MouseButtons mouseButton in mouseButtons)
        {
            bool prev = MInput.Mouse.PreviousState.Check(mouseButton);
            bool current = MInput.Mouse.Check(mouseButton);

            inputs += GetIncrement(prev, current, settings.Mouse);
        }

        // gamepad buttons
        foreach (Buttons button in padButtons)
        {
            bool prev = Gamepad.PreviousState.IsButtonDown(button);
            bool current = Gamepad.CurrentState.IsButtonDown(button);

            inputs += GetIncrement(prev, current, settings.Gamepad);
        }

        // left + right  sticks
        inputs += Gamepad.GetStickIncrement(settings.Analog);
        

        return inputs;
    }

    static int GetIncrement(bool prevPressed, bool currentPressed, Meter.Options options)
    {
        if (prevPressed == currentPressed) return 0;

        if (currentPressed && options.Press()) return 1;
        if (prevPressed && options.Release()) return 1;

        return 0;
    }
}

public static class Extensions
{
    public static bool Check(this MouseState state, MouseButtons button)
    {
        MouseState saved = MInput.Mouse.CurrentState;
        
        MInput.Mouse.CurrentState = state;
        bool result = MInput.Mouse.Check(button);
        MInput.Mouse.CurrentState = saved;

        return result;
    }

    public static int GetStickIncrement(this GamePadData gamepad, Meter.Options options)
    {
        GamePadThumbSticks prev = gamepad.PreviousState.ThumbSticks;
        GamePadThumbSticks current = gamepad.CurrentState.ThumbSticks;

        Vector2 prevLeft = Snap(prev.Left);
        Vector2 prevRight = Snap(prev.Right);
        Vector2 currentLeft = Snap(current.Left);
        Vector2 currentRight = Snap(current.Right);

        return AimIncrement(prevLeft, currentLeft, options) + AimIncrement(prevRight, currentRight, options);
    }
    public static int AimIncrement(Vector2 prev, Vector2 current, Meter.Options options)
    {
        if (prev == current) return 0;

        if (current == Vector2.Zero && options.Release()) return 1;
        if (options.Press()) return 1;

        return 0;
    }

    static Vector2 Snap(Vector2 vector)
    {
        float angle = vector.Angle();
        int sign = (angle < 0f) ? 1 : 0;
        float theshold = MathF.PI/8f - sign * 0.08726646f;

        if (Calc.AbsAngleDiff(angle, 0f) < theshold) return new Vector2(1f, 0f);
        if (Calc.AbsAngleDiff(angle, MathF.PI) < theshold) return new Vector2(-1f, 0f);
        if (Calc.AbsAngleDiff(angle, -MathF.PI / 2f) < theshold) return new Vector2(0f, -1f);
        if (Calc.AbsAngleDiff(angle, MathF.PI / 2f) < theshold) return new Vector2(0f, 1f);
        return new Vector2(Math.Sign(vector.X), Math.Sign(vector.Y));
    }
}