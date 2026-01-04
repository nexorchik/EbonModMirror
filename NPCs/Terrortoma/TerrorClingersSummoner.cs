using EbonianMod.Dusts;
using EbonianMod.Items.Misc;
using EbonianMod.NPCs.Corruption;
using EbonianMod.NPCs.Corruption.Ebonflies;
using EbonianMod.Projectiles.Friendly.Corruption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.NPCs.Terrortoma;
public class TerrorClingerSummoner : TerrorClingerGeneric // Disgusting
{
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
        NPC.width = 58;
        NPC.height = 62;
    }
    public override void DrawBehind(int index) => Main.instance.DrawCacheNPCsBehindNonSolidTiles.Add(index);
    public override void AI()
    {
        Player player = Main.player[NPC.target];

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
            AITimer = 0;
            Vector2 pos = center.Center + new Vector2(-80, 80).RotatedBy(center.rotation);
            Vector2 target = pos;
            Vector2 moveTo = target - NPC.Center;
            NPC.velocity = (moveTo) * 0.09f;
            if ((int)center.ai[1] == 150)
            {
                NPC.life = 0;
                NPC.checkDead();
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

                CameraSystem.ScreenShakeAmount += 5f;
                MPUtils.NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ProjectileType<OstertagiExplosion>(), 0, 0, 0);
                for (int i = 0; i < 5; i++)
                {
                    if (!Main.dedServ)
                    {
                        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Find<ModGore>("EbonianMod/EbonFlyGore").Type, NPC.scale);
                        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Find<ModGore>("EbonianMod/EbonFlyGore2").Type, NPC.scale);
                        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Find<ModGore>("EbonianMod/EbonFlyGore3").Type, NPC.scale);
                    }
                    Projectile a = MPUtils.NewProjectile(NPC.GetSource_Death(), NPC.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(5, 7), ProjectileType<OstertagiWorm>(), 20, 0, 0);
                    a.SetToHostile();
                }
            }
        }
        if ((int)center.ai[2] == 1 && NPC.ai[3] == 0)
        {
            if ((int)center.ai[0] != 15)
                bloomAlpha = 1f;
            NPC.ai[3] = 1;
        }
        if (bloomAlpha > 0f) bloomAlpha -= 0.025f;
        if (AIState == -2)
        {
            if (CenterAITimer % 120 == 0)
            {
                MPUtils.NewNPC(NPC.Center + Main.rand.NextVector2Circular(NPC.width / 2, NPC.height / 2), NPCType<BloatedEbonfly>(), ai3: -3);
            }
        }
        if (((int)center.ai[2] != 1 && (int)center.ai[2] <= 2) || (int)center.ai[2] == 4)
        {
            Vector2 pos = center.Center + new Vector2(-85, 85).RotatedBy(center.rotation);
            if (AIState == 6 || AIState == 14)
            {
                pos = center.Center - new Vector2(-85, 85).RotatedBy(center.rotation);
            }
            Vector2 target = pos;
            Vector2 moveTo = target - NPC.Center;
            NPC.velocity = (moveTo) * lerpSpeed;
            NPC.ai[3] = 0;
        }
        else
        {
            if (AIState == 0 || AIState == 1 || AIState == 12 || AIState == 9 || AIState == 14 || AIState == 10 || AIState == 13 || AIState == 11 || AIState == -2 || AIState == 4 || AIState == 999 || AIState == 5)
            {
                Vector2 pos = center.Center + new Vector2(-85, 85).RotatedBy(center.rotation);
                Vector2 target = pos;
                Vector2 moveTo = target - NPC.Center;
                NPC.velocity = (moveTo) * lerpSpeed;
                NPC.scale = Lerp(NPC.scale, 1, 0.1f);
            }
            if ((int)center.ai[2] != 4)
            {
                switch (AIState)
                {
                    case 2:
                        if (CenterAITimer <= 300)
                        {
                            Vector2 pos = center.Center + new Vector2(-85, 85).RotatedBy(MathHelper.ToRadians(AITimer * 2));
                            Vector2 target = pos;
                            Vector2 moveTo = target - NPC.Center;
                            NPC.velocity = (moveTo) * 0.05f;

                            AITimer++;
                            if (AITimer % 25 == 0)
                            {
                                Projectile a = MPUtils.NewProjectile(NPC.GetSource_Death(), NPC.Center, Helper.FromAToB(NPC.Center, player.Center) * Main.rand.NextFloat(5, 7), ProjectileType<OstertagiWorm>(), 20, 0, 0);
                                a.SetToHostile();
                            }
                            if (AITimer == 50)
                            {
                                MPUtils.NewNPC(NPC.Center + Main.rand.NextVector2Circular(NPC.width / 2, NPC.height / 2), NPCType<BloatedEbonfly>(), ai3: -3);
                                //float angle = Helper.CircleDividedEqually(i, 6) + off;
                                //MPUtils.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.One.RotatedBy(angle), ProjectileType<TSpike>(), 15, 0);
                            }
                            if (AITimer == 80)
                            {
                                MPUtils.NewNPC(NPC.Center + Main.rand.NextVector2Circular(NPC.width / 2, NPC.height / 2), NPCType<BloatedEbonfly>(), ai3: -3);
                                //float angle = Helper.CircleDividedEqually(i, 8) + off;
                                //MPUtils.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.One.RotatedBy(angle), ProjectileType<TSpike>(), 15, 0);
                            }
                            if (AITimer >= 100)
                            {
                                lerpSpeed = 0.005f;
                                center.ai[2] = 4;
                                AITimer = 0;
                                AITimer2 = 0;
                                center.netUpdate = true;
                            }
                        }
                        break;

                    case 3:
                        if (AITimer2 == 0 || (AITimer2 > 30 && AITimer2 < 40))
                        {
                            NPC.velocity = Vector2.Lerp(NPC.velocity, Helper.FromAToB(NPC.Center, center.Center + new Vector2(-105, -125).RotatedBy(center.rotation + MathHelper.ToRadians(-AITimer * (5 + AITimer2 * 0.1f))), false) * 0.1f, 0.1f + AITimer * 0.03f);
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
                                        NPC.velocity = Helper.FromAToB(NPC.Center, center.Center + new Vector2(-85, 85).RotatedBy(center.rotation)) * 20;
                                        AITimer2 = AITimer2 > 30 ? 41 : 1;
                                        NPC.netUpdate = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            NPC.rotation = Utils.AngleLerp(NPC.rotation, center.rotation, 0.2f);
                            Vector2 pos = center.Center + new Vector2(-85, 85).RotatedBy(center.rotation);
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
                    case 15:
                        {
                            AITimer++;
                            if (AITimer < 100)
                            {
                                if (AITimer < 85)
                                {
                                    Vector2 pos = NPC.Center + new Vector2(0, 300).RotatedByRandom(TwoPi);
                                    Dust.NewDustPerfect(pos, DustType<LineDustFollowPoint>(), Helper.FromAToB(pos, NPC.Center) * Main.rand.NextFloat(4, 10), newColor: Color.LawnGreen, Scale: Main.rand.NextFloat(0.06f, 0.2f)).customData = NPC.Center + new Vector2(0, 20);

                                    Dust.NewDustPerfect(pos, DustID.CursedTorch, Helper.FromAToB(pos, NPC.Center) * Main.rand.NextFloat(4, 10)).noGravity = true;
                                }
                                if (AITimer > 50)
                                    NPC.Center = savedP + Main.rand.NextVector2Circular(30, 30) * (NPC.scale - 1);
                                NPC.scale = Lerp(NPC.scale, 2, 0.01f);

                                if (AITimer <= 50 && AITimer > 48)
                                {
                                    savedP = NPC.Center;
                                    NPC.netUpdate = true;
                                }

                                if (AITimer < 50)
                                {
                                    Vector2 pos = center.Center + new Vector2(-150, -85).RotatedBy(center.rotation);
                                    Vector2 target = pos;
                                    Vector2 moveTo = target - NPC.Center;
                                    NPC.velocity = (moveTo) * 0.05f;
                                }
                                else
                                    NPC.velocity *= 0.8f;
                            }
                            else
                            {
                                NPC.velocity *= 0.8f;
                                if (AITimer < 107)
                                    NPC.scale = Lerp(NPC.scale, 0.5f, 0.5f);
                                else
                                    NPC.scale = Lerp(NPC.scale, 1f, 0.1f);
                            }
                            if (AITimer == 100)
                            {
                                MPUtils.NewProjectile(null, NPC.Center, Vector2.Zero, ProjectileType<OstertagiExplosion>(), 0, 0);

                                for (int i = 0; i < 5; i++)
                                {
                                    float angle = Helper.CircleDividedEqually(i, 5);
                                    Projectile a = MPUtils.NewProjectile(NPC.GetSource_Death(), NPC.Center, angle.ToRotationVector2() * Main.rand.NextFloat(5, 7), ProjectileType<OstertagiWorm>(), 24, 0, 0);
                                    a.SetToHostile();
                                }

                                for (int i = 0; i < 5; i++)
                                {
                                    float angle = Helper.CircleDividedEqually(i, 5);
                                    MPUtils.NewNPC(NPC.Center, NPCType<Regorger>(), ai3: 1);
                                }
                            }
                        }
                        break;
                }
            }
            else
            {
                Vector2 pos = center.Center + new Vector2(-85, 85).RotatedBy(center.rotation);
                Vector2 target = pos;
                Vector2 moveTo = target - NPC.Center;
                NPC.velocity = (moveTo) * 0.05f;
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
                spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Terrortoma/ClingerChain").Value, center - pos,
                    new Rectangle(0, 0, 26, 20), Lighting.GetColor((int)center.X / 16, (int)center.Y / 16), projRotation,
                    new Vector2(26 * 0.5f, 20 * 0.5f), 1f, SpriteEffects.None, 0);
        }
        Texture2D tex = Assets.ExtraSprites.Terrortoma.TerrorClingerSummoner_Bloom.Value;
        if (bloomAlpha > 0)
        {
            spriteBatch.Reload(BlendState.Additive);
            spriteBatch.Draw(tex, NPC.Center - Main.screenPosition, null, Color.LawnGreen * bloomAlpha, NPC.rotation, tex.Size() / 2, NPC.scale * 1.05f, SpriteEffects.None, 0);
            spriteBatch.Reload(BlendState.AlphaBlend);
        }
        return true;

    }
}