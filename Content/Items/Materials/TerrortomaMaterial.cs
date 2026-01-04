namespace EbonianMod.Content.Items.Materials;

public class TerrortomaMaterial : ModItem
{
    public override string Texture => Helper.AssetPath + "Items/Materials/TerrortomaMaterial";
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.SoulofNight);
    }
}
