using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EbonianMod.Content.Dusts
{
    public class StarshroomDust : ModDust
    {
        public override string Texture => Helper.AssetPath + "Dusts/StarshroomDust";
        public override Color? GetAlpha(Dust dust, Color lightColor) => Color.White;
    }
}
