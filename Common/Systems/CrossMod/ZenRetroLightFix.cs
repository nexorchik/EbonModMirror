using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.Common.Systems.CrossMod;
public class ZenRetroLightFix : ModSystem
{
    public static bool HasRetroFix;
    public override void PostSetupContent()
    {
        if (ModLoader.TryGetMod("FixRetroLighting", out Mod fixRetro))
            HasRetroFix = true;
    }
    public override void UpdateUI(GameTime gameTime)
    {
        if (!HasRetroFix && !Lighting.NotRetro)
        {
            Lighting.Mode = Terraria.Graphics.Light.LightMode.Color;
            Main.NewText(Language.GetTextValue("EbonianMod.Misc.LightingWarning"), Main.OurFavoriteColor);
        }
    }
}
