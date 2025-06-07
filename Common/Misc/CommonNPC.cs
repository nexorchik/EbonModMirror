using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.Common.Misc;
public abstract class CommonNPC : ModNPC
{
    public Vector2 gfxOff => new(0, NPC.gfxOffY);
    public float AIState { get => NPC.ai[0]; set => NPC.ai[0] = value; }
    public float AITimer { get => NPC.ai[1]; set => NPC.ai[1] = value; }
    public float AITimer2 { get => NPC.ai[2]; set => NPC.ai[2] = value; }
    public float AITimer3 { get => NPC.ai[3]; set => NPC.ai[3] = value; }
    public virtual void Reset()
    {
        AITimer = 0;
        AITimer2 = 0;
        AITimer3 = 0;
        NPC.netUpdate = true;
    }
    public virtual void SwitchState(float state)
    {
        AIState = state;
        Reset();
    }
}
