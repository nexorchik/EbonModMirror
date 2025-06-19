using EbonianMod.Dusts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.Projectiles.VFXProjectiles;

public class MagicChargeUp : ModProjectile
{
    public override string Texture => Helper.Empty;
    public override void SetDefaults()
    {
        Projectile.height = 1;
        Projectile.width = 1;
        Projectile.hostile = true;
        Projectile.friendly = false;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 50;
    }
    public override bool ShouldUpdatePosition() => false;
    public override bool? CanDamage() => false;
    int seed;
    public override bool PreDraw(ref Color lightColor)
    {
        Color col = Projectile.ai[2] switch
        {
            2 => Color.Gold, // 2 = No Inner Rings
            1 => Color.HotPink,
            _ => Color.Gold,
        };
        if (seed == 0) { seed = Main.rand.Next(int.MaxValue / 2); return false; }
        Texture2D tex = Assets.Extras.cone5.Value;
        Texture2D tex2 = Assets.Extras.Extras2.flare_01.Value;
        float max = Projectile.ai[0] <= 0 ? 33 : Projectile.ai[0];
        UnifiedRandom rand = new UnifiedRandom(seed);
        rand = new UnifiedRandom(seed + 1);
        float ringScale = MathHelper.Clamp(MathF.Sin(Projectile.localAI[1] * 4 * MathHelper.Pi) * 0.5f, 0, 0.3f);
        if (ringScale > 0.01f && Projectile.ai[2] != 2)
        {
            for (float i = 0; i < max; i++)
            {
                UnifiedRandom rand2 = new UnifiedRandom(seed + (int)i);
                float angle = Helper.CircleDividedEqually(i, max) + Main.GameUpdateCount * 0.04f * rand.NextFloat(-2f, 2f);
                float scale = rand.NextFloat(0.1f, .5f) * (2 + Projectile.ai[1]);
                Vector2 offset = new Vector2(rand2.NextFloat(200, 400) * (ringScale + rand2.NextFloat(-0.2f, 0.5f)) * (scale / 3), 0).RotatedBy(angle);
                for (float j = 0; j < 2; j++)
                    Main.spriteBatch.Draw(tex, Projectile.Center + offset - Main.screenPosition, null, Color.White with { A = 0 } * (ringScale * 0.3f), angle, tex.Size() / 2, new Vector2(MathHelper.Clamp(Projectile.localAI[1] * 6.5f, 0, 1.1f), ringScale) * scale * 0.3f, SpriteEffects.None, 0);

                Main.spriteBatch.Draw(tex2, Projectile.Center - angle.ToRotationVector2() * 20 + offset - Main.screenPosition, null, Color.Lerp(Color.White, col, Projectile.localAI[1] * 2) with { A = 0 } * (ringScale * 0.7f), angle, tex2.Size() / 2, MathHelper.Clamp(Projectile.localAI[1] * 6.5f, 0, 1.2f) * scale, SpriteEffects.None, 0);
            }
        }
        tex = Assets.Extras.Extras2.scratch_02.Value;
        rand = new UnifiedRandom(seed + 1);
        ringScale = MathHelper.Lerp(1.2f, 0, MathHelper.Clamp(Projectile.localAI[0] * 3.5f, 0, 1));
        if (ringScale > 0.01f)
        {
            float additionalMult = Projectile.ai[2] == 2 ? 0.5f : 1;
            for (float i = 0; i < max - 5; i++)
            {
                UnifiedRandom rand2 = new UnifiedRandom(seed + (int)i);
                float angle = Helper.CircleDividedEqually(i, max - 5) + Main.GameUpdateCount * 0.02f * rand.NextFloat(-2f, 2f);
                float scale = rand.NextFloat(0.1f, .7f) * (2 + Projectile.ai[1]);
                Vector2 offset = new Vector2(rand2.NextFloat(250, 400) * (ringScale + rand2.NextFloat(-0.2f, 0.5f)) * (scale / 4), 0).RotatedBy(angle) * additionalMult;

                Main.spriteBatch.Draw(tex, Projectile.Center + offset - Main.screenPosition, null, Color.Lerp(Color.White, col, Projectile.localAI[1] * 2) with { A = 0 } * ringScale * 0.5f, angle, tex.Size() / 2, new Vector2(Clamp(Projectile.localAI[0] * 6.5f, 0, 1.2f * additionalMult * additionalMult), ringScale * additionalMult) * scale * 0.6f, SpriteEffects.None, 0);
            }
        }
        return true;
    }
    public override void AI()
    {
        if (Projectile.timeLeft < 50)
            Projectile.localAI[2] = MathHelper.Lerp(Projectile.localAI[2], 1f, 0.0077f);
        if (Projectile.timeLeft < 40)
            Projectile.localAI[1] = MathHelper.Lerp(Projectile.localAI[1], 1f, 0.0131f);
        if (Projectile.timeLeft < 30)
            Projectile.localAI[0] = MathHelper.Lerp(Projectile.localAI[0], 1f, 0.0165f);
    }
}