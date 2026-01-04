namespace EbonianMod.Content.Items.Vanity;

[AutoloadEquip(EquipType.Head)]
public class ClementinesCap : ModItem
{
    public override string Texture => Helper.AssetPath + "Items/Vanity/ClementinesCap";
    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
    }
    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 24;
        Item.value = Item.sellPrice(gold: 1);
        Item.rare = ItemRarityID.Blue;
        Item.vanity = true;
    }

}
