using EbonianMod.Core.Systems.Boss;
using Terraria.ModLoader.IO;

namespace EbonianMod.Core.Systems.Misc;

public class DownedBossSystem : ModSystem
{
    public bool downedGarbage, downedXareusCheck, downedCecitior, downedTerrortoma;

    public override void PostUpdateEverything()
    {
        downedXareusCheck = GetInstance<XareusSystem>().downedXareus;
    }
    public override void PostWorldGen()
    {
        downedGarbage = false;
        downedCecitior = false;
        downedTerrortoma = false;
    }
    public override void SaveWorldData(TagCompound tag)
    {
        tag.Set("GarbageDown", downedGarbage);
        tag.Set("CeciDown", downedCecitior);
        tag.Set("TTomaDown", downedTerrortoma);
    }
    public override void LoadWorldData(TagCompound tag)
    {
        downedGarbage = tag.GetBool("GarbageDown");
        downedCecitior = tag.GetBool("CeciDown");
        downedTerrortoma = tag.GetBool("TTomaDown");
    }

}
