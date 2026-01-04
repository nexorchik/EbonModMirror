using EbonianMod.Content.Projectiles.VFXProjectiles;

namespace EbonianMod.Content.Projectiles.Cecitior;

public class CecitiorBombThing : ModProjectile
{
    public override string Texture => Helper.AssetPath + "Projectiles/Cecitior/"+Name;
    public override void SetDefaults()
    {
        Projectile.Size = new Vector2(32);
        Projectile.friendly = false;
        Projectile.hostile = true;
        Projectile.aiStyle = 2;
    }
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Helper.TileRaycast.Cast(Projectile.Center, Vector2.UnitY, 200), Vector2.Zero, ProjectileType<IchorExplosion>(), Projectile.damage, 0);
        Terraria.Audio.SoundEngine.PlaySound(Sounds.eggplosion, Projectile.Center);
        return true;
    }
    public override void AI()
    {
        if (Projectile.timeLeft % 5 == 0)
            for (int i = 0; i < 3; i++)
            {
                Vector2 offset = Main.rand.NextVector2CircularEdge(15, 15);
                Dust.NewDustPerfect(Projectile.Center + offset, DustID.IchorTorch, Helper.FromAToB(Projectile.Center + offset, Projectile.Center));
            }
    }
}
