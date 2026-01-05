using EbonianMod.Core.Systems.Verlets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.Content.NPCs.Cecitior;
public partial class Cecitior : ModNPC
{
    public override void BossHeadRotation(ref float rotation)
    {
        rotation = NPC.rotation;
    }


    public override void DrawBehind(int index)
    {
        Main.instance.DrawCacheNPCsOverPlayers.Add(index);
    }


    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        if (AIState == Chomp && AITimer < 25 && AIByte % 2 != (phase2 ? 1 : 0))
        {
            float rot = Utils.AngleLerp(openRotation, ToRadians(90), 0.5f);
            if (!MPUtils.NotMPClient)
                rot = ToRadians(90);
            openRotation = rot;
            rotation = rot;
        }
        NPC.rotation = Utils.AngleLerp(NPC.rotation, rotation, 0.35f);
        Texture2D glow = Assets.ExtraSprites.Cecitior.Cecitior_Glow.Value;
        Texture2D tex = TextureAssets.Npc[Type].Value;
        Vector2 shakeOffset = Main.rand.NextVector2Circular(shakeVal, shakeVal);
        if (verlet[0] is null)
            InitVerlet();
        else
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 offset = Vector2.Zero;
                Vector2 offset2 = Vector2.Zero;
                float scale = 2;
                switch (i)
                {
                    case 1:
                        scale = 4;
                        offset = new Vector2(4, 28);
                        offset2 = new Vector2(-7, -10);
                        break;
                    case 2:
                        scale = 2;
                        offset = new Vector2(10, -20);
                        offset2 = new Vector2(1, 20);
                        break;
                    case 3:
                        scale = 3;
                        offset = new Vector2(-15, 10);
                        offset2 = new Vector2(10, -20);
                        break;
                    case 4:
                        scale = 1;
                        offset = new Vector2(1, 20);
                        offset2 = new Vector2(4, -32);
                        break;
                    case 5:
                        scale = 3;
                        offset = new Vector2(-20, -20);
                        offset2 = new Vector2(-6, 32);
                        break;
                    case 6:
                        scale = 2;
                        offset = new Vector2(10, -10);
                        offset2 = new Vector2(4, 12);
                        break;
                    case 7:
                        scale = 4;
                        offset = new Vector2(10, -20);
                        offset2 = new Vector2(-4, 32);
                        break;
                    case 8:
                        scale = 2;
                        offset = new Vector2(10, -25);
                        offset2 = new Vector2(-14, 32);
                        break;
                    case 9:
                        scale = 1;
                        offset = new Vector2(1, -20);
                        offset2 = new Vector2(4, 22);
                        break;
                }
                verlet[i].Update(NPC.Center + offset2 - openOffset, NPC.Center + openOffset + new Vector2(30, 4) + offset);
                if (verlet[i].segments[10].cut)
                    verlet[i].Draw(spriteBatch, new VerletDrawData(new VerletTextureData(Helper.AssetPath+"Extras/maroon"), _scale: scale));
            }
        }
        if (phase2 && claw is not null)
        {
            Texture2D trail = Helper.GetTexture(Helper.AssetPath+"NPCs/Cecitior/Hook/CecitiorHook_1").Value;
            if (claw[0].verlet is null && AIState > 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    claw[i].verlet = new Verlet(NPC.Center, 12, 22, 0.15f, stiffness: 50);
                }
            }
            else if (claw[0].verlet is not null)
            {
                for (int i = 0; i < claw.Length; i++)
                {
                    for (int num16 = claw[i].oldPosition.Length - 1; num16 > 0; num16--)
                    {
                        claw[i].oldPosition[num16] = claw[i].oldPosition[num16 - 1];
                    }
                    claw[i].oldPosition[0] = claw[i].position;

                    for (int num16 = claw[i].oldRotation.Length - 1; num16 > 0; num16--)
                    {
                        claw[i].oldRotation[num16] = claw[i].oldRotation[num16 - 1];
                    }
                    claw[i].oldRotation[0] = claw[i].verlet.endRot;

                    var fadeMult = 1f / claw[i].oldPosition.Length;
                    for (int j = 0; j < claw[i].oldPosition.Length; j++)
                    {
                        float mult = (1f - fadeMult * j);
                        float mult2 = (1f - fadeMult * (j));
                        if (j > 0)
                        {
                            mult2 = (1f - fadeMult * (j - 1));
                            for (float k = 0; k < 5; k++)
                            {
                                Vector2 pos = Vector2.Lerp(claw[i].oldPosition[j], claw[i].oldPosition[j - 1], (float)(k / 5));
                                Main.spriteBatch.Draw(trail, pos - Main.screenPosition, null, Color.Maroon * 0.05f * MathHelper.Lerp(mult2, mult, (float)(k / 5)), claw[i].oldRotation[j], trail.Size() / 2, MathHelper.Lerp(mult2, mult, (float)(k / 5)), SpriteEffects.None, 0);
                            }
                        }
                    }

                    claw[i].verlet.Update(NPC.Center + (new Vector2(20 + i * 6f, (i - 1) * 10).RotatedBy(openRotation) + openOffset) * (i == 2 ? -1 : 1), claw[i].position);
                    if (i == (int)AITimer3)
                    {
                        if (AIState == Phase2ClawGrab && (int)AITimer2 == 1)
                        {
                            claw[i].verlet.Draw(spriteBatch, new VerletDrawData(new VerletTextureData(Helper.AssetPath+"NPCs/Cecitior/Hook/CecitiorHook_0", _endTex: Helper.AssetPath+"NPCs/Cecitior/Hook/CecitiorHook_8")));
                            claw[i].verlet.Draw(spriteBatch, new VerletDrawData(new VerletTextureData(Helper.AssetPath+"Extras/Empty", _endTex: Helper.AssetPath+"NPCs/Cecitior/Hook/CecitiorHook_8_Glow"), _color: Color.White));
                        }
                        else
                        {
                            claw[i].verlet.Draw(spriteBatch, new VerletDrawData(new VerletTextureData(Helper.AssetPath+"NPCs/Cecitior/Hook/CecitiorHook_0", _endTex: Helper.AssetPath+"NPCs/Cecitior/Hook/CecitiorHook_" + hookFrame)));
                            claw[i].verlet.Draw(spriteBatch, new VerletDrawData(new VerletTextureData(Helper.AssetPath+"Extras/Empty", _endTex: Helper.AssetPath+"NPCs/Cecitior/Hook/CecitiorHook_" + hookFrame + "_Glow"), _color: Color.White));
                        }
                    }
                    else
                    {
                        claw[i].verlet.Draw(spriteBatch, new VerletDrawData(new VerletTextureData(Helper.AssetPath+"NPCs/Cecitior/Hook/CecitiorHook_0", _endTex: Helper.AssetPath+"NPCs/Cecitior/Hook/CecitiorHook_" + hookFrame)));
                        claw[i].verlet.Draw(spriteBatch, new VerletDrawData(new VerletTextureData(Helper.AssetPath+"Extras/Empty", _endTex: Helper.AssetPath+"NPCs/Cecitior/Hook/CecitiorHook_" + hookFrame + "_Glow"), _color: Color.White));
                    }
                }
            }
        }

        Texture2D teeth = Assets.ExtraSprites.Cecitior.CecitiorTeeth.Value;
        Texture2D partTeeth = Assets.ExtraSprites.Cecitior.CecitiorTeeth2.Value;
        Texture2D part = Assets.ExtraSprites.Cecitior.Cecitior_Part.Value;
        Texture2D partGlow = Assets.ExtraSprites.Cecitior.Cecitior_Part_Glow.Value;
        if (open || NPC.frame.Y == 6 * 102)
        {
            spriteBatch.Draw(teeth, NPC.Center + shakeOffset - openOffset - new Vector2(0, -2) - screenPos, null, new Color(Lighting.GetSubLight(NPC.Center - openOffset)), NPC.rotation, teeth.Size() / 2, NPC.scale, SpriteEffects.None, 0);
            spriteBatch.Draw(partTeeth, NPC.Center - shakeOffset + new Vector2(30, 4) + openOffset - screenPos, null, new Color(Lighting.GetSubLight(NPC.Center + openOffset)), openRotation, partTeeth.Size() / 2, NPC.scale, SpriteEffects.None, 0);
            if (verlet[0] is not null)
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector2 offset = Vector2.Zero;
                    Vector2 offset2 = Vector2.Zero;
                    float scale = 2;
                    switch (i)
                    {
                        case 1:
                            scale = 4;
                            offset = new Vector2(4, 28);
                            offset2 = new Vector2(-7, -10);
                            break;
                        case 2:
                            scale = 2;
                            offset = new Vector2(10, -20);
                            offset2 = new Vector2(1, 20);
                            break;
                        case 3:
                            scale = 3;
                            offset = new Vector2(-15, -10);
                            offset2 = new Vector2(10, -20);
                            break;
                        case 4:
                            scale = 1;
                            offset = new Vector2(1, 20);
                            offset2 = new Vector2(4, 32);
                            break;
                    }

                    if (!verlet[i].segments[10].cut)
                        verlet[i].Draw(spriteBatch, new VerletDrawData(new VerletTextureData(Helper.AssetPath+"Extras/maroon"), _scale: scale));
                }
            }
            spriteBatch.Draw(part, NPC.Center - shakeOffset + new Vector2(30, 4) + openOffset - screenPos, null, new Color(Lighting.GetSubLight(NPC.Center + new Vector2(30, 4) + openOffset) * 1.25f), openRotation, part.Size() / 2, NPC.scale, SpriteEffects.None, 0);
            spriteBatch.Draw(partGlow, NPC.Center - shakeOffset + new Vector2(30, 4) + openOffset - screenPos, null, Color.White, openRotation, part.Size() / 2, NPC.scale, SpriteEffects.None, 0);

            spriteBatch.Draw(tex, NPC.Center + shakeOffset - openOffset - screenPos, NPC.frame, new Color(Lighting.GetSubLight(NPC.Center - openOffset) * 1.25f), NPC.rotation, NPC.Size / 2, NPC.scale, SpriteEffects.None, 0);
            spriteBatch.Draw(glow, NPC.Center + shakeOffset - openOffset - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.Size / 2, NPC.scale, SpriteEffects.None, 0);
            return false;
        }
        spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, NPC.Size / 2, NPC.scale, SpriteEffects.None, 0);
        spriteBatch.Draw(glow, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.Size / 2, NPC.scale, SpriteEffects.None, 0);
        return false;
    }
    int hookFrame = 1;


    public override void FindFrame(int frameHeight)
    {
        NPC.frameCounter++;
        if (NPC.frameCounter % 5 == 0)
        {
            hookFrame++;
            if (hookFrame > 7 || hookFrame < 1)
                hookFrame = 1;
        }

        if (openOffset.Length() > 1 || openRotation != 0)
            NPC.frame.Y = frameHeight * 6;
        else if (NPC.frameCounter % 5 == 0)
        {
            if (NPC.frame.Y < frameHeight * 5)
                NPC.frame.Y += frameHeight;
            else
                NPC.frame.Y = 0;
        }
    }
}
