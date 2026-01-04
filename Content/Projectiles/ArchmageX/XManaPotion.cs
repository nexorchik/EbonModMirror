namespace EbonianMod.Content.Projectiles.ArchmageX;

internal class XManaPotion : ModProjectile
{
    public override string Texture => Helper.AssetPath + "Projectiles/ArchmageX/"+Name;
    public override void SetDefaults()
    {
        Projectile.Size = new Vector2(20, 26);
        Projectile.friendly = false;
        Projectile.hostile = true;
        Projectile.aiStyle = 2;
    }
    public override void OnHitPlayer(Player target, Player.HurtInfo info)
    {
        Projectile.Kill();
    }
    public override Color? GetAlpha(Color lightColor) => Color.White * 0.9f;
    public override void OnKill(int timeLeft)
    {
        for (int i = 0; i < 10; i++)
            Dust.NewDustPerfect(Projectile.Center, DustID.Glass, Main.rand.NextVector2Circular(Projectile.velocity.Length(), Projectile.velocity.Length()));
        Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ProjectileType<XExplosionTiny>(), Projectile.damage, 0);
        SoundEngine.PlaySound(SoundID.Shatter, Projectile.Center);
    }
}
