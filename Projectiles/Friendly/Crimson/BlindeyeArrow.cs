using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.Projectiles.Friendly.Crimson;
public class BlindeyeArrow : ModProjectile
{
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 5;
        ProjectileID.Sets.TrailingMode[Type] = 0;
    }
    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.IchorArrow);
        Projectile.width = 18;
        Projectile.height = 18;
        Projectile.extraUpdates = 1;
    }
    public override void AI()
    {
        if (Main.rand.NextBool(6))
            Dust.NewDustPerfect(Projectile.Center + Projectile.velocity * 2, DustID.Ichor, Projectile.velocity.RotatedByRandom(0.2f) * 0.3f, Scale: 0.8f);
    }
    public override void OnKill(int timeLeft)
    {
        for (int i = 0; i < 4; i++)
            Dust.NewDustPerfect(Projectile.Center + Projectile.velocity * 2, DustID.Ichor, Projectile.velocity.RotatedByRandom(0.2f) * Main.rand.NextFloat(0.1f, 0.5f), Scale: 0.8f);
        Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(BuffID.Ichor, 180);
    }
}
