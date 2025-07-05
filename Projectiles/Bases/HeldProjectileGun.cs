
using EbonianMod.Items.Consumables.Food;
using System;

namespace EbonianMod.Projectiles.Bases;

public abstract class HeldProjectileGun : HeldProjectile
{
    protected float RotationSpeed;
    protected float AnimationRotationSpeed = 0.2f, AnimationRotation;
    public override void AI()
    {
        base.AI();
        Player player = Main.player[Projectile.owner];

        Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Helper.FromAToB(player.Center, Main.MouseWorld).ToRotation(), RotationSpeed) + AnimationRotation;
        player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - PiOver2);

        AnimationRotation = Lerp(AnimationRotation, 0, AnimationRotationSpeed);
    }
    public void UseAmmo(int AmmoType)
    {
        Player player = Main.player[Projectile.owner];
        for (int j = 0; j < 58; j++)
        {
            if (player.inventory[j].ammo == AmmoType && player.inventory[j].stack > 0)
            {
                if (player.inventory[j].maxStack > 1)
                    player.inventory[j].stack--;
                break;
            }
        }
    }
}
