namespace EbonianMod.Content.Buffs;

public class ExolStun : ModBuff
{
    public override string Texture => Helper.AssetPath + "Buffs/ExolStun";
    public override void SetStaticDefaults()
    {

        Main.buffNoTimeDisplay[Type] = false;
    }
}