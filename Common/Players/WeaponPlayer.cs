using EbonianMod.Content.Items.Weapons.Melee;

namespace EbonianMod.Common.Players;

public class WeaponPlayer : ModPlayer
{
    public Vector2 stabDirection;
    public override void PostUpdateRunSpeeds()
    {
        if (Player.HeldItem.type == ItemType<EbonianScythe>() && !Player.ItemTimeIsZero)
        {
            Player.maxRunSpeed += 2;
            Player.accRunSpeed += 2;
        }
    }
}