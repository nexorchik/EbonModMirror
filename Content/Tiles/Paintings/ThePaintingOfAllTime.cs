using Terraria.ObjectData;

namespace EbonianMod.Content.Tiles.Paintings;

public class ThePaintingOfAllTime : ModTile
{
    public override bool IsLoadingEnabled(Mod mod) => false;
    public override void SetStaticDefaults()
    {

        Main.tileFrameImportant[Type] = true;
        Main.tileLavaDeath[Type] = false;
        Main.tileNoAttach[Type] = true;
        TileID.Sets.FramesOnKillWall[Type] = true;
        Main.tileSolid[Type] = false;
        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
        TileObjectData.newTile.Height = 15;
        TileObjectData.newTile.Width = 15;
        TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 };
        TileObjectData.newTile.CoordinateWidth = 16;
        TileObjectData.newTile.CoordinatePadding = 2;
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(0, 0, 0), Language.GetText("My Name is Ebon White Yo!"));
    }
    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
        //Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemType<HorriblePainting>());
    }
}
