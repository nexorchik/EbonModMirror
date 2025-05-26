using EbonianMod.Items.Weapons.Melee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ObjectData;

namespace EbonianMod.Tiles.Ambient;
public class RustyAxeT : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileObsidianKill[Type] = true;
        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
        TileObjectData.newTile.Width = 4;
        TileObjectData.newTile.CoordinateWidth = 16;
        TileObjectData.newTile.CoordinatePadding = 2;
        TileObjectData.addTile(Type);
        Main.tileMerge[TileID.Sand][Type] = true;

        DustType = DustID.Bone;

        RegisterItemDrop(ItemType<RustyWaraxe>());

        AddMapEntry(Color.LawnGreen);
    }
    public override IEnumerable<Item> GetItemDrops(int i, int j)
    {
        yield return new Item(ItemType<RustyWaraxe>());
    }
}
