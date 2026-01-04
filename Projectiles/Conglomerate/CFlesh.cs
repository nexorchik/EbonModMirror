namespace EbonianMod.Projectiles.Conglomerate;

public class CFlesh : ModProjectile
{
    public override string Texture => Helper.Empty;
    public override void SetDefaults()
    {
        Projectile.Size = new Vector2(32);
        Projectile.friendly = false;
        Projectile.hostile = true;
        Projectile.aiStyle = 2;
    }
    public override void OnSpawn(IEntitySource source)
    {
        Projectile.ai[2] = Main.rand.Next(1, 13);
    }
    public override void OnKill(int timeLeft)
    {
        Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D tex = Images.Extras.Textures.Empty.Value;
        switch (Projectile.ai[2])
        {
            case 1:
                tex = Helper.GetTexture("Gores/CrimsonGoreChunk9").Value;
                break;
            case 2:
                tex = Helper.GetTexture("Gores/CrimsonGoreChunk8").Value;
                break;
            case 3:
                tex = Helper.GetTexture("Gores/CrimsonGoreChunk7").Value;
                break;
            case 4:
                tex = Helper.GetTexture("Gores/CrimsonGoreChunk6").Value;
                break;
            case 5:
                tex = Helper.GetTexture("Gores/CrimsonGoreChunk5").Value;
                break;
            case 6:
                tex = Helper.GetTexture("Gores/CrimsonGoreChunk4").Value;
                break;
            case 7:
                tex = Helper.GetTexture("Gores/VileSlimeGore4").Value;
                break;
            case 8:
                tex = Helper.GetTexture("Gores/VileSlimeGore3").Value;
                break;
            case 9:
                tex = Helper.GetTexture("Gores/VileSlimeGore2").Value;
                break;
            case 10:
                tex = Helper.GetTexture("Gores/VileSlimeGore").Value;
                break;
            case 11:
                tex = Helper.GetTexture("Gores/EbonCrawlerGore2").Value;
                break;
            case 12:
                tex = Helper.GetTexture("Gores/EbonCrawlerGore1").Value;
                break;
        }
        Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, tex.Size() / 2, Projectile.scale, SpriteEffects.None);
        return false;
    }
    public override void AI()
    {
        if (Projectile.ai[2] < 7)
        {
            Dust.NewDust(Projectile.position, Projectile.height, Projectile.width, DustID.IchorTorch);
        }
        else
        {
            Dust.NewDust(Projectile.position, Projectile.height, Projectile.width, DustID.CursedTorch);
        }
    }
}
