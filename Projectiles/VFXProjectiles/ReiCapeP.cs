using EbonianMod.Common.Players;
using EbonianMod.Common.Systems.Verlets;
using EbonianMod.Effects.Prims;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.Chat;
using Terraria.UI.Chat;
using Terraria.Utilities.Terraria.Utilities;

namespace EbonianMod.Projectiles.VFXProjectiles;

public class ReiCapeP : ModProjectile
{
    public override string Texture => Helper.Empty;
    Verlet v;
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 5;
        ProjectileID.Sets.TrailingMode[Type] = 2;
    }
    public void InitVerlet()
    {
        Player player = Main.player[Projectile.owner];
        v = new(player.RotatedRelativePoint(player.MountedCenter - new Vector2(0, 14 * player.gravDir)), 1, 40, -0.2f, true, false, 25, false);
        for (int i = 0; i < 80; i++)
        {
            v.Update(player.RotatedRelativePoint(player.MountedCenter - new Vector2(0, 14 * player.gravDir)), Projectile.Center);
            v.lastP.position -= Vector2.UnitX * (MathF.Sign(player.velocity.X) == 0 ? player.direction : MathF.Sign(player.velocity.X)) * (10f + (MathF.Sin(Main.GlobalTimeWrappedHourly) + 1) * 2);
        }
    }
    public override void SetDefaults()
    {
        Projectile.Size = Vector2.One * 2;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.penetrate = -1;
        Projectile.netImportant = true;
        Projectile.netUpdate = true;
        Projectile.netUpdate2 = true;
        Projectile.originalDamage = 0;
    }
    public override bool ShouldUpdatePosition() => false;
    public override bool? CanDamage() => false;
    public override void AI()
    {
        Player player = Main.player[Projectile.owner];
        if (Main.player[Projectile.owner].GetModPlayer<AccessoryPlayer>().rei || Main.player[Projectile.owner].GetModPlayer<AccessoryPlayer>().reiV)
            Projectile.timeLeft = 10;
        if (Main.player[Projectile.owner].GetModPlayer<EbonianPlayer>().sheep)
            Projectile.Kill();
        Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) + new Vector2(-5, 19);
        Projectile.rotation = player.velocity.ToRotation();
        if (v is not null)
        {
            for (int i = 0; i < 5; i++)
            {
                v.Update(player.RotatedRelativePoint(player.MountedCenter - new Vector2(0, 14 * player.gravDir)), Projectile.Center);
                v.lastP.position -= Vector2.Lerp(Vector2.UnitX * (MathF.Sign(player.velocity.X) == 0 ? player.direction : MathF.Sign(player.velocity.X)) * (10f + (MathF.Sin(Main.GlobalTimeWrappedHourly) + 1) * 2),
                    player.velocity, Clamp(player.velocity.Length() / 15f, 0, 1));
            }
        }
    }
    public override bool PreAI()
    {
        Player player = Main.player[Projectile.owner];
        if (v is not null)
        {
            v.firstP.position = player.RotatedRelativePoint(player.MountedCenter - new Vector2(0, 14 * player.gravDir));
        }
        return true;
    }
    public override void PostAI()
    {
        Player player = Main.player[Projectile.owner];
        if (v is not null)
        {
            v.firstP.position = player.RotatedRelativePoint(player.MountedCenter - new Vector2(0, 14 * player.gravDir));
        }
    }
    public override bool PreDraw(ref Color lightColor)
    {
        return true;
    }
    public override bool PreDrawExtras()
    {
        Player player = Main.player[Projectile.owner];
        Lighting.AddLight(player.Center, TorchID.Purple);
        return true;
    }
    float playerAlpha = 1f;
    public override void PostDraw(Color lightColor)
    {
        Player player = Main.player[Projectile.owner];
        SpritebatchParameters sbParams = Main.spriteBatch.Snapshot();
        playerAlpha = Lerp(playerAlpha, (1f - player.immuneAlpha / 255f) * (!player.ShouldNotDraw).ToInt() * (player.vortexStealthActive ? 0.2f : 1), player.ShouldNotDraw ? 0.4f : 0.15f);
        if (v is null)
            InitVerlet();
        else
        {
            List<VertexPositionColorTexture>[] vertex = new List<VertexPositionColorTexture>[3] { new(), new(), new() };
            List<VertexPositionColorTexture>[] vertex2 = new List<VertexPositionColorTexture>[3] { new(), new(), new() };
            for (int i = 0; i < v.points.Count; i++)
            {
                if (i != 1 && (i == 0 || i % 3 > 1)) continue;
                Vector2 basePos = v.startPos - Main.screenPosition;
                Vector2 pos = v.points[i - 1].position - Main.screenPosition;
                float mult = (float)i / v.points.Count;

                float rot = Helper.FromAToB(v.points[i - 1].position, v.points[i].position).ToRotation();
                if (i >= 2)
                {
                    float lastRot = Helper.FromAToB(v.points[i - 2].position, v.points[i - 1].position).ToRotation();
                    float curRot = Helper.FromAToB(v.points[i - 1].position, v.points[i].position).ToRotation();
                    rot = Clamp(Utils.AngleLerp(lastRot, curRot, 0.5f), MathF.Min(curRot % TwoPi, lastRot % TwoPi) - PiOver4 * 0.05f, MathF.Max(curRot % TwoPi, lastRot % TwoPi) + PiOver4 * 0.05f);
                    pos = v.points[i - 1].position + lastRot.ToRotationVector2() * 5 + rot.ToRotationVector2() * 5 - Main.screenPosition;
                }
                Color col = Color.White * Lerp(Lerp(10, 1, Clamp(mult * 10, 0, 1)), 0, mult) * playerAlpha;

                for (int j = -1; j < 2; j++)
                {
                    float _rot = j * 0.1f;
                    float off = mult + Main.GlobalTimeWrappedHourly * (-0.5f - (j == 0 ? 0.5f : 0));
                    vertex[j + 1].Add(Helper.AsVertex(basePos + Helper.FromAToB(basePos, pos, false).RotatedBy(_rot) + new Vector2(10, 0).RotatedBy(rot + PiOver2 + _rot), col, new Vector2(off, 0)));
                    vertex[j + 1].Add(Helper.AsVertex(basePos + Helper.FromAToB(basePos, pos, false).RotatedBy(_rot) + new Vector2(10, 0).RotatedBy(rot - PiOver2 + _rot), col, new Vector2(off, 1)));
                }
                col = Color.White * SmoothStep(1, 0, Clamp(MathF.Pow(mult, 2) * 20, 0, 1)) * playerAlpha;
                for (int j = -1; j < 2; j++)
                {
                    float _rot = j * 0.1f;
                    float off = mult + Main.GlobalTimeWrappedHourly * (-0.5f - (j == 0 ? 0.5f : 0));
                    vertex2[j + 1].Add(Helper.AsVertex(basePos + Helper.FromAToB(basePos, pos, false).RotatedBy(_rot) + new Vector2(10, 0).RotatedBy(rot + PiOver2 + _rot), col, new Vector2(off, 0)));
                    vertex2[j + 1].Add(Helper.AsVertex(basePos + Helper.FromAToB(basePos, pos, false).RotatedBy(_rot) + new Vector2(10, 0).RotatedBy(rot - PiOver2 + _rot), col, new Vector2(off, 1)));
                }
            }
            if (vertex[0].Count > 2)
            {
                EbonianMod.pixelationDrawCache.Add(() =>
                {
                    Main.spriteBatch.Snapshot(out SpritebatchParameters sbParams2);
                    if (Main.spriteBatch.beginCalled)
                        Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, MiscDrawingMethods.Subtractive, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    Main.spriteBatch.Draw(Assets.Extras.explosion.Value, v.startPos - Main.screenPosition, null, Color.White * playerAlpha, v.segments[0].Rotation(), Assets.Extras.explosion.Value.Size() / 2, new Vector2(0.08f, 0.07f), SpriteEffects.None, 0);
                    for (int j = 0; j < 3; j++)
                    {
                        Helper.DrawTexturedPrimitives(vertex[j].ToArray(), PrimitiveType.TriangleStrip, Assets.Extras.wavyLaser2, false);
                        Helper.DrawTexturedPrimitives(vertex2[j].ToArray(), PrimitiveType.TriangleStrip, Assets.Extras.Tentacle, false);
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(sbParams2);
                });
            }
        }
        for (int i = 0; i < 2; i++) // Big SpriteBatch is hiding this from you: WHY DO I HAVE TO RESET THIS TWICE FOR THE ZENITH TO WORK ??
            Main.spriteBatch.ApplySaved(sbParams);
    }
}
public class ReiCapeTrail : ModProjectile
{
    public override string Texture => Helper.Empty;
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 9;
        ProjectileID.Sets.TrailingMode[Type] = 2;
    }
    public override void SetDefaults()
    {
        Projectile.Size = Vector2.One * 2;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.penetrate = -1;
    }
    public override bool ShouldUpdatePosition() => false;
    public override bool? CanDamage() => false;
    public override void AI()
    {
        Player player = Main.player[Projectile.owner];
        if (Main.player[Projectile.owner].GetModPlayer<AccessoryPlayer>().rei || Main.player[Projectile.owner].GetModPlayer<AccessoryPlayer>().reiV)
            Projectile.timeLeft = 10;
        else Projectile.Kill();
        if (Main.player[Projectile.owner].GetModPlayer<EbonianPlayer>().sheep)
            Projectile.Kill();
        Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) + new Vector2(5 * Projectile.ai[0], 19);
        Projectile.rotation = player.velocity.ToRotation();
        if (player.GetModPlayer<AccessoryPlayer>().reiBoostCool == 59)
            for (int i = 0; i < Projectile.oldPos.Length; i++)
                Projectile.oldPos[i] = Projectile.Center;
    }
    public override void PostDraw(Color lightColor)
    {
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        (default(ReiTrail)).Draw(Projectile);
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
    }
}
public class ReiExplosion : ModProjectile
{
    public override string Texture => "EbonianMod/Extras/Fire";
    public override void SetDefaults()
    {
        Projectile.height = 200;
        Projectile.width = 200;
        Projectile.hostile = false;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 100;
    }
    public override bool ShouldUpdatePosition() => false;
    int seed;
    public override void PostDraw(Color lightColor)
    {
        if (lightColor != Color.Transparent) return;
        if (seed == 0) seed = Main.rand.Next(9421814);
        Texture2D tex = Assets.Extras.cone2.Value;
        Texture2D tex2 = Assets.Extras.Extras2.trace_02.Value;
        UnifiedRandom rand = new UnifiedRandom(seed);
        Main.spriteBatch.Reload(BlendState.Additive);
        float max = 40;
        float alpha = MathHelper.Lerp(0.5f, 0, Projectile.ai[1]) * 2;
        for (float i = 0; i < max; i++)
        {
            float angle = Helper.CircleDividedEqually(i, max);
            float scale = rand.NextFloat(0.2f, 1f);
            Vector2 offset = new Vector2(Main.rand.NextFloat(50) * Projectile.ai[1] * scale, 0).RotatedBy(angle);
            Main.spriteBatch.Draw(tex, Projectile.Center + offset - Main.screenPosition, null, Color.Cyan * (alpha * 0.5f), angle, new Vector2(0, tex.Height / 2), new Vector2(Projectile.ai[1], alpha) * scale * 0.7f * 4, SpriteEffects.None, 0);
            for (float j = 0; j < 3; j++)
                Main.spriteBatch.Draw(tex2, Projectile.Center + offset - Main.screenPosition, null, Color.Cyan * alpha, angle + MathHelper.PiOver2, new Vector2(tex2.Width / 2, 0), new Vector2(alpha, Projectile.ai[1]) * scale * 1.2f * 2, SpriteEffects.None, 0);
        }

        Main.spriteBatch.Reload(BlendState.AlphaBlend);
    }
    public override void OnSpawn(IEntitySource source)
    {
        CameraSystem.ScreenShakeAmount = 5;

        Projectile.rotation = Main.rand.NextFloat(0, MathHelper.TwoPi);

        for (int k = 0; k < 20; k++)
        {
            Dust.NewDustPerfect(Projectile.Center, DustID.Electric, Main.rand.NextVector2Unit() * Main.rand.NextFloat(1, 15), 0, default, Main.rand.NextFloat(.1f, .3f)).noGravity = true;
            Dust.NewDustPerfect(Projectile.Center, DustID.Electric, Main.rand.NextVector2Unit() * Main.rand.NextFloat(1, 15), 100, default, Main.rand.NextFloat(.1f, .5f)).noGravity = true;
        }
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D tex = Assets.Extras.explosion.Value;
        Texture2D tex2 = Assets.Extras.Extras2.star_09.Value;
        Main.spriteBatch.Reload(BlendState.Additive);
        Texture2D texture = TextureAssets.Projectile[Type].Value;

        int frameHeight = texture.Height / Main.projFrames[Projectile.type];
        int frameY = frameHeight * Projectile.frame;

        Rectangle sourceRectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
        Vector2 origin = sourceRectangle.Size() / 2f;
        Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
        float alpha = MathHelper.Lerp(2, 0, Projectile.ai[0]);
        Color color = Color.Cyan * alpha;

        for (int i = 0; i < 4; i++)
            Main.EntitySpriteDraw(texture, position, sourceRectangle, color, Projectile.rotation, origin, Projectile.scale - 0.8f, SpriteEffects.None, 0);

        for (int i = 0; i < 2; i++)
        {
            Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, Color.Cyan * 0.5f * alpha, Projectile.rotation, tex2.Size() / 2, Projectile.ai[0] * 2, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.Cyan * 0.5f * alpha, Projectile.rotation, tex2.Size() / 2, Projectile.ai[0] * 0.3f * 2, SpriteEffects.None, 0);
        }
        Main.spriteBatch.Reload(BlendState.AlphaBlend);
        return false;
    }
    public override void AI()
    {
        Lighting.AddLight(Projectile.Center, TorchID.Mushroom);
        Projectile.ai[0] += 0.05f;
        Projectile.ai[1] += 0.075f;
        if (Projectile.ai[0] > 1)
            Projectile.Kill();
    }
}
