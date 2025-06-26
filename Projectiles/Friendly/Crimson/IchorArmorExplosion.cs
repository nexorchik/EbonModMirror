using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.Projectiles.Friendly.Crimson;
public class IchorArmorExplosion : ModProjectile
{
    public override string Texture => Helper.Placeholder;
    public override void SetDefaults()
    {
        Projectile.Size = new(100, 100);
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.DamageType = DamageClass.MeleeNoSpeed;
        Projectile.penetrate = -1;
        Projectile.extraUpdates = 1;
    }
    int seed = 0;
    public override bool ShouldUpdatePosition() => false;
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(BuffID.Ichor, 300);
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D tex = Assets.Extras.Extras2.trace_04.Value;
        Texture2D tex2 = Assets.Extras.Extras2.trace_05.Value;
        Texture2D tex3 = Assets.Extras.speckle.Value;
        UnifiedRandom rand = new UnifiedRandom(seed);
        float max = 30;
        float alpha = Lerp(0.5f, 0, Projectile.ai[1]) * 2;
        Main.spriteBatch.Draw(tex3, Projectile.Center - Main.screenPosition, null, Color.Orange with { A = 0 } * MathF.Pow(alpha, 2) * 4, 0, tex3.Size() / 2, Projectile.ai[1], SpriteEffects.None, 0);
        for (float i = 0; i < max; i++)
        {
            float angle = Helper.CircleDividedEqually(i, max);
            float scale = rand.NextFloat(0.1f, 0.5f);
            Vector2 offset = new Vector2(rand.NextFloat(20, 80) * Projectile.ai[1] * 10 * scale, 0).RotatedBy(angle);
            Main.spriteBatch.Draw(tex, Projectile.Center + offset - Main.screenPosition, null, Color.Gold with { A = 0 } * (alpha * 0.5f), angle + PiOver2, new Vector2(tex.Width / 2, 0), new Vector2(alpha, Projectile.ai[1] * 2) * scale, SpriteEffects.None, 0);
            for (float j = 0; j < 3; j++)
                Main.spriteBatch.Draw(tex2, Projectile.Center + offset - Main.screenPosition, null, Color.Orange with { A = 0 } * alpha, angle + MathHelper.PiOver2, new Vector2(tex2.Width / 2, 0), new Vector2(alpha, Projectile.ai[1]) * scale * .7f * 2, SpriteEffects.None, 0);
        }
        return false;
    }
    public override void OnSpawn(IEntitySource source)
    {
        Projectile.scale = 0.01f;
        seed = Main.rand.Next(1000000);
    }
    public override void AI()
    {
        Projectile.ai[0]++;
        if (Projectile.ai[0] == 5)
        {
            for (int i = 0; i < Main.rand.Next(1, 4) - Projectile.ai[2]; i++)
            {
                Vector2 vel = new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, -2));
                Projectile a = MPUtils.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - vel, vel, ProjectileID.GoldenShowerFriendly, Projectile.damage, 0, Projectile.owner, ai2: 52); // magic number as identity since ai2 isnt used.
                if (a != null)
                {
                    a.DamageType = DamageClass.Melee;
                    a.netUpdate = true;
                }
            }
        }
        Projectile.ai[1] = Lerp(0, 1, InOutSine.Invoke(Projectile.ai[0] / 40f));
        Projectile.scale = Projectile.ai[1];
        if (Projectile.ai[0] > 40)
            Projectile.Kill();
    }
}
