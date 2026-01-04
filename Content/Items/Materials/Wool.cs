namespace EbonianMod.Content.Items.Materials;

public class Wool : ModItem
{
    public override string Texture => Helper.AssetPath + "Items/Materials/Wool";
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Silk);
    }
}
