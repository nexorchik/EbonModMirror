namespace EbonianMod.Projectiles.ArchmageX;

public class SheepeningPlayerProjectile : ModProjectile
{
    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 5;
    }
    public override void SetDefaults()
    {
        Projectile.friendly = false;
        Projectile.hostile = true;
        Projectile.tileCollide = false;
        Projectile.aiStyle = -1;
        Projectile.timeLeft = 1000;
        Projectile.netImportant = true;
        Projectile.extraUpdates = 3;
        Projectile.Size = new(38, 28);
    }
    public override bool PreDraw(ref Color lightColor)
    {
        lightColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
        return true;
    }
    public override void OnSpawn(IEntitySource source)
    {
        SoundEngine.PlaySound(EbonianSounds.sheep_player);
        for (int i = 0; i < 30; i++)
        {
            Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke);
        }
    }
    public override void OnKill(int timeLeft)
    {
        for (int i = 0; i < 30; i++)
        {
            Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke);
        }
    }
    public override void AI()
    {
        Player player = Main.player[(int)Projectile.ai[0]];
        if (player.TryGetModPlayer<EbonianPlayer>(out var p))
            if (!p.sheep || player.dead || !player.active) Projectile.Kill();

        Projectile.timeLeft = Projectile.extraUpdates * 10;
        Projectile.Center = player.Bottom + new Vector2(0, -Projectile.height / 2 + player.gfxOffY);
        Projectile.direction = Projectile.spriteDirection = player.direction;
        if (Helper.TRay.CastLength(Projectile.Bottom, Vector2.UnitY, 50, true) > 1 || player.velocity.Y < 0)
        {
            Projectile.frame = 4;
        }
        else
        {
            if (player.velocity.X < -1 || player.velocity.X > 1)
            {
                if (++Projectile.frameCounter % 15 == 0)
                {
                    Projectile.frame++;
                    if (Projectile.frame > 3)
                        Projectile.frame = 0;
                }
            }
            else
                Projectile.frame = 0;
        }
    }
}
