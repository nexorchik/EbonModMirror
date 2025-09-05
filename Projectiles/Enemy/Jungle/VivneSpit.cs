
using EbonianMod.Dusts;

namespace EbonianMod.Projectiles.Enemy.Jungle;
public class VivineSpit : ModProjectile
{
    Vector2 Scale;
    public override void SetDefaults()
    {
        Projectile.Size = Vector2.One * 24;
        Projectile.friendly = false;
        Projectile.hostile = true;
        Projectile.tileCollide = true;
        Projectile.timeLeft = 300;
    }
    public override void AI()
    {
        Lighting.AddLight(Projectile.Center, new Vector3(0.425f, 0.2f, 0.252f));
        Projectile.rotation = Projectile.velocity.ToRotation();
        if (Projectile.ai[0] > -0.5f)
            Projectile.ai[0] -= 0.02f;
        Projectile.velocity.Y -= Projectile.ai[0];
        float velocityLength = Projectile.velocity.Length();
        Scale = new Vector2(Clamp(velocityLength / 15, 1, 4), Clamp(2 / velocityLength, 0.6f, 1));
        Dust.NewDustPerfect(Projectile.Center, DustType<JunglePinkDust>(), (Projectile.rotation + Pi + Main.rand.NextFloat(-Pi / 6, Pi / 6)).ToRotationVector2() * Main.rand.NextFloat(2, 3), Scale: Main.rand.NextFloat(0.4f, 0.6f));
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, Projectile.Size / 2, Projectile.scale * Scale, SpriteEffects.None);
        return false;
    }
    public override void OnKill(int timeleft)
    {
        for (int i = 0; i < 15; i++)
        {
            Dust.NewDustPerfect(Projectile.Center, DustType<JunglePinkDust>(), (Main.rand.NextFloat(0, -Pi)).ToRotationVector2() * Main.rand.NextFloat(1, 3) + new Vector2(0, Main.rand.NextFloat(0, -6)), Scale: Main.rand.NextFloat(0.4f, 0.7f));
        }
    }
}