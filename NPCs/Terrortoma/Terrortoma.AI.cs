using EbonianMod.Bossbars;
using EbonianMod.Dusts;
using EbonianMod.Items.Armor.Vanity;
using EbonianMod.Items.BossTreasure;
using EbonianMod.Items.Materials;
using EbonianMod.Items.Misc;
using EbonianMod.Items.Pets.Hightoma;
using EbonianMod.Items.Tiles;
using EbonianMod.Items.Tiles.Trophies;
using EbonianMod.NPCs.Corruption.Ebonflies;
using EbonianMod.Projectiles.Enemy.Corruption;
using EbonianMod.Projectiles.Friendly.Corruption;
using EbonianMod.Projectiles.Terrortoma;
using EbonianMod.Projectiles.VFXProjectiles;
using System;
using System.IO;
using System.Linq;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics.CameraModifiers;

namespace EbonianMod.NPCs.Terrortoma;

[AutoloadBossHead]
public partial class Terrortoma : ModNPC
{
    public override bool CheckDead()
    {
        if (NPC.life <= 0 && !ded)
        {
            NPC.life = 1;
            foreach (Projectile projectile in Main.ActiveProjectiles)
            {
                if (projectile.hostile && projectile.active)
                    projectile.Kill();
            }
            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (npc.type == NPCType<BloatedEbonfly>() && npc.active)
                {
                    npc.life = 0;
                    npc.checkDead();
                }
            }
            AIState = Death;
            NPC.frameCounter = 0;
            NPC.immortal = true;
            NPC.dontTakeDamage = true;
            CameraSystem.ChangeCameraPos(NPC.Center, 170, new ZoomInfo(2, 1.1f, InOutElastic, InOutCirc), 1.5f, InOutQuart);
            CameraSystem.ScreenShakeAmount = 20;
            ded = true;
            AITimer = 0;
            AITimer2 = 0;
            NPC.velocity = Vector2.Zero;
            NPC.life = 1;
            NPC.netUpdate = true;
            return false;
        }
        if (Main.dedServ)
            return true;
        Gore.NewGore(NPC.GetSource_Death(), NPC.position, syncedRand.NextVector2Unit() * 5, Find<ModGore>("EbonianMod/Terrortoma1").Type, NPC.scale);
        Gore.NewGore(NPC.GetSource_Death(), NPC.position, syncedRand.NextVector2Unit() * 5, Find<ModGore>("EbonianMod/Terrortoma2").Type, NPC.scale);
        Gore.NewGore(NPC.GetSource_Death(), NPC.position, syncedRand.NextVector2Unit() * 5, Find<ModGore>("EbonianMod/Terrortoma3").Type, NPC.scale);
        Gore.NewGore(NPC.GetSource_Death(), NPC.position, syncedRand.NextVector2Unit() * 5, Find<ModGore>("EbonianMod/Terrortoma4").Type, NPC.scale);
        Gore.NewGore(NPC.GetSource_Death(), NPC.position, syncedRand.NextVector2Unit() * 5, Find<ModGore>("EbonianMod/Terrortoma5").Type, NPC.scale);
        for (int i = 0; i < 40; i++)
        {
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CursedTorch, syncedRand.NextFloat(-1, 1), syncedRand.NextFloat(-1, 1));
        }
        return true;
    }


    public override void AI()
    {
        Target();
        MiscChecks();
        NPC.ai[3]++;
        AITimer++;
        NPC.rotation = Utils.AngleLerp(NPC.rotation, rotation, 0.25f);
        if (AITimer == 1)
            NPC.netUpdate = true;
        switch (AIState)
        {
            case -12124:
                {
                    isLaughing = true;
                    NPC.velocity = new Vector2(0, 5f);
                }
                break;

            case Death:
                DoDeath();
                break;

            case Intro:
                DoIntro();
                break;

            case Idle:
                DoIdle();
                break;

            case PhaseTransition:
                DoPhaseTransition();
                break;

            case Dash:
                DoDash();
                if (AITimer >= 290)
                    ResetState();
                break;

            case DifferentClingerAttacks:
                DoGenericClingerMove();
                if (AITimer >= 300)
                    ResetState();
                break;

            case ClingerSlam:
                DoClingerSlam();
                if (AITimer >= 170)
                    ResetState();
                break;

            case CursedFlamesRain:
                DoCursedFlameRain();
                if (AITimer >= 270 - (phase2 ? 100 : 0))
                    ResetState();
                break;

            case Pendulum:
                DoPendulum();
                if (AITimer >= 370 - (phase2 ? 100 : 0))
                    ResetState();
                break;

            case ThrowUpVilethorns:
                DoVilethornVomit();
                if (AITimer >= 100)
                    ResetState();
                break;

            case BodySlam:
                if (!phase2) ResetState();
                else
                {
                    DoBodySlam();
                    NPC.damage = 65;
                    if (AITimer >= 200)
                        ResetState();
                }
                break;

            case Ostertagi:
                if (!phase2) ResetState();
                else
                {
                    DoOstertagi();
                    if (AITimer >= 60)
                        ResetState();
                }
                break;

            case BranchingFlame:
                if (!phase2) ResetState();
                else
                {
                    DoBranchingFlame();
                    if (AITimer >= 210)
                        ResetState();
                }
                break;

            case GeyserSweep:
                if (!phase2) ResetState();
                else
                {
                    DoGeyserSweep();
                    if (AITimer >= 210)
                        ResetState();
                }
                break;

            case RangedHeadSlam:
                if (!phase2) ResetState();
                else
                {
                    DoRangedHeadSlam();
                    if (AITimer >= 400)
                        ResetState();
                }
                break;

            case CursedDollCopy:
                if (!phase2) ResetState();
                else
                {
                    DoCursedFlameRain2();
                    if (AITimer >= 180)
                        ResetState();
                }
                break;

            case ShadowOrbVomit:
                if (!phase2) ResetState();
                else
                {
                    DoShadowOrbVomit();
                    if (AITimer >= 180)
                        ResetState();
                }
                break;

            case TitteringSpawn:
                if (!phase2) ResetState();
                else
                {
                    DoTitteringSpawn();
                    if (AITimer >= 180)
                        ResetState();
                }
                break;

            case EyeHomingFlames:
                if (!phase2) ResetState();
                else
                {
                    DoEyeHomingFlame();
                    if (AITimer >= 250)
                        ResetState();
                }
                break;
        }
    }
}