using EbonianMod.Content.Tiles.Trophies;

namespace EbonianMod.Content.Items.Tiles.Trophies;

public class GarbageTrophy : ModItem
{
    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(TileType<HotGarbTrophy>());

        Item.width = 32;
        Item.height = 32;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.buyPrice(0, 1);
    }
}
