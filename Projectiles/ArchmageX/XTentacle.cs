using System;
using System.Collections.Generic;

namespace EbonianMod.Projectiles.ArchmageX;

public class XTentacle : ModProjectile
{
    public override string Texture => "EbonianMod/Extras/laser_purple";
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Projectile.type] = 100;
        ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        ProjectileID.Sets.DontAttachHideToAlpha[Type] = true;
    }
    private List<float> rots;

    public int len;
    public override void SetDefaults()
    {
        Projectile.width = 1;
        Projectile.height = 1;
        Projectile.hostile = true;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.timeLeft = 160;
        Projectile.netUpdate = true;
        Projectile.netUpdate2 = true;
        Projectile.netImportant = true;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 15;
        Projectile.extraUpdates = 2;
        rots = new List<float>();
        len = 0;
        Projectile.hide = true;
    }
    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) => behindNPCs.Add(index);
    public override bool ShouldUpdatePosition()
    {
        return false;
    }
    public Color drawColor;
    public Vector2 OldPoint(int i)
    {
        return Projectile.Center - Main.screenPosition + (Projectile.oldRot[i] * (float)(90f - i) / 90f + Projectile.ai[0]).ToRotationVector2() * (.12f * i * (float)(1f - (float)Math.Pow(.975f, i)) / .025f);
    }
    float value;
    public override void AI()
    {
        if (Projectile.timeLeft == 99)
        {
            if (Projectile.ai[0] == 0)
                Projectile.ai[0] = 70;
            if (Projectile.ai[1] == 0)
                Projectile.ai[1] = 0.5f;
        }
        for (int i = 0; i < 3; i++)
        {
            value += Projectile.ai[1];
            if (Projectile.timeLeft % 1 == 0)
            {
                float factor = 1f;
                Vector2 velocity = Projectile.velocity * factor * 4f;
                Projectile.rotation = 0.1f * (float)Math.Sin((double)(value / 100f)) + velocity.ToRotation();
                rots.Insert(0, Projectile.rotation);
                while (rots.Count > Projectile.ai[0])
                {
                    rots.RemoveAt(rots.Count - 1);
                }
            }
            if (len < Projectile.ai[0] && Projectile.timeLeft > 80)
            {
                len++;
            }
            if (len >= 0 && Projectile.timeLeft <= 80)
            {
                len--;
            }
        }
    }

    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        for (int i = 1; i < len; i++)
        {
            float factor = (float)i / (float)len;
            float w = 2 * Lerp(0.8f, 0.5f, InOutQuint.Invoke(factor)) * Lerp(1, 0.4f, factor);
            if (Collision.CheckAABBvAABBCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - new Vector2(w, w) + Utils.RotatedBy(new Vector2((float)(5 * i), 0f), rots[i]), new Vector2(w, w) * 2f))
            {
                return true;
            }
        }
        return false;
    }
    public override bool PreDraw(ref Color lightColor)
    {
        //if (lightColor != Color.Transparent)
        //  return false;
        List<VertexPositionColorTexture> bars = new List<VertexPositionColorTexture>();
        for (int i = 1; i < len; i++)
        {
            float factor = (float)i / (float)len;
            Vector2 v0 = Projectile.Center + Utils.RotatedBy(new Vector2((float)(5 * (i - 1)), 0f), rots[i - 1]);
            Vector2 v1 = Projectile.Center + Utils.RotatedBy(new Vector2((float)(5 * i), 0f), rots[i]);
            Vector2 normaldir = v1 - v0;
            normaldir = new Vector2(normaldir.Y, 0f - normaldir.X);
            float w = 2 * Lerp(0.8f, 0.5f, InOutQuint.Invoke(factor)) * Lerp(1, 0.4f, factor);
            bars.Add(Helper.AsVertex(v1 - w * normaldir - Main.screenPosition, Color.Lerp(Color.LightPink, Color.Indigo * 2, InOutCubic.Invoke(factor)) with { A = 0 } * Lerp(2, 0, factor), new Vector2(factor, 1f)));
            bars.Add(Helper.AsVertex(v1 + w * normaldir - Main.screenPosition, Color.Lerp(Color.LightPink, Color.Indigo * 2, InOutCubic.Invoke(factor)) with { A = 0 } * Lerp(2, 0, factor), new Vector2(factor, 0f)));
        }
        if (bars.Count > 2)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            Helper.DrawTexturedPrimitives(bars.ToArray(), PrimitiveType.TriangleStrip, Assets.Extras.Tentacle.Value, false);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        return false;
    }
}
