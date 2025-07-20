using EbonianMod.NPCs.Corruption.Ebonflies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terraria.Chat;

namespace EbonianMod.NPCs.Terrortoma;
public partial class Terrortoma : ModNPC
{
    public void NetUpdateAtSpecificTime(params int[] times)
    {
        if (times.Contains(AITimer))
            NPC.netUpdate = true;
    }


    public UnifiedRandom syncedRand => new UnifiedRandom((int)NPC.ai[3]);


    public void ResetState()
    {
        AITimer = 0;
        AITimer2 = 0;
        SelectedClinger = 4;
        AIState = Idle;
        NPC.damage = 0;
        rotation = 0;
        NPC.velocity = Vector2.Zero;
        isLaughing = false;
        angry = false;
        NPC.netUpdate = true;
    }


    public void MiscChecks()
    {
        if (bloomAlpha > 0f) bloomAlpha -= 0.025f;
        if (glareAlpha > 0) glareAlpha -= 0.025f;

        if (AIState != Death && NPC.life <= 1 && !ded)
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
        }

        if (NPC.life <= NPC.lifeMax - NPC.lifeMax / 2 + 2000 && !phase2)
        {
            glareAlpha = 1f;
            AIState = PhaseTransition;
            phase2 = true;
            AITimer = 0;
        }


        if (NPC.alpha <= 0 && AIState != Death)
        {
            if (!NPC.AnyNPCs(NPCType<TerrorClingerMelee>()))
            {
                MPUtils.NewNPC(NPC.Center, NPCType<TerrorClingerMelee>(), false, NPC.whoAmI);
            }
            if (!NPC.AnyNPCs(NPCType<TerrorClingerSummoner>()))
            {
                MPUtils.NewNPC(NPC.Center, NPCType<TerrorClingerSummoner>(), false, NPC.whoAmI);
            }
            if (!NPC.AnyNPCs(NPCType<TerrorClingerRanged>()))
            {
                MPUtils.NewNPC(NPC.Center, NPCType<TerrorClingerRanged>(), false, NPC.whoAmI);
            }
        }
    }


    public void Target()
    {
        if (!NPC.HasPlayerTarget)
            NPC.TargetClosest(false);
        if (!player.active || player.dead && AIState != Death)
        {
            NPC.TargetClosest(false);
            if (NPC.HasValidTarget)
            {
                AIState = Intro;
                AITimer = 0;
            }
            if (!player.active || player.dead && AIState != Death)
            {
                if (AIState != -12124)
                {
                    SoundEngine.PlaySound(EbonianSounds.terrortomaLaugh, NPC.Center);
                    AIState = -12124;
                }
                NPC.velocity = new Vector2(0, 10f);
                if (NPC.timeLeft > 120)
                {
                    NPC.timeLeft = 120;
                }
                return;
            }
        }
    }


    public void Shriek()
    {
        angry = true;
        SoundEngine.PlaySound(EbonianSounds.shriek.WithPitchOffset(-1f).WithVolumeScale(1.6f), NPC.Center);
        SoundEngine.PlaySound(EbonianSounds.shriek.WithPitchOffset(-0.6f).WithVolumeScale(1.6f), NPC.Center);
    }
}
