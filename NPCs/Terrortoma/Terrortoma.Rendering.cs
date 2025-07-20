using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.NPCs.Terrortoma;
public partial class Terrortoma : ModNPC
{
    Rectangle introFrame = new Rectangle(0, 0, 118, 108), laughFrame = new Rectangle(0, 0, 118, 108);
    bool angry;
    bool isLaughing;
    float glareAlpha;
    float bloomAlpha;


    public override void BossHeadRotation(ref float rotation)
    {
        rotation = NPC.rotation;
    }


    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 pos, Color lightColor)
    {
        Texture2D laughTex = Assets.ExtraSprites.Terrortoma.TerrortomaLaughing.Value;
        Texture2D tomaTex = TextureAssets.Npc[Type].Value;
        Texture2D spawnTex = Assets.ExtraSprites.Terrortoma.TerrortomaSpawn.Value;
        Player player = Main.player[NPC.target];
        Vector2 drawOrigin = new Vector2(tomaTex.Width * 0.5f, NPC.height * 0.5f);
        if (NPC.IsABestiaryIconDummy)
            spriteBatch.Draw(tomaTex, NPC.Center - pos, NPC.frame, NPC.IsABestiaryIconDummy ? Color.White : lightColor, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0);
        else
        {
            if (AITimer % 5 == 0)
            {
                if (laughFrame.Y < laughFrame.Height * 2)
                    laughFrame.Y += laughFrame.Height;
                else
                    laughFrame.Y = 0;
            }
            if (isLaughing || AIState == -12124)
            {
                spriteBatch.Draw(laughTex, NPC.Center - pos, laughFrame, lightColor, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0);
            }
            if (((AIState == EyeHomingFlames ? true : !isLaughing) && AIState != -12124 && AIState != Intro))
            {
                Texture2D tex = Assets.ExtraSprites.Terrortoma.Terrortoma_Bloom.Value;
                spriteBatch.Reload(BlendState.Additive);
                spriteBatch.Draw(tex, NPC.Center - Main.screenPosition, null, Color.LawnGreen * bloomAlpha, NPC.rotation, tex.Size() / 2 - new Vector2(0, 2).RotatedBy(NPC.rotation), NPC.scale, SpriteEffects.None, 0);
                spriteBatch.Reload(BlendState.AlphaBlend);
                if (!isLaughing)
                    spriteBatch.Draw(tomaTex, NPC.Center - pos, NPC.frame, NPC.IsABestiaryIconDummy ? Color.White : lightColor, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0);
            }
            if (isLaughing || AIState == -12124)
            {
                spriteBatch.Draw(laughTex, NPC.Center - pos, laughFrame, lightColor, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0);
            }
            if (AIState == Intro)
            {
                spriteBatch.Draw(spawnTex, NPC.Center - pos, introFrame, lightColor, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0);
            }
        }
        return false;
    }


    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Player player = Main.player[NPC.target];
        Texture2D tex = Assets.ExtraSprites.Terrortoma.TerrorEye.Value;
        Texture2D laughTex = Assets.ExtraSprites.Terrortoma.TerrortomaLaughing.Value;
        Texture2D tomaTex = TextureAssets.Npc[Type].Value;
        Texture2D spawnTex = Assets.ExtraSprites.Terrortoma.TerrortomaSpawn.Value;
        Vector2 eyeOGPosition = NPC.Center - new Vector2(-7, 14).RotatedBy(NPC.rotation);
        Vector2 eyePosition = NPC.Center - new Vector2(-7, 14).RotatedBy(NPC.rotation);
        Vector2 fromTo = Helper.FromAToB(eyeOGPosition, player.Center);
        if (NPC.IsABestiaryIconDummy)
        {
            fromTo = Helper.FromAToB(eyeOGPosition, Main.MouseScreen);
            float dist = MathHelper.Clamp(Helper.FromAToB(eyeOGPosition, Main.MouseScreen, false).Length() * 0.1f, 0, 5);
            eyePosition += dist * fromTo;
            spriteBatch.Draw(tex, eyePosition - screenPos, null, Color.White, 0, Vector2.One * 2, 1, SpriteEffects.None, 0);
        }
        if (AIState != -12124 && AIState != Intro && (AIState == EyeHomingFlames ? true : !isLaughing))
        {
            float dist = MathHelper.Clamp(Helper.FromAToB(eyeOGPosition, player.Center, false).Length() * 0.1f, 0, 6);
            if (AIState == Death)
            {
                Vector2 vel = NPC.velocity;
                vel.Normalize();
                if (NPC.velocity == Vector2.Zero)
                    eyePosition += Main.rand.NextVector2Unit() * Main.rand.NextFloat(3);
                else
                    eyePosition += vel * 5;
            }
            else
                eyePosition += dist * fromTo;
            if (!isLaughing)
                spriteBatch.Draw(tex, eyePosition - screenPos, null, drawColor, 0, Vector2.One * 2, 1, SpriteEffects.None, 0);

            Texture2D tex2 = Assets.Extras.crosslight.Value;
            if (glareAlpha > 0)
            {
                Main.spriteBatch.Reload(BlendState.Additive);
                Main.spriteBatch.Draw(tex2, isLaughing ? eyeOGPosition : eyePosition - Main.screenPosition, null, Color.LawnGreen * glareAlpha, 0, tex2.Size() / 2, glareAlpha * 0.2f, SpriteEffects.None, 0);
                if (AIState == Death)
                {
                    Texture2D tex3 = Assets.Extras.Extras2.flare_01.Value;
                    Texture2D tex4 = Assets.Extras.Extras2.star_02.Value;
                    Main.spriteBatch.Draw(tex2, eyePosition - Main.screenPosition, null, Color.Olive * (glareAlpha - 1), Main.GameUpdateCount * 0.03f, tex2.Size() / 2, (glareAlpha - 1) * 0.5f, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(tex3, eyePosition - Main.screenPosition, null, Color.Green * (glareAlpha - 2), Main.GameUpdateCount * -0.03f, tex3.Size() / 2, (glareAlpha - 2) * 0.45f * 2, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(tex4, eyePosition - Main.screenPosition, null, Color.Green * (glareAlpha - 3), Main.GameUpdateCount * -0.03f, tex4.Size() / 2, (glareAlpha - 3) * 0.75f * 2, SpriteEffects.None, 0);
                }
                Main.spriteBatch.Reload(BlendState.AlphaBlend);
            }
        }
        Vector2 drawOrigin = new Vector2(tomaTex.Width * 0.5f, NPC.height * 0.5f);
        if (isLaughing || AIState == -12124)
        {
            if (!NPC.IsABestiaryIconDummy)
                spriteBatch.Draw(laughTex, NPC.Center - screenPos, laughFrame, drawColor, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0);
        }
        if (AIState == Intro)
        {
            if (!NPC.IsABestiaryIconDummy)
                spriteBatch.Draw(spawnTex, NPC.Center - screenPos, introFrame, drawColor, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0);
        }
    }


    public override void FindFrame(int frameHeight)
    {
        if (AIState == Death || angry)
        {
            if (++NPC.frameCounter < 5)
                NPC.frame.Y = 12 * frameHeight;
            else if (NPC.frameCounter < 10)
                NPC.frame.Y = 13 * frameHeight;
            else
            {
                NPC.frameCounter = 0;
                NPC.frame.Y = 12 * frameHeight;
            }
        }
        else
        {
            if (++NPC.frameCounter % 5 == 0)
            {
                if (NPC.frame.Y < frameHeight * 11)
                    NPC.frame.Y += frameHeight;
                else
                    NPC.frame.Y = 0;
            }
        }
    }
}
