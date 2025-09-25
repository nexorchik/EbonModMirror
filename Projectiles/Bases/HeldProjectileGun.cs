using System.IO;

namespace EbonianMod.Projectiles.Bases;

public abstract class HeldProjectileGun : HeldProjectile
{
    protected float RotationSpeed, AnimationRotationSpeed = 0.2f, AnimationRotation;
    protected Vector2 CursorOffset;
    public override void AI()
    {
        base.AI();
        Player player = Main.player[Projectile.owner];

        if (player.whoAmI == Main.myPlayer)
            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Helper.FromAToB(Projectile.Center, Main.MouseWorld + CursorOffset.RotatedBy(Projectile.rotation) * player.direction).ToRotation(), RotationSpeed) + AnimationRotation * AttackDelayMultiplier;
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
