using EbonianMod.Common.Systems.Verlets;
using EbonianMod.Effects.Prims;
using System;
using System.Collections.Generic;
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
    public override void OnSpawn(IEntitySource source)
    {
        Player player = Main.player[Projectile.owner];
        v = new(player.RotatedRelativePoint(player.MountedCenter) - new Vector2(0, 14 * player.gravDir), 1, 40, -0.2f, true, false, 25, false);
        for (int i = 0; i < 80; i++)
        {
            v.Update(player.RotatedRelativePoint(player.MountedCenter) - new Vector2(0, 14 * player.gravDir), Projectile.Center);
            v.lastP.position -= Vector2.UnitX * player.direction * (10f + (MathF.Sin(Main.GlobalTimeWrappedHourly) + 1) * 2);
        }

        for (int i = 0; i < smoke.Length; i++)
        {
            Smoke dust = smoke[i];
            dust.position = new Vector2(0, player.height / 2 - 10);
            dust.velocity = new Vector2(-player.velocity.X * Main.rand.NextFloat(0, 0.1f) + Main.rand.NextFloat(0, 2f) * -player.direction, Main.rand.NextFloat(-2f, -0.25f));
            dust.scale = Main.rand.NextFloat(0.01f, 0.05f);
        }
    }
    public override void SetDefaults()
    {
        Projectile.Size = Vector2.One * 2;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.penetrate = -1;
    }
    public struct Smoke
    {
        public float scale;
        public Vector2 position; //is actually offset
        public Vector2 velocity;
    }
    public Smoke[] smoke = new Smoke[250];
    public override bool ShouldUpdatePosition() => false;
    public override bool? CanDamage() => false;
    void UpdateSmoke()
    {
        Player player = Main.player[Projectile.owner];
        for (int i = 0; i < smoke.Length; i++)
        {
            smoke[i].position -= smoke[i].velocity;
            smoke[i].scale -= 0.0015f;
            smoke[i].velocity *= 0.95f;
            if (smoke[i].scale < 0.005f)
                smoke[i].velocity *= 0.85f;
            if (smoke[i].scale <= 0)
            {
                Vector2 vel = new Vector2(-player.velocity.X * Main.rand.NextFloat(0, 0.2f) + Main.rand.NextFloat(0, 2f) * -player.direction, Main.rand.NextFloat(-2f, -0.25f) + MathHelper.Lerp(0, 1f, MathHelper.Clamp(player.velocity.X * (player.velocity.X < 0 ? -1 : 1) * 0.1f, 0, 1f)));
                if (v != null && !Main.rand.NextBool(5))
                    smoke[i].velocity = Helper.FromAToB(player.Center, v.lastP.position).RotatedByRandom(PiOver4) * vel.Length();
                else
                    smoke[i].velocity = vel;
                smoke[i].position = new Vector2(0, player.height / 2 - 10) + vel;
                smoke[i].scale = Main.rand.NextFloat(0.01f, 0.045f);
            }
        }
    }
    void DrawSmoke(SpriteBatch sb)
    {
        Player player = Main.player[Projectile.owner];
        for (int i = 0; i < smoke.Length; i++)
        {
            Smoke d = smoke[i];
            Texture2D tex = Assets.Extras.explosion.Value;
            sb.Draw(tex, player.RotatedRelativePoint(player.MountedCenter) - d.position - Main.screenPosition, null, Color.White * d.scale * 10, 0, tex.Size() / 2, d.scale * 2, SpriteEffects.None, 0);
        }
    }
    public override void AI()
    {
        Player player = Main.player[Projectile.owner];
        if (Main.player[Projectile.owner].GetModPlayer<EbonianPlayer>().rei || Main.player[Projectile.owner].GetModPlayer<EbonianPlayer>().reiV)
            Projectile.timeLeft = 10;
        if (Main.player[Projectile.owner].GetModPlayer<EbonianPlayer>().sheep)
            Projectile.Kill();
        UpdateSmoke();
        //for (int i = 0; i < 2; i++)
        //    Dust.NewDustPerfect(player.RotatedRelativePoint(player.MountedCenter) - new Vector2(0, player.height / 2 - 10), DustType<ReiSmoke>(), new Vector2(-player.velocity.X * Main.rand.NextFloat(-0.1f, 0.1f) + Main.rand.NextFloat(-0.5f, 2f) * -player.direction, Main.rand.NextFloat(-2f, -0.25f))).scale = Main.rand.NextFloat(0.01f, 0.05f);
        Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) + new Vector2(-5, 19);
        Projectile.rotation = player.velocity.ToRotation();
        if (v != null)
        {
            for (int i = 0; i < 5; i++)
            {
                v.Update(player.RotatedRelativePoint(player.MountedCenter) - new Vector2(0, 14 * player.gravDir), Projectile.Center);
                v.lastP.position -= Vector2.Lerp(Vector2.UnitX * player.direction * (10f + (MathF.Sin(Main.GlobalTimeWrappedHourly) + 1) * 2),
                    player.velocity, Clamp(player.velocity.Length() / 15f, 0, 1));
            }
        }
    }
    public override bool PreAI()
    {
        Player player = Main.player[Projectile.owner];
        if (v != null)
        {
            v.firstP.position = player.RotatedRelativePoint(player.MountedCenter) - new Vector2(0, 14 * player.gravDir);
        }
        return true;
    }
    public override void PostAI()
    {
        Player player = Main.player[Projectile.owner];
        if (v != null)
        {
            v.firstP.position = player.RotatedRelativePoint(player.MountedCenter) - new Vector2(0, 14 * player.gravDir);
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
    public override void PostDraw(Color lightColor)
    {
        Player player = Main.player[Projectile.owner];
        SpritebatchParameters sbParams = Main.spriteBatch.Snapshot();
        if (v != null)
        {
            List<VertexPositionColorTexture>[] vertex = new List<VertexPositionColorTexture>[3] { new(), new(), new() };
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
                Color col = Color.White * Lerp(1, 0, mult);

                for (int j = -1; j < 2; j++)
                {
                    float _rot = j * 0.1f;
                    float off = mult + Main.GlobalTimeWrappedHourly * (-0.5f - (j == 0 ? 0.5f : 0));
                    vertex[j + 1].Add(Helper.AsVertex(basePos + Helper.FromAToB(basePos, pos, false).RotatedBy(_rot) + new Vector2(10, 0).RotatedBy(rot + PiOver2 + _rot), col, new Vector2(off, 0)));
                    vertex[j + 1].Add(Helper.AsVertex(basePos + Helper.FromAToB(basePos, pos, false).RotatedBy(_rot) + new Vector2(10, 0).RotatedBy(rot - PiOver2 + _rot), col, new Vector2(off, 1)));
                }
            }
            if (vertex[0].Count > 2)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, MiscDrawingMethods.Subtractive, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                DrawSmoke(Main.spriteBatch);
                for (int j = 0; j < 3; j++)
                    Helper.DrawTexturedPrimitives(vertex[j].ToArray(), PrimitiveType.TriangleStrip, Assets.Extras.wavyLaser2, false);
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
        if (Main.player[Projectile.owner].GetModPlayer<EbonianPlayer>().rei || Main.player[Projectile.owner].GetModPlayer<EbonianPlayer>().reiV)
            Projectile.timeLeft = 10;
        else Projectile.Kill();
        if (Main.player[Projectile.owner].GetModPlayer<EbonianPlayer>().sheep)
            Projectile.Kill();
        Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) + new Vector2(5 * Projectile.ai[0], 19);
        Projectile.rotation = player.velocity.ToRotation();
        if (player.GetModPlayer<EbonianPlayer>().reiBoostCool == 59)
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
