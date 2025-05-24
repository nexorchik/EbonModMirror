using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EbonianMod.Dusts
{
    public class StarshroomDust : ModDust
    {
        public override Color? GetAlpha(Dust dust, Color lightColor) => Color.White;
    }
}
