using EbonianMod.Common.Players;
using Terraria.GameContent.Creative;

namespace EbonianMod.Content.Items.Accessories;

public class StarBit : ModItem
{

    public override string Texture => Helper.AssetPath + "Items/Accessories/StarBit";
    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 24;
        Item.accessory = true;
        Item.value = Item.buyPrice(0, 1, 0, 0);
        Item.rare = ItemRarityID.Blue;
    }
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<AccessoryPlayer>().starBit = true;
        player.GetCritChance(DamageClass.Generic) += 3f;
    }

    public override Color? GetAlpha(Color lightColor)
    {
        return Color.White;
    }
}
