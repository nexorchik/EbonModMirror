using EbonianMod.Common.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.Common.Utilities;
public static class PlayerExtensions
{
    public static int OldDirection(this Player player) => player.GetModPlayer<OldDirPlayer>().oldDir;
}
