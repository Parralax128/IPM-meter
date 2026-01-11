using Celeste.Mod.IPMmeter.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.IPMmeter.CalcMethods;

public abstract class Calculator
{
    public static float Buffer => IPMmeterModule.Settings.BufferSize;

    public abstract void Update(int inputs);
    public abstract float GetDisplayValue();

    public virtual void OnRespawn() { }
    public virtual void OnStart() { }

    public static List<T> Repeat<T>(Func<T> factory, int count)
    {
        List<T> result = new();
        for (int c = 0; c < count; c++)
        {
            result.Add(factory.Invoke());
        }
        return result;
    }
}