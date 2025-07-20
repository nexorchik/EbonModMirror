using EbonianMod.Projectiles.Friendly.Corruption;
using EbonianMod.Projectiles.Terrortoma;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.NPCs.Terrortoma;
public partial class Terrortoma : ModNPC
{
    public void DoDash()
    {
        NetUpdateAtSpecificTime(50, 250);
        SelectedClinger = 4;
        if (AITimer == 1)
            bloomAlpha = 1f;
        NPC.damage = 65;
        NPC.localAI[0] = 100;
        AITimer2++;
        if (AITimer < 250 && AITimer >= 50)
        {
            if (AITimer2 == 14)
            {
                SoundEngine.PlaySound(EbonianSounds.terrortomaDash, NPC.Center);
                for (int i = 0; i < 40; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CursedTorch, syncedRand.NextFloat(-1, 1), syncedRand.NextFloat(-1, 1));
                }
            }
            if (AITimer2 < 10)
            {
                rotation = Helper.FromAToB(NPC.Center, player.Center).ToRotation() - PiOver2;
                NPC.velocity -= Helper.FromAToB(NPC.Center, player.Center) * (AITimer2 % 50) * 0.3f;
            }
            if (AITimer2 < 30 && AITimer > 10)
            {
                if (AITimer2 > 20)
                    rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;
                if (AITimer2 == 14)
                {
                    for (int i = -3; i < 4; i += (Main.expertMode ? 1 : 2))
                    {
                        MPUtils.NewProjectile(null, NPC.Center, Helper.FromAToB(NPC.Center, player.Center).RotatedBy(i * (phase2 ? 0.3f : 0.5f)) * 10, ProjectileType<TFlameThrower4>(), 20, 0);
                    }
                    NPC.velocity = Helper.FromAToB(NPC.Center, player.Center) * 25;
                    NPC.netUpdate = true;
                }
            }
            if (AITimer2 > 40)
                NPC.velocity *= 0.9f;

            if (AITimer2 > 62)
            {
                AITimer2 = 0;
                NPC.netUpdate = true;
            }
        }
    }


    public void DoGenericClingerMove()
    {
        if (SelectedClinger == 4 && AITimer < 290)
        {
            int[] allowedClingers = [0, 1, 2];
            if (NPC.Center.Distance(player.Center) > 900)
                allowedClingers = [0, 1];
            SelectedClinger = syncedRand.Next(allowedClingers);
            NPC.netUpdate = true;
        }
        NPC.damage = 0;
        NPC.localAI[0] = 0;
        if (AITimer <= 300)
        {
            NPC.velocity = NPC.FromAToB(player.Center - Vector2.UnitY * 200, false) * 0.02545f;
        }
        else NPC.velocity *= 0.9f;
    }


    public void DoClingerSlam()
    {
        SelectedClinger = 3;
        NPC.damage = 0;
        NPC.localAI[0] = 100;
        if (AITimer < 100)
            NPC.velocity = -Vector2.UnitY * MathHelper.Clamp(MathHelper.Lerp(1, 5, player.Center.Y - NPC.Center.Y / 300), 1, 5);
        else
            NPC.velocity *= 0.9f;
        rotation = Vector2.UnitY.ToRotation() - MathHelper.PiOver2;
    }


    public void DoCursedFlameRain()
    {
        SelectedClinger = 4;
        NPC.damage = 0;
        Vector2 toPlayer = new Vector2(0, -10);
        rotation = toPlayer.ToRotation() - MathHelper.PiOver2;
        NPC.localAI[0] = 100;
        if (AITimer <= 150)
        {
            NPC.velocity = NPC.FromAToB(player.Center - Vector2.UnitY * 75, false) * 0.0445f;
            if (++AITimer2 % 60 == 0)
            {
                for (int i = 0; i <= 5; i++)
                    MPUtils.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(syncedRand.NextFloat(-10f, 10f), -10), ProjectileType<TFlameThrower2>(), 20, 1f, Main.myPlayer);
                MPUtils.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, -10), ProjectileType<TFlameThrower2>(), 20, 1f, Main.myPlayer);
            }
            if (phase2)
                if (AITimer2 % 20 == 0)
                    MPUtils.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(syncedRand.NextFloat(-7f, 7f), -10), ProjectileType<TerrorStaffPEvil>(), 24, 1f, NPC.target);
        }
        else NPC.velocity *= 0.9f;
    }


    public void DoPendulum()
    {
        SelectedClinger = 3;
        NPC.damage = 0;
        NPC.localAI[0] = 100;
        AITimer2++;
        if ((int)AITimer2 % 25 == 0)
        {
            Vector2 vSpawnPos = player.Center + new Vector2(900, syncedRand.NextFloat(900)).RotatedBy(PiOver2 * syncedRand.Next(4));
            if (syncedRand.NextBool(5))
                MPUtils.NewProjectile(NPC.GetSource_FromAI(), vSpawnPos, vSpawnPos.FromAToB(player), ProjectileType<TerrorVilethorn1>(), 20, 0, 0);
        }
        Vector2 toPlayer = player.Center - NPC.Center;
        rotation = toPlayer.ToRotation() - MathHelper.PiOver2;
        NPC.velocity = NPC.FromAToB(player.Center - Vector2.UnitY * 280, false) * 0.0545f;
    }


    public void DoVilethornVomit()
    {
        SelectedClinger = 4;
        if (AITimer == 1)
            bloomAlpha = 1f;
        rotation = Helper.FromAToB(NPC.Center, player.Center).ToRotation() - MathHelper.PiOver2;
        if (AITimer > 30 && AITimer % 7 == 0 && AITimer <= (phase2 ? 100 : 75))
            MPUtils.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, (NPC.rotation + MathHelper.PiOver2).ToRotationVector2().RotatedByRandom(MathHelper.PiOver2) * 0.5f, ProjectileType<TerrorVilethorn1>(), 20, 0, 0);
    }
}
