using System.Collections.Generic;

namespace Celeste.Mod.IPMmeter.CalcMethods;

class InputSaving : Calculator
{
    static List<float> inputList = new();

    public override void Update(int inputs)
    {
        if (inputs > 0)
        {
            inputList.AddRange(Repeat(() => Buffer, inputs));
            inputs = 0;
        }

        if (inputList.Count == 0) return;
        
        for (int c = inputList.Count-1; c >= 0; c--)
        {
            inputList[c] -= Monocle.Engine.DeltaTime;
            if (inputList[c] <= 0f)
            {
                inputList.RemoveAt(c);
            }
        }
    }

    public override float GetDisplayValue()
    {
        return inputList.Count / Buffer;
    }
}