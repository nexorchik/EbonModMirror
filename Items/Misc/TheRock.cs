namespace EbonianMod.Items.Misc;

public class TheRock : ModItem
{ 
    public override void SetStaticDefaults()
    {
        ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.PoopBlock;
    }
    public override void SetDefaults()
    {
        Item.Size = new Vector2(20);
        Item.rare = ItemRarityID.Gray;
        Item.value = 1;
    }
}