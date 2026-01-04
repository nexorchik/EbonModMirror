using EbonianMod.Content.Items.Misc;

namespace EbonianMod.Content.Projectiles.Friendly.Corruption;

public class CorruptionBalls : ModProjectile
{
    public override string Texture => Helper.AssetPath + "Projectiles/Friendly/Corruption/" + Name;
    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 2;
    }
    public override void SetDefaults()
    {
        Projectile.Size = new Vector2(32);
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.aiStyle = 2;
        Projectile.DamageType = DamageClass.Ranged;
    }
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        if (Helper.TileRaycast.CastLength(Projectile.Center, -Vector2.UnitY, Projectile.height * 1.5f) < Projectile.height)
        {
            Projectile.velocity.Y = 0;
            return false;
        }
        if ((Helper.TileRaycast.CastLength(Projectile.Center, Vector2.UnitX, Projectile.height * 1.5f) < Projectile.width || Helper.TileRaycast.CastLength(Projectile.Center, -Vector2.UnitX, Projectile.height * 1.5f) < Projectile.width) && Helper.TileRaycast.CastLength(Projectile.Center, Vector2.UnitY, Projectile.height * 1.5f) > Projectile.height)
        {
            Projectile.velocity.X = 0;
            return false;
        }
        Projectile.Kill();
        return true;
    }

    public override void OnKill(int timeLeft)
    {
        Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ProjectileType<OstertagiExplosion>(), 0, 0);
        SoundEngine.PlaySound(Sounds.eggplosion, Projectile.Center);
    }

    public override void AI()
    {
        Projectile.frameCounter++;
        if (Projectile.frameCounter % 5 == 0)
        {
            if (Projectile.frame == 0) Projectile.frame++;
            else Projectile.frame = 0;
        }
        Projectile.velocity.Y += 0.25f;
    }
}
