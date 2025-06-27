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
    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D a = TextureAssets.Projectile[Type].Value;
        var fadeMult = Helper.Safe(1f / Projectile.oldPos.Length);
        for (int i = 0; i < Projectile.oldPos.Length; i++)
        {
            Main.spriteBatch.Draw(a, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, lightColor * (1f - fadeMult * i), Projectile.rotation, a.Size() / 2, Projectile.scale * (1f - fadeMult * i), SpriteEffects.None, 0);
        }
        return true;
    }
    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.IchorArrow);
        Projectile.width = 18;
        Projectile.height = 42;
        Projectile.extraUpdates = 1;
    }
    public override void AI()
    {
        Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(10, 10), DustID.IchorTorch, Projectile.velocity * 0.3f, Scale: 2).noGravity = true;
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(BuffID.Ichor, 180);
    }
}
