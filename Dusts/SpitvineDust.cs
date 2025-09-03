
namespace EbonianMod.Dusts;
public class SpitvineDust : ModDust
{
    public override string Texture => Helper.Empty;
    public override bool Update(Dust dust)
    {
        return base.Update(dust);
    }
    public static void DrawAll(SpriteBatch sb)
    {
        foreach (Dust dust in Main.dust)
        {
            if (dust.active && dust.type == DustType<SpitvineDust>())
            {
                sb.Draw(Helper.GetTexture("EbonianMod/Dusts/SpitvineDust").Value, dust.position - Main.screenPosition, new Rectangle(0, 0, 16, 16), Color.White, dust.velocity.ToRotation(), new Vector2(8, 8), dust.scale, SpriteEffects.None, 0);
            }
        }
    }
}