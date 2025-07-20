using EbonianMod.Dusts;
using EbonianMod.Items.Misc;
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
    public void DoEyeHomingFlame()
    {
        NetUpdateAtSpecificTime(100);
        Vector2 eyePos = NPC.Center - new Vector2(-7, 14).RotatedBy(NPC.rotation);
        isLaughing = true;
        if ((AITimer % 30 == 0 || AITimer == 1) && AITimer <= 200)
            SoundEngine.PlaySound(EbonianSounds.terrortomaLaugh, NPC.Center);

        if (AITimer == 1)
            SoundEngine.PlaySound(EbonianSounds.BeamWindUp, NPC.Center);
        if (AITimer == 55)
        {
            glareAlpha = 1;
            MPUtils.NewProjectile(null, eyePos, Vector2.Zero, ProjectileType<GreenChargeUp>(), 0, 0);
        }
        if (AITimer == 100)
            lastPos = player.Center;
        if (AITimer > 120)
        {
            if (AITimer < 200 && AITimer % 15 == 0)
            {
                bloomAlpha = 1;
                SoundEngine.PlaySound(EbonianSounds.xSpirit.WithPitchOffset(-1f), NPC.Center);
                MPUtils.NewProjectile(null, eyePos, syncedRand.NextVector2CircularEdge(5, 5) * syncedRand.NextFloat(0.5f, 1), ProjectileType<TFlameThrowerHoming>(), 23, 0);
            }
            if (AITimer < 170 && AITimer % 5 == 0)
                MPUtils.NewProjectile(null, eyePos, Helper.FromAToB(NPC.Center, player.Center) * syncedRand.NextFloat(0.5f, 1.5f), ProjectileType<RegorgerBolt>(), 23, 0);
        }
    }


    public void DoTitteringSpawn()
    {
        SelectedClinger = 1;

        if (AITimer == 1)
            lastPos = player.Center + new Vector2(syncedRand.NextFloat(-50, 50), 0);

        Vector2 to = Helper.TRay.Cast(lastPos, Vector2.UnitY, 800, true) - new Vector2(0, 200);
        if (NPC.Distance(to) > 100)
            NPC.velocity = Vector2.Lerp(NPC.velocity, Helper.FromAToB(NPC.Center, to) * 30, 0.1f);
        else NPC.velocity *= 0.9f;
    }


    public void DoShadowOrbVomit()
    {
        SelectedClinger = 4;
        if (AITimer == 1)
            bloomAlpha = 1f;
        rotation = Helper.FromAToB(NPC.Center, player.Center).ToRotation() - MathHelper.PiOver2;
        if (AITimer == 40 || (AITimer > 80 && AITimer % 5 == 0 && AITimer <= 150))
        {
            if (AITimer % 15 == 0 || AITimer == 40)
                SoundEngine.PlaySound(SoundID.NPCDeath13.WithPitchOffset(.4f + syncedRand.NextFloat(-.3f, .1f)), NPC.Center);
            MPUtils.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + (rotation + PiOver2).ToRotationVector2().RotatedByRandom(PiOver4) * 25, (NPC.rotation + MathHelper.PiOver2).ToRotationVector2().RotatedByRandom(PiOver4 * 0.6f) * syncedRand.NextFloat(5, 20), ProjectileType<TerrorStaffPEvil>(), 20, 0, 0);
        }
    }


    public void DoCursedFlameRain2()
    {
        if (NPC.Distance(player.Center) > 200)
            NPC.velocity = Vector2.Lerp(NPC.velocity, Helper.FromAToB(NPC.Center, player.Center - new Vector2(0, 200)) * 30, 0.02f);
        else NPC.velocity *= 0.9f;
        if (AITimer < 175)
        {
            if (AITimer == 1)
            {
                glareAlpha = 1;
                Shriek();
            }
            if (AITimer % 20 == 0)
                Helper.AddCameraModifier(new PunchCameraModifier(NPC.Center, syncedRand.NextVector2Unit(), 10, 6, 30, 1000));
            if (AITimer % 5 == 0)
                MPUtils.NewProjectile(null, NPC.Center, Vector2.Zero, ProjectileType<TerrortomaScream>(), 0, 0);
        }
        else angry = false;
        if (AITimer > 40 && AITimer % 12 == 0 && AITimer < 170)
        {
            Vector2 pos = player.Center - new Vector2(syncedRand.NextFloat(-700, 700), 700);
            MPUtils.NewProjectile(null, pos, new Vector2(Helper.FromAToB(pos, player.Center).X * syncedRand.NextFloat(8), syncedRand.NextFloat(3, 10)), ProjectileType<TFlameThrower2>(), 20, 0);

            pos = player.Center + new Vector2(900 * (syncedRand.NextFloatDirection() > 0 ? 1 : -1), -20);
            if (AITimer % 48 == 0)
                MPUtils.NewProjectile(null, pos, Helper.FromAToB(pos, player.Center) * 16, ProjectileType<TerrorVilethorn1>(), 20, 0);
        }
    }


    public void DoRangedHeadSlam()
    {
        SelectedClinger = 3;
        angry = true;
        if (AITimer == 1)
            lastPos = player.Center + new Vector2(syncedRand.NextFloat(-50, 50), 0);

        Vector2 to = Helper.TRay.Cast(lastPos, Vector2.UnitY, 800, true) - new Vector2(0, 300);
        if (NPC.Distance(to) > 100)
            NPC.velocity = Vector2.Lerp(NPC.velocity, Helper.FromAToB(NPC.Center, to) * 30, 0.1f);
        else NPC.velocity *= 0.9f;
    }


    public void DoGeyserSweep()
    {
        NetUpdateAtSpecificTime(80, 100);
        if (AITimer < 80)
        {
            rotation = -PiOver4;
            if (AITimer < 20)
                lastPos = player.Center;
            Vector2 to = Helper.TRay.Cast(lastPos + new Vector2(750, -200), Vector2.UnitY, 800, true) - new Vector2(200);
            if (NPC.Distance(to) > 200)
                NPC.velocity = Vector2.Lerp(NPC.velocity, Helper.FromAToB(NPC.Center, to) * 30, 0.1f);
            else NPC.velocity *= 0.9f;
            Vector2 pos = NPC.Center + new Vector2(0, 300).RotatedByRandom(PiOver2 * 0.7f);
            Dust.NewDustPerfect(pos, DustType<LineDustFollowPoint>(), Helper.FromAToB(pos, NPC.Center) * syncedRand.NextFloat(4, 10), newColor: Color.LawnGreen, Scale: syncedRand.NextFloat(0.06f, 0.2f)).customData = NPC.Center + new Vector2(0, 20);

            Dust.NewDustPerfect(pos, DustID.CursedTorch, Helper.FromAToB(pos, NPC.Center) * syncedRand.NextFloat(4, 10)).noGravity = true;
        }
        if (AITimer == 85)
        {
            bloomAlpha = 1;
            glareAlpha = 1;
        }
        if (AITimer > 100)
        {
            Vector2 to = Helper.TRay.Cast(lastPos + new Vector2(-750, -400), Vector2.UnitY, 800, true) - new Vector2(200);
            if (NPC.Distance(to) > 200)
                NPC.velocity = Vector2.Lerp(NPC.velocity, Helper.FromAToB(NPC.Center, to) * 30, 0.02f);
            else
            {
                AITimer++;
                NPC.velocity *= 0.9f;
            }


            MPUtils.NewProjectile(null, NPC.Center + Vector2.UnitY.RotatedByRandom(PiOver4) * 20, Vector2.UnitY.RotatedBy(NPC.rotation).RotatedByRandom(PiOver4) * syncedRand.NextFloat(3, 8), ProjectileType<TFlameThrower>(), 23, 0);
            if (AITimer % 3 == 0)
                SoundEngine.PlaySound(SoundID.Item34, NPC.Center);
            if (AITimer % 9 == 0 && AITimer > 120 && NPC.Distance(to) > 200)
            {
                MPUtils.NewProjectile(null, Helper.TRay.Cast(NPC.Center, Vector2.UnitY, 300, true), -Vector2.UnitY, ProjectileType<TFlameThrower4>(), 23, 0);
                MPUtils.NewProjectile(null, Helper.TRay.Cast(NPC.Center, Vector2.UnitY, 300, true), Vector2.UnitY, ProjectileType<TFlameThrower4>(), 23, 0);
            }

        }
    }


    public void DoBranchingFlame()
    {
        if (AITimer == 80)
            glareAlpha = 1;
        if (AITimer < 80)
        {
            if (NPC.Distance(player.Center) > 200)
                NPC.velocity = Vector2.Lerp(NPC.velocity, Helper.FromAToB(NPC.Center, player.Center - new Vector2(0, 200)) * 30, 0.02f);
            else NPC.velocity *= 0.9f;
            Vector2 pos = NPC.Center + new Vector2(0, 300).RotatedByRandom(PiOver2 * 0.7f);
            Dust.NewDustPerfect(pos, DustType<LineDustFollowPoint>(), Helper.FromAToB(pos, NPC.Center) * syncedRand.NextFloat(4, 10), newColor: Color.LawnGreen, Scale: syncedRand.NextFloat(0.06f, 0.2f)).customData = NPC.Center + new Vector2(0, 20);
            Dust.NewDustPerfect(pos, DustID.CursedTorch, Helper.FromAToB(pos, NPC.Center) * syncedRand.NextFloat(4, 10)).noGravity = true;
        }
        else NPC.velocity *= 0.9f;
        if (AITimer >= 100 && AITimer < 190)
        {
            MPUtils.NewProjectile(null, NPC.Center + Vector2.UnitY.RotatedByRandom(PiOver4) * 20, Vector2.UnitY.RotatedByRandom(PiOver4) * syncedRand.NextFloat(3, 8), ProjectileType<TFlameThrower>(), 23, 0);
            if (AITimer % 4 == 0)
            {
                SoundEngine.PlaySound(SoundID.Item34, NPC.Center);
                if (AITimer < 170)
                    MPUtils.NewProjectile(null, NPC.Center + Vector2.UnitY.RotatedByRandom(PiOver4) * 20, new Vector2(syncedRand.NextFloat(-20, 20), syncedRand.NextFloat(-23, -16)), ProjectileType<TFlameThrower2_Inverted>(), 23, 0);
            }
        }
    }
    public void DoBodySlam()
    {
        NetUpdateAtSpecificTime(30, 100);
        SelectedClinger = 2;
        if (AITimer == 1)
            if (!player.velocity.Y.InRange(0, 0.2f) || player.Center.Y < NPC.Center.Y)
                ResetState();
        if (AITimer < 20)
        {
            NPC.velocity = Helper.FromAToB(NPC.Center, player.Center + new Vector2(0, -200), false) / 50f;
            rotation = Helper.FromAToB(NPC.Center, player.Center).ToRotation() - MathHelper.PiOver2;
        }
        else if (AITimer < 30 && AITimer >= 20)
            NPC.velocity *= 0.8f;
        if (AITimer == 30)
        {
            lastPos = Helper.TRay.Cast(NPC.Center, Vector2.Clamp(Helper.FromAToB(NPC.Center, player.Center), new Vector2(-0.35f, 1), new Vector2(0.35f, 1)), 2028);
            bloomAlpha = 1f;
        }
        if (AITimer > 100 && AITimer < 170)
        {
            NPC.velocity += Helper.FromAToB(NPC.Center, lastPos, false) / 40f;
            if (NPC.Center.Distance(lastPos) < NPC.height * 0.75f)
            {
                if (NPC.velocity.Y > 0)
                    AITimer2 = 1;
                AITimer = 170;
                NPC.netUpdate = true;
            }
        }
        if (AITimer == 170 && AITimer2 == 1)
        {
            SoundEngine.PlaySound(EbonianSounds.chomp1, NPC.Center);
            CameraSystem.ScreenShakeAmount = 5f;

            for (int i = 0; i < 10; i += (phase2 ? 1 : 2))
            {
                float angle = Helper.CircleDividedEqually(i, 10);
                Projectile a = MPUtils.NewProjectile(NPC.GetSource_Death(), NPC.Center, angle.ToRotationVector2() * syncedRand.NextFloat(5, 7), ProjectileType<OstertagiWorm>(), 24, 0, 0);
                a.SetToHostile();
                MPUtils.NewProjectile(NPC.GetSource_FromThis(), NPC.Bottom, new Vector2(0, 0), ProjectileType<GluttonImpact>(), 50, 0, 0, 0);
            }
            MPUtils.NewProjectile(NPC.GetSource_FromThis(), NPC.Bottom, new Vector2(0, 0), ProjectileType<GluttonImpact>(), 50, 0, 0, 0);
        }
        if (AITimer >= 170) { NPC.velocity *= 0.1f; }
    }


    public void DoOstertagi()
    {
        if (AITimer == 1)
            bloomAlpha = 0.5f;
        if (AITimer == 30)
            MPUtils.NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ProjectileType<OstertagiExplosion>(), 0, 0, 0);
        if (AITimer == 30)
        {
            MPUtils.NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ProjectileType<OstertagiExplosion>(), 0, 0, 0);
            for (int i = 0; i < 10; i++)
            {
                float angle = Helper.CircleDividedEqually(i, 10);
                Projectile a = MPUtils.NewProjectile(NPC.GetSource_Death(), NPC.Center, angle.ToRotationVector2() * syncedRand.NextFloat(5, 7), ProjectileType<OstertagiWorm>(), 24, 0, 0);
                a.SetToHostile();
            }
        }
        if (AITimer == 40)
        {
            MPUtils.NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ProjectileType<OstertagiExplosion>(), 0, 0, 0);
            for (int i = 0; i < 10; i++)
            {
                float angle = Helper.CircleDividedEqually(i, 10) + MathHelper.PiOver4;
                Projectile a = MPUtils.NewProjectile(NPC.GetSource_Death(), NPC.Center, angle.ToRotationVector2() * syncedRand.NextFloat(5, 7), ProjectileType<OstertagiWorm>(), 24, 0, 0);
                a.SetToHostile();
            }
        }
        if (phase2)
            if (AITimer == 50)
            {
                MPUtils.NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ProjectileType<OstertagiExplosion>(), 0, 0, 0);
                for (int i = 0; i < 10; i++)
                {
                    float angle = Helper.CircleDividedEqually(i, 10) + MathHelper.PiOver2;
                    Projectile a = MPUtils.NewProjectile(NPC.GetSource_Death(), NPC.Center, angle.ToRotationVector2() * syncedRand.NextFloat(5, 7), ProjectileType<OstertagiWorm>(), 24, 0, 0);
                    a.SetToHostile();
                }
            }
    }
}
