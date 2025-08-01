using EbonianMod.Projectiles.Minions;
namespace EbonianMod.Buffs;

public class CecitiorClawBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        EbonianPlayer modPlayer = player.GetModPlayer<EbonianPlayer>();
        if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[ProjectileType<CecitiorClawMinion>()] > 0)
        {
            modPlayer.cClawMinion = true;
        }
        if (!modPlayer.cClawMinion)
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