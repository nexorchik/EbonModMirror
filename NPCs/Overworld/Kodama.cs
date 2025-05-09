using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.NPCs.Overworld;
public class Kodama : ModNPC
{
    public override void SetStaticDefaults()
    {
        NPCID.Sets.TrailingMode[Type] = 3;
        NPCID.Sets.TrailCacheLength[Type] = 20;
    }
    public override void SetDefaults()
    {
        NPC.Size = new Vector2(22, 26);
        NPC.knockBackResist = 0f;
        NPC.noGravity = true;
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
        NPC.noTileCollide = true;
        NPC.lifeMax = 600;
        NPC.defense = 10;
        NPC.damage = 30;
        NPC.aiStyle = -1;
    }
    public override Color? GetAlpha(Color drawColor) => Color.White;
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        EbonianMod.pixelationDrawCache.Add(() =>
        {
            List<VertexPositionColorTexture> vertices = new();
            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                if (NPC.oldPos[i] == Vector2.Zero) continue;
                Vector2 oldPos = NPC.oldPos[i] + new Vector2(0, MathF.Sin(i * 0.5f) * 2).RotatedBy(NPC.rotation);
                float mult = (1 - 1f / NPC.oldPos.Length * i);
                float rotOffset = Helper.FromAToB(oldPos, NPC.position).ToRotation();
                if (i > 0)
                {
                    Vector2 oldPos2 = NPC.oldPos[i - 1] + new Vector2(0, MathF.Sin(i * 0.5f) * 2).RotatedBy(NPC.rotation);
                    rotOffset = Helper.FromAToB(oldPos2, oldPos).ToRotation();
                }
                rotOffset += MathF.Sin(Main.GlobalTimeWrappedHourly * 3) * SmoothStep(1, 0, mult);
                Vector2 off = i <= 1 ? NPC.rotation.ToRotationVector2() * NPC.velocity.Length() * 0.5f : Vector2.Zero;
                Vector2 pos = oldPos + NPC.Size / 2 + new Vector2(0, -4).RotatedBy(NPC.rotation) - rotOffset.ToRotationVector2() * 10 + off - Main.screenPosition;
                vertices.Add(Helper.AsVertex(pos + new Vector2(13 * Clamp(mult * 2, 0, 1), 0).RotatedBy(PiOver2 + rotOffset), Color.White * (i < 3 ? 0 : 1), new Vector2((float)i / NPC.oldPos.Length * 3 - Main.GlobalTimeWrappedHourly * 1.5f, 0)));
                vertices.Add(Helper.AsVertex(pos + new Vector2(13 * Clamp(mult * 2, 0, 1), 0).RotatedBy(-PiOver2 + rotOffset), Color.White * (i < 3 ? 0 : 1), new Vector2((float)i / NPC.oldPos.Length * 3 - Main.GlobalTimeWrappedHourly * 1.5f, 1)));
            }
            if (vertices.Count > 2)
                Helper.DrawTexturedPrimitives(vertices.ToArray(), PrimitiveType.TriangleStrip, ExtraSpriteTextures.KodamaTrail.Value, false, true);
        }
        );
        EbonianMod.finalDrawCache.Add(() =>
        spriteBatch.Draw(TextureAssets.Npc[Type].Value, NPC.Center - screenPos, null, Main.LocalPlayer.HasBuff(BuffID.Hunter) ? NPC.HunterPotionColor() : Color.White, NPC.rotation + (NPC.direction == -1 ? Pi : 0), NPC.Size / 2, NPC.scale, NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0)
        );
        return false;
    }
    public override void AI()
    {
        if (NPC.ai[3] == 0)
            NPC.ai[3] = Main.rand.Next(ushort.MaxValue);
        for (int i = 0; i < NPC.oldPos.Length; i += 2)
            Lighting.AddLight(NPC.oldPos[i] + NPC.Size / 2, 197f / 255f * (1 - 1f / NPC.oldPos.Length * i), 226f / 255f * (1 - 1f / NPC.oldPos.Length * i), 105f / 255f * (1 - 1f / NPC.oldPos.Length * i));

        Lighting.AddLight(NPC.Center + NPC.velocity, 197f / 255f, 226f / 255f, 105f / 255f);
        //NPC.direction = NPC.spriteDirection = MathF.Sign(NPC.velocity.X);
        NPC.direction = NPC.spriteDirection = -1;
        NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.velocity.ToRotation(), 0.2f);

        NPC.ai[0] += 0.5f;
        NPC.TargetClosest(false);
        Player player = Main.player[NPC.target];

        float dist = NPC.Distance(player.Center);
        float off = Clamp(MathF.Sin(NPC.ai[0] * 0.03f), 0.5f, 1) * MathF.Sign(MathF.Sin(NPC.ai[0] * 0.03f));
        if (dist > 300)
            off = Clamp(MathF.Sin(NPC.ai[0] * 0.03f), 0.5f, 1);
        else if (dist > 400)
            off = 1f;
        if (dist < 600)
            NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.Center.FromAToB(player.Center + new Vector2(new UnifiedRandom((int)NPC.ai[3]).NextFloat(80, 260)).RotatedBy(ToRadians(NPC.ai[0] * 4)) * off, false) * 0.075f, 0.1f);
        else
            NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.Center.FromAToB(player.Center) * 20, 0.1f);
    }
}

