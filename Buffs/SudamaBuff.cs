using EbonianMod.Items.Weapons.Summoner;
namespace EbonianMod.Buffs;

public class SudamaBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        EbonianPlayer modPlayer = player.GetModPlayer<EbonianPlayer>();
        if (player.ownedProjectileCounts[ProjectileType<SudamaF>()] > 0)
        {
            modPlayer.sudamaMinion = true;
        }
        if (!modPlayer.sudamaMinion)
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