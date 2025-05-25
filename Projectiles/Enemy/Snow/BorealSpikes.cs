using System.Collections.Generic;
using static EbonianMod.Helper;

namespace EbonianMod.Projectiles.Enemy.Snow;

public class BorealSpike : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.Size = new Vector2(12, 30);
        Projectile.aiStyle = -1;
        Projectile.friendly = false;
        Projectile.hostile = true;
        Projectile.penetrate = -1;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 80;
    }
    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) => behindNPCsAndTiles.Add(index);
    public override void OnSpawn(IEntitySource source)
    {
        Projectile.rotation = Main.rand.NextFloat(-Pi / 12, Pi / 12);
        for (int i = 0; i < 12; i++)
        {
            Dust.NewDustPerfect(Projectile.Center - new Vector2(0, 7), 76, (Projectile.rotation - Pi / 2).ToRotationVector2() * Main.rand.NextFloat(0, 5), Scale: Main.rand.NextFloat(0.7f, 1.1f)).noGravity = true;
        }
        Projectile.scale = 0;
    }
    public override bool ShouldUpdatePosition() => false;
    public override void AI()
    {
        if(Projectile.timeLeft == 78 && Projectile.frame < 14)
        {
            Projectile.ai[0]++;
            Projectile Current = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), TRay.Cast(Projectile.Center - new Vector2(-10 * Projectile.ai[1], 70), Vector2.UnitY, 5000, true)+new Vector2(0, 3), Vector2.Zero, ProjectileType<BorealSpike>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            Current.frame = Projectile.frame + 2;
            Current.ai[1] = Projectile.ai[1];
        }
        if(Projectile.timeLeft < 20)
            Projectile.scale = Lerp(Projectile.scale, 0, 0.25f);
        else
            Projectile.scale = Lerp(Projectile.scale, 1, 0.3f);
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        Rectangle frameRect = new Rectangle(0, Projectile.frame/2 * Projectile.height, Projectile.width, Projectile.height);
        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, frameRect, lightColor, Projectile.rotation, new Vector2(Projectile.Size.X / 2, Projectile.Size.Y / 2 + 12), new Vector2(1, Projectile.scale), Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically);
        return false;
    }
}