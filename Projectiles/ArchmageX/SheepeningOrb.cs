using EbonianMod.Buffs;
using EbonianMod.Dusts;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.Graphics.CameraModifiers;

namespace EbonianMod.Projectiles.ArchmageX;

public class SheepeningOrb : ModProjectile
{
    int MaxTime = 80;
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 100;
        ProjectileID.Sets.TrailingMode[Type] = 2;
    }
    public override void SetDefaults()
    {
        Projectile.friendly = false;
        Projectile.hostile = true;
        Projectile.tileCollide = true;
        Projectile.aiStyle = -1;
        Projectile.timeLeft = MaxTime;
        Projectile.Size = new(32, 32);
    }
    public override bool ShouldUpdatePosition() => false;
    public override bool? CanDamage() => Projectile.ai[1] > 0.1f && Projectile.timeLeft > 25;
    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        float a = 0f;
        return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity.ToRotation().ToRotationVector2() * 2000, 30, ref a);
    }
    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.Write(Projectile.localAI[0]);
        writer.Write(Projectile.localAI[1]);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
        Projectile.localAI[0] = reader.ReadSingle();
        Projectile.localAI[1] = reader.ReadSingle();
    }
    float vfxOff;
    public override void AI()
    {
        if (Projectile.timeLeft > 70 && Projectile.ai[0] > -1)
            if (Main.player[(int)Projectile.ai[0]].active)
            {
                Projectile.velocity = Helper.FromAToB(Projectile.Center, Main.player[(int)Projectile.ai[0]].Center);
                Projectile.netUpdate = true;
            }
        Projectile.ai[2] = MathHelper.Lerp(Projectile.ai[2], 0, 0.1f);
        if (Projectile.timeLeft < 31 && Projectile.timeLeft > 10)
            Projectile.ai[1] = MathHelper.Lerp(Projectile.ai[1], 1, 0.3f);
        if (Projectile.timeLeft > 30)
            Projectile.localAI[1] = MathHelper.Lerp(Projectile.localAI[1], 1f, 0.1f);
        else
        {
            if (Projectile.timeLeft > 10)
            {
                Projectile.localAI[1] = MathHelper.Lerp(Projectile.localAI[1], 0, 0.2f);
            }
            else
            {
                Projectile.ai[1] = MathHelper.Lerp(Projectile.ai[1], 0, 0.05f);
            }
        }
        if (Projectile.timeLeft < 30 && Projectile.localAI[0] < 1)
        {
            Helper.AddCameraModifier(new PunchCameraModifier(Projectile.Center, Projectile.velocity, 7, 15, 30));
            MPUtils.NewProjectile(null, Projectile.Center + Projectile.velocity * 10, Vector2.Zero, ProjectileType<XExplosion>(), 0, 0);
            SoundEngine.PlaySound(EbonianSounds.xSpirit, Projectile.Center);
            for (int i = 0; i < 20; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustType<SparkleDust>(), Projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(5, 15), 0, Color.White, Main.rand.NextFloat(0.05f, 0.175f));
                Dust.NewDustPerfect(Projectile.Center, DustType<LineDustFollowPoint>(), Projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(5, 15), 0, Color.White, Main.rand.NextFloat(0.05f, 0.24f));
            }
            Projectile.ai[2] = 0.5f;
            Projectile.localAI[0] = 1;
            Projectile.netUpdate = true;
        }
        if (Projectile.timeLeft is < 29 and > 27)
            Projectile.ai[2] = 1;

        vfxOff -= 0.07f;
    }
    public override bool PreDraw(ref Color lightColor)
    {

        SpritebatchParameters sbParams = Main.spriteBatch.Snapshot();
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

        List<VertexPositionColorTexture> verticesMain1 = new List<VertexPositionColorTexture>();
        List<VertexPositionColorTexture> verticesMain2 = new List<VertexPositionColorTexture>();

        List<VertexPositionColorTexture> verticesFlash = new List<VertexPositionColorTexture>();

        List<VertexPositionColorTexture> verticesTelegraph1 = new List<VertexPositionColorTexture>();
        List<VertexPositionColorTexture> verticesTelegraph2 = new List<VertexPositionColorTexture>();
        Texture2D texture = Assets.Extras.wavyLaser.Value;
        Texture2D texture2 = Assets.Extras.Ex1.Value;
        Texture2D texture3 = Assets.Extras.Tentacle.Value;
        Vector2 start = Projectile.Center - Main.screenPosition;
        Vector2 off = (Projectile.velocity.ToRotation().ToRotationVector2() * 1528);
        Vector2 end = start + off;
        float rot = Helper.FromAToB(start, end).ToRotation();

        float s = 0f;
        float sLin = 0f;
        for (float i = 0; i < 1; i += 0.002f)
        {
            if (i < 0.5f)
                s = MathHelper.Clamp(i * 3.5f, 0, 0.5f);
            else
                s = MathHelper.Clamp((-i + 1) * 2, 0, 0.5f);

            if (i < 0.5f)
                sLin = MathHelper.Clamp(i, 0, 0.5f);
            else
                sLin = MathHelper.Clamp((-i + 1), 0, 0.5f);

            float _off = vfxOff;

            Color col = Color.White * 0.5f * s;
            if (Projectile.localAI[1] > 0)
            {
                verticesTelegraph1.Add(Helper.AsVertex(start + off * i + new Vector2(MathHelper.SmoothStep(10, 450, i), 0).RotatedBy(rot + MathHelper.PiOver2) * Projectile.localAI[1], new Vector2(_off, 1), col * Projectile.localAI[1]));
                verticesTelegraph1.Add(Helper.AsVertex(start + off * i + new Vector2(MathHelper.SmoothStep(10, 450, i), 0).RotatedBy(rot - MathHelper.PiOver2) * Projectile.localAI[1], new Vector2(_off, 0), col * Projectile.localAI[1]));

                col = Color.Magenta * 0.25f * s;
                verticesTelegraph2.Add(Helper.AsVertex(start + off * i + new Vector2(420, 0).RotatedBy(rot + MathHelper.PiOver2) * Projectile.localAI[1], new Vector2(_off, 1), col * Projectile.localAI[1]));
                verticesTelegraph2.Add(Helper.AsVertex(start + off * i + new Vector2(420, 0).RotatedBy(rot - MathHelper.PiOver2) * Projectile.localAI[1], new Vector2(_off, 0), col * Projectile.localAI[1]));
            }
            if (Projectile.ai[1] > 0)
            {
                float cA = MathHelper.Lerp(s, sLin, i);
                col = Color.Indigo * (cA);
                verticesMain1.Add(Helper.AsVertex(start + off * i + new Vector2(MathHelper.SmoothStep(0, 50, sLin * 2), 0).RotatedBy(rot + MathHelper.PiOver2) * Projectile.ai[1], new Vector2(_off, 1), col * Projectile.ai[1] * 2));
                verticesMain1.Add(Helper.AsVertex(start + off * i + new Vector2(MathHelper.SmoothStep(0, 50, sLin * 2), 0).RotatedBy(rot - MathHelper.PiOver2) * Projectile.ai[1], new Vector2(_off, 0), col * Projectile.ai[1] * 2));

                col = Color.Lerp(Color.White, Color.Indigo, Projectile.ai[2]) * (cA * 0.5f);
                verticesMain2.Add(Helper.AsVertex(start + off * i + new Vector2(MathHelper.SmoothStep(0, 20, sLin * 2), 0).RotatedBy(rot + MathHelper.PiOver2) * Projectile.ai[1], new Vector2(_off, 1), col * Projectile.ai[1]));
                verticesMain2.Add(Helper.AsVertex(start + off * i + new Vector2(MathHelper.SmoothStep(0, 20, sLin * 2), 0).RotatedBy(rot - MathHelper.PiOver2) * Projectile.ai[1], new Vector2(_off, 0), col * Projectile.ai[1]));
            }
            if (Projectile.ai[2] > 0)
            {
                col = Color.Lerp(Color.MistyRose, Color.Transparent, i) * (s);
                verticesFlash.Add(Helper.AsVertex(start + off * 1.2f * i + new Vector2(MathHelper.SmoothStep(20, 85, i * 3), 0).RotatedBy(rot + MathHelper.PiOver2) * Projectile.ai[2], new Vector2(_off, 1), col * Projectile.ai[2] * 4));
                verticesFlash.Add(Helper.AsVertex(start + off * 1.2f * i + new Vector2(MathHelper.SmoothStep(20, 85, i * 3), 0).RotatedBy(rot - MathHelper.PiOver2) * Projectile.ai[2], new Vector2(_off, 0), col * Projectile.ai[2] * 4));
            }
        }

        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        if (verticesMain1.Count >= 3 && verticesMain2.Count >= 3)
        {
            for (int i = 0; i < 6; i++)
            {
                Helper.DrawTexturedPrimitives(verticesMain1.ToArray(), PrimitiveType.TriangleStrip, texture, false);
                Helper.DrawTexturedPrimitives(verticesMain2.ToArray(), PrimitiveType.TriangleStrip, texture3, false);
            }
        }
        if (verticesTelegraph1.Count >= 3 && verticesTelegraph2.Count >= 3)
        {
            Helper.DrawTexturedPrimitives(verticesTelegraph1.ToArray(), PrimitiveType.TriangleStrip, texture2, false);
            Helper.DrawTexturedPrimitives(verticesTelegraph2.ToArray(), PrimitiveType.TriangleStrip, texture2, false);
        }
        if (verticesFlash.Count >= 3)
        {
            for (int i = 0; i < 2; i++)
            {
                Helper.DrawTexturedPrimitives(verticesFlash.ToArray(), PrimitiveType.TriangleStrip, texture3, false);
            }
        }
        Main.spriteBatch.ApplySaved(sbParams);
        return false;
    }
    public override void OnHitPlayer(Player target, Player.HurtInfo info)
    {
        target.AddBuff(BuffType<Sheepened>(), (Main.expertMode ? Main.masterMode ? 10 : 8 : 6) * 60);
    }
}
