using Celeste.Mod.IPMmeter.CalcMethods;
using Celeste.Mod.IPMmeter.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monocle;
using Microsoft.Xna.Framework.Input;
using static Monocle.MInput.MouseData;

namespace Celeste.Mod.IPMmeter.AccountingMethods;

class NewAccounter : Accounter
{
    List<Buttons> pressedButtons = new();
    List<Keys> pressedKeys = new();
    List<MouseButtons> pressedMouse = new();

    public override int Update(IPMmeterSettings settings, Player player)
    {


        return 0;
    }
}
