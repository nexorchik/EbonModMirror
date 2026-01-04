namespace EbonianMod.Content.Projectiles.VFXProjectiles;

public class IchorExplosion : ModProjectile
{
    public override string Texture => Helper.AssetPath + "Projectiles/VFXProjectiles/" + Name;
    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 6;
    }
    public override void SetDefaults()
    {
        Projectile.width = 97;
        Projectile.height = 84;
        Projectile.aiStyle = -1;
        Projectile.friendly = false;
        Projectile.hostile = false;
        Projectile.penetrate = -1;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D tex = TextureAssets.Projectile[Type].Value;
        Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 86, 97, 86), Color.White, Projectile.rotation, new Vector2(tex.Width / 2, 78), Projectile.scale, SpriteEffects.None, 0);
        return false;
    }
    public override Color? GetAlpha(Color lightColor) => Color.White;
    public override void AI()
    {
        if (++Projectile.frameCounter >= 3)
        {
            Projectile.frameCounter = 0;
            if (++Projectile.frame >= 6)
            {
                Projectile.Kill();
            }
        }
    }
}
