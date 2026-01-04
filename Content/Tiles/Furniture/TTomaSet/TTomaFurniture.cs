using EbonianMod.Content.Items.Tiles.Furniture.TTomaSet;
using Terraria.Enums;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ObjectData;

namespace EbonianMod.Content.Tiles.Furniture.TTomaSet;

public class TTomaChair : ModTile
{
    public const int NextStyleHeight = 40;

    public override void SetStaticDefaults()
    {

        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;
        TileID.Sets.HasOutlines[Type] = true;
        TileID.Sets.CanBeSatOnForNPCs[Type] = true;
        TileID.Sets.CanBeSatOnForPlayers[Type] = true;
        TileID.Sets.DisableSmartCursor[Type] = true;

        AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);

        DustType = DustID.Silk;
        AdjTiles = new int[] { TileID.Chairs };


        AddMapEntry(Color.LawnGreen, Language.GetText("MapObject.Chair"));


        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
        TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
        TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, 2);
        TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;

        TileObjectData.newTile.StyleWrapLimit = 2;
        TileObjectData.newTile.StyleMultiplier = 2;
        TileObjectData.newTile.StyleHorizontal = true;

        TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
        TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
        TileObjectData.addAlternate(1);
        TileObjectData.addTile(Type);
    }

    public override void NumDust(int i, int j, bool fail, ref int num)
    {
        num = fail ? 1 : 3;
    }

    public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
    {
        return settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance);
    }

    public override void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info)
    {

        Tile tile = Framing.GetTileSafely(i, j);




        info.TargetDirection = -1;
        if (tile.TileFrameX != 0)
        {
            info.TargetDirection = 1;
        }



        info.AnchorTilePosition.X = i;
        info.AnchorTilePosition.Y = j;

        if (tile.TileFrameY % NextStyleHeight == 0)
        {
            info.AnchorTilePosition.Y++;
        }
    }

    public override bool RightClick(int i, int j)
    {
        Player player = Main.LocalPlayer;

        if (player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
        {
            player.GamepadEnableGrappleCooldown();
            player.sitting.SitDown(player, i, j);
        }

        return true;
    }

    public override void MouseOver(int i, int j)
    {
        Player player = Main.LocalPlayer;

        if (!player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
        {
            return;
        }

        player.noThrow = 2;
        player.cursorItemIconEnabled = true;
        player.cursorItemIconID = ItemType<TTomaChairI>();

        if (Main.tile[i, j].TileFrameX / 18 < 1)
        {
            player.cursorItemIconReversed = true;
        }
    }
}
public class TTomaPlatform : ModTile
{
    public override void SetStaticDefaults()
    {
        // Properties
        Main.tileLighted[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileSolidTop[Type] = true;
        Main.tileSolid[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileTable[Type] = true;
        Main.tileLavaDeath[Type] = true;
        TileID.Sets.Platforms[Type] = true;
        TileID.Sets.DisableSmartCursor[Type] = true;

        AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
        AddMapEntry(Color.LawnGreen);

        DustType = DustID.Silk;
        AdjTiles = new int[] { TileID.Platforms };

        // Placement
        TileObjectData.newTile.CoordinateHeights = new[] { 16 };
        TileObjectData.newTile.CoordinateWidth = 16;
        TileObjectData.newTile.CoordinatePadding = 2;
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.newTile.StyleMultiplier = 27;
        TileObjectData.newTile.StyleWrapLimit = 27;
        TileObjectData.newTile.UsesCustomCanPlace = false;
        TileObjectData.newTile.LavaDeath = true;
        TileObjectData.addTile(Type);
    }

    public override void PostSetDefaults() => Main.tileNoSunLight[Type] = false;

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
}
public class TTomaWall : ModWall
{
    public override void SetStaticDefaults()
    {
        Main.wallHouse[Type] = true;

        DustType = DustID.Silk;

        AddMapEntry(Color.LawnGreen);
    }

    public override void NumDust(int i, int j, bool fail, ref int num)
    {
        num = fail ? 1 : 3;
    }
}
