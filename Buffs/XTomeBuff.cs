using EbonianMod.Items.Weapons.Summoner;
namespace EbonianMod.Buffs;

public class XTomeBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        EbonianPlayer modPlayer = player.GetModPlayer<EbonianPlayer>();
        if (player.ownedProjectileCounts[ProjectileType<XTomeSummon>()] > 0)
        {
            modPlayer.spiritMinion = true;
        }
        if (!modPlayer.spiritMinion)
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