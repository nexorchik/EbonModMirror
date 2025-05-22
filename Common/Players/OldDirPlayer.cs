using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.Common.Players;
// because player.oldDirection doesnt actually store the OLD DIRECTION. Mass Re-Logic Dev Massacre 2032 Spread the Word (in minecraft)
public class OldDirPlayer : ModPlayer
{
    public int oldDir;
    public override void PreUpdate() => oldDir = Player.direction;
}
