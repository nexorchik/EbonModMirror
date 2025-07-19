using EbonianMod.NPCs.Corruption;
using EbonianMod.Projectiles.Cecitior;
using EbonianMod.Projectiles.VFXProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.NPCs.Cecitior;
public partial class Cecitior : ModNPC
{
    public void DoThrowUpEyes()
    {
        NetUpdateAtSpecificTime(15, 30, 80, 90);
        NPC.velocity *= 0.9f;
        open = true;
        if (AITimer < 15)
        {
            AITimer2 = 10;
            NPC.velocity = Helper.FromAToB(NPC.Center, player.Center - new Vector2(0, 200), false) / 20f;
            openOffset.X++;
            openRotation += ToRadians(2);
            rotation -= ToRadians(2);
        }
        if (AITimer == 20)
            MPUtils.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + Main.rand.NextVector2CircularEdge(30, 30), Main.rand.NextVector2Unit(), ProjectileType<EyeVFX>(), 0, 0);
        if (AITimer >= 30 && AITimer <= 80 && AITimer % 5 == 0)
        {
            if (AITimer % 15 == 0)
                SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
            MPUtils.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + Main.rand.NextVector2CircularEdge(30, 30), Main.rand.NextVector2Unit() * 2, ProjectileType<CecitiorEyeP>(), 30, 0);
        }
        if (AITimer >= 80)
        {
            openOffset.X--;
            openRotation -= ToRadians(2);
            rotation += ToRadians(2);
        }
        if (AITimer >= 90)
        {
            openOffset = Vector2.Zero;
            open = false;
        }
    }


    public void DoClaw()
    {
        NetUpdateAtSpecificTime(35, 45);
        if ((int)AITimer2 == 0)
        {
            AITimer2 = new UnifiedRandom((int)NPC.ai[2]).NextFloat(0.01f, Pi * 2);
            NPC.netUpdate = true;
        }
        if (AITimer < 35)
        {
            NPC.velocity = Helper.FromAToB(NPC.Center, player.Center - new Vector2(0, 200), false) / 2;
            savedPos = player.Center;
            claw[0].position = Vector2.Lerp(claw[0].position, savedPos + new Vector2(45, -170).RotatedBy(AITimer2), 0.3f);
            claw[1].position = Vector2.Lerp(claw[1].position, savedPos + new Vector2(0, -200).RotatedBy(AITimer2), 0.3f);
            claw[2].position = Vector2.Lerp(claw[2].position, savedPos + new Vector2(-45, -170).RotatedBy(AITimer2), 0.3f);
        }
        if (AITimer == 45)
        {
            for (int i = 0; i < claw.Length; i++)
            {
                MPUtils.NewProjectile(NPC.GetSource_FromAI(), claw[i].position + Vector2.UnitY.RotatedBy(AITimer2) * 110, -Vector2.UnitY.RotatedBy(AITimer2), ProjectileType<CecitiorClawSlash>(), 30, 0, ai2: 1);
            }
        }
        if (AITimer >= 45)
        {
            NPC.velocity *= 0.9f;
            for (int i = 0; i < claw.Length; i++)
            {
                claw[0].position = Vector2.Lerp(claw[0].position, savedPos - new Vector2(45, -170).RotatedBy(AITimer2), 0.04f);
                claw[1].position = Vector2.Lerp(claw[1].position, savedPos - new Vector2(0, -200).RotatedBy(AITimer2), 0.04f);
                claw[2].position = Vector2.Lerp(claw[2].position, savedPos - new Vector2(-45, -170).RotatedBy(AITimer2), 0.04f);
            }
        }
    }


    public void DoClawGrab()
    {
        NetUpdateAtSpecificTime(40, 80, 155, 160);
        if (AITimer < 40)
        {
            savedPos = claw[(int)AITimer3].position + Vector2.Clamp(Helper.FromAToB(claw[(int)AITimer3].position, player.Center, false), -Vector2.One * 320, Vector2.One * 320);
            if (AITimer3 == 0)
            {
                claw[0].position = Vector2.Lerp(claw[0].position, NPC.Center + openOffset + new Vector2(150, -65).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.2f);
                claw[1].position = Vector2.Lerp(claw[1].position, NPC.Center + openOffset + new Vector2(165, 45).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.2f);
                claw[2].position = Vector2.Lerp(claw[2].position, NPC.Center - openOffset + new Vector2(-110, 55).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.2f);
                NPC.velocity = Helper.FromAToB(NPC.Center, player.Center - new Vector2(0, 200), false) / 20f;
            }
        }
        if (AITimer == 40)
        {
            if (AITimer3 == 0)
            {
                savedClawPos = claw[(int)AITimer3].position;
                SoundEngine.PlaySound(EbonianSounds.cecitiorSlice, NPC.Center);
            }
            MPUtils.NewProjectile(NPC.GetSource_FromAI(), claw[(int)AITimer3].position, Vector2.Zero, ProjectileType<BloodShockwave2>(), 0, 0);
        }
        if (AITimer > 40)
        {
            NPC.velocity *= 0.9f;
            if (AITimer < 80)
            {
                claw[(int)AITimer3].position = Vector2.Lerp(claw[(int)AITimer3].position, savedPos + Helper.FromAToB(savedClawPos, savedPos) * (100 + AITimer3 * 40), 0.15f + (AITimer3 * 0.025f));
                if (AITimer3 == 1)
                {
                    claw[0].position = Vector2.Lerp(claw[0].position, NPC.Center + openOffset + new Vector2(165, 45).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.015f);
                }
                if (AITimer3 == 2)
                {
                    claw[0].position = Vector2.Lerp(claw[0].position, NPC.Center + openOffset + new Vector2(165, 45).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.015f);
                    claw[1].position = Vector2.Lerp(claw[1].position, NPC.Center - openOffset + new Vector2(-110, 55).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.015f);
                }
            }
            if (AITimer > 80 && (int)AITimer2 == 0)
            {
                if (oldHP < NPC.lifeMax / 2)
                {
                    if (AITimer3 < 2)
                    {
                        AITimer3++;
                        AITimer2 = 0;
                        AITimer = 60;
                        savedClawPos = claw[(int)AITimer3].position;
                        SoundEngine.PlaySound(EbonianSounds.cecitiorSlice, NPC.Center);
                        savedPos = claw[(int)AITimer3].position + Vector2.Clamp(Helper.FromAToB(claw[(int)AITimer3].position, player.Center, false), -Vector2.One * 320, Vector2.One * 320);
                    }
                    else
                        ResetState();
                }
                else if (oldHP < NPC.lifeMax - NPC.lifeMax / 4)
                {
                    if (AITimer3 == 0)
                    {
                        AITimer3++;
                        AITimer2 = 0;
                        AITimer = 50;
                        savedClawPos = claw[(int)AITimer3].position;
                        SoundEngine.PlaySound(EbonianSounds.cecitiorSlice, NPC.Center);
                        savedPos = claw[(int)AITimer3].position + Vector2.Clamp(Helper.FromAToB(claw[(int)AITimer3].position, player.Center, false), -Vector2.One * 320, Vector2.One * 320);
                    }
                    else
                        ResetState();
                }
                else
                {
                    ResetState();
                }
                NPC.netUpdate = true;
            }
            foreach (Player p in Main.ActivePlayers)
            {
                if (claw[(int)AITimer3].position.Distance(p.Center) < 27 && (int)AITimer2 == 0)
                {
                    NPC.target = p.whoAmI;
                    AITimer2 = 1;
                    AITimer = 70;
                    NPC.netUpdate = true;
                }
            }
        }
        if ((int)AITimer2 == 1)
        {
            if (AITimer < 159)
            {
                player.Center = claw[(int)AITimer3].position;
                claw[(int)AITimer3].position = Vector2.Lerp(claw[(int)AITimer3].position, NPC.Center + new Vector2(30, 100 - AITimer * 0.6f), 0.1f);
            }
            else
            {
                AITimer += 2;
                claw[0].position = Vector2.Lerp(claw[0].position, NPC.Center + openOffset + new Vector2(150, -65).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.2f);
                claw[1].position = Vector2.Lerp(claw[1].position, NPC.Center + openOffset + new Vector2(165, 45).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.2f);
                claw[2].position = Vector2.Lerp(claw[2].position, NPC.Center - openOffset + new Vector2(-110, 55).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.2f);
            }
            if (open)
            {
                openOffset += Vector2.UnitX * 6;
            }
            if (AITimer >= 110 && AITimer < 155)
            {
                open = true;
            }
            if (AITimer >= 155)
            {
                open = false;
            }
            if (AITimer >= 190)
            {
                ResetState();
            }
        }
    }


    public void DoClawMultiple()
    {
        NetUpdateAtSpecificTime(40, 90, 140, 190);
        if (AITimer % 50 == 1)
        {
            AITimer3 = new UnifiedRandom((int)NPC.ai[2]).Next(3);
            NPC.netUpdate = true;
        }

        if (AITimer % 50 == 2)
            for (int i = 0; i < 3; i++)
                MPUtils.NewProjectile(NPC.GetSource_FromAI(), claw[(int)AITimer3].position, Vector2.Zero, ProjectileType<BloodShockwave2>(), 0, 0);
        if (AITimer < 201 - (oldHP < NPC.lifeMax - NPC.lifeMax / 4 ? (oldHP < NPC.lifeMax / 2 ? 0 : 50) : 100))
        {
            if (AITimer3 != 0)
                claw[0].position = Vector2.Lerp(claw[0].position, NPC.Center + openOffset + new Vector2(150, -65).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.1f);
            if (AITimer3 != 1)
                claw[1].position = Vector2.Lerp(claw[1].position, NPC.Center + openOffset + new Vector2(165, 45).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.1f);
            if (AITimer3 != 2)
                claw[2].position = Vector2.Lerp(claw[2].position, NPC.Center - openOffset + new Vector2(-110, 55).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.1f);
            if (AITimer % 50 == 1 || (int)AITimer2 == 0)
            {
                AITimer2 = new UnifiedRandom((int)NPC.ai[2]).NextFloat(0.01f, Pi * 2);
                NPC.netUpdate = true;
            }
            if (AITimer % 50 < 40)
            {
                NPC.velocity = Helper.FromAToB(NPC.Center, player.Center - new Vector2(0, 200), false) / 2;
                savedPos = player.Center;
                if (AITimer3 == 0)
                    claw[0].position = Vector2.Lerp(claw[0].position, savedPos + new Vector2(0, -200).RotatedBy(AITimer2), 0.3f);
                if (AITimer3 == 1)
                    claw[1].position = Vector2.Lerp(claw[1].position, savedPos + new Vector2(0, -200).RotatedBy(AITimer2), 0.3f);
                if (AITimer3 == 2)
                    claw[2].position = Vector2.Lerp(claw[2].position, savedPos + new Vector2(0, -200).RotatedBy(AITimer2), 0.3f);
            }
            if (AITimer % 50 == 40)
            {
                for (int i = 0; i < claw.Length; i++)
                {
                    if (i == AITimer3)
                        MPUtils.NewProjectile(NPC.GetSource_FromAI(), claw[i].position + Vector2.UnitY.RotatedBy(AITimer2) * 110, -Vector2.UnitY.RotatedBy(AITimer2), ProjectileType<CecitiorClawSlash>(), 30, 0, ai2: 1);
                }
            }
            if (AITimer % 50 >= 40)
            {
                NPC.velocity *= 0.9f;
                for (int i = 0; i < claw.Length; i++)
                {
                    if (AITimer3 == 0)
                        claw[0].position = Vector2.Lerp(claw[0].position, savedPos - new Vector2(0, -200).RotatedBy(AITimer2), 0.04f);
                    if (AITimer3 == 1)
                        claw[1].position = Vector2.Lerp(claw[1].position, savedPos - new Vector2(0, -200).RotatedBy(AITimer2), 0.04f);
                    if (AITimer3 == 2)
                        claw[2].position = Vector2.Lerp(claw[2].position, savedPos - new Vector2(0, -200).RotatedBy(AITimer2), 0.04f);
                }
            }
        }
    }


    public void DoGrabBomb()
    {
        NetUpdateAtSpecificTime(15, 20, 66, 70, 120);
        void DoTheThing(int index, float dist)
        {
            AITimer2--;
            float swingProgress = Ease(Utils.GetLerpValue(0f, 35, AITimer2));
            float defRot = Helper.FromAToB(NPC.Center, savedPos).ToRotation();
            float start = defRot - (PiOver2 + PiOver4);
            float end = defRot + (PiOver2 + PiOver4);
            float rotation = start + Pi * 3 / 2 * swingProgress;
            Vector2 position = NPC.Center +
                rotation.ToRotationVector2() * dist * ScaleFunction(swingProgress);
            claw[index].position = Vector2.Lerp(claw[index].position, position, 0.2f);
            if (AITimer % 6 == 1)
            {
                SoundEngine.PlaySound(SoundID.Item1, claw[index].position);
                MPUtils.NewProjectile(NPC.GetSource_FromThis(), claw[index].position, rotation.ToRotationVector2() * 6, ProjectileType<CecitiorBombThing>(), 30, 0);
            }
        }

        if (AITimer < 20 || (AITimer >= 65 && AITimer < 75) || (AITimer >= 115 && AITimer < 125))
            NPC.velocity = Helper.FromAToB(NPC.Center, player.Center - new Vector2(0, 250), false) / 20f;
        else
            NPC.velocity *= 0.9f;

        if (AITimer <= 165 - (oldHP < NPC.lifeMax - NPC.lifeMax / 4 ? (oldHP < NPC.lifeMax / 2 ? 0 : 50) : 100))
            open = true;

        if (AITimer >= 20 && AITimer <= 30)
            savedPos = player.Center;
        if (AITimer < 15)
        {
            AITimer2 = 35;
            openOffset.X++;
            openRotation += ToRadians(2);
            rotation -= ToRadians(2);
        }
        if (AITimer == 66 || AITimer == 120)
        {
            AITimer2 = 35;
            savedPos = player.Center;
        }

        if (AITimer < 65)
        {
            claw[1].position = Vector2.Lerp(claw[1].position, NPC.Center + openOffset + new Vector2(165, 45).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.2f);
            claw[2].position = Vector2.Lerp(claw[2].position, NPC.Center - openOffset + new Vector2(-110, 55).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.2f);
        }

        if (AITimer >= 15 && AITimer < 25)
            claw[0].position = Vector2.Lerp(claw[0].position, NPC.Center + new Vector2(0, 10), 0.2f);

        if (AITimer >= 30 && AITimer <= 65)
            DoTheThing(0, 210);

        if (oldHP < NPC.lifeMax - NPC.lifeMax / 4)
        {
            if (AITimer >= 65 && AITimer < 115)
            {
                claw[0].position = Vector2.Lerp(claw[0].position, NPC.Center + openOffset + new Vector2(150, -65).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.2f);
                claw[2].position = Vector2.Lerp(claw[2].position, NPC.Center - openOffset + new Vector2(-110, 55).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.2f);
            }
            if (AITimer >= 50 && AITimer < 70)
                claw[1].position = Vector2.Lerp(claw[1].position, NPC.Center + new Vector2(0, 10), 0.2f);

            if (AITimer >= 80 && AITimer <= 115)
                DoTheThing(1, 160);

            if (oldHP < NPC.lifeMax / 2)
            {
                if (AITimer >= 115 && AITimer < 165)
                {
                    claw[0].position = Vector2.Lerp(claw[0].position, NPC.Center + openOffset + new Vector2(150, -65).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.2f);
                    claw[1].position = Vector2.Lerp(claw[1].position, NPC.Center + openOffset + new Vector2(165, 45).RotatedBy(MathF.Sin(NPC.ai[2] * 0.01f) * 0.4f), 0.2f);
                }
                if (AITimer >= 105 && AITimer < 125)
                    claw[2].position = Vector2.Lerp(claw[2].position, NPC.Center + new Vector2(0, 10), 0.2f);

                if (AITimer >= 125 && AITimer <= 165)
                    DoTheThing(2, 170);
            }
        }

        if (AITimer >= 165 - (oldHP < NPC.lifeMax - NPC.lifeMax / 4 ? (oldHP < NPC.lifeMax / 2 ? 0 : 50) : 100))
        {
            openOffset = Vector2.Zero;
            openRotation = Lerp(openRotation, 0f, 0.1f);
            rotation = Lerp(rotation, 0f, 0.1f);
            open = false;
        }
    }


    public void DoBodySlam()
    {
        if (AITimer < 20)
            NPC.velocity = Helper.FromAToB(NPC.Center, Helper.TRay.Cast(player.Center, Vector2.UnitY, 1200, true) - new Vector2(0, 200), false) / 10f;
        if (AITimer > 20 && AITimer < 45)
        {
            NPC.velocity *= 0.8f;
            claw[0].position = Vector2.Lerp(claw[0].position, Helper.TRay.Cast(NPC.Center + new Vector2(70, 0), new Vector2(0.2f, 1), 400, true) + new Vector2(0, 25), 0.1f);
            claw[1].position = Vector2.Lerp(claw[1].position, Helper.TRay.Cast(NPC.Center, new Vector2(-0.05f, 1), 400, true) + new Vector2(0, 25), 0.1f);
            claw[2].position = Vector2.Lerp(claw[2].position, Helper.TRay.Cast(NPC.Center + new Vector2(-75, 0), new Vector2(-0.25f, 1), 400, true) + new Vector2(0, 25), 0.1f);
        }
        if (AITimer <= 60 && AITimer >= 50)
        {
            NPC.velocity.Y += 3;
            NPC.damage = 100;
        }
        if (Helper.TRay.CastLength(NPC.Center, Vector2.UnitY, NPC.height * 2, true) < NPC.height * 0.6f && (int)AITimer2 == 0 && AITimer > 45)
        {
            if (AITimer < 60)
                AITimer = 61;
            AITimer2 = 1;
            CameraSystem.ScreenShakeAmount = 5;
            NPC.velocity = Vector2.UnitY * -17.5f;

            SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
            for (int i = 0; i < 6; i++)
            {
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center - new Vector2(0, 100), Main.rand.NextVector2Circular(7, 7), ProjectileType<HostileGibs>(), 40, 0, 0, 0);

                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center - new Vector2(0, 100), Main.rand.NextVector2Circular(4, 4), ProjectileType<CecitiorEyeP>(), 40, 0, 0, 0);
            }

            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ProjectileType<FatSmash>(), 0, 0, 0, 0);
            SoundEngine.PlaySound(EbonianSounds.cecitiorSlam, NPC.Center);
            NPC.netUpdate = true;
        }
        if (AITimer > 65 && (int)AITimer2 == 1)
        {
            NPC.velocity *= 0.9f;
            NPC.damage = 0;
        }

        if (AITimer >= 100 && oldHP > NPC.lifeMax - NPC.lifeMax / 4)
        {
            NPC.damage = 0;
            ResetState();
        }

        if (AITimer >= 120 && oldHP <= NPC.lifeMax - NPC.lifeMax / 4)
        {
            AITimer = 70;
            AITimer2 = 2;
            NPC.damage = 0;
            AITimer3++;
            if (AITimer3 > (oldHP < NPC.lifeMax / 2 ? 1 : 0))
            {
                ResetState();
            }
        }
        if (oldHP <= NPC.lifeMax - NPC.lifeMax / 4)
        {
            if (AITimer <= 80 && AITimer >= 71)
            {
                if (AITimer == 71)
                {
                    AITimer2 = 2;
                    NPC.netUpdate = true;
                }
                NPC.damage = 100;
                NPC.velocity.Y += 3;
            }
            if (Helper.TRay.CastLength(NPC.Center, Vector2.UnitY, NPC.height * 2, true) < NPC.height * 0.6f && (int)AITimer2 == 2)
            {
                if (AITimer < 80)
                    AITimer = 81;
                AITimer2 = 3;
                CameraSystem.ScreenShakeAmount = 5;
                NPC.velocity = Vector2.UnitY * -17.5f;

                MPUtils.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ProjectileType<FatSmash>(), 0, 0, 0, 0);
                for (int i = 0; i < 2; i++)
                    MPUtils.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ProjectileType<BloodShockwave2>(), 0, 0);
                if ((int)AITimer3 == 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center - new Vector2(0, 100), Main.rand.NextVector2Circular(14, 14), ProjectileType<Gibs>(), 40, 0, 0, 0);
                        p.SetAsHostile();
                        MPUtils.NewProjectile(NPC.GetSource_FromThis(), NPC.Center - new Vector2(0, 100), new Vector2(Main.rand.NextFloat(-4f, 4), Main.rand.NextFloat(-7, -3)), ProjectileType<CIchor>(), 40, 0, 0, 0);
                    }
                }
                if ((int)AITimer3 == 1)
                {
                    SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                    for (int i = 0; i < 16; i++)
                    {
                        Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center - new Vector2(0, 100), Main.rand.NextVector2Circular(14, 14), ProjectileType<Gibs>(), 40, 0, 0, 0);
                        p.SetAsHostile();
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center - new Vector2(0, 100), Main.rand.NextVector2Circular(4, 4), ProjectileType<CecitiorEyeP>(), 40, 0, 0, 0);
                    }
                }
                SoundEngine.PlaySound(EbonianSounds.cecitiorSlam, NPC.Center);
                NPC.netUpdate = true;
            }
            if (AITimer > 85 && (int)AITimer2 == 3)
            {
                NPC.damage = 0;
                NPC.velocity *= 0.8f;
                NPC.velocity.Y--;
            }
        }
    }
}
