using EbonianMod.Common.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Creative;

namespace EbonianMod.Content.Items.Accessories;

public class GoldenTip : ModItem
{
    public override string Texture => Helper.AssetPath + "Items/Accessories/GoldenTip";
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
        player.GetModPlayer<AccessoryPlayer>().goldenTip = true;
    }

}