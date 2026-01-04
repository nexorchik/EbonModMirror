namespace EbonianMod.Content.Projectiles.VFXProjectiles;

internal class QuickFlare : ModProjectile
{
    public override string Texture => Helper.Empty;
    public override void SetDefaults()
    {
        Projectile.height = 300;
        Projectile.width = 300;
        Projectile.hostile = true;
        Projectile.friendly = false;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hitinfo, int damage)
    {
        Projectile.ai[1] = 1;
    }
    public override void PostAI()
    {
        if (Projectile.ai[1] == 1)
            Projectile.damage = 0;
    }
    public override bool ShouldUpdatePosition() => false;
    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D tex = Assets.Extras.crosslight.Value;
        Texture2D tex2 = Assets.Extras.Extras2.star_04.Value;
        float alpha = MathHelper.Lerp(1, 0, Projectile.ai[0]);
        Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, Color.White with { A = 0 } * 0.75f * Projectile.ai[0], Projectile.rotation, tex.Size() / 2, alpha * 4f, SpriteEffects.None, 0);
        Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.OrangeRed with { A = 0 } * Projectile.ai[0], Projectile.rotation, tex.Size() / 2, alpha * 4, SpriteEffects.None, 0);
        return false;
    }
    public override void AI()
    {
        Projectile.ai[0] += 0.05f;
        if (Projectile.ai[0] > 1)
            Projectile.Kill();
    }
}
