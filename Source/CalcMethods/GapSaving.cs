using System;
using System.Collections.Generic;
using System.Linq;
using Monocle;

namespace Celeste.Mod.IPMmeter.CalcMethods;

class GapSaving : Calculator
{
    static float gapTimer = 0f;
    static List<Gap> Gaps = new();

    public override void OnRespawn()
    {
        gapTimer = 0f;
    }

    public override void Update(int inputs)
    {
        if (gapTimer < Buffer) gapTimer += Engine.DeltaTime;

        if (inputs > 0)
        {
            if (gapTimer > 0f)
            {
                Gaps.Add(new Gap(Buffer, gapTimer));
                gapTimer = 0f;
                inputs--;

                if (inputs > 0)
                {
                    Gaps.AddRange(Repeat(() => new Gap(Buffer, Engine.DeltaTime / inputs), inputs));
                }
            }
            else
            {
                Gaps.AddRange(Repeat(() => new Gap(Buffer, Engine.DeltaTime / inputs), inputs));
            }
        }

        if (Gaps.Count == 0) return;
        for (int c = Gaps.Count - 1; c >= 0; c--)
        {
            Gap gap = Gaps[c];

            gap.LifeTime -= Engine.DeltaTime;
            if (gap.LifeTime < 0f)
            {
                gap = null;
                Gaps.RemoveAt(c);
            }
        }
    }

    public override float GetDisplayValue()
    {
        if (Gaps.Count == 0) return 0f;
        
        float gapSumSize = (float)Gaps.Sum(gap => gap.Size) + gapTimer;
        float gapCount = Gaps.Count + (gapTimer > 0f ? 1f : 0f);
        return 1f / (gapSumSize / gapCount);
    }
}