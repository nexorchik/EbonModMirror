
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
        Scale = new Vector2(Clamp(velocityLength / 9 - 1.2f, 0.6f, 1.3f), Clamp(3 / velocityLength, 0.38f, 0.6f));
        Dust.NewDustPerfect(Projectile.Center, DustType<JunglePinkDust>(), (Projectile.rotation + Pi + Main.rand.NextFloat(-Pi / 8, Pi / 8)).ToRotationVector2() * Main.rand.NextFloat(2f, 4f) * Projectile.velocity.Length() / 14, Scale: Main.rand.NextFloat(0.4f, 0.5f));
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, Projectile.Size / 2, Projectile.scale * Scale, SpriteEffects.None);
        return false;
    }
    public override void OnKill(int timeleft)
    {
        for (int i = 0; i < 12; i++)
        {
            Dust.NewDustPerfect(Projectile.Center, DustType<JunglePinkDust>(), (Main.rand.NextFloat(0, -Pi)).ToRotationVector2() * Main.rand.NextFloat(1, 3) + new Vector2(0, Main.rand.NextFloat(0, -5)), Scale: Main.rand.NextFloat(0.34f, 0.46f));
        }
    }
}