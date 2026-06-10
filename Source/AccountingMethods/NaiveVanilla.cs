
using static Celeste.Mod.IPMmeter.Meter;
using Monocle;
using Microsoft.Xna.Framework;


namespace Celeste.Mod.IPMmeter.AccountingMethods;

class NaiveVanilla : Accounter
{
    static Vector2 savedMove;

    public override int Update(Module.IPMmeterSettings allSettings, Player player)
    {
        var settings = allSettings.VanillaAccounting;

        int inputs = 0;

        // grab
        if (Input.Grab.Binding.Pressed(Input.Grab.GamepadIndex, Input.Grab.Threshold))
        {
            if (settings.GrabPress == GrabOptions.All || (settings.GrabPress == GrabOptions.OnlyHoldable) == (player?.Holding != null))
            {
                inputs++;
            }
        }
        if (Input.Grab.Binding.Released(Input.Grab.GamepadIndex, Input.Grab.Threshold))
        {
            if (settings.GrabRelease == GrabOptions.All || (settings.GrabRelease == GrabOptions.OnlyHoldable) == (player?.Holding != null))
            {
                inputs++;
            }
        }

        // dash
        if (Input.Dash.Binding.Pressed(Input.Dash.GamepadIndex, Input.Dash.Threshold) && settings.Dash.Press())
            inputs++;
        if (Input.Dash.Binding.Released(Input.Dash.GamepadIndex, Input.Dash.Threshold) && settings.Dash.Release())
            inputs++;

        // demo
        if (Input.CrouchDash.Binding.Pressed(Input.CrouchDash.GamepadIndex, Input.CrouchDash.Threshold) && settings.DemoDash.Press())
            inputs++;
        if (Input.CrouchDash.Binding.Released(Input.CrouchDash.GamepadIndex, Input.CrouchDash.Threshold) && settings.DemoDash.Release())
            inputs++;

        // jump
        if (Input.Jump.Binding.Pressed(Input.Jump.GamepadIndex, Input.Jump.Threshold) && settings.Jump.Press())
            inputs++;
        if (Input.Jump.Binding.Released(Input.Jump.GamepadIndex, Input.Jump.Threshold) && settings.Jump.Release())
            inputs++;

        // directions: x
        if (savedMove.X != Input.MoveX.Value)
        {
            if (savedMove.X == 0 && settings.Directions.Press()) inputs++;
            else if (Input.MoveX == 0 && settings.Directions.Release()) inputs++;
            else
            {
                if (settings.Directions.Press()) inputs++;
                if (settings.Directions.Release()) inputs++;
            }
        }
        savedMove.X = Input.MoveX.Value;

        // directions: y
        if (savedMove.Y != Input.MoveY.Value)
        {
            if (savedMove.Y == 0 && settings.Directions.Press()) inputs++;
            else if (Input.MoveY == 0 && settings.Directions.Release()) inputs++;
            else
            {
                if (settings.Directions.Press()) inputs++;
                if (settings.Directions.Release()) inputs++;
            }
        }
        savedMove.Y = Input.MoveY.Value;

        return inputs;
    }
}
