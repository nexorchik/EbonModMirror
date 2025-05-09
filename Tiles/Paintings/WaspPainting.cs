using Terraria.ObjectData;

namespace EbonianMod.Tiles.Paintings;

public class WaspPainting : ModTile
{
    public override void SetStaticDefaults()
    {

        Main.tileFrameImportant[Type] = true;
        Main.tileLavaDeath[Type] = false;
        Main.tileNoAttach[Type] = true;
        TileID.Sets.FramesOnKillWall[Type] = true;
        Main.tileSolid[Type] = false;
        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
        TileObjectData.newTile.Height = 2;
        TileObjectData.newTile.Width = 2;
        TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, };
        TileObjectData.newTile.CoordinateWidth = 16;
        TileObjectData.newTile.CoordinatePadding = 2;
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(0, 0, 0), Language.GetText("Ebon"));
    }
    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
        //Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemType<WaspPaintingI>());
    }
}
