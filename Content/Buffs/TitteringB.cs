using EbonianMod.Common.Players;
using EbonianMod.Content.Projectiles.Minions;

namespace EbonianMod.Content.Buffs;

internal class TitteringB : ModBuff
{
    public override string Texture => Helper.AssetPath + "Buffs/TitteringB";
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        SummonPlayer modPlayer = player.GetModPlayer<SummonPlayer>();
        if (player.ownedProjectileCounts[ProjectileType<TitteringMinion>()] > 0)
        {
            modPlayer.titteringMinion = true;
        }
        if (!modPlayer.titteringMinion)
        {
            player.DelBuff(buffIndex);
            buffIndex--;
        }
        else
        {
            player.buffTime[buffIndex] = 18000;
        }
    }

}
