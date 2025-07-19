using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.NPCs.Cecitior;
public partial class Cecitior : ModNPC
{
    public void NetUpdateAtSpecificTime(params int[] times)
    {
        if (times.Contains(AITimer))
            NPC.netUpdate = true;
    }
    public void KeepClosedJustIncaseYouNeverKnow()
    {
        open = false;
        openOffset = Vector2.Lerp(openOffset, Vector2.Zero, 0.1f);
        openRotation = Utils.AngleLerp(openRotation, 0, 0.1f);
    }


    public void ClawIdle()
    {
        if (phase2)
        {
            claw[0].position = Vector2.Lerp(claw[0].position, NPC.Center + openOffset + new Vector2(150, -65).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.2f);
            claw[1].position = Vector2.Lerp(claw[1].position, NPC.Center + openOffset + new Vector2(165, 45).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.2f);
            claw[2].position = Vector2.Lerp(claw[2].position, NPC.Center - openOffset + new Vector2(-110, 55).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.2f);
        }
    }


    void ResetState()
    {
        AITimer = 0;
        AITimer2 = 0;
        AITimer3 = 0;
        shakeVal = 0;
        AIState = Idle;
        NPC.damage = 0;
        rotation = 0;
        openRotation = 0;
        open = false;
        openOffset = Vector2.Zero;
        NPC.velocity = Vector2.Zero;
        NPC.netUpdate = true;
    }


    public void InitVerlet()
    {
        for (int i = 0; i < 10; i++)
        {
            float scale = 2;
            switch (i)
            {
                case 1:
                case 5:
                    scale = 4;
                    break;
                case 2:
                case 6:
                    scale = 2;
                    break;
                case 3:
                case 7:
                    scale = 3;
                    break;
                case 4:
                case 8:
                    scale = 1;
                    break;
            }
            verlet[i] = new(NPC.Center, 2, 15, 1 * scale, true, true, (int)(5f * scale));
        }
    }


    public void SafetyChecks()
    {
        if (claw is null)
        {
            claw = new CecitiorClaw[3];
            for (int i = 0; i < 3; i++)
            {
                claw[i] = new CecitiorClaw(Vector2.Zero, null);
                claw[i].oldPosition = new Vector2[25];
                claw[i].oldRotation = new float[25];
            }
        }

        if (!phase2 && AIState > 8)
        {
            ResetState();
        }
    }


    public void MiscChecks()
    {
        if (!deathAnim && NPC.life <= 1)
        {
            open = false;
            deathAnim = true;
            NPC.life = 1;
            savedPos = NPC.Center;
            NPC.velocity = Vector2.Zero;
            if (tongue is not null)
                tongue.Kill();
            AIState = PrePreDeath;
            AITimer = 0;
            openRotation = 0f;
            rotation = 0f;
            openOffset = Vector2.Zero;
            AITimer2 = 0;
            CameraSystem.ChangeCameraPos(NPC.Center, 180, new ZoomInfo(2, 1.1f, InOutElastic, InOutCirc), 1.5f, InOutQuart);
            NPC.dontTakeDamage = true;
        }

        if (oldHP == 0)
            oldHP = NPC.life;

        if (AIState != Idle && AIState != Intro && AIState != PrePreDeath && AIState != PreDeath && AIState != Death && AIState != PhaseTransition)
            OldState = AIState;

        if (AIState > 1 && phase2)
            NPC.dontTakeDamage = false;

        int eyeCount = 0;
        foreach (NPC npc in Main.ActiveNPCs)
        {
            if (npc.type == NPCType<CecitiorEye>())
                eyeCount++;
        }
        if (eyeCount == 0 && !phase2 && AIState > 1 && NPC.ai[2] > 200) //arbitrary fail-safe for mp
        {
            AITimer = 0;
            AITimer2 = 0;
            open = false;
            openRotation = 0;
            rotation = 0;
            openOffset = Vector2.Zero;
            AIState = PhaseTransition;
            NPC.velocity = Vector2.Zero;
            phase2 = true;
            NPC.netUpdate = true;
        }
        if (AIState == Idle)
            halfEyesPhase2 = eyeCount <= 3;

        if (AITimer == 1)
            NPC.netUpdate = true;
    }


    public void Ambience()
    {
        if (!Main.dedServ)
        {
            if (SoundEngine.TryGetActiveSound(cachedSound, out var _activeSound))
            {
                _activeSound.Pitch = Lerp(-1, 1, NPC.velocity.Length() / 13);
                _activeSound.Position = NPC.Center;
            }
            else
            {
                cachedSound = SoundEngine.PlaySound(EbonianSounds.cecitiorIdle.WithVolumeScale(0.35f), NPC.Center, (_) => NPC.AnyNPCs(Type));
            }
        }
        NPC.rotation = MathHelper.Lerp(NPC.rotation, rotation, 0.35f);
    }


    public void Target()
    {
        if (!NPC.HasPlayerTarget)
            NPC.TargetClosest(false);
        if (!player.active || player.dead)
        {
            NPC.TargetClosest(false);
            if (NPC.HasValidTarget)
            {
                if (AIState != Intro)
                    AIState = Idle;
                AITimer = 0;
            }
            if (!player.active || player.dead)
            {
                AIState = -12124;
                shakeVal = 0;
                open = false;
                NPC.velocity = new Vector2(0, 10f);

                if (phase2)
                {
                    claw[0].position = Vector2.Lerp(claw[0].position, NPC.Center + openOffset + new Vector2(150, -65).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.2f);
                    claw[1].position = Vector2.Lerp(claw[1].position, NPC.Center + openOffset + new Vector2(165, 45).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.2f);
                    claw[2].position = Vector2.Lerp(claw[2].position, NPC.Center - openOffset + new Vector2(-110, 55).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.2f);
                }

                if (NPC.timeLeft > 60)
                {
                    NPC.timeLeft = 60;
                }
                NPC.netUpdate = true;
                return;
            }
        }
    }


    public void Closing()
    {
        NPC.ai[3] = 0;
        if (!(AIState == Chomp && AITimer2 % 2 != 0) && !(AIState == PreDeath && ((int)AITimer2 == 1 || (int)AITimer2 == 3)))
            openOffset = Vector2.Lerp(openOffset, Vector2.Zero, 0.5f);

        if ((openOffset.Length() < 2.5f && openOffset.Length() > 1f) || (openOffset.Length() > -2.5f && openOffset.Length() < -1f))
        {
            if (SoundEngine.TryGetActiveSound(openSound, out var sound) && AITimer > 60)
            {
                sound.Stop();
            }
            SoundEngine.PlaySound(EbonianSounds.cecitiorClose, NPC.Center);
            CameraSystem.ScreenShakeAmount = 5;
        }
        if (openOffset != Vector2.Zero && AIState != ThrowUpBlood && AIState != SpitTeeth && NPC.frame.Y == 6 * 102)
            if (player.Center.Distance(NPC.Center - openOffset) < 75 || player.Center.Distance(NPC.Center + openOffset) < 75)
                player.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), 150, 0);
    }
    public float Ease(float x)
    {
        return x < 0.5 ? 4 * x * x * x : 1 - MathF.Pow(-2 * x + 2, 3) / 2;
    }
    public float ScaleFunction(float progress)
    {
        return 0.7f + (float)Math.Sin(progress * Math.PI) * 0.5f;
    }
}
