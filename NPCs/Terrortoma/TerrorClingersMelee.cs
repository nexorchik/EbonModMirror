using EbonianMod.Items.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Bestiary;

namespace EbonianMod.NPCs.Terrortoma;

public class TerrorClingerMelee : TerrorClingerGeneric // Disgusting
{
    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[NPCType<Terrortoma>()], quickUnlock: true);
        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
            new FlavorTextBestiaryInfoElement("Mods.EbonianMod.Misc.Types.Cecitoma"),
            new FlavorTextBestiaryInfoElement("Mods.EbonianMod.NPCs.TerrorClingerMelee.Bestiary"),
        });
    }
    public override void SetStaticDefaults()
    {

        NPCID.Sets.MustAlwaysDraw[Type] = true;
        var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
        {
            CustomTexturePath = "EbonianMod/NPCs/Terrortoma/TerrorClinger_Bestiary",
            Position = new Vector2(2f, -45f),
            PortraitPositionXOverride = 0f,
            PortraitPositionYOverride = -45f
        };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        NPCID.Sets.TrailCacheLength[NPC.type] = 4;
        NPCID.Sets.TrailingMode[NPC.type] = 0;
    }
    public override void SetDefaults()
    {
        base.SetDefaults();
        NPC.damage = 40;
        NPC.width = 58;
        NPC.height = 46;
        NPC.behindTiles = true;
    }
    Vector2 lastPos;
    private bool IsDashing = false;
    float alpha;
    Vector2 savedP2;
    public override void SendExtraAI(BinaryWriter writer)
    {
        base.SendExtraAI(writer);
        writer.WriteVector2(lastPos);
        writer.WriteVector2(savedP2);
        writer.Write(IsDashing);
    }
    public override void ReceiveExtraAI(BinaryReader reader)
    {
        lastPos = reader.ReadVector2();
        savedP2 = reader.ReadVector2();
        IsDashing = reader.ReadBoolean();
    }
    public override void OnSpawn(IEntitySource source)
    {
        NPC center = Main.npc[(int)NPC.ai[0]];
        if (NPC.Center == Vector2.Zero && center.Center != Vector2.Zero)
            NPC.Center = center.Center;
        NPC.netUpdate = true; // TEST
    }
    public override void AI()
    {
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
        Player player = Main.player[NPC.target];
        NPC.TargetClosest(false);

        if (!player.active || player.dead || (int)center.ai[0] == -12124)
        {
            NPC.TargetClosest(false);
            player = Main.player[NPC.target];
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
            AITimer = (int)(int)center.ai[1];

        NPC.damage = (int)center.localAI[0];
        lerpSpeed = Clamp(Lerp(lerpSpeed, ((int)center.ai[0] == 0 ? 0.05f : 0.15f), 0.1f), 0, 0.15f);
        int AIState = (int)(int)center.ai[0];
        bool phase2 = center.life <= center.lifeMax - center.lifeMax / 3 + 3500;
        int CenterAITimer = (int)(int)center.ai[1];
        if ((int)center.ai[0] == -1)
        {
            IsDashing = false;
            if ((int)center.ai[1] > 100)
            {
                NPC.velocity *= 1.025f;
                NPC.rotation += MathHelper.ToRadians(3);
            }
            else
            {
                Vector2 toPlayer = player.Center - NPC.Center;
                NPC.rotation = toPlayer.ToRotation() - MathHelper.PiOver2;
                Vector2 pos = center.Center + new Vector2(0, 80).RotatedBy(center.rotation);
                Vector2 target = pos;
                Vector2 moveTo = target - NPC.Center;
                NPC.velocity = moveTo * 0.09f;
            }
            if ((int)center.ai[1] == 100)
            {
                NPC.netUpdate = true;
                NPC.velocity = Vector2.UnitY * 5;
                CameraSystem.ScreenShakeAmount += 5f;
                MPUtils.NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ProjectileType<OstertagiExplosion>(), 0, 0, 0);
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
            }
            if ((int)center.ai[1] == 350)
            {
                NPC.life = 0;
                NPC.checkDead();
                if (!Main.dedServ)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Find<ModGore>("EbonianMod/EbonFlyGore").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Find<ModGore>("EbonianMod/EbonFlyGore2").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Find<ModGore>("EbonianMod/EbonFlyGore3").Type, NPC.scale);
                }
                NPC.netUpdate = true;
                for (int i = 0; i < 10; i++)
                {
                    Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.CursedTorch, Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
                }
            }
        }
        else
        {
            if ((int)center.ai[2] == 2 && NPC.ai[3] == 0)
            {
                bloomAlpha = 1f;
                NPC.ai[3] = 1;
            }
            if (bloomAlpha > 0f) bloomAlpha -= 0.025f;
            if (alpha > 0f) alpha -= 0.01f;
            if (((int)center.ai[2] != 2 && (int)center.ai[2] <= 2) || (int)center.ai[2] == 4)
            {
                alpha = 0;
                NPC.rotation = Utils.AngleLerp(NPC.rotation, center.rotation, 0.2f);
                Vector2 pos = center.Center + new Vector2(0, 105).RotatedBy(center.rotation);
                if (AIState == 6 || AIState == 14)
                {
                    pos = center.Center - new Vector2(0, 85).RotatedBy(center.rotation);
                }
                Vector2 moveTo = NPC.FromAToB(pos, false);
                NPC.velocity = moveTo * lerpSpeed;
                NPC.ai[3] = 0;
            }
            else
            {
                if (AIState == 0 || AIState == 1 || AIState == 15 || AIState == 12 || AIState == 9 || AIState == 14 || AIState == 10 || AIState == 13 || AIState == 11 || AIState == -2 || AIState == 4 || AIState == 999)
                {
                    NPC.rotation = Utils.AngleLerp(NPC.rotation, center.rotation, 0.2f);
                    Vector2 pos = center.Center + new Vector2(0, 105).RotatedBy(center.rotation);
                    Vector2 moveTo = NPC.FromAToB(pos, false);
                    NPC.velocity = moveTo * lerpSpeed;
                }
                if ((int)center.ai[2] != 4)
                {
                    switch (AIState)
                    {
                        case 2:
                            if (CenterAITimer <= 300)
                            {
                                NetUpdateAtSpecificTime(45, 50, 65, 75, 100);
                                NPC.damage = 100;
                                AITimer++;
                                if (AITimer < 75 && AITimer > 30)
                                {
                                    NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.Center.FromAToB(player.Center).ToRotation() - MathHelper.PiOver2, 0.1f);
                                    if (AITimer < 45)
                                    {
                                        savedP2 = Helper.FromAToB(center.Center, NPC.Center, false);
                                        NPC.velocity -= Helper.FromAToB(NPC.Center, lastPos) * 1.6f;
                                    }
                                    else
                                    {
                                        NPC.velocity *= 0.8f;
                                        NPC.Center = center.Center + savedP2 + Main.rand.NextVector2Circular(SmoothStep(0, AITimer - 45, (AITimer - 45) / 30) * 0.9f, SmoothStep(0, AITimer - 45, (AITimer - 45) / 30) * 0.9f);
                                    }
                                }
                                if (AITimer < 65)
                                {
                                    lastPos = player.Center + player.velocity * 4;
                                }
                                if (AITimer == 50)
                                {
                                    NPC.velocity = Vector2.Zero;
                                    alpha = 1f;
                                }
                                if (AITimer == 75)
                                {
                                    NPC.velocity = Helper.FromAToB(NPC.Center, lastPos) * 35;
                                }
                                if (AITimer > 75)
                                {
                                    if (AITimer < 95)
                                        NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.velocity.ToRotation() - MathHelper.PiOver2, 0.1f);
                                    //NPC.Center += NPC.velocity * 0.75f;
                                    if (NPC.Center.Distance(lastPos) < NPC.width * 0.75f && AITimer < 90)
                                    {
                                        AITimer = 90;
                                    }
                                }
                                if (AITimer == 95)
                                    MPUtils.NewProjectile(null, NPC.Center + NPC.velocity, Vector2.Zero, ProjectileType<OstertagiExplosion>(), 0, 0);
                                if (AITimer > 95 || AITimer < 50)
                                {
                                    NPC.rotation = Utils.AngleLerp(NPC.rotation, 0, 0.2f);
                                    Vector2 pos = center.Center + new Vector2(0, 105).RotatedBy(center.rotation);
                                    Vector2 target = pos;
                                    Vector2 moveTo = target - NPC.Center;
                                    NPC.velocity = moveTo * 0.05f;
                                }
                                if (AITimer >= 100)
                                {
                                    lerpSpeed = 0.005f;
                                    center.ai[2] = 4;
                                    AITimer = 0;
                                    center.netUpdate = true;
                                }
                            }
                            break;
                        case 3:
                            {
                                NPC.damage = 100;
                                NPC.rotation = Utils.AngleLerp(NPC.rotation, 0, 0.2f);
                                Vector2 pos = center.Center + new Vector2(0, 125).RotatedBy(center.rotation);
                                Vector2 moveTo = NPC.FromAToB(pos, false);
                                NPC.velocity = moveTo * 0.1f;
                            }
                            break;
                        case 5:
                            {
                                if (CenterAITimer < 40)
                                    NPC.damage = 0;
                                else NPC.damage = 100;
                                if (CenterAITimer == 41)
                                    bloomAlpha = 1f;
                                NPC.Center = Vector2.Lerp(NPC.Center, center.Center + new Vector2(0, Helper.TRay.CastLength(center.Center, Vector2.UnitY, 360)).RotatedBy((float)Math.Sin((float)Main.GlobalTimeWrappedHourly * 2)), 0.1f);
                                NPC.rotation = Helper.FromAToB(NPC.Center, center.Center + new Vector2(0, 340).RotatedBy((float)Math.Sin((float)Main.GlobalTimeWrappedHourly * 2))).ToRotation();
                                if (CenterAITimer > 369 + (center.life < center.lifeMax / 2 ? 50 : 0))
                                    NPC.damage = 0;
                            }
                            break;
                        case 7:
                            NetUpdateAtSpecificTime(30);
                            AITimer++;
                            if (AITimer == 1)
                                bloomAlpha = 1f;
                            if (AITimer == 30)
                            {
                                NPC.velocity = Vector2.Zero;
                                alpha = 1f;
                                lastPos = Helper.TRay.Cast(NPC.Center, Vector2.Clamp(Helper.FromAToB(NPC.Center, player.Center), new Vector2(-0.45f, 1), new Vector2(0.45f, 1)), 2028);
                            }
                            if (AITimer > 30 && AITimer < 100)
                            {
                                NPC.velocity += Helper.FromAToB(NPC.Center, lastPos, false) / 50f;
                                if (NPC.Center.Distance(lastPos) < NPC.width)
                                {
                                    AITimer = 100;
                                    NPC.netUpdate = true;
                                }
                            }
                            else NPC.velocity *= 0.1f;
                            break;
                    }
                }
                else
                {
                    NPC.rotation = Utils.AngleLerp(NPC.rotation, 0, 0.2f);
                    Vector2 pos = center.Center + new Vector2(0, 105).RotatedBy(center.rotation);
                    Vector2 moveTo = NPC.FromAToB(pos, false);
                    NPC.velocity = moveTo * 0.05f;
                }
            }
        }
    }
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 pos, Color drawColor)
    {
        NPC _center = Main.npc[(int)NPC.ai[0]];
        if (_center.Distance(NPC.Center) > 2000 || ((int)_center.ai[0] == 0 && (int)_center.ai[1] < 2)) return true;
        Player player = Main.player[NPC.target];

        if (NPC.IsABestiaryIconDummy || NPC.Center == Vector2.Zero)
            return true;
        Vector2 drawOrigin = new Vector2(TextureAssets.Npc[Type].Value.Width * 0.5f, NPC.height * 0.5f);
        if (IsDashing)
        {
            for (int k = 0; k < NPC.oldPos.Length; k++)
            {
                Vector2 drawPos = NPC.oldPos[k] - pos + drawOrigin + new Vector2(0, NPC.gfxOffY);
                spriteBatch.Draw(TextureAssets.Npc[Type].Value, drawPos, NPC.frame, Color.White * 0.5f, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0);
            }
        }
        if (Main.npc[(int)NPC.ai[0]].ai[0] == -1 && Main.npc[(int)NPC.ai[0]].ai[1] > 100)
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
        Texture2D tex = Assets.ExtraSprites.Terrortoma.TerrorClingerMelee_Bloom.Value;
        if (bloomAlpha > 0)
        {
            spriteBatch.Reload(BlendState.Additive);
            spriteBatch.Draw(tex, NPC.Center - Main.screenPosition, null, Color.LawnGreen * bloomAlpha, NPC.rotation, tex.Size() / 2 - new Vector2(0, 2).RotatedBy(NPC.rotation), NPC.scale * 1.05f, SpriteEffects.None, 0);
            spriteBatch.Reload(BlendState.AlphaBlend);
        }
        return true;
    }
}
