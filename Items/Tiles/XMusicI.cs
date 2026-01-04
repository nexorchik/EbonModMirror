using EbonianMod.Tiles;
using Terraria.GameContent.Creative;

namespace EbonianMod.Items.Tiles;

public class XMusicI : ModItem
{
    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "/Assets/SoundsMusic/xareus"), ItemType<XMusicI>(), TileType<XMusic>());
    }

    public override void SetDefaults()
    {
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTurn = true;
        Item.useAnimation = 15;
        Item.useTime = 10;
        Item.autoReuse = true;
        Item.consumable = true;
        Item.createTile = TileType<XMusic>();
        Item.width = 24;
        Item.height = 24;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.buyPrice(0, 10, 0, 0);
        Item.accessory = true;
    }
    public override bool? PrefixChance(int pre, UnifiedRandom rand)
    {
        return false;
    }
}
