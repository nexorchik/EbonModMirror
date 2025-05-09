using EbonianMod.Tiles;
using EbonianMod.Tiles.Furniture.TTomaSet;

namespace EbonianMod.Items.Tiles.Furniture.TTomaSet;

public class TTomaChairI : ModItem
{
    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(TileType<TTomaChair>());
        Item.rare = ItemRarityID.Red;
    }
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient(ItemType<CorruptrotI>(), 4).AddTile(TileID.WorkBenches).Register();
    }
}
public class TTomaWallI : ModItem
{
    public override void SetDefaults()
    {
        Item.DefaultToPlaceableWall(ModContent.WallType<TTomaWall>());
        Item.rare = ItemRarityID.Red;
    }
    public override void AddRecipes()
    {
        CreateRecipe(4).AddIngredient(ItemType<CorruptrotI>(), 1).AddTile(TileID.WorkBenches).Register();
    }
}
public class TTomaPlatformI : ModItem
{
    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(TileType<TTomaPlatform>());
        Item.rare = ItemRarityID.Red;
    }
    public override void AddRecipes()
    {
        CreateRecipe(2).AddIngredient(ItemType<CorruptrotI>(), 1).Register();
    }
}
