using EbonianMod.Projectiles.VFXProjectiles;

namespace EbonianMod.Projectiles.Friendly.Underworld;

public class CoreProjectile : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.Size = new Vector2(30);
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.aiStyle = 2;
    }
    public override Color? GetAlpha(Color lightColor)
    {
        return Color.White;
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        ExplosionSpawn();
    }
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        ExplosionSpawn();
        return true;
    }
    void ExplosionSpawn()
    {
        Projectile currentProjectile = Projectile.NewProjectileDirect(NPC.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ProjectileType<FlameExplosionWSprite>(), Projectile.damage, 0);
        currentProjectile.friendly = true;
        currentProjectile.hostile = false;
        currentProjectile.netUpdate = true;
        SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
    }
}
