using Terraria.ObjectData;

namespace EbonianMod.Tiles.Paintings;

public class Dappertoma : ModTile
{
    public override void SetStaticDefaults()
    {

        Main.tileFrameImportant[Type] = true;
        Main.tileLavaDeath[Type] = false;
        Main.tileNoAttach[Type] = true;
        TileID.Sets.FramesOnKillWall[Type] = true;
        Main.tileSolid[Type] = false;
        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
        TileObjectData.newTile.Height = 4;
        TileObjectData.newTile.Width = 6;
        TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, };
        TileObjectData.newTile.CoordinateWidth = 16;
        TileObjectData.newTile.CoordinatePadding = 2;
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(0, 0, 0), Language.GetText("Dapper Fellow"));
    }
    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
        //Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemType<DappertomaI>());
    }
}
