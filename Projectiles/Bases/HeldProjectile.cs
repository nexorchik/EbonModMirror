
namespace EbonianMod.Projectiles.Bases;

public abstract class HeldProjectile : ModProjectile
{
    protected int ItemType;
    protected Vector2 PositionOffset;

    public override void SetDefaults()
    {
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
    }
    public override void AI()
    {
        Player player = Main.player[Projectile.owner];

        player.itemTime = 2;
        player.itemAnimation = 2;
        if (Projectile.timeLeft == 0) Projectile.timeLeft = 100;

        if (!player.active || player.dead || player.CCed || player.HeldItem.type != ItemType) Projectile.Kill();

        Projectile.Center = player.MountedCenter + PositionOffset.RotatedBy(player.fullRotation * 100);

        player.direction = player.Center.X < Main.MouseWorld.X ? 1 : -1;
    }
}
