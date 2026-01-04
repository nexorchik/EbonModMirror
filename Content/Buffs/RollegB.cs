using EbonianMod.Common.Players;
using EbonianMod.Content.Projectiles.Dev;

namespace EbonianMod.Content.Buffs;

public class RollegB : ModBuff
{
    public override string Texture => Helper.AssetPath + "Buffs/RollegB";
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }
    public override void Update(Player player, ref int buffIndex)
    {
        player.GetModPlayer<SummonPlayer>().rolleg = true;
        if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[ProjectileType<Rolleg>()] > 0)
            player.buffTime[buffIndex] = 2;
    }
}
