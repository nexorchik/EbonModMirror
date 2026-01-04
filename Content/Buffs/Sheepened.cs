using EbonianMod.Common.Players;
using EbonianMod.Content.Projectiles;
using EbonianMod.Content.Projectiles.ArchmageX;

namespace EbonianMod.Content.Buffs;

public class Sheepened : ModBuff
{
    public override string Texture => Helper.AssetPath + "Buffs/Sheepened";
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = true;
        Main.debuff[Type] = true;
    }
    public override void Update(Player player, ref int buffIndex)
    {
        foreach (Projectile projectile in Main.ActiveProjectiles)
        {
            if (projectile.friendly && projectile.minion && projectile.owner == player.whoAmI && player.whoAmI == Main.myPlayer)
            {
                projectile.Kill();
            }
        }
        for (int i = 1; i < BuffID.Count; i++)
        {
            if (!Main.buffNoSave[i] && !Main.buffNoTimeDisplay[i] && !Main.debuff[i])
                player.ClearBuff(i);
        }
        player.GetModPlayer<SheepPlayer>().sheep = true;
        if (player.ownedProjectileCounts[ProjectileType<SheepeningPlayerProjectile>()] < 1)
            Projectile.NewProjectile(null, player.Center, Vector2.Zero, ProjectileType<SheepeningPlayerProjectile>(), 0, 0, player.whoAmI);
    }
}
