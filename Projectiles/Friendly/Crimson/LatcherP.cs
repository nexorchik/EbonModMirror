using EbonianMod.NPCs.Corruption;
using EbonianMod.Projectiles.Cecitior;
using System;
using System.Collections.Generic;
using System.IO;

namespace EbonianMod.Projectiles.Friendly.Crimson;

public class LatcherP : ModProjectile
{
    public override string Texture => Helper.Empty;
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.DrawScreenCheckFluff[Type] = 3000;
    }
    public override void SetDefaults()
    {
        Projectile.width = 25;
        Projectile.height = 25;
        Projectile.aiStyle = -1;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.hostile = false;
        Projectile.penetrate = -1;
        Projectile.timeLeft = 200;
        Projectile.hide = true;
    }

    float Rotation = 0;

    public override void OnSpawn(IEntitySource source)
    {
        Rotation = Helper.FromAToB(Main.player[Projectile.owner].Center, Projectile.Center).ToRotation();
    }

    bool IsAttached;
    Vector2 PositionOffset;
    int TargetIndex = -1;
    float Speed = 0.25f;
    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.Write(IsAttached);
        writer.WriteVector2(PositionOffset);
        writer.Write(TargetIndex);
        writer.Write(Speed);
    }
    public override void ReceiveExtraAI(BinaryReader reader)
    {
        IsAttached = reader.ReadBoolean();
        PositionOffset = reader.ReadVector2();
        TargetIndex = reader.ReadInt32();
        Speed = reader.ReadSingle();
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hitinfo, int damage)
    {
        if (!IsAttached)
        {
            PositionOffset = Projectile.Center - target.Center;
            Projectile.velocity = Vector2.Zero;
            TargetIndex = target.whoAmI;
            IsAttached = true;
            Projectile.netUpdate = true; // TEST
        }
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        behindProjectiles.Add(index);
    }

    public override void AI()
    {
        Player player = Main.player[Projectile.owner];
        Rotation = Utils.AngleLerp(Rotation, Helper.FromAToB(player.Center, Main.MouseWorld).ToRotation(), 0.02f);
        Projectile.ai[0]++;
        if (IsAttached && TargetIndex > -1)
        {
            NPC Target = Main.npc[TargetIndex];
            if (!Target.active)
                Projectile.Kill();

            Speed *= 1.08f;
            Speed = Clamp(Speed, 0, 23);
            Projectile.timeLeft = 10;
            player.velocity += Helper.FromAToB(player.Center, Projectile.Center, true) * Speed;
            player.SyncPlayerControls();
            Projectile.Center = Target.Center + PositionOffset;
            if (Vector2.Distance(player.Center, Target.Center) < 100)
            {
                SoundEngine.PlaySound(EbonianSounds.chomp2.WithPitchOffset(Main.rand.NextFloat(0.2f, 0.4f)), Projectile.Center);
                SoundEngine.PlaySound(EbonianSounds.chomp1.WithPitchOffset(Main.rand.NextFloat(-0.6f, -0.2f)), Projectile.Center);
                player.velocity *= 0.4f;
                Target.SimpleStrikeNPC((int)player.velocity.Length() * Main.rand.Next(7, 8), 0, false);
                for (int i = 0; i < 15; i++)
                {
                    Dust.NewDustPerfect(Target.Center, DustID.Blood, (Projectile.rotation + Main.rand.NextFloat(-PiOver4, PiOver4)).ToRotationVector2() * Main.rand.NextFloat(2, 6) * player.direction, Scale: 1.5f);
                }
                Projectile.Kill();
            }
            if (Target.life <= 0)
                Projectile.Kill();
        }
        else if (Projectile.ai[0] > 10)
        {
            Projectile.ai[1]++;
            Projectile.Center += Helper.FromAToB(Projectile.Center, player.Center) * Projectile.ai[1] * 3;
            if (Vector2.Distance(player.Center, Projectile.Center) < 35)
                Projectile.Kill();
        }
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Player player = Main.player[Projectile.owner];
        Vector2 neckOrigin = player.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * 40;
        Vector2 center = Projectile.Center;
        Vector2 distToProj = neckOrigin - Projectile.Center;
        float projRotation = distToProj.ToRotation() - 1.57f;
        float distance = distToProj.Length();
        while (distance > 20 && !float.IsNaN(distance))
        {
            distToProj.Normalize();
            distToProj *= 20;
            center += distToProj;
            distToProj = neckOrigin - center;
            distance = distToProj.Length();

            //Draw chain
            Main.spriteBatch.Draw(Assets.Projectiles.Friendly.Crimson.LatcherP_Chain.Value, center - Main.screenPosition,
                null, new Color(Lighting.GetSubLight(center)), projRotation,
                Assets.Projectiles.Friendly.Crimson.LatcherP_Chain.Value.Size() / 2, 1f, SpriteEffects.None, 0);
        }
        Main.spriteBatch.Draw(Assets.Projectiles.Friendly.Crimson.LatcherP_Chain.Value, Projectile.Center - Main.screenPosition, null, new Color(Lighting.GetSubLight(Projectile.Center)), projRotation, Assets.Projectiles.Friendly.Crimson.LatcherP_Chain.Value.Size() / 2, 1f, SpriteEffects.None, 0);
        return false;
    }
}
public class LatcherPCecitior : ModProjectile
{
    public override string Texture => "EbonianMod/Projectiles/Friendly/Crimson/LatcherP";
    public override void SetDefaults()
    {
        Projectile.width = 12;
        Projectile.height = 20;
        Projectile.aiStyle = -1;
        Projectile.friendly = false;
        Projectile.tileCollide = true;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.hide = true;
        Projectile.hostile = true;
        Projectile.penetrate = -1;
        Projectile.timeLeft = 300;
    }
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.DrawScreenCheckFluff[Type] = 3000;
    }
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Projectile.velocity = Vector2.Zero;
        if (Projectile.localAI[0] == 0)
        {
            Projectile.timeLeft = 200;
            Projectile.ai[1] = 1;
        }
        return false;
    }
    public override void OnHitPlayer(Player target, Player.HurtInfo info)
    {
        Projectile.velocity = Vector2.Zero;
        if (Projectile.localAI[0] == 0 && Projectile.ai[1] == 0)
        {
            Projectile.localAI[0] = target.whoAmI;
            Projectile.timeLeft = 100;
            Projectile.ai[1] = 2;
        }
    }

    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.Write(Projectile.localAI[0]);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
        Projectile.localAI[0] = reader.ReadSingle();
    }

    public override void AI()
    {
        if (Helper.TRay.CastLength(Projectile.Center, Vector2.UnitY, 30, true) < 20)
        {
            Projectile.velocity = Vector2.Zero;
            if (Projectile.localAI[0] == 0)
            {
                Projectile.timeLeft = 200;
                Projectile.ai[1] = 1;
            }
        }
        if (Projectile.ai[1] == 0)
            Projectile.rotation = Projectile.velocity.ToRotation();
        if (!NPC.AnyNPCs(NPCType<NPCs.Cecitior.Cecitior>()))
            Projectile.Kill();
        NPC player = Main.npc[(int)Projectile.ai[0]];
        if (player.ai[0] != 8)
            Projectile.Kill();
        if (Projectile.ai[1] == 1)
        {
            player.velocity = Helper.FromAToB(player.Center, Projectile.Center) * 25;
            if (player.Center.Distance(Projectile.Center) < 50)
            {
                Projectile.ai[2] = 1;
                MPUtils.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, 0), ProjectileType<FatSmash>(), 0, 0, 0, 0);

                for (int i = -6; i < 6; i++)
                {
                    if (i == 0) continue;
                    MPUtils.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(i * 3, Lerp(-3, -5, MathF.Abs(i) / 6)), ProjectileType<CecitiorTeeth>(), 20, 0, 0, 0);
                }
                player.velocity = Projectile.rotation.ToRotationVector2().RotatedByRandom(PiOver4) * -10f;
                Projectile.Kill();
                SoundEngine.PlaySound(EbonianSounds.cecitiorSlam, Projectile.Center);
                Projectile.netUpdate = true;
            }
        }
        else if (Projectile.ai[1] == 2)
        {
            Player playerr = Main.player[(int)Projectile.localAI[0]];
            playerr.velocity = Helper.FromAToB(playerr.Center, player.Center, false) / 10;
            Projectile.velocity = Helper.FromAToB(Projectile.Center, player.Center) * 20;
        }
        else
        {
            if (Projectile.velocity.Length() < 24)
                Projectile.velocity *= 1.15f;
            if (Projectile.timeLeft < 100)
            {
                Projectile.Center = Vector2.Lerp(Projectile.Center, player.Center, 0.4f);

                if (player.Center.Distance(Projectile.Center) < 50)
                    Projectile.Kill();
                Projectile.netUpdate = true;
            }
        }
    }
    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        behindNPCs.Add(index);
    }
    public override bool PreDraw(ref Color lightColor)
    {

        NPC player = Main.npc[(int)Projectile.ai[0]];
        Vector2 neckOrigin = Projectile.Center;
        Vector2 center = player.Center;
        Vector2 distToProj = neckOrigin - player.Center;
        float projRotation = distToProj.ToRotation() - 1.57f;
        float distance = distToProj.Length();
        while (distance > 20 && !float.IsNaN(distance))
        {
            distToProj.Normalize();
            distToProj *= 20;
            center += distToProj;
            distToProj = neckOrigin - center;
            distance = distToProj.Length();

            //Draw chain
            Main.spriteBatch.Draw(Assets.Projectiles.Friendly.Crimson.LatcherP_Chain.Value, center - Main.screenPosition,
                null, Lighting.GetColor((int)center.X / 16, (int)center.Y / 16), projRotation,
                Assets.Projectiles.Friendly.Crimson.LatcherP_Chain.Value.Size() / 2, 1f, SpriteEffects.None, 0);
        }
        return true;
    }
}
