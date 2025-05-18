using EbonianMod.Projectiles.Enemy.Desert;
using System;

namespace EbonianMod.NPCs.Snow;

public class BorealDancer : ModNPC
{
    public override void SetDefaults()
    {
        NPC.width = 48;
        NPC.height = 50;

        NPC.damage = 30;
        NPC.defense = 0;
        NPC.lifeMax = 120;
    }

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = 14;
    }

    public override void AI()
    {
        NPC.TargetClosest(true);
        Player player = Main.player[NPC.target];    

        if(Vector2.Distance(player.Center, NPC.Center) < 100 && NPC.ai[0] == 0)
        {
            NPC.ai[0] = 1;
        }
        if (NPC.ai[0] == 1)
        {
            NPC.velocity.X += NPC.direction * 0.08f;
            NPC.velocity.X = Clamp(NPC.velocity.X, -11, 11);
            if (Vector2.Distance(player.Center, NPC.Center) < 50)
            {
                NPC.ai[0] = 2;
            }
        }
        else
        {
            NPC.spriteDirection = player.Center.X > NPC.Center.X ? 1 : -1;
            NPC.velocity.X = 0;
        }
    }

    public override void FindFrame(int frameHeight)
    {
        NPC.frameCounter++;
        if (NPC.frameCounter > 5)
        {
            NPC.frameCounter = 0;
            if(NPC.ai[0] != 0)
                NPC.frame.Y += frameHeight;
            if(NPC.ai[0] == 1)
            {
                if (NPC.frame.Y > 7 * frameHeight)
                    NPC.frame.Y = frameHeight;
            }
            if(NPC.ai[0] == 2)
            {
                NPC.ai[0] = -1;
                NPC.frame.Y = 7 * frameHeight;
            }
            if (NPC.frame.Y > 13 * frameHeight)
            {
                NPC.ai[0] = 1;
                NPC.frame.Y = frameHeight;
            }
        }
    }
}
