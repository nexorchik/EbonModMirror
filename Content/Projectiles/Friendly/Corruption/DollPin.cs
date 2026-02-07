
using Terraria;

namespace EbonianMod.Content.Projectiles.Friendly.Corruption;

public class DollPin : ModProjectile
{
    public override string Texture => Helper.AssetPath + "Projectiles/Friendly/Corruption/" + Name;
    public override void SetDefaults()
    {
        Projectile.Size = new Vector2(42, 14);
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.timeLeft = 3;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
    }
    int TargetIndex = -1;
    public override void OnSpawn(IEntitySource source)
    {
        Projectile.rotation = Projectile.velocity.ToRotation();
        Projectile.ai[2] = Projectile.damage;
        Projectile.damage = 1;

    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Projectile.ai[0] == 0)
        {
            Projectile.localAI[1] = Clamp(target.Size.Length() / 15, 10, 1000);
            Projectile.Center = target.Center;
            TargetIndex = target.whoAmI;
            Projectile.timeLeft = 999;
            Projectile.ai[0] = 1;
            Projectile.ai[1] = Projectile.localAI[1];
        }
    }
    public override bool? CanDamage() => Projectile.ai[0] == 0;

    public override void AI()
    {
        if (Projectile.ai[0] == 0) return;

        NPC Target = Main.npc[TargetIndex];
        if (Target.life > 0 && Target.active) Projectile.Center = Target.Center;

        if (Projectile.ai[1] == 0) Projectile.localAI[1] *= 1.4f;
        else
        {
            Projectile.localAI[1] *= 0.9f;
            Projectile.ai[1] = Lerp(Projectile.ai[1], 0, 0.15f);
            Projectile.rotation = Pi / 4 + Projectile.ai[1];
            if (Projectile.ai[1] < 0.02f)
            {
                Projectile.damage = (int)Projectile.ai[2];
                Projectile.localAI[1] = -1;
                Projectile.ai[1] = 0;
            }
        }
        Projectile.localAI[0] += Projectile.localAI[1];

        if (Projectile.ai[1] == 0 && Projectile.localAI[0] < -15f)
        {
            Projectile.Kill();
            Target.StrikeNPC(Projectile.damage, Projectile.knockBack, Target.direction);
            Target.AddBuff(BuffID.CursedInferno, 400);
        }
    }
    public override void OnKill(int timeLeft)
    {
        if (Projectile.ai[0] == 0) return;

        for (int i = 0; i < 50; i++)
            Dust.NewDustPerfect(Projectile.Center, DustID.CursedTorch, Main.rand.NextFloat(0, Pi * 2).ToRotationVector2() * Main.rand.NextFloat(1, 12), Scale: Main.rand.NextFloat(1.2f, 3f)).noGravity = true;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        if (Projectile.ai[0] == 0) return false;

        Texture2D texture = TextureAssets.Projectile[Type].Value;
        for(float i = 0; i <= Pi * 2; i += Pi / 2) 
        {
            Main.EntitySpriteDraw(texture, Projectile.Center + new Vector2(-Projectile.localAI[0], 0).RotatedBy(Projectile.rotation + i) - Main.screenPosition, null, lightColor, Projectile.rotation + i, Projectile.Size / 2, Projectile.scale, SpriteEffects.None);
        }
        return false;
    }
}