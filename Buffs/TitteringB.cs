using EbonianMod.Projectiles.Minions;

namespace EbonianMod.Buffs;

internal class TitteringB : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        EbonianPlayer modPlayer = player.GetModPlayer<EbonianPlayer>();
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
