
namespace EbonianMod.Content.Dusts;
public class JunglePinkDust : ModDust
{
    public override string Texture => Helper.Empty;
    public override bool Update(Dust dust)
    {
        dust.scale -= 0.005f;
        return base.Update(dust);
    }
    public static void DrawAll(SpriteBatch sb)
    {
        foreach (Dust dust in Main.dust)
        {
            if (dust.active && dust.type == DustType<JunglePinkDust>())
            {
                sb.Draw(Helper.GetTexture(Helper.AssetPath+"Projectiles/Enemy/Jungle/VivineSpit").Value, dust.position - Main.screenPosition, new Rectangle(0, 0, 24, 24), Color.White, dust.velocity.ToRotation(), new Vector2(12, 12), new Vector2(2, 1) * dust.scale, SpriteEffects.None, 0);
            }
        }
    }
}