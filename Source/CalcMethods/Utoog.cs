using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.IPMmeter.CalcMethods;

class Utoog : Calculator
{
    static int accumulated = 0;


    public override void Update(int inputs)
    {
        accumulated += inputs;
    }

    public override float GetDisplayValue()
    {
        float result = accumulated / DisplayComponent.Interval;
        accumulated = 0;
        return result;
    }
}