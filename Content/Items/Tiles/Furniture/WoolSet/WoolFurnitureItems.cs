using EbonianMod.Content.Tiles.Furniture.WoolSet;

namespace EbonianMod.Content.Items.Tiles.Furniture.WoolSet;

public class WoolChairI : ModItem
{
    public override void SetDefaults() => Item.DefaultToPlaceableTile(TileType<WoolChair>());
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient(ItemType<WoolWoodI>(), 4).AddTile(TileID.WorkBenches).Register();
    }
}
public class WoolDoorI : ModItem
{
    public override void SetDefaults() => Item.DefaultToPlaceableTile(TileType<WoolDoorClosed>());
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient(ItemType<WoolWoodI>(), 6).AddTile(TileID.WorkBenches).Register();
    }
}
public class WoolLampI : ModItem
{
    public override void SetDefaults() => Item.DefaultToPlaceableTile(TileType<WoolLamp>());
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient(ItemType<WoolWoodI>(), 3).AddIngredient(ItemID.Torch).AddTile(TileID.WorkBenches).Register();
    }
}
public class WoolTableI : ModItem
{
    public override void SetDefaults() => Item.DefaultToPlaceableTile(TileType<WoolTable>());
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient(ItemType<WoolWoodI>(), 8).AddTile(TileID.WorkBenches).Register();
    }
}
public class WoolWorkbenchI : ModItem
{
    public override void SetDefaults() => Item.DefaultToPlaceableTile(TileType<WoolWorkbench>());
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient(ItemType<WoolWoodI>(), 10).Register();
    }
}
