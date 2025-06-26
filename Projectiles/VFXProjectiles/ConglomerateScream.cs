using System;

namespace EbonianMod.Projectiles.VFXProjectiles;

public class ConglomerateScream : ModProjectile
{
    public override string Texture => Helper.Empty;
    public override void SetDefaults()
    {
        Projectile.width = 30;
        Projectile.height = 30;
        Projectile.aiStyle = -1;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.timeLeft = 19;
        Projectile.scale = 0;
        Projectile.tileCollide = false;
    }
    public override bool? CanDamage()
    {
        return false;
    }
    public override bool ShouldUpdatePosition()
    {
        return false;
    }
    int seed;
    public override void OnSpawn(IEntitySource source)
    {
        seed = Main.rand.Next(int.MaxValue);
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D tex = Assets.Extras.cone4.Value;
        UnifiedRandom rand = new UnifiedRandom(seed);
        float max = 40;
        float alpha = Lerp(0.5f, 0, Projectile.ai[1]) * 2;
        for (float i = 0; i < max; i++)
        {
            float angle = Helper.CircleDividedEqually(i, max);
            Color col = Color.Lerp(Color.LawnGreen, Color.Maroon, rand.NextFloat());
            if (Projectile.ai[2] == 1)
                col = Color.LawnGreen;

            else if (Projectile.ai[2] == 2)
                col = Color.Maroon;

            if (Projectile.velocity.LengthSquared() > 0)
                angle = Projectile.velocity.RotatedBy(rand.NextFloat(PiOver4 * -0.8f, PiOver4 * 0.8f)).RotatedByRandom(PiOver4 * 0.1f).ToRotation();

            float scale = rand.NextFloat(0.2f, 1f) * Projectile.ai[0] == 0 ? 1 : Projectile.ai[0];
            Vector2 offset = new Vector2(rand.NextFloat(150, 300) * Main.rand.NextFloat(0.9f, 1.1f) * Projectile.ai[1] * scale, 0).RotatedBy(angle);
            for (float j = 0; j < 2; j++)
                Main.spriteBatch.Draw(tex, Projectile.Center + offset - Main.screenPosition, null, col with { A = 0 } * alpha * 0.5f, angle, new Vector2(0, tex.Height / 2), new Vector2(Projectile.ai[1], alpha) * scale * 2, SpriteEffects.None, 0);
        }
        return false;
    }
    public override void AI()
    {
        float progress = Utils.GetLerpValue(0, 20, Projectile.timeLeft);
        Projectile.scale = Clamp((float)Math.Sin(progress * Pi) * 0.5f, 0, 0.5f);
        Projectile.ai[1] = Lerp(Projectile.ai[1], 1, 0.2f);
    }
}
