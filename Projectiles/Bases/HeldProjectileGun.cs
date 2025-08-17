
using EbonianMod.Items.Consumables.Food;
using System;
using System.IO;

namespace EbonianMod.Projectiles.Bases;

public abstract class HeldProjectileGun : HeldProjectile
{
    protected float RotationSpeed;
    protected float AnimationRotationSpeed = 0.2f, AnimationRotation;
    public override void AI()
    {
        base.AI();
        Player player = Main.player[Projectile.owner];

        if (player.whoAmI == Main.myPlayer)
            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Helper.FromAToB(player.Center, Main.MouseWorld).ToRotation(), RotationSpeed) + AnimationRotation;
        player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - PiOver2);

        AnimationRotation = Lerp(AnimationRotation, 0, AnimationRotationSpeed);
        Projectile.netUpdate = true;
    }
    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.Write(Projectile.rotation);
    }
    public override void ReceiveExtraAI(BinaryReader reader)
    {
        Projectile.rotation = reader.ReadSingle();
    }
}
