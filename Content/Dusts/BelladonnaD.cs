using Terraria;
using Terraria.ModLoader;

namespace EbonianMod.Content.Dusts
{
    public class BelladonnaD1 : ModDust
    {
        public override string Texture => Helper.AssetPath + "Dusts/BelladonnaD1";
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = false;
            dust.noLight = true;
        }
        public override bool MidUpdate(Dust dust)
        {
            if (!dust.noGravity)
            {
                dust.velocity.Y += 0.1f;
            }

            dust.rotation += 0.1f;
            dust.scale -= 0.025f;
            dust.position += dust.velocity;
            if (dust.scale <= 0)
                dust.active = false;

            return false;
        }
    }
    public class BelladonnaD2 : ModDust
    {
        public override string Texture => Helper.AssetPath + "Dusts/BelladonnaD2";
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = false;
            dust.noLight = true;
        }
        public override bool MidUpdate(Dust dust)
        {
            if (!dust.noGravity)
            {
                dust.velocity.Y += 0.1f;
            }

            dust.rotation += 0.1f;
            dust.scale -= 0.025f;
            dust.position += dust.velocity;
            if (dust.scale <= 0)
                dust.active = false;

            return false;
        }
    }
}
