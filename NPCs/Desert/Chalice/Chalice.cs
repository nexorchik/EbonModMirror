using EbonianMod.Projectiles.Enemy.Desert;
using System;

namespace EbonianMod.NPCs.Desert;

public class Chalice : ModNPC
{
    public override string Texture => Helper.Empty;

    public override void SetDefaults()
    {
        NPC.width = 28;
        NPC.height = 34;
        NPC.damage = 30;
        NPC.defense = 0;
        NPC.lifeMax = 70;
        NPC.noGravity = true;
        NPC.HitSound = SoundID.NPCHit4;
        NPC.DeathSound = SoundID.NPCDeath52;
    }

    bool AimingMode, Animation;
    Vector2 Position;
    float AttackCooldown, HoldOffset = 17, NeededRotation, Argument = 0;

    public override void OnSpawn(IEntitySource source)
    {
        Position = NPC.position;
        AttackCooldown = Main.rand.NextFloat(4f, 6f);
    }

    public override void AI()
    {
        AttackCooldown -= 0.02f;
        if (AttackCooldown <= 0)
        {
            AimingMode = true;
        }
        NPC.TargetClosest(true);
        NPC.velocity = Vector2.Zero;
        Player TargetPlayer = Main.player[NPC.target];
        HoldOffset = Lerp(HoldOffset, 17, 0.1f);
        if (AimingMode == false)
        {
            Argument += 0.035f;
            Position = new Vector2(Lerp(Position.X, TargetPlayer.position.X, 0.002f), Lerp(Position.Y, TargetPlayer.position.Y, 0.01f));
            NPC.position = new Vector2(Position.X + MathF.Sin(Argument) * 100, Position.Y - 120 + MathF.Cos(Argument) * 26);
            NPC.rotation = Utils.AngleLerp(NPC.rotation, (float)MathF.Cos(Argument) / 3.5f, 0.05f);
            NeededRotation = NPC.DirectionTo(TargetPlayer.Center).ToRotation() + Pi * 2.5f;
        }
        else
        {
            NPC.rotation = Lerp(NPC.rotation, NeededRotation, 0.14f);
            if (MathF.Abs(NPC.rotation - NeededRotation) < 0.04f && Animation == false)
            {
                Animation = true;
                SoundEngine.PlaySound(SoundID.Item14.WithPitchOffset(Main.rand.NextFloat(0.7f, 1.1f)), NPC.Center);
                SoundEngine.PlaySound(SoundID.Item17.WithPitchOffset(Main.rand.NextFloat(0.7f, 1.1f)), NPC.Center);
                SoundEngine.PlaySound(SoundID.Item21.WithPitchOffset(Main.rand.NextFloat(0.2f, 0.5f)), NPC.Center);
                MPUtils.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, (NPC.rotation - Pi / 2).ToRotationVector2() * 30, ProjectileType<AshBlast>(), 4, 0);
                HoldOffset = 0;
            }
            if (Animation == true)
            {
                NPC.ai[0]++;
                if (NPC.ai[0] > 50)
                {
                    NPC.ai[0] = 0;
                    Animation = false;
                    AimingMode = false;
                    AttackCooldown = Main.rand.NextFloat(4f, 6f);
                }
            }
        }
    }

    public override bool CanHitPlayer(Player target, ref int cooldownSlot)
    {
        return true;
    }

    public override void HitEffect(NPC.HitInfo hit)
    {

    }

    public override void OnKill()
    {

    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D texture = Helper.GetTexture("NPCs/Desert/Chalice/Chalice").Value;
        spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, null, drawColor, NPC.rotation, new Vector2(texture.Size().X / 2, HoldOffset), NPC.scale, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        return false;
    }
}
