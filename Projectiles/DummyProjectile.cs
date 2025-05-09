namespace EbonianMod.Projectiles;

public class DummyProjectile : ModProjectile
{
    public override string Texture => Helper.Empty;
    public override void SetDefaults()
    {
        Projectile.Size = Vector2.One;
    }
    public override bool? CanDamage() => false;
    public override void AI()
    {
        Projectile.Kill();
    }
}
