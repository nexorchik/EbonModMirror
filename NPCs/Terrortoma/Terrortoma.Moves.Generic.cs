using EbonianMod.Dusts;
using EbonianMod.Projectiles.Enemy.Corruption;
using EbonianMod.Projectiles.Friendly.Corruption;
using EbonianMod.Projectiles.Terrortoma;
using EbonianMod.Projectiles.VFXProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.CameraModifiers;

namespace EbonianMod.NPCs.Terrortoma;
public partial class Terrortoma : ModNPC
{
    public void DoDeath()
    {
        NetUpdateAtSpecificTime(30, 326, 480);
        SelectedClinger = 4;
        NPC.damage = 0;
        NPC.timeLeft = 10;
        isLaughing = false;
        if (AITimer < 250)
        {
            NPC.dontTakeDamage = true;
            NPC.velocity = Vector2.Zero;
            rotation = 0;
        }
        if (AITimer < 205 && AITimer >= 30)
        {
            if (AITimer == 30)
            {
                Shriek();
                SoundEngine.PlaySound(EbonianSounds.shriek.WithPitchOffset(-0.6f).WithVolumeScale(1.6f), NPC.Center);
            }
            if (AITimer % 20 == 0)
                Helper.AddCameraModifier(new PunchCameraModifier(NPC.Center, Main.rand.NextVector2Unit(), 15, 12, 30, 1000));
            if (AITimer % 5 == 0)
                MPUtils.NewProjectile(null, NPC.Center, Vector2.Zero, ProjectileType<TerrortomaScream>(), 0, 0);
        }
        if (AITimer < 300 && AITimer > 250)
        {
            Vector2 pos = NPC.Center + new Vector2(0, Main.rand.NextFloat(500, 1000)).RotatedByRandom(PiOver2 * 0.7f).RotatedBy(NPC.rotation);
            Dust.NewDustPerfect(pos, DustType<LineDustFollowPoint>(), Helper.FromAToB(pos, NPC.Center).RotatedByRandom(PiOver4) * Main.rand.NextFloat(20, 50), newColor: Color.LawnGreen, Scale: Main.rand.NextFloat(0.06f, 0.2f)).customData = NPC.Center + new Vector2(0, 20);
        }
        if (AITimer >= 250 && AITimer < 450)
        {
            if (AITimer % 10 == 0)
                MPUtils.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, (rotation + PiOver2).ToRotationVector2().RotatedByRandom(PiOver2) * 0.5f, ProjectileType<TerrorVilethorn1>(), 20, 0, 0);
            if (AITimer % 25 == 0)
            {
                for (int i = -1; i < 2; i++)
                {
                    if (i == 0) continue;
                    Projectile a = MPUtils.NewProjectile(NPC.GetSource_Death(), NPC.Center, (rotation + PiOver2).ToRotationVector2().RotatedByRandom(PiOver4 * 0.5f) * Main.rand.NextFloat(4, 6), ProjectileType<OstertagiWorm>(), 24, 0, 0, i * Main.rand.NextFloat(0.15f, 0.5f));
                    a.SetToHostile();
                }
            }
            if (AITimer == 305)
            {
                if (!Main.dedServ)
                    Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ambience");
                SoundEngine.PlaySound(EbonianSounds.chargedBeamWindUp, NPC.Center);
                MPUtils.NewProjectile(NPC.InheritSource(NPC), NPC.Center, (rotation + PiOver2).ToRotationVector2(), ProjectileType<VileTearTelegraph>(), 0, 0);
            }
            glareAlpha = Lerp(glareAlpha, 4f, 0.05f);
            if (AITimer < 305)
                rotation = Helper.FromAToB(NPC.Center, player.Center).ToRotation() - PiOver2;
        }
        if (AITimer > 326 && AITimer < 480)
        {
            if (AITimer % 3 == 0)
                MPUtils.NewProjectile(null, NPC.Center, Vector2.Zero, ProjectileType<TerrortomaScream>(), 0, 0);
            rotation = NPC.rotation;
            NPC.rotation += ToRadians(2f) * Lerp(0, 1, InOutCirc.Invoke((AITimer - 326) / 160f));
            rotation = NPC.rotation;
        }

        if (AITimer == 326)
        {
            NPC.velocity = Vector2.Zero;
            CameraSystem.ScreenShakeAmount = 15f;
            SoundEngine.PlaySound(EbonianSounds.chargedBeamImpactOnly, NPC.Center);
            MPUtils.NewProjectile(NPC.InheritSource(NPC), NPC.Center, (NPC.rotation + PiOver2).ToRotationVector2(), ProjectileType<TBeam>(), 10000, 0);
        }

        if (AITimer == 480)
            NPC.velocity = Vector2.UnitY;

        if (AITimer >= 480)
        {
            Helper.DustExplosion(NPC.Center, Vector2.One, 2, Color.Gray * 0.35f, false, false, 0.4f, 0.5f, new Vector2(Main.rand.NextFloat(-7, 7), Main.rand.NextFloat(-6, -4)));
            NPC.velocity *= 1.025f;
            rotation += Clamp(ToRadians(NPC.velocity.Y), 0, ToRadians(15));
            if (Helper.TRay.CastLength(NPC.Center, Vector2.UnitY, 1920) < NPC.width / 2)
            {
                MPUtils.NewProjectile(NPC.InheritSource(NPC), Helper.TRay.Cast(NPC.Center, Vector2.UnitY, 1920), Vector2.Zero, ProjectileType<TExplosion>(), 0, 0).scale = 2f;
                SoundEngine.PlaySound(EbonianSounds.eggplosion);
                SoundEngine.PlaySound(EbonianSounds.evilOutro);
                NPC.immortal = false;
                NPC.life = 0;
                GetInstance<DownedBossSystem>().downedTerrortoma = true;
                NPC.checkDead();
                NPC.netUpdate = true;
            }
        }
    }


    public void DoIntro()
    {
        SelectedClinger = 4;
        rotation = 0;
        if (AITimer == 1)
            CameraSystem.ChangeCameraPos(NPC.Center, 130, new ZoomInfo(2, 1.6f, InOutQuad), 1.5f, easingFunction: InOutCirc);

        if (AITimer2 == 0 && AITimer % 5 == 0 && introFrame.Y < introFrame.Height * 15)
        {
            introFrame.Y += introFrame.Height;
            if (introFrame.Y >= introFrame.Height * 15)
            {
                AITimer2 = 1;
                NPC.netUpdate = true;
            }
            if (introFrame.Y == introFrame.Height * 11 || introFrame.Y == introFrame.Height * 13)
                SoundEngine.PlaySound(EbonianSounds.blink, NPC.Center);
        }
        if (AITimer2 == 1 && AITimer % 5 == 0 && introFrame.Y > introFrame.Height)
        {
            if (introFrame.Y > 9 * introFrame.Height)
            {
                introFrame.Y = 9 * introFrame.Height;
            }
            else
            {
                introFrame.Y -= introFrame.Height;
            }
        }

        if (AITimer >= 300)
        {
            NPC.damage = 0;
            AIState = Idle;
            NPC.dontTakeDamage = false;
            AITimer = 100;
            NPC.netUpdate = true;
        }
    }


    public void DoIdle()
    {
        NPC.localAI[0] = 0;
        isLaughing = AITimer < 100;
        if (AITimer == 2)
        {
            if (phase2)
                for (int i = 0; i < 3; i++)
                {
                    Vector2 vSpawnPos = player.Center + new Vector2(900, syncedRand.NextFloat(900)).RotatedBy(PiOver2 * syncedRand.Next(4));
                    if (syncedRand.NextBool(5))
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), vSpawnPos, vSpawnPos.FromAToB(player), ProjectileType<TerrorVilethorn1>(), 20, 0, 0);
                    }
                }
            SoundEngine.PlaySound(EbonianSounds.terrortomaLaugh, NPC.Center);
        }
        if (NPC.Distance(player.Center) > 200)
            NPC.velocity = Vector2.Lerp(NPC.velocity, Helper.FromAToB(NPC.Center, player.Center - new Vector2(0, 200) + Helper.FromAToB(player.Center, NPC.Center) * 50) * 13, 0.1f);
        else NPC.velocity *= 0.8f;
        rotation = Vector2.UnitY.ToRotation() - MathHelper.PiOver2;

        if (AITimer > 190)
        {
            NPC.velocity = Vector2.Zero;
            AIState = syncedRand.Next(1, 15 - (phase2 ? 0 : 8) + 1);
            AITimer = 0;
            AITimer2 = 0;
            NPC.netUpdate = true;
        }
    }


    public void DoPhaseTransition()
    {
        NPC.dontTakeDamage = true;
        NPC.velocity = Vector2.Zero;
        isLaughing = false;
        NPC.rotation = Utils.AngleLerp(NPC.rotation, 0, 0.1f);
        if (AITimer == 30)
            CameraSystem.ChangeCameraPos(NPC.Center, 200, new ZoomInfo(2, 1.1f, InOutQuad, InOutCirc), 1.5f, InOutQuart);
        if (AITimer < 205 && AITimer >= 30)
        {
            rotation = 0;
            if (AITimer == 30)
                Shriek();
            if (AITimer % 20 == 0)
                Helper.AddCameraModifier(new PunchCameraModifier(NPC.Center, syncedRand.NextVector2Unit(), 10, 12, 30, 1000));
            if (AITimer % 5 == 0)
                Projectile.NewProjectile(null, NPC.Center, Vector2.Zero, ProjectileType<TerrortomaScream>(), 0, 0);
        }
        else angry = false;

        if (AITimer >= 230)
        {
            NPC.dontTakeDamage = false;
            AIState = TitteringSpawn;
            if (NPC.life <= 1)
                AIState = Death;
            AITimer = 0;
            AITimer2 = 0;
            NPC.netUpdate = true;
        }
    }
}
