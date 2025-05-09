using EbonianMod.Tiles;

namespace EbonianMod.Items.Tiles;

public class FleshichorI : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 16;
        Item.height = 16;
        Item.maxStack = Item.CommonMaxStack;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.useAnimation = 15;
        Item.rare = ItemRarityID.Red;
        Item.useTime = 10;
        Item.useStyle = 1;
        Item.consumable = true;
        Item.createTile = TileType<FleshichorTile>();
    }
}
