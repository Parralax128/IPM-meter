using Monocle;
using System;

namespace Celeste.Mod.IPMmeter.CalcMethods;

class Room : Calculator
{
    static float roomTimer = 0f;
    static int inputsAccumulated = 0;

    public override void Update(int inputs)
    {
        roomTimer += Engine.DeltaTime;
        inputsAccumulated += inputs;
    }
    public override float GetDisplayValue()
    {
        return inputsAccumulated / roomTimer;
    }

    public override void OnRespawn()
    {
        inputsAccumulated = 0;
        roomTimer = 0f;
    }
    public override void OnStart()
    {
        inputsAccumulated = 0;
        roomTimer = 0f;
    }
}