using EbonianMod.Common.Players;
using EbonianMod.Content.Projectiles.Minions;
namespace EbonianMod.Content.Buffs;

public class CecitiorClawBuff : ModBuff
{
    public override string Texture => Helper.AssetPath + "Buffs/CecitiorClawBuff";
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        SummonPlayer modPlayer = player.GetModPlayer<SummonPlayer>();
        if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[ProjectileType<CecitiorClawMinion>()] > 0)
        {
            modPlayer.ClawMinion = true;
        }
        if (!modPlayer.ClawMinion)
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