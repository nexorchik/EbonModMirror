using EbonianMod.Content.Tiles;
using Terraria.GameContent.Creative;

namespace EbonianMod.Content.Items.Tiles;

public class EvilMusicI : ModItem
{
    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Assets/Music/EvilMiniboss"), ItemType<EvilMusicI>(), TileType<EvilMusic>());
    }

    public override void SetDefaults()
    {
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTurn = true;
        Item.useAnimation = 15;
        Item.useTime = 10;
        Item.autoReuse = true;
        Item.consumable = true;
        Item.createTile = TileType<EvilMusic>();
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
