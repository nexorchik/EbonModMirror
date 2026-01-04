using System.Collections.Generic;

namespace EbonianMod.Projectiles.VFXProjectiles;

public class Gibs : ModProjectile
{
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 25;
        ProjectileID.Sets.TrailingMode[Type] = 0;
    }
    public override void OnKill(int timeLeft)
    {
        /*foreach (Vector2 pos in Projectile.oldPos)
            for (int i = 0; i < 2; i++)
                Dust.NewDustPerfect(pos + Projectile.Size / 2, Projectile.ai[2] == 0 ? DustID.Blood : DustID.Torch, Main.rand.NextVector2Circular(3, 3));*/
    }
    public override string Texture => "EbonianMod/Assets/Extras/Explosion";
    float vfxOffset;
    public override bool PreDraw(ref Color lightColor)
    {
        var fadeMult = Helper.Safe(1f / Projectile.oldPos.Length);
        vfxOffset -= 0.015f;
        if (vfxOffset <= 0)
            vfxOffset = 1;
        vfxOffset = MathHelper.Clamp(vfxOffset, float.Epsilon, 1 - float.Epsilon);
        List<VertexPositionColorTexture> vertices = new List<VertexPositionColorTexture>();
        float s = 0;
        for (int i = 0; i < Projectile.oldPos.Length; i++)
        {
            float mult = (1f - fadeMult * i);

            if (mult < 0.5f)
                s = MathHelper.Clamp(mult * 3.5f, 0, 0.5f) * 3;
            else
                s = MathHelper.Clamp((-mult + 1) * 2, 0, 0.5f) * 5;

            if (i > 0 && Projectile.oldPos[i] != Vector2.Zero)
            {
                Color col = (Projectile.ai[2] == 0 ? Color.Maroon : Color.OrangeRed) * mult * 2 * s;

                float __off = vfxOffset;
                if (__off > 1) __off = -__off + 1;
                float _off = __off + mult;
                vertices.Add(Helper.AsVertex(Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition + new Vector2(50 * mult, 0).RotatedBy(Helper.FromAToB(Projectile.oldPos[i - 1], Projectile.oldPos[i]).ToRotation() + MathHelper.PiOver2), col, new Vector2(_off, 0)));
                vertices.Add(Helper.AsVertex(Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition + new Vector2(50 * mult, 0).RotatedBy(Helper.FromAToB(Projectile.oldPos[i - 1], Projectile.oldPos[i]).ToRotation() - MathHelper.PiOver2), col, new Vector2(_off, 1)));
            }
        }
        SpritebatchParameters sbParams = Main.spriteBatch.Snapshot();
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Immediate, (Projectile.ai[2] == 0 ? BlendState.AlphaBlend : BlendState.Additive), SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        if (vertices.Count > 2)
        {
            Helper.DrawTexturedPrimitives(vertices.ToArray(), PrimitiveType.TriangleStrip, Images.Extras.Textures.LaserFaintTransparent.Value, false);
        }
        Main.spriteBatch.ApplySaved(sbParams);
        return false;
    }
    public override void SetDefaults()
    {
        Projectile.width = 5;
        Projectile.height = 5;
        Projectile.aiStyle = 14;
        AIType = ProjectileID.StickyGlowstick;
        Projectile.friendly = true;
        Projectile.tileCollide = true;
        Projectile.hostile = false;
        Projectile.timeLeft = 150;
        Projectile.penetrate = -1;
    }
    public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
    {
        if (Projectile.Center.Y >= Main.LocalPlayer.Center.Y - 100)
            fallThrough = false;
        return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
    }
    public override bool? CanDamage()
    {
        return Projectile.velocity.Length() > 0.5f;
    }
    public override void AI()
    {
        Lighting.AddLight(Projectile.Center, 0.25f, 0, 0);
        if (Projectile.ai[1] == 1)
        {
            AIType = -1;
            Projectile.aiStyle = 0;
        }
        if (Projectile.timeLeft < 50)
            Projectile.velocity.X *= 0.975f;
        if (Projectile.velocity.Length() > 0.5f)
        {
            if (Projectile.timeLeft % 2 == 0)
                Dust.NewDustPerfect(Projectile.oldPos[3] + Projectile.Size / 2, Projectile.ai[2] == 0 ? DustID.Blood : DustID.Torch, Projectile.velocity).noGravity = true;
            else
                Dust.NewDustPerfect(Projectile.oldPos[3] + Projectile.Size / 2, Projectile.ai[2] == 0 ? DustID.Blood : DustID.Torch, Main.rand.NextVector2Circular(1.5f, 1.5f));
        }
    }
}
public class HostileGibs : ModProjectile
{
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 25;
        ProjectileID.Sets.TrailingMode[Type] = 0;
    }
    public override void OnKill(int timeLeft)
    {
        /*foreach (Vector2 pos in Projectile.oldPos)
            for (int i = 0; i < 2; i++)
                Dust.NewDustPerfect(pos + Projectile.Size / 2, Projectile.ai[2] == 0 ? DustID.Blood : DustID.Torch, Main.rand.NextVector2Circular(3, 3));*/
    }
    public override string Texture => "EbonianMod/Assets/Extras/Explosion";
    float vfxOffset;
    public override bool PreDraw(ref Color lightColor)
    {
        var fadeMult = Helper.Safe(1f / Projectile.oldPos.Length);
        vfxOffset -= 0.015f;
        if (vfxOffset <= 0)
            vfxOffset = 1;
        vfxOffset = MathHelper.Clamp(vfxOffset, float.Epsilon, 1 - float.Epsilon);
        List<VertexPositionColorTexture> vertices = new List<VertexPositionColorTexture>();
        float s = 0;
        for (int i = 0; i < Projectile.oldPos.Length; i++)
        {
            float mult = (1f - fadeMult * i);

            if (mult < 0.5f)
                s = MathHelper.Clamp(mult * 3.5f, 0, 0.5f) * 3;
            else
                s = MathHelper.Clamp((-mult + 1) * 2, 0, 0.5f) * 5;

            if (i > 2 && Projectile.oldPos[i] != Vector2.Zero)
            {
                Color col = (Projectile.ai[2] == 0 ? Color.Maroon : Color.OrangeRed) * mult * 2 * s;

                float __off = vfxOffset;
                if (__off > 1) __off = -__off + 1;
                float _off = __off + mult;
                float rot = Helper.FromAToB(Projectile.oldPos[i - 1], Projectile.oldPos[i]).ToRotation();
                if (Projectile.oldPos[i - 1].Equals(Projectile.oldPos[i]) || Helper.FromAToB(Projectile.oldPos[i - 1], Projectile.oldPos[i]).LengthSquared() == 0)
                    rot = Projectile.velocity.ToRotation();
                vertices.Add(Helper.AsVertex(Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition + new Vector2(100 * mult, 0).RotatedBy(rot + MathHelper.PiOver2), col, new Vector2(_off, 0)));
                vertices.Add(Helper.AsVertex(Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition + new Vector2(100 * mult, 0).RotatedBy(rot - MathHelper.PiOver2), col, new Vector2(_off, 1)));
            }
        }
        SpritebatchParameters sbParams = Main.spriteBatch.Snapshot();
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Immediate, (Projectile.ai[2] == 0 ? BlendState.AlphaBlend : BlendState.Additive), SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        if (vertices.Count > 2)
        {
            Helper.DrawTexturedPrimitives(vertices.ToArray(), PrimitiveType.TriangleStrip, Images.Extras.Textures.LaserFaintTransparent.Value, false);
        }
        Main.spriteBatch.ApplySaved(sbParams);
        return false;
    }
    public override void SetDefaults()
    {
        Projectile.width = 5;
        Projectile.height = 5;
        Projectile.aiStyle = 14;
        AIType = ProjectileID.Glowstick;
        Projectile.friendly = false;
        Projectile.tileCollide = false;
        Projectile.hostile = true;
        Projectile.timeLeft = 500;
        Projectile.penetrate = -1;
    }
    public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
    {
        if (Projectile.Center.Y >= Main.LocalPlayer.Center.Y - 100)
            fallThrough = false;
        return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
    }
    public override bool? CanDamage()
    {
        return Projectile.velocity.Length() > 0.5f;
    }
    public override void AI()
    {
        Lighting.AddLight(Projectile.Center, 0.25f, 0, 0);
        if (Projectile.ai[1] == 1)
        {
            AIType = -1;
            Projectile.aiStyle = 0;
        }
        if (Projectile.timeLeft < 50)
            Projectile.velocity.X *= 0.975f;
        if (Projectile.velocity.Length() > 0.5f)
        {
            if (Projectile.timeLeft % 2 == 0)
                Dust.NewDustPerfect(Projectile.oldPos[3] + Projectile.Size / 2, Projectile.ai[2] == 0 ? DustID.Blood : DustID.Torch, Projectile.velocity).noGravity = true;
            else
                Dust.NewDustPerfect(Projectile.oldPos[3] + Projectile.Size / 2, Projectile.ai[2] == 0 ? DustID.Blood : DustID.Torch, Main.rand.NextVector2Circular(1.5f, 1.5f));
        }
    }
}
public class AmbientGibs : ModProjectile
{
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 25;
        ProjectileID.Sets.TrailingMode[Type] = 0;
    }
    public override void OnKill(int timeLeft)
    {
        foreach (Vector2 pos in Projectile.oldPos)
            for (int i = 0; i < 2; i++)
                Dust.NewDustPerfect(pos + Projectile.Size / 2, DustID.Blood, Main.rand.NextVector2Circular(3, 3));
    }
    public override string Texture => "EbonianMod/Assets/Extras/Explosion";
    float vfxOffset;
    public override bool PreDraw(ref Color lightColor)
    {
        var fadeMult = Helper.Safe(1f / Projectile.oldPos.Length);
        vfxOffset -= 0.015f;
        if (vfxOffset <= 0)
            vfxOffset = 1;
        vfxOffset = MathHelper.Clamp(vfxOffset, float.Epsilon, 1 - float.Epsilon);
        List<VertexPositionColorTexture> vertices = new List<VertexPositionColorTexture>();
        float s = 0;
        for (int i = 0; i < Projectile.oldPos.Length; i++)
        {
            float mult = (1f - fadeMult * i);

            if (mult < 0.5f)
                s = MathHelper.Clamp(mult * 3.5f, 0, 0.5f);
            else
                s = MathHelper.Clamp((-mult + 1) * 2, 0, 0.5f) * 3;

            if (i > 0 && Projectile.oldPos[i] != Vector2.Zero)
            {
                Color col = Color.Maroon * mult * s;

                float __off = vfxOffset;
                if (__off > 1) __off = -__off + 1;
                float _off = __off + mult;
                vertices.Add(Helper.AsVertex(Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition + new Vector2(50 * mult, 0).RotatedBy(Helper.FromAToB(Projectile.oldPos[i - 1], Projectile.oldPos[i]).ToRotation() + MathHelper.PiOver2), col, new Vector2(_off, 0)));
                vertices.Add(Helper.AsVertex(Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition + new Vector2(50 * mult, 0).RotatedBy(Helper.FromAToB(Projectile.oldPos[i - 1], Projectile.oldPos[i]).ToRotation() - MathHelper.PiOver2), col, new Vector2(_off, 1)));
            }
        }
        SpritebatchParameters sbParams = Main.spriteBatch.Snapshot();
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        if (vertices.Count > 2)
        {
            Helper.DrawTexturedPrimitives(vertices.ToArray(), PrimitiveType.TriangleStrip, Images.Extras.Textures.LaserFaintTransparent.Value, false);
        }
        Main.spriteBatch.ApplySaved(sbParams);
        return false;
    }
    public override void SetDefaults()
    {
        Projectile.width = 2;
        Projectile.height = 2;
        Projectile.aiStyle = 14;
        AIType = ProjectileID.StickyGlowstick;
        Projectile.friendly = true;
        Projectile.tileCollide = true;
        Projectile.hostile = false;
        Projectile.timeLeft = 150;
        Projectile.penetrate = -1;
    }
    public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
    {
        if (Projectile.Center.Y >= Main.LocalPlayer.Center.Y - 100)
            fallThrough = false;
        return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
    }
    public override bool? CanDamage()
    {
        return Projectile.velocity.Length() > 0.5f;
    }
    public override void AI()
    {
        if (Projectile.ai[1] == 1)
        {
            AIType = -1;
            Projectile.aiStyle = 0;
        }
        if (Projectile.timeLeft < 50)
            Projectile.velocity.X *= 0.975f;
        if (Projectile.velocity.Length() > 0.5f)
        {
            if (Projectile.timeLeft % 2 == 0)
                Dust.NewDustPerfect(Projectile.oldPos[3] + Projectile.Size / 2, DustID.Blood, Projectile.velocity).noGravity = true;
            else
                Dust.NewDustPerfect(Projectile.oldPos[3] + Projectile.Size / 2, DustID.Blood, Main.rand.NextVector2Circular(1.5f, 1.5f));
        }
    }
}
