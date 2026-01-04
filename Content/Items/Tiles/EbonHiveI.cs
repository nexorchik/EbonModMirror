using EbonianMod.Content.Tiles;

namespace EbonianMod.Content.Items.Tiles;

public class EbonHiveI : ModItem
{
    public override void SetStaticDefaults()
    {
    }
    public override void SetDefaults()
    {
        Item.width = 16;
        Item.height = 16;
        Item.maxStack = Item.CommonMaxStack;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.useAnimation = 15;
        Item.rare = 2;
        Item.useTime = 10;
        Item.useStyle = 1;
        Item.consumable = true;
        Item.createTile = TileType<EbonHiveBlock>();
    }
}
