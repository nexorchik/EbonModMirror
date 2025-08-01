using EbonianMod.Projectiles.VFXProjectiles;

namespace EbonianMod.Projectiles.Friendly.Underworld;

public class CorebreakerP : ModProjectile
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
        Projectile CurrentProjectile = Projectile.NewProjectileDirect(NPC.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ProjectileType<FlameExplosionWSprite>(), Projectile.damage, 0);
        CurrentProjectile.friendly = true;
        CurrentProjectile.hostile = false;
        CurrentProjectile.netUpdate = true; // TEST
        SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
    }
}
