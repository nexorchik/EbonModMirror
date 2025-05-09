using EbonianMod.Tiles;
using EbonianMod.Tiles.Furniture.CeciSet;

namespace EbonianMod.Items.Tiles.Furniture.CeciSet;

public class CeciChairI : ModItem
{
    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(TileType<CeciChair>());
        Item.rare = ItemRarityID.Red;
    }
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient(ItemType<CorruptrotI>(), 4).AddTile(TileID.WorkBenches).Register();
    }
}
public class CeciWallI : ModItem
{
    public override void SetDefaults()
    {
        Item.DefaultToPlaceableWall(ModContent.WallType<CeciWall>());
        Item.rare = ItemRarityID.Red;
    }
    public override void AddRecipes()
    {
        CreateRecipe(4).AddIngredient(ItemType<CorruptrotI>(), 1).AddTile(TileID.WorkBenches).Register();
    }
}
public class CeciPlatformI : ModItem
{
    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(TileType<CeciPlatform>());
        Item.rare = ItemRarityID.Red;
    }
    public override void AddRecipes()
    {
        CreateRecipe(2).AddIngredient(ItemType<CorruptrotI>(), 1).Register();
    }
}
