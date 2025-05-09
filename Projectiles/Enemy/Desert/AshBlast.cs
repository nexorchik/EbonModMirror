namespace EbonianMod.Projectiles.Enemy.Desert;

public class AshBlast : ModProjectile
{
    public override string Texture => Helper.Empty;

    public override void SetDefaults()
    {
        Projectile.width = 30;
        Projectile.height = 30;
        Projectile.tileCollide = true;
        Projectile.friendly = false;
        Projectile.hostile = true;
        Projectile.timeLeft = 290;
    }

    float Gravity;

    public override void AI()
    {
        Projectile.ai[0]++;
        if (Gravity > -0.5f)
            Gravity -= 0.02f;
        Projectile.velocity = new Vector2(Projectile.velocity.X, Projectile.velocity.Y - Gravity);
        Projectile.NewProjectileDirect(NPC.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ProjectileType<Ash>(), 0, 0);
        Dust.NewDustPerfect(Projectile.Center, DustID.Smoke, Main.rand.NextFloat(0, Pi * 2).ToRotationVector2(), Scale: 1.5f).noGravity = true;
    }

    public override void OnKill(int timeLeft)
    {
        SoundEngine.PlaySound(SoundID.NPCHit15.WithPitchOffset(Main.rand.NextFloat(0.8f, 1.2f)), Projectile.Center);
        SoundEngine.PlaySound(SoundID.Item1.WithPitchOffset(Main.rand.NextFloat(-1f, -0.4f)), Projectile.Center);
        SoundEngine.PlaySound(SoundID.Item20.WithPitchOffset(Main.rand.NextFloat(0.1f, 0.3f)), Projectile.Center);
        for (int i = 0; i < 30; i++)
        {
            Dust.NewDustPerfect(Projectile.Center, DustID.Smoke, Main.rand.NextFloat(-Pi, Pi).ToRotationVector2() * Main.rand.NextFloat(5, 15), Scale: Main.rand.NextFloat(1f, 3f)).noGravity = true;
            Dust.NewDustPerfect(Projectile.Center, DustID.Smoke, Main.rand.NextFloat(-Pi, Pi).ToRotationVector2() * Main.rand.NextFloat(5, 15), Scale: Main.rand.NextFloat(1f, 3f)).noGravity = true;
            Projectile.NewProjectileDirect(NPC.InheritSource(Projectile), Projectile.Center, Main.rand.NextFloat(-Pi, Pi).ToRotationVector2() * Main.rand.NextFloat(8, 17), ProjectileType<Ash>(), 3, 0);
        }
    }
    public override void OnHitPlayer(Player player, Player.HurtInfo Info)
    {
        Projectile.Kill();
    }
}

public class Ash : ModProjectile
{
    public override string Texture => Helper.Empty;

    int ChosenSpriteIndex = Main.rand.Next(1, 4);
    float Alpha = 0;

    public override void SetDefaults()
    {
        Projectile.width = 38;
        Projectile.height = 36;
        Projectile.tileCollide = false;
        Projectile.friendly = false;
        Projectile.hostile = true;
        Projectile.penetrate = 10;
        Projectile.timeLeft = 150;
    }

    public override void AI()
    {
        Projectile.ai[0]++;
        if (Projectile.ai[0] > 80)
        {
            Alpha = Lerp(Alpha, 0, 0.12f);
            Projectile.damage = 0;
        }
        else
            Alpha = Lerp(Alpha, 0.4f, 0.4f);
        Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Zero, 0.12f);
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D Texture = Helper.GetTexture(Mod.Name + "/Projectiles/Enemy/Desert/Ash" + ChosenSpriteIndex).Value;
        Rectangle sourceRect = new Rectangle(0, 0, Projectile.width, Projectile.height);
        Main.EntitySpriteDraw(Texture, Projectile.Center - Main.screenPosition, sourceRect, lightColor * Alpha, Projectile.rotation, sourceRect.Size() / 2f, Projectile.scale, SpriteEffects.None);
        return false;
    }
}