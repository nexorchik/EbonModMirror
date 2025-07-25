using EbonianMod.Dusts;
using EbonianMod.Items.Pets.LilPilg;
using EbonianMod.NPCs.Overworld.Starine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

//this code is so fucking evil
namespace EbonianMod.NPCs.Overworld.Starine;
public class Starine_Skipper : ModNPC
{
    public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
    {
        NPC.lifeMax = (int)(NPC.lifeMax * bossAdjustment * balance);
    }
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Starine Skipper");
        Main.npcFrameCount[NPC.type] = 8;
        NPCID.Sets.TrailCacheLength[NPC.type] = 9;
        NPCID.Sets.TrailingMode[NPC.type] = 1;
        NPCID.Sets.NPCBestiaryDrawModifiers value = new()
        {
            Velocity = 1f
        };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
    }
    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
        {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement(NPC.BestiaryKey())
        });
    }
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        //3hi31mg
        var off = new Vector2(NPC.width / 2, NPC.height / 2 + 4 + NPC.gfxOffY);
        var clr = new Color(255, 255, 255, 255); // full white
        Texture2D texture = Assets.NPCs.Overworld.Starine.Starine_Skipper_Trail.Value;
        var frame = new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height);
        var orig = frame.Size() / 2f;
        var trailLength = NPCID.Sets.TrailCacheLength[NPC.type];
        for (int i = 1; i < trailLength; i++)
        {
            float scale = Lerp(1f, 0.95f, (float)(trailLength - i) / trailLength);
            var fadeMult = 1f / trailLength;
            SpriteEffects flipType = NPC.spriteDirection == -1 /* or 1, idfk */ ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(texture, NPC.oldPos[i] - screenPos + off, frame, clr * (1f - fadeMult * i), NPC.oldRot[i], orig, scale, flipType, 0f);
        }
        return true;
    }
    public override void SetDefaults()
    {
        NPC.width = 36;
        NPC.height = 40;
        NPC.aiStyle = -1;
        NPC.damage = 15;
        NPC.defense = 2;
        NPC.lifeMax = 80;
        NPC.HitSound = SoundID.NPCHit19;
        NPC.DeathSound = SoundID.NPCDeath1;
    }
    public override void DrawEffects(ref Color drawColor)
    {
        drawColor = Color.White;
    }
    public int f;
    public override void FindFrame(int frameHeight)
    {
        NPC.frameCounter++;
        if (NPC.IsABestiaryIconDummy)
        {
            if (NPC.frameCounter % 4 == 0)
            {
                f++;
            }
            if (f > 3)
                f = 0;
        }
        else
            switch (NPC.ai[0])
            {
                case 0:
                    if (NPC.Grounded())
                    {
                        int speed = 5;
                        if (NPC.ai[1] > 80)
                            speed = 2;
                        else if (NPC.ai[1] > 60)
                            speed = 3;
                        else if (NPC.ai[1] > 40)
                            speed = 4;
                        if (NPC.frameCounter % speed == 0)
                        {
                            if (f < 3 && NPC.ai[1] > NPC.ai[2] + 25)
                                f++;
                            else
                                f = 0;
                        }
                    }
                    else if (NPC.velocity.Y > 0)
                    {
                        f = 0;
                    }
                    else if (NPC.frameCounter % 5 == 0)
                    {
                        if (f < 4)
                            f = 4;

                        else if (f < 7)
                            f++;
                    }
                    break;
                case 1:
                    if (NPC.frameCounter % 5 == 0)
                    {
                        if (f < 7)
                            f++;
                        if (f < 4)
                            f = 4;
                    }
                    break;
            }
        NPC.frame.Y = f * frameHeight;
    }
    public bool HasStarineEnemies = NPC.AnyNPCs(ModContent.NPCType<Starine_Skipper>());
    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return HasStarineEnemies ? 0 : SpawnCondition.OverworldNight.Chance * .05f * Star.starfallBoost;
    }
    public override void HitEffect(NPC.HitInfo hit)
    {
        for (int i = 0; i < 4; i++)
        {
            int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<StarineDust>(), 2 * hit.HitDirection, -1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].scale = 1.5f;
        }
        if (NPC.life <= 0)
        {
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<StarineDust>(), 2 * hit.HitDirection, -1.5f);
                Main.dust[dust].scale = 2f;
                Main.dust[dust].noGravity = true;
            }
            if (Main.netMode == NetmodeID.Server)
                return;
            for (int i = 0; i < Main.rand.Next(3, 5); i++)
                Gore.NewGore(NPC.GetSource_OnHit(NPC), NPC.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), Vector2.Zero, ModContent.Find<ModGore>("EbonianMod/Starine").Type);
        }
    }
    public override void AI()
    {
        Lighting.AddLight(NPC.Center, new Vector3(.25f, .3f, .4f));
        Player target = Main.player[NPC.target];
        NPC.TargetClosest(false);
        Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY);
        if (NPC.Grounded())
        {
            NPC.spriteDirection = NPC.direction = target.Center.X > NPC.Center.X ? 1 : -1;
            NPC.ai[1]++;
            if (NPC.ai[1] > 100)
            {
                if (NPC.ai[0] == 0)
                {
                    NPC.velocity = new Vector2(Clamp(NPC.FromAToB(target, false).X / 30, -5, 5), -6f);
                    for (int i = 0; i < 3; i++)
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity.RotatedBy(ToRadians(i - 1) * 5) * Main.rand.NextFloat(0.5F, 1), ModContent.ProjectileType<Starine_Sparkle>(), 10, 1f);
                    for (int i = 0; i < 15; i++)
                    {
                        Dust.NewDustPerfect(NPC.Center + new Vector2(-NPC.direction * 10, NPC.height / 2), DustType<StarineDust>(), -NPC.velocity.RotatedByRandom(1) * Main.rand.NextFloat(0.25f));
                    }
                    NPC.netUpdate = true;
                    NPC.ai[0] = 1;
                }
            }
            else
            {
                NPC.velocity.X *= 0.9f;
                NPC.ai[0] = 0;
            }
        }
        else if (NPC.ai[0] == 1)
        {
            NPC.ai[1] = NPC.ai[2] = Main.rand.NextFloat(50);
            NPC.netUpdate = true;
        }

    }
}
public class Starine_Sightseer : ModNPC
{
    public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
    {
        NPC.lifeMax = (int)(NPC.lifeMax * bossAdjustment * balance);
    }
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = 4;
        NPCID.Sets.TrailCacheLength[NPC.type] = 25;
        NPCID.Sets.TrailingMode[NPC.type] = 1;
        NPCID.Sets.NPCBestiaryDrawModifiers value = new()
        {
            Velocity = 1f,
            Position = new Vector2(0, 0),
            PortraitPositionYOverride = 0,
            Direction = 1
        };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
    }
    public override void SetDefaults()
    {
        NPC.width = 38;
        NPC.height = 40;
        NPC.aiStyle = -1;
        NPC.damage = 0;
        NPC.defense = 3;
        NPC.lifeMax = 140;
        NPC.noTileCollide = true;
        NPC.noGravity = true;
        NPC.knockBackResist = 0.2f;
        NPC.HitSound = SoundID.NPCHit19;
        NPC.DeathSound = SoundID.NPCDeath1;
    }
    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
        {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement(NPC.BestiaryKey())
        });
    }
    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(ItemDropRule.Common(ItemType<LilPilgI>(), 60));
    }
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        //3hi31mg
        var off = new Vector2(NPC.width / 2, NPC.height / 2);
        var clr = new Color(255, 255, 255, 255); // full white
        Texture2D texture = Assets.NPCs.Overworld.Starine.Starine_Sightseer_Trail.Value;
        var frame = new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height);
        var orig = frame.Size() / 2f;
        var trailLength = NPCID.Sets.TrailCacheLength[NPC.type] / 2;

        for (int i = 1; i < trailLength; i++)
        {
            float scale = Lerp(0.9f, 1f, (float)(trailLength - i) / trailLength);
            var fadeMult = 1f / (trailLength * 2);
            SpriteEffects flipType = NPC.spriteDirection == -1 /* or 1, idfk */ ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Main.spriteBatch.Draw(texture, NPC.oldPos[i] - screenPos + off, frame, clr * (1f - fadeMult * i) * 0.05f, NPC.rotation, orig, scale, flipType, 0f);
        }
        return true;
    }
    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D tex = Assets.Extras.solidCone.Value;
        spriteBatch.Reload(BlendState.Additive);
        for (int i = -1; i < 2; i++)
        {
            float scale = SmoothStep(0.7f, 1, SolidConeAlpha);
            float offset = SmoothStep(-20, 30, SolidConeAlpha);
            float angle = (PiOver4 + i * (0.35f)) * NPC.direction;
            Vector2 off = new Vector2(0, -NPC.height + offset).RotatedBy(angle);
            for (int j = 0; j < 4; j++)
                spriteBatch.Draw(tex, NPC.Center + off - Main.screenPosition, null, new Color(0, 230, 230) * SolidConeAlpha, angle + PiOver2 * NPC.direction, new Vector2(0, tex.Height / 2), new Vector2(scale * 0.35f, 0.2f), NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }
        spriteBatch.Reload(BlendState.AlphaBlend);
    }
    public override void SendExtraAI(BinaryWriter writer)
    {
        for (int i = 0; i < AITimer.Length; i++)
            writer.Write(AITimer[i]);
        writer.WriteVector2(savedP);
    }
    public override void ReceiveExtraAI(BinaryReader reader)
    {
        for (int i = 0; i < AITimer.Length; i++)
            AITimer[i] = reader.ReadSingle();
        savedP = reader.ReadVector2();
    }
    public float AIState
    {
        get => NPC.ai[0];
        set => NPC.ai[0] = value;
    }
    public float[] AITimer = new float[6]; // ???????????????????????????????????????????????????
    public float InterestMeter
    {
        get => NPC.ai[1];
        set => NPC.ai[1] = value;
    }
    public float SolidConeAlpha = 0;
    public const int WanderAround = 0, Interested = 1, Angry = 2;
    public override bool PreAI()
    {
        NPC.oldVelocity = NPC.velocity;
        return base.PreAI();
    }
    public override void AI()
    {
        NPC.TargetClosest(false);
        Player player = Main.player[NPC.target];
        Lighting.AddLight(NPC.Center, new Vector3(.25f, .3f, .4f));
        if (player.Distance(NPC.Center) < 400)
            InterestMeter += SmoothStep(7, 1.5f, player.Distance(NPC.Center) / 400f) + Clamp(Lerp(0, 5, player.velocity.Length() / 15), 0, 1);

        if (InterestMeter > 0)
        {
            InterestMeter--;
            if (player.direction != NPC.direction)
                InterestMeter -= 3;
            if (player.Distance(NPC.Center) > 600)
                InterestMeter -= 15;
        }
        AITimer[0]++;
        if (AITimer[1] == 0 || AITimer[2] == 0)
        {
            AITimer[1] = Main.rand.Next(int.MaxValue);
            NPC.netUpdate = true;
        }
        UnifiedRandom rand = new UnifiedRandom((int)AITimer[1]);
        AITimer[2]++;
        if (NPC.life < NPC.lifeMax)
        {
            AIState = Angry;
        }
        if (SolidConeAlpha > 0)
            SolidConeAlpha = Lerp(SolidConeAlpha, 0, 0.1f);


        switch (AIState)
        {
            case WanderAround:
                NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.velocity.ToRotation() + (NPC.direction == -1 ? Pi : 0), 0.025f);
                if (NPC.direction == 0)
                {
                    NPC.direction = Main.rand.NextBool(2) ? 1 : -1;
                }
                float threshold = rand.Next(100, 200);
                if (AITimer[0] > threshold)
                {
                    if (rand.NextBool(2))
                    {
                        AITimer[1] = Main.rand.Next(int.MaxValue);
                        AITimer[0] = 0;
                    }
                    NPC.velocity *= 0.98f;
                    if (NPC.velocity.Length() < 0.25f)
                    {
                        NPC.direction = -NPC.direction;
                        AITimer[1] = Main.rand.Next(int.MaxValue);
                        AITimer[0] = 0;
                    }
                    NPC.netUpdate = true;
                }
                else
                {
                    NPC.spriteDirection = NPC.direction;
                    NPC.velocity.X = Lerp(NPC.velocity.X, NPC.direction * 1.5f, 0.05f);
                    Vector2 Target = Helper.TRay.Cast(NPC.Center, Vector2.UnitY, 400) - new Vector2(0, 220);
                    NPC.velocity.Y = Lerp(NPC.velocity.Y, NPC.Center.FromAToB(new Vector2(NPC.Center.X, (float)(Target.Y + Math.Cos(AITimer[2] / rand.NextFloat(50, 100)) * rand.NextFloat(5, 10)))).Y, rand.NextFloat(0.005f, 0.02f));
                }

                if (InterestMeter > 300)
                {
                    AIState = Interested;
                    NPC.direction = player.direction;
                    AITimer[1] = Main.rand.Next(int.MaxValue);
                    InterestMeter = 0;
                    AITimer[2] = 0;
                    AITimer[3] = 0;
                    NPC.netUpdate = true;
                }
                break;
            case Interested:
                {
                    int fac = rand.Next(60, 200) + (int)Clamp(MathF.Floor(InterestMeter), 100, 1000);
                    if (AITimer[2] < fac)
                    {
                        if (NPC.Center.X > player.Center.X)
                            NPC.direction = -1;
                        else if (NPC.Center.X < player.Center.X)
                            NPC.direction = 1;
                        NPC.spriteDirection = NPC.direction;
                        NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.FromAToB(player).ToRotation() + (NPC.direction == -1 ? Pi : 0), 0.025f);
                        NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.Center.FromAToB(player.Center - new Vector2(0, 85) + new Vector2(100 * -player.direction, 0)).RotatedBy(ToRadians(MathF.Sin(AITimer[2] / rand.NextFloat(15, 30)) * rand.NextFloat(40, 60)) * player.direction) * 1.5f, 0.02f);
                    }
                    else
                    {
                        AIState = WanderAround;
                        AITimer[1] = Main.rand.Next(int.MaxValue);
                        InterestMeter = -300;
                        AITimer[2] = 0;
                        NPC.netUpdate = true;
                    }
                }
                break;
            case Angry:
                {
                    switch (AITimer[5])
                    {
                        case 0:
                            AITimer[3]++;
                            NPC.damage = 0;
                            if (AITimer[3] < 200)
                            {
                                if (NPC.Center.X > player.Center.X)
                                    NPC.direction = -1;
                                else if (NPC.Center.X < player.Center.X)
                                    NPC.direction = 1;
                                NPC.spriteDirection = NPC.direction;
                                NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.FromAToB(player).ToRotation() + (NPC.direction == -1 ? Pi : 0), 0.025f);
                                Vector2 pos = player.Center + new Vector2(rand.NextFloat(175, 300) * (int)(rand.NextFloatDirection() > 0 ? 1 : -1), -rand.NextFloat(-50, 100));

                                NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.Center.FromAToB(pos) * 4.5f, 0.02f);
                                if (NPC.Center.Distance(pos) < 30 && AITimer[3] < 150)
                                    AITimer[3] = 150;
                            }
                            if (AITimer[3] > 200)
                                NPC.velocity *= 0.9f;
                            if (AITimer[3] >= 230)
                            {
                                AITimer[4] = Main.rand.NextBool() ? 9 : Main.rand.Next(4, 10 - (Main.rand.NextBool(20) ? 4 : 0));
                                AITimer[3] = 0;
                                AITimer[1] = Main.rand.Next(int.MaxValue);
                                AITimer[5]++;
                                NPC.netUpdate = true;
                            }
                            break;
                        case 1:
                            NPC.damage = 10 + (int)AITimer[4] * 2;
                            AITimer[3]++;
                            if (AITimer[3] == 3)
                            {
                                savedP = player.Center;
                                NPC.velocity = Helper.FromAToB(NPC.Center, player.Center) * 5;
                                NPC.netUpdate = true;
                            }
                            if (AITimer[3] < 10)
                            {
                                if (NPC.Center.X > player.Center.X)
                                    NPC.direction = -1;
                                else if (NPC.Center.X < player.Center.X)
                                    NPC.direction = 1;
                                NPC.spriteDirection = NPC.direction;
                                NPC.rotation = Utils.AngleLerp(NPC.rotation, Helper.FromAToB(NPC.Center, savedP).ToRotation() + (NPC.direction == -1 ? Pi : 0), 0.5f);
                            }
                            if (AITimer[3] == 10)
                                NPC.velocity = Helper.FromAToB(NPC.Center, player.Center) * 28;
                            if (AITimer[3] > 15)
                            {
                                AITimer[3] = 0;
                                AITimer[5]++;
                                NPC.netUpdate = true;
                            }
                            break;
                        case 2:
                            if (NPC.Center.X > player.Center.X)
                                NPC.direction = -1;
                            else if (NPC.Center.X < player.Center.X)
                                NPC.direction = 1;
                            NPC.spriteDirection = NPC.direction;
                            NPC.rotation = Utils.AngleLerp(NPC.rotation, Helper.FromAToB(NPC.Center, player.Center).ToRotation() + (NPC.direction == -1 ? Pi : 0), 0.05f);
                            NPC.velocity = NPC.velocity.RotatedBy(ToRadians(2.5f)) * 0.95f;
                            AITimer[3]++;
                            if (AITimer[3] > 60)
                            {
                                AITimer[4]++;
                                if (AITimer[4] < 10)
                                    AITimer[5] = 1;
                                else
                                    AITimer[5] = 0;
                                AITimer[3] = 0;
                                AITimer[1] = Main.rand.Next(int.MaxValue);
                                NPC.netUpdate = true;
                            }
                            break;
                    }
                    break;
                }
        }
    }
    Vector2 savedP;
    public override void HitEffect(NPC.HitInfo hit)
    {
        if (AIState != Angry)
        {
            NPC.velocity = Vector2.Zero;
            SolidConeAlpha = 1f;
            AIState = Angry;
            AITimer[1] = Main.rand.Next(int.MaxValue);
            InterestMeter = 0;
            AITimer[2] = 0;
            AITimer[3] = 0;
            NPC.netUpdate = true;
        }
        else
        {
            NPC.damage = 0;
            AITimer[5] = 0;
            AITimer[1] = Main.rand.Next(int.MaxValue);
            NPC.netUpdate = true;
        }
        for (int i = 0; i < 4; i++)
        {
            Dust dust = Dust.NewDustPerfect(Main.rand.NextVector2FromRectangle(NPC.getRect()), ModContent.DustType<StarineDust>(), Main.rand.NextVector2Circular(3, 3));
            dust.noGravity = true;
            dust.scale = 1.5f;
        }
        if (NPC.life <= 0)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustPerfect(Main.rand.NextVector2FromRectangle(NPC.getRect()), ModContent.DustType<StarineDust>(), Main.rand.NextVector2Circular(3, 3));
                dust.scale = 2f;
                dust.noGravity = true;
            }
            for (int i = 0; i < Main.rand.Next(20, 30); i++)
            {
                Gore.NewGore(NPC.GetSource_OnHit(NPC), Main.rand.NextVector2FromRectangle(NPC.getRect()), Main.rand.NextVector2Circular(1, 1), ModContent.Find<ModGore>("EbonianMod/Starine").Type, Main.rand.NextFloat(0.8f, 1.2f));
            }
        }
    }
    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return SpawnCondition.OverworldNight.Chance * .05f * Star.starfallBoost;
    }
    public override void DrawEffects(ref Color drawColor)
    {
        drawColor = Color.White;
    }
    public override void FindFrame(int frameHeight)
    {
        NPC.frameCounter++;
        if (NPC.frameCounter % (6 - AIState) == 0)
        {
            if (NPC.frame.Y < 3 * frameHeight)
                NPC.frame.Y += frameHeight;
            else
                NPC.frame.Y = 0;

        }
    }
}