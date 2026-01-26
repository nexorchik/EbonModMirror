using System.Collections.Generic;

namespace EbonianMod.Content.NPCs.Garbage;

public partial class HotGarbage : ModNPC
{
    readonly List<Vector2> redFrames = new List<Vector2>
    {
        new Vector2(0, 76*8),new Vector2(0, 76*10),new Vector2(0, 76*11),new Vector2(0, 76*12),

        new Vector2(80, 0),new Vector2(80, 76*1),new Vector2(80, 76*2),

        new Vector2(80*2, 0),new Vector2(80*2, 76*1),new Vector2(80*2, 76*2),new Vector2(80*2, 76*3)
    };
    readonly List<Vector2> yellowFrames = new List<Vector2>
    {
        new Vector2(80, 76*3),new Vector2(80, 76*4),new Vector2(80, 76*5)
    };
    readonly List<Vector2> greenFrames = new List<Vector2>
    {
        new Vector2(80, 76*6),new Vector2(80, 76*7),new Vector2(80, 76*8),new Vector2(80, 76*9)
    };
    void AmbientVFX() {
        if (redFrames.Contains(new Vector2(NPC.frame.X, NPC.frame.Y)))
            Lighting.AddLight(NPC.Center, TorchID.Red);
        if (yellowFrames.Contains(new Vector2(NPC.frame.X, NPC.frame.Y)))
            Lighting.AddLight(NPC.Center, TorchID.Yellow);
        if (greenFrames.Contains(new Vector2(NPC.frame.X, NPC.frame.Y)))
            Lighting.AddLight(NPC.Center, TorchID.Green);
        if (NPC.frame.X == 80 * 2 && NPC.frame.Y > 0)
        {
            Lighting.AddLight(NPC.Center, TorchID.Torch);
        }
    }
    
    void TargetingLogic() {
        NPC.TargetClosest(false);
        if (AIState != Death)
        {
            bool shouldntDespawn = false;
            foreach (Player p in Main.ActivePlayers)
                if (!p.dead)
                {
                    shouldntDespawn = true;
                    break;
                }
            if (!shouldntDespawn)
            {
                if (NPC.timeLeft > 10)
                {
                    NPC.timeLeft = 10;
                }
                NPC.active = false;
                return;
            }
        }
        NPC.timeLeft = 10;
    }
    
    void JumpCheck()
    {
        Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY, 1, false, 0);
        if (NPC.Grounded(offsetX: 0.5f) && (NPC.collideX || Helper.Raycast(NPC.Center, Vector2.UnitX, 1000).RayLength < NPC.width || Helper.Raycast(NPC.Center, -Vector2.UnitX, 1000).RayLength < NPC.width))
            NPC.velocity.Y = -10;
        if (NPC.Grounded(offsetX: 0.5f) && player.Center.Y < NPC.Center.Y - 300)
            NPC.velocity.Y = -20;
        else if (NPC.Grounded(offsetX: 0.5f) && player.Center.Y < NPC.Center.Y - 200)
            NPC.velocity.Y = -15;
        else if (NPC.Grounded(offsetX: 0.5f) && player.Center.Y < NPC.Center.Y - 100)
            NPC.velocity.Y = -10;
        if (AIState == Idle)
        {
            if (Helper.Raycast(NPC.Center, -Vector2.UnitY, NPC.height).RayLength < NPC.height - 1 && !Collision.CanHit(NPC, player))
            {
                if (!NPC.noTileCollide)
                {
                    NPC.noTileCollide = true;
                    NPC.netUpdate = true;
                }

                if (player.Center.Y < NPC.Center.Y)
                    NPC.Center -= Vector2.UnitY * 2;
                else
                    NPC.Center += Vector2.UnitY * 2;

                NPC.Center += new Vector2(Helper.FromAToB(NPC.Center, player.Center).X * 2, 0);
            }
            else if ((!Collision.CanHit(NPC, player) || !Collision.CanHitLine(NPC.TopLeft, 10, 10, player.position, player.width, player.height) || !Collision.CanHitLine(NPC.TopRight, 10, 10, player.position, player.width, player.height)) && player.Center.X.InRange(NPC.Center.X, NPC.width))
            {
                if (!NPC.noTileCollide)
                {
                    NPC.noTileCollide = true;
                    NPC.netUpdate = true;
                }

                if (player.Center.Y < NPC.Center.Y)
                    NPC.Center -= Vector2.UnitY * 2;
                else
                    NPC.Center += Vector2.UnitY * 2;
            }
            else if (NPC.noTileCollide)
            {
                NPC.noTileCollide = false;
                NPC.netUpdate = true;
            }
        }
    }
}