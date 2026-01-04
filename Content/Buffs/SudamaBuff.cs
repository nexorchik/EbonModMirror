using EbonianMod.Common.Players;
using EbonianMod.Content.Items.Weapons.Summoner;
namespace EbonianMod.Content.Buffs;

public class SudamaBuff : ModBuff
{
    public override string Texture => Helper.AssetPath + "Buffs/SudamaBuff";
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        SummonPlayer modPlayer = player.GetModPlayer<SummonPlayer>();
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