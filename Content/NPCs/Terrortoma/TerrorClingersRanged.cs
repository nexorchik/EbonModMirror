using EbonianMod.Content.Items.Misc;
using EbonianMod.Content.Projectiles.Terrortoma;
using EbonianMod.Core.Systems.Cinematic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.Content.NPCs.Terrortoma;
public class TerrorClingerRanged : TerrorClingerGeneric // Disgusting
{
    public override string Texture => Helper.AssetPath + "NPCs/Terrortoma/"+Name;
    public override void SetStaticDefaults()
    {

        NPCID.Sets.MustAlwaysDraw[Type] = true;
        NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
        {
            Hide = true
        };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
    }
    public override void SetDefaults()
    {
        base.SetDefaults();
        NPC.hide = true;
        NPC.width = 54;
        NPC.height = 58;
    }
    public override void DrawBehind(int index) => Main.instance.DrawCacheNPCsBehindNonSolidTiles.Add(index);
    public override void AI()
    {
        Player player = Main.player[NPC.target];
        NPC.damage = 0;
        NPC center = Main.npc[(int)NPC.ai[0]];
        if (!center.active || center.type != NPCType<Terrortoma>())
        {
            bool found = false;
            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (npc.type == NPCType<Terrortoma>())
                {
                    NPC.ai[0] = npc.whoAmI;
                    npc.netUpdate = true;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                NPC.active = false;
                NPC.netUpdate = true;
                return;
            }
        }
        lerpSpeed = Clamp(Lerp(lerpSpeed, ((int)center.ai[0] == 0 ? 0.05f : 0.1f), 0.1f), 0, 0.1f);
        int AIState = (int)(int)center.ai[0];
        bool phase2 = center.life <= center.lifeMax - center.lifeMax / 3 + 3500;
        int CenterAITimer = (int)(int)center.ai[1];

        if (!player.active || player.dead || (int)center.ai[0] == -12124)
        {
            NPC.TargetClosest(false);
            player = Main.player[NPC.target];
            if (NPC.HasValidTarget)
            {
                AITimer = 0;
            }
            if (!player.active || player.dead || (int)center.ai[0] == -12124)
            {
                NPC.velocity = new Vector2(0, 10f);
                if (NPC.timeLeft > 10)
                {
                    NPC.timeLeft = 10;
                    NPC.netUpdate = true;
                }
                return;
            }
        }
        if ((int)center.ai[1] < 2)
        {
            AITimer = (int)(int)center.ai[1];
            AITimer2 = 0;
        }
        if ((int)center.ai[0] == -1)
        {
            Vector2 toPlayer = player.Center - NPC.Center;
            NPC.rotation = toPlayer.ToRotation() - MathHelper.PiOver2;
            AITimer = 0;
            Vector2 pos = center.Center + new Vector2(80, 80).RotatedBy(center.rotation);
            Vector2 moveTo = NPC.FromAToB(pos, false);
            NPC.velocity = moveTo * 0.09f;
            if ((int)center.ai[1] == 50)
            {
                Vector2 neckOrigin = center.Center;
                Vector2 NPCcenter = NPC.Center;
                Vector2 distToProj = neckOrigin - NPC.Center;
                float projRotation = distToProj.ToRotation() - 1.57f;
                float distance = distToProj.Length();
                while (distance > 20 && !float.IsNaN(distance))
                {
                    distToProj.Normalize();
                    distToProj *= 20;
                    NPCcenter += distToProj;
                    distToProj = neckOrigin - NPCcenter;
                    distance = distToProj.Length();

                    if (!Main.dedServ)
                    {
                        Gore.NewGore(NPC.GetSource_Death(), NPCcenter, NPC.velocity, Find<ModGore>("EbonianMod/EbonFlyGore").Type, NPC.scale);
                        Gore.NewGore(NPC.GetSource_Death(), NPCcenter, NPC.velocity, Find<ModGore>("EbonianMod/EbonFlyGore2").Type, NPC.scale);
                        Gore.NewGore(NPC.GetSource_Death(), NPCcenter, NPC.velocity, Find<ModGore>("EbonianMod/EbonFlyGore3").Type, NPC.scale);
                    }
                }
                NPC.life = 0;
                NPC.checkDead();
                if (!Main.dedServ)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Find<ModGore>("EbonianMod/EbonFlyGore").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Find<ModGore>("EbonianMod/EbonFlyGore2").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Find<ModGore>("EbonianMod/EbonFlyGore3").Type, NPC.scale);
                }
                NPC.netUpdate = true;
                CameraSystem.ScreenShakeAmount += 5f;
                MPUtils.NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ProjectileType<OstertagiExplosion>(), 0, 0, 0);
            }
        }
        if ((int)center.ai[2] == 0 && NPC.ai[3] == 0 && AIState != 0)
        {
            bloomAlpha = 1f;
            NPC.ai[3] = 1;
            NPC.netUpdate = true;
        }
        if (bloomAlpha > 0f) bloomAlpha -= 0.025f;
        if (((int)center.ai[2] != 0 && (int)center.ai[2] <= 2) || (int)center.ai[2] == 4)
        {
            NPC.rotation = Utils.AngleLerp(NPC.rotation, center.rotation, 0.2f);
            Vector2 pos = center.Center + new Vector2(85, 85).RotatedBy(center.rotation);
            if (AIState == 6 || AIState == 14)
            {
                pos = center.Center - new Vector2(85, 85).RotatedBy(center.rotation);
            }
            Vector2 moveTo = NPC.FromAToB(pos, false);
            NPC.velocity = moveTo * lerpSpeed;
            NPC.ai[3] = 0;
        }
        else
        {
            if (AIState == 0 || AIState == 1 || AIState == 15 || AIState == 9 || AIState == 14 || AIState == 10 || AIState == 13 || AIState == 11 || AIState == -2 || AIState == 4 || AIState == 999)
            {
                AITimer3 = 0;
                AITimer = 0;
                NPC.rotation = Utils.AngleLerp(NPC.rotation, center.rotation, 0.2f);
                Vector2 pos = center.Center + new Vector2(85, 85).RotatedBy(center.rotation);
                Vector2 moveTo = pos - NPC.Center;
                NPC.velocity = (moveTo) * lerpSpeed;
            }
            if ((int)center.ai[2] != 4)
            {
                switch (AIState)
                {
                    case 2:
                        AITimer++;
                        if (AITimer > 30)
                        {
                            NPC.rotation = Utils.AngleLerp(NPC.rotation, Helper.FromAToB(NPC.Center, player.Center).ToRotation() - MathHelper.PiOver2, 0.2f);
                            NPC.velocity = Vector2.Lerp(NPC.velocity, Helper.FromAToB(NPC.Center, player.Center + new Vector2(120).RotatedBy(MathHelper.ToRadians(AITimer * 4f)), false) * 0.25f, 0.3f);
                            if (AITimer % 10 == 0 && AITimer > 50)
                            {
                                MPUtils.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Helper.FromAToB(NPC.Center, player.Center) * 2, ProjectileType<TFlameThrower4>(), 20, 0, ai2: AITimer - 100);
                            }
                        }
                        else
                        {
                            NPC.rotation = Utils.AngleLerp(NPC.rotation, center.rotation, 0.2f);
                            Vector2 pos = center.Center + new Vector2(85, 85).RotatedBy(center.rotation);
                            Vector2 target = pos;
                            Vector2 moveTo = target - NPC.Center;
                            NPC.velocity = (moveTo) * lerpSpeed;
                        }
                        if (AITimer >= 101)
                        {
                            lerpSpeed = 0.005f;
                            center.ai[2] = 4;
                            AITimer = 0;
                            center.netUpdate = true;
                        }
                        break;
                    case 3:
                        if (AITimer2 <= 0 || (AITimer2 > 30 && AITimer2 < 40))
                        {
                            NPC.velocity = Vector2.Lerp(NPC.velocity, Helper.FromAToB(NPC.Center, center.Center + new Vector2(105, -125).RotatedBy(center.rotation + MathHelper.ToRadians(AITimer * (5 + AITimer2 * 0.1f))), false) * 0.1f, 0.1f + AITimer * 0.03f);
                            AITimer++;
                            for (int i = 0; i < Main.maxNPCs; i++)
                            {
                                NPC npc = Main.npc[i];
                                if (npc.active && npc.type == NPCType<TerrorClingerMelee>())
                                {
                                    if (npc.Center.Distance(NPC.Center) < npc.width)
                                    {
                                        for (int j = 0; j < 30; j++)
                                        {
                                            Dust.NewDustPerfect(NPC.Center + Helper.FromAToB(NPC.Center, npc.Center) * npc.width / 2, DustID.CursedTorch, (Main.rand.NextBool(5) ? Main.rand.NextVector2Unit() : Helper.FromAToB(NPC.Center, npc.Center).RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(1, 5)), Scale: Main.rand.NextFloat(2));
                                        }
                                        if (AITimer2 > 30)
                                        {
                                            for (int j = -5 - phase2.ToInt(); j < 6 + phase2.ToInt(); j++)
                                            {
                                                if (!phase2 && j == 0) continue;
                                                MPUtils.NewProjectile(NPC.GetSource_FromAI(), npc.Center, Vector2.UnitY.RotatedBy(MathHelper.ToRadians(MathHelper.Lerp(-100, 100, (float)(j + 5) / 10))) * 10, ProjectileType<TFlameThrower3>(), 20, 0);
                                            }
                                        }
                                        else
                                        {
                                            for (int j = -3 - phase2.ToInt(); j < 4 + phase2.ToInt(); j++)
                                            {
                                                if (!phase2 && j == 0) continue;
                                                MPUtils.NewProjectile(NPC.GetSource_FromAI(), npc.Center, Vector2.UnitY.RotatedBy(j * 0.5f) * 10, ProjectileType<TFlameThrower3>(), 20, 0);
                                            }
                                        }
                                        SoundEngine.PlaySound(Sounds.fleshHit, npc.Center);
                                        NPC.velocity = Helper.FromAToB(NPC.Center, center.Center + new Vector2(85, 85).RotatedBy(center.rotation)) * 20;
                                        AITimer2 = AITimer2 > 30 ? 41 : 1;
                                        NPC.netUpdate = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            NPC.rotation = Utils.AngleLerp(NPC.rotation, center.rotation, 0.2f);
                            Vector2 pos = center.Center + new Vector2(85, 85).RotatedBy(center.rotation);
                            Vector2 target = pos;
                            Vector2 moveTo = target - NPC.Center;
                            NPC.velocity = Vector2.Lerp(NPC.velocity, (moveTo) * 0.05f, 0.1f);

                            AITimer2++;
                            if (AITimer2 > 30)
                            {
                                AITimer = 0;
                                NPC.netUpdate = true;
                            }
                        }
                        break;
                    case 5:
                        {
                            Vector2 pos = player.Center - new Vector2((float)Math.Sin(CenterAITimer * 0.05f) * 120, 250);
                            Vector2 target = pos;
                            Vector2 moveTo = target - NPC.Center;
                            NPC.velocity = (moveTo) * 0.05f;

                            NPC.rotation = Utils.AngleLerp(NPC.rotation, Helper.FromAToB(NPC.Center, player.Center).ToRotation() - MathHelper.PiOver2, 0.2f);
                            AITimer++;
                            if (AITimer > 100) AITimer = 0;
                            if (AITimer >= 25)
                            {
                                if (((float)(Math.Sin(CenterAITimer * 0.05f)) < -0.95f || (float)(Math.Sin(CenterAITimer * 0.05f)) > 0.95f))
                                {
                                    if (AITimer2 <= 0)
                                    {
                                        MPUtils.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Helper.FromAToB(NPC.Center, player.Center).RotatedByRandom(MathHelper.PiOver4 / 3) * 5, ProjectileType<TFlameThrower3>(), 25, 0);
                                        AITimer2 = 1;
                                        NPC.netUpdate = true;
                                    }
                                }
                                else AITimer2 = 0;
                            }
                        }
                        break;
                    case 12:
                        {
                            AITimer++;
                            if (AITimer < 80)
                            {
                                AITimer3 = 0;
                                if (AITimer > 30)
                                    NPC.velocity = Vector2.Lerp(NPC.velocity, -Vector2.UnitY.RotatedBy(PiOver4 * 0.3f) * 10, 0.02f);
                                else
                                {
                                    NPC.rotation = Utils.AngleLerp(NPC.rotation, center.rotation, 0.2f);
                                    Vector2 pos = center.Center + new Vector2(85, 85).RotatedBy(center.rotation);
                                    Vector2 target = pos;
                                    Vector2 moveTo = target - NPC.Center;
                                    NPC.velocity = (moveTo) * 0.1f;
                                }
                            }
                            else if (AITimer > 80 && AITimer3 < 4)
                            {

                                Vector2 to = Helper.TileRaycast.Cast(NPC.Center - new Vector2(0, 50), Vector2.UnitY, 800);
                                if (AITimer2 < 0.5f)
                                    NPC.velocity = Vector2.Lerp(NPC.velocity, Helper.FromAToB(NPC.Center, to) * 40, 0.05f);
                                AITimer2 = Lerp(AITimer2, 0, 0.1f);
                                if (NPC.Distance(to) < NPC.width && AITimer2 < 0.5f)
                                {
                                    SoundEngine.PlaySound(Sounds.eggplosion, NPC.Center);
                                    NPC.velocity = new Vector2(Main.rand.NextFloat(0, 50) * (Helper.FromAToB(NPC.Center, player.Center).X > 0 ? 1 : -1), -50); ;
                                    MPUtils.NewProjectile(null, NPC.Center, Vector2.Zero, ProjectileType<OstertagiExplosion>(), 0, 0);
                                    center.velocity = Helper.FromAToB(NPC.Center, center.Center) * 15;
                                    AITimer3++;
                                    for (int i = -2; i < 2; i++)
                                    {
                                        MPUtils.NewProjectile(null, NPC.Center, new Vector2(Main.rand.NextFloat(-1.25f, 1.25f), -1) * 5, ProjectileType<TFlameThrower4>(), 25, 0);
                                    }
                                    AITimer2 = 1;
                                    NPC.netUpdate = true;
                                }
                            }
                            else
                            {
                                NPC.rotation = Utils.AngleLerp(NPC.rotation, center.rotation, 0.2f);
                                Vector2 pos = center.Center + new Vector2(85, 85).RotatedBy(center.rotation);
                                Vector2 target = pos;
                                Vector2 moveTo = target - NPC.Center;
                                NPC.velocity = (moveTo) * 0.1f;
                            }
                        }
                        break;
                }
            }
            else
            {
                NPC.rotation = Utils.AngleLerp(NPC.rotation, center.rotation, 0.2f);
                Vector2 pos = center.Center + new Vector2(85, 85).RotatedBy(center.rotation);
                Vector2 target = pos;
                Vector2 moveTo = target - NPC.Center;
                NPC.velocity = (moveTo) * 0.1f;
            }
        }

    }
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 pos, Color drawColor)
    {
        NPC _center = Main.npc[(int)NPC.ai[0]];
        if (_center.Distance(NPC.Center) > 2000 || ((int)_center.ai[0] == 0 && _center.ai[1] < 2)) return true;
        Player player = Main.player[NPC.target];

        if (NPC.IsABestiaryIconDummy || NPC.Center == Vector2.Zero)
            return true;
        Vector2 neckOrigin = _center.Center;
        Vector2 center = NPC.Center;
        Vector2 distToProj = neckOrigin - NPC.Center;
        float projRotation = distToProj.ToRotation() - 1.57f;
        float distance = distToProj.Length();
        while (distance > 20 && !float.IsNaN(distance))
        {
            distToProj.Normalize();
            distToProj *= 20;
            center += distToProj;
            distToProj = neckOrigin - center;
            distance = distToProj.Length();

            //Draw chain

            if (new Rectangle((int)center.X, (int)center.Y, 5, 5).Intersects(new Rectangle((int)Main.screenPosition.X - 200, (int)Main.screenPosition.Y - 200, Main.screenWidth + 200, Main.screenHeight + 200)))
                spriteBatch.Draw(Helper.GetTexture(Helper.AssetPath+"NPCs/Terrortoma/ClingerChain").Value, center - pos,
                new Rectangle(0, 0, 26, 20), Lighting.GetColor((int)center.X / 16, (int)center.Y / 16), projRotation,
                new Vector2(26 * 0.5f, 20 * 0.5f), 1f, SpriteEffects.None, 0);
        }
        Texture2D tex = Assets.ExtraSprites.Terrortoma.TerrorClingerRanged_Bloom.Value;
        if (bloomAlpha > 0)
        {
            spriteBatch.Reload(BlendState.Additive);
            spriteBatch.Draw(tex, NPC.Center - Main.screenPosition, null, Color.LawnGreen * bloomAlpha, NPC.rotation, tex.Size() / 2 - new Vector2(0, 2).RotatedBy(NPC.rotation), NPC.scale * 1.05f, SpriteEffects.None, 0);
            spriteBatch.Reload(BlendState.AlphaBlend);
        }
        return true;

    }
}