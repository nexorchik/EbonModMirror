using EbonianMod.Common.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.Common.Utilities;
public static class EntityExtensions
{
    public static int OldDirection(this Player player) => player.GetModPlayer<OldDirPlayer>().oldDir;
    public static Vector2 GFX(this Player entity) => new Vector2(0, entity.gfxOffY);
    public static Vector2 GFX(this NPC entity) => new Vector2(0, entity.gfxOffY);
    public static Vector2 GFX(this Projectile entity) => new Vector2(0, entity.gfxOffY);
}
