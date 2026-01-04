using EbonianMod.Common.Players;

namespace EbonianMod.Content.Buffs;

public class DoomsdayBuff : ModBuff
{
    public override string Texture => Helper.AssetPath + "Buffs/DoomsdayBuff";
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        SummonPlayer modPlayer = player.GetModPlayer<SummonPlayer>();
        if (player.ownedProjectileCounts[ProjectileType<Content.Projectiles.Minions.DoomsdayDrone>()] > 0)
        {
            modPlayer.doomMinion = true;
        }
        if (!modPlayer.doomMinion)
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