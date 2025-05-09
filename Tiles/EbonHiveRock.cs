using System.Collections.Generic;
using Terraria.ObjectData;

namespace EbonianMod.Tiles;

internal class EbonHiveRock : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileObsidianKill[Type] = true;
        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
        TileObjectData.addTile(Type);
        Main.tileMerge[TileType<EbonHiveBlock>()][Type] = true;

        DustType = DustID.GreenBlood;

        RegisterItemDrop(ItemType<Items.Tiles.EbonHiveI>());

        AddMapEntry(Color.LawnGreen);
    }
    public override IEnumerable<Item> GetItemDrops(int i, int j)
    {
        yield return new Item(ItemType<Items.Tiles.EbonHiveI>());
    }
}
internal class EbonHiveRock2 : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileObsidianKill[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
        TileObjectData.addTile(Type);

        Main.tileMerge[TileType<EbonHiveBlock>()][Type] = true;
        DustType = DustID.GreenBlood;

        RegisterItemDrop(ItemType<Items.Tiles.EbonHiveI>());

        AddMapEntry(Color.LawnGreen);
    }
    public override IEnumerable<Item> GetItemDrops(int i, int j)
    {
        yield return new Item(ItemType<Items.Tiles.EbonHiveI>());
    }
}
