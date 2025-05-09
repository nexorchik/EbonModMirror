namespace EbonianMod.Buffs;

public class DoomsdayBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        EbonianPlayer modPlayer = player.GetModPlayer<EbonianPlayer>();
        if (player.ownedProjectileCounts[ProjectileType<Projectiles.Minions.DoomsdayDrone>()] > 0)
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