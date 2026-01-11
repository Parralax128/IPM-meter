using Celeste.Mod.IPMmeter.Module;
using static Celeste.Mod.IPMmeter.Module.IPMmeterSettings;

namespace Celeste.Mod.IPMmeter.CalcMethods;

public abstract class Accounter
{
    public abstract int Update(IPMmeterSettings settings, Player player);
}