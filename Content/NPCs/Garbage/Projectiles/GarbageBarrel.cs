namespace EbonianMod.Content.NPCs.Garbage.Projectiles;

public class GarbageBarrel : ModProjectile
{
    public override string Texture => Helper.AssetPath + "Projectiles/Garbage/" + Name;
    public override void SetDefaults()
    {
        Projectile.width = 48;
        Projectile.height = 48;
        Projectile.aiStyle = -1;
        Projectile.friendly = false;
        Projectile.tileCollide = true;
        Projectile.aiStyle = 14;
        AIType = ProjectileID.BouncyGlowstick;
        Projectile.hostile = true;
        Projectile.timeLeft = 400;
    }
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Projectile.velocity.Y = -5;
        return false;
    }
    public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
    {
        if (Projectile.Center.Y >= Main.LocalPlayer.Center.Y - 100)
            fallThrough = false;
        return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
    }
    int dir;
    public override void OnSpawn(IEntitySource source)
    {
        dir = Projectile.velocity.X > 0 ? 1 : -1;
        if (Projectile.Center.Y >= Main.LocalPlayer.Center.Y + 100)
            Projectile.velocity.Y -= (Projectile.Center.Y - Main.LocalPlayer.Center.Y) * 0.035f;
    }
    public override void AI()
    {
        Projectile.direction = dir;
        Projectile.velocity.X = Projectile.direction * 5;

        Projectile.rotation += MathHelper.ToRadians(3);
    }
}
