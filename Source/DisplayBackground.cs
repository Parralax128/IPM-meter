using Celeste.Mod.IPMmeter.Module;
using Monocle;
using static Celeste.Mod.IPMmeter.DisplayComponent;
using Microsoft.Xna.Framework;
using System;

namespace Celeste.Mod.IPMmeter;

static class DisplayBackground
{
    static MTexture bgEdge = GFX.Gui["IPMmetertriangleBG"];
    static Vector2 BGoffset => Scale * new Vector2(0f, 34f);
    static Gap peak = new Gap(0f, 0f);

    static float Height => 4f * Scale * 6f;

    public static void Render(float textWidth)
    {        
        if (textWidth >= peak.Size)
            peak = new Gap(IPMmeterModule.Settings.BackGroundSettings.PeakLifeTime, textWidth);

        if (peak.LifeTime <= 0f) peak.Size -= Engine.DeltaTime * IPMmeterModule.Settings.BackGroundSettings.PeakDecreaseSpeed * 6f;
        else peak.LifeTime -= Engine.DeltaTime;



        if (!IPMmeterModule.Settings.ShowBG) return;


        float bgWidth = peak.Size;

        bool leftWall = (TextPos.X < 24f * Scale) && (Align == Alignment.Left);
        bool rightWall = (TextPos.X > 1920f - (24f * Scale)) && (Align == Alignment.Right);

        Vector2 rectPos = TextPos + BGoffset;
        float height = Height;

        Color Color = OutlineColor;

        if (leftWall)
        {
            Draw.Rect(rectPos = new Vector2(-1f, TextPos.Y + BGoffset.Y),
                bgWidth = (bgWidth + TextPos.X + 6f), height, Color);
        }
        else if (rightWall)
        {
            Draw.Rect(rectPos = new Vector2(TextPos.X - bgWidth, TextPos.Y + BGoffset.Y),
                1920f + bgWidth - TextPos.X, height, Color);
        }
        else
        {
            switch (Align)
            {
                case Alignment.Center:
                    {
                        rectPos = new Vector2(TextPos.X - bgWidth/2f, TextPos.Y+BGoffset.Y);
                        break;
                    }
                case Alignment.Right:
                    {
                        rectPos = new Vector2(TextPos.X - bgWidth, TextPos.Y + BGoffset.Y);
                        break;
                    }
            }
            Draw.Rect(rectPos, bgWidth, height, Color);
        }


        rectPos -= Vector2.One;
        float edgeScale = height / 200f;

        if (!rightWall) //right triangle edge
        {
            bgEdge.Draw(rectPos + Vector2.UnitX * (bgWidth - 1f),
                Vector2.Zero, Color, edgeScale);
        }

        if (!leftWall) //left triangle edge
        {
            bgEdge.Draw(rectPos + Vector2.UnitX, new Vector2(1f, 0f),
                Color, new Vector2(-edgeScale, edgeScale));
        }
    }

}
