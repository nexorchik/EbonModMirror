

using System.IO;
using Terraria.Chat;

namespace EbonianMod.Content.Projectiles.Bases;

public abstract class HeldProjectile : ModProjectile
{
    protected int ItemType;
    protected float AttackDelayMultiplier, AttackSpeedMultiplier;

    public override void SetDefaults()
    {
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
    }
    Vector2 Direction;
    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.Write(AttackDelayMultiplier);
        writer.Write(AttackSpeedMultiplier);
        writer.WriteVector2(Direction);
    }
    public override void ReceiveExtraAI(BinaryReader reader)
    {
        AttackDelayMultiplier = reader.ReadSingle();
        AttackSpeedMultiplier = reader.ReadSingle();
        Direction = reader.ReadVector2();
    }
    protected void CalculateAttackSpeedParameters(float baseValue)
    {
        Player player = Main.player[Projectile.owner];
        float itemTime = player.itemTime;
        AttackDelayMultiplier = itemTime / baseValue;
        AttackSpeedMultiplier = baseValue / itemTime;
        Projectile.netUpdate = true;
    }
    public override void AI()
    {
        Player player = Main.player[Projectile.owner];

        player.itemTime = 2;
        player.itemAnimation = 2;
        if (Projectile.timeLeft == 0) Projectile.timeLeft = 100;

        if (!player.active || player.dead || player.CCed || player.HeldItem.type != ItemType) Projectile.Kill();

        if (player.whoAmI == Main.myPlayer)
        {
            Direction = Main.MouseWorld - player.Center;
        }
        player.ChangeDir(Direction.X > 0 ? 1 : -1);
        Projectile.Center = player.MountedCenter;
    }
}
