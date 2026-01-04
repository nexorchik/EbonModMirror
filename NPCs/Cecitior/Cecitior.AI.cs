using EbonianMod.Bossbars;
using EbonianMod.Common.Systems.Verlets;
using EbonianMod.Items.Armor.Vanity;
using EbonianMod.Items.BossTreasure;
using EbonianMod.Items.Materials;
using EbonianMod.Items.Misc;
using EbonianMod.Items.Pets;
using EbonianMod.Items.Tiles;
using EbonianMod.Items.Tiles.Trophies;
using EbonianMod.NPCs.Corruption;
using EbonianMod.Projectiles.Cecitior;
using EbonianMod.Projectiles.Friendly.Crimson;
using EbonianMod.Projectiles.VFXProjectiles;
using ReLogic.Utilities;
using System;
using System.IO;
using Terraria.GameContent.Bestiary;

namespace EbonianMod.NPCs.Cecitior;
public struct CecitiorClaw
{
    public Vector2 position;
    public Vector2[] oldPosition;
    public float[] oldRotation;
    public Verlet verlet;
    public CecitiorClaw(Vector2 _position, Verlet _verlet)
    {
        position = _position;
        verlet = _verlet;
    }
}

[AutoloadBossHead]
public partial class Cecitior : ModNPC
{
    public override bool CheckDead()
    {
        Player player = Main.player[NPC.target];
        if (!deathAnim)
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
            NPC.netUpdate = true;
        }
        return AIState == Death;
    }
    public override void AI()
    {
        if (!deathAnim)
            Target();
        MiscChecks();
        Ambience();
        SafetyChecks();

        NPC.ai[2]++; // For eye rotation;
        if (AIState != PrePreDeath)
            AITimer++;
        NPC.localAI[0] = openOffset.X;
        NPC.localAI[1] = openOffset.Y;
        if (open)
        {
            NPC.ai[3] = 1;
            NPC.damage = 0;
        }
        else
            Closing();

        switch (AIState)
        {
            case Death:
                DoDeathPart3();
                break;

            case PreDeath:
                if (!Main.dedServ)
                    Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ambience");
                DoDeathPart2();
                break;

            case PrePreDeath:
                DoDeathPart1();
                break;

            case PhaseTransition:
                DoPhaseTransition();
                break;

            case Intro:
                if (NPC.ai[2] > 210)
                {
                    AIState = Idle;
                    NPC.netUpdate = true;
                    break;
                }
                DoIntro();
                break;

            case Idle:
                KeepClosedJustIncaseYouNeverKnow();
                ClawIdle();
                DoIdle();
                break;

            case EyeBehaviour:
                KeepClosedJustIncaseYouNeverKnow();
                NPC.velocity = Helper.FromAToB(NPC.Center, player.Center - new Vector2(0, 350), false) / 20f;
                if (AITimer >= 220 - (halfEyesPhase2 ? 121 : 0) || phase2)
                    ResetState();
                break;

            case Chomp:
                NPC.dontTakeDamage = true;
                ClawIdle();
                DoChomp();
                break;

            case Teeth:
                NPC.dontTakeDamage = true;
                ClawIdle();
                DoTeethChomp();
                if (AITimer >= 85)
                {
                    if (phase2)
                        NPC.dontTakeDamage = false;
                    ResetState();
                }
                break;

            case Tongue:
                ClawIdle();
                DoTongue();
                if (AITimer >= 260)
                {
                    tongue = null;
                    ResetState();
                }
                break;

            case EyeBehaviour2:
                KeepClosedJustIncaseYouNeverKnow();
                NPC.velocity = Helper.FromAToB(NPC.Center, player.Center - new Vector2(0, 200), false) / 10f;
                if (AITimer >= 200 - (halfEyesPhase2 ? 130 : 0) || phase2)
                    ResetState();
                break;

            case SpitTeeth:
                DoSpitTeeth();
                if (AITimer >= 70 || phase2)
                    ResetState();
                break;

            case ThrowUpBlood:
                ClawIdle();
                DoThrowUpBlood();
                if (AITimer >= 75)
                    ResetState();
                break;

            case Phase2ThrowUpEyes:
                if (!phase2)
                    ResetState();
                else
                {
                    ClawIdle();
                    DoThrowUpEyes();
                    if (AITimer >= 95)
                        ResetState();
                }
                break;

            case Phase2Claw:
                if (!phase2)
                    ResetState();
                else
                {
                    KeepClosedJustIncaseYouNeverKnow();
                    DoClaw();
                    if (AITimer >= 55)
                        ResetState();
                }
                break;

            case Phase2ClawGrab:
                if (!phase2)
                    ResetState();
                else
                    DoClawGrab();
                break;

            case Phase2ClawMultiple:
                if (!phase2)
                    ResetState();
                else
                {
                    KeepClosedJustIncaseYouNeverKnow();
                    DoClawMultiple();
                    if (AITimer >= 201 - (oldHP < NPC.lifeMax - NPC.lifeMax / 4 ? (oldHP < NPC.lifeMax / 2 ? 0 : 50) : 100))
                        ResetState();
                }
                break;

            case Phase2GrabBomb:
                if (!phase2)
                    ResetState();
                else
                {
                    DoGrabBomb();
                    if (AITimer >= 180 - (oldHP < NPC.lifeMax - NPC.lifeMax / 4 ? (oldHP < NPC.lifeMax / 2 ? 0 : 50) : 100) || !phase2)
                        ResetState();
                }
                break;

            case Phase2ClawBodySlam:
                if (!phase2)
                    ResetState();
                else
                {
                    KeepClosedJustIncaseYouNeverKnow();
                    DoBodySlam();
                }
                break;
        }
    }
}
