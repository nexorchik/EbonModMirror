using EbonianMod.Content.Items.Consumables.Food;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Effects;

namespace EbonianMod.Common.Players;

public class FlashPlayer : ModPlayer
{
    public int flashTime;
    public int flashMaxTime;
    public int flashCause;
    public float flashStr;
    public Vector2 flashPosition;
    public void FlashScreen(Vector2 pos, int time, float str = 2f, int cause = 0)
    {
        flashCause = cause;
        flashStr = str;
        flashMaxTime = time;
        flashTime = time;
        flashPosition = pos;
    }
    public override void PostUpdateBuffs()
    {
        switch (flashCause)
        {
            case 1:
                if (!Player.HasBuff<ConglomerateEnergyBuff>())
                    flashTime -= 40;
                break;
        }
        if (!Main.dedServ)
        {
            if (flashTime > 0)
            {
                flashTime--;
                if (!Filters.Scene["EbonianMod:ScreenFlash"].IsActive())
                    Filters.Scene.Activate("EbonianMod:ScreenFlash", flashPosition);
                Filters.Scene["EbonianMod:ScreenFlash"].GetShader().UseProgress((float)Math.Sin((float)flashTime / flashMaxTime * Math.PI) * flashStr);
                Filters.Scene["EbonianMod:ScreenFlash"].GetShader().UseTargetPosition(flashPosition);
            }
            else
            {
                if (Filters.Scene["EbonianMod:ScreenFlash"].IsActive())
                    Filters.Scene["EbonianMod:ScreenFlash"].Deactivate();
            }
        }
    }
}
