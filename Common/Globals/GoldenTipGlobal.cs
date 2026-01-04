using EbonianMod.Common.Players;
using EbonianMod.Content.Projectiles.Friendly.Generic;

namespace EbonianMod.Common.Globals;

public class GoldenTipGlobal : GlobalItem
{
    public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        if (player.GetModPlayer<AccessoryPlayer>().goldenTip)
            if (item.useAmmo == AmmoID.Arrow)
            {
                player.GetModPlayer<AccessoryPlayer>().goldenTipI++;
                if (player.GetModPlayer<AccessoryPlayer>().goldenTipI > 4)
                {
                    type = ProjectileType<GoldTippedArrow>();

                    player.GetModPlayer<AccessoryPlayer>().goldenTipI = 0;
                }
            }
    }
}
