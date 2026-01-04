using EbonianMod.Common.Players;
using EbonianMod.Content.Items.Weapons.Summoner;
namespace EbonianMod.Content.Buffs;

public class XTomeBuff : ModBuff
{
    public override string Texture => Helper.AssetPath + "Buffs/XTomeBuff";
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        SummonPlayer modPlayer = player.GetModPlayer<SummonPlayer>();
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