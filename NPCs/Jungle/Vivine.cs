using EbonianMod.Dusts;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using EbonianMod.Items.Weapons.Ranged;
using EbonianMod.Common.Globals;
using EbonianMod.Projectiles.Enemy.Jungle;

namespace EbonianMod.NPCs.Jungle;
public class Vivine : ModNPC
{
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = 10;
    }
    public override void SetDefaults()
    {
        NPC.width = 40;
        NPC.height = 58;
        NPC.lifeMax = 110;
        NPC.defense = 5;
        NPC.damage = 15;
        NPC.knockBackResist = 0.8f;
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
        NPC.aiStyle = -1;
    }
    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Spitvine>(), 50));
        npcLoot.Add(ItemDropRule.Common(ItemID.Vine, 3));
    }
    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
        {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle,
                new FlavorTextBestiaryInfoElement("A type of tripod carnivorous flower native to the jungle's moist caverns. Spits a pink corrosive substance at prey, approaching only to slurp up the pre-digested slurry.")
        });
    }
    public float AIState
    {
        get => NPC.ai[0];
        set => NPC.ai[0] = value;
    }
    public float AITimer
    {
        get => NPC.ai[1];
        set => NPC.ai[1] = value;
    }
    public float AITimer2
    {
        get => NPC.ai[2];
        set => NPC.ai[2] = value;
    }
    public float AITimer3
    {
        get => NPC.ai[3];
        set => NPC.ai[3] = value;
    }
    const int Idle = 0, Move = 1, Spit = 2;
    public override void FindFrame(int frameHeight)
    {
        if (!NPC.IsABestiaryIconDummy)
        {
            if (NPC.velocity.Y > .1f || NPC.velocity.Y < -.1f)
            {
                NPC.frameCounter = 1;
                NPC.frame.Y = 4 * NPC.height;
            }
            if (AIState == Idle)
                NPC.frame.Y = 0;
            else if (AIState == Move)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter % 5 == 0)
                {
                    if (NPC.frame.Y < 4 * NPC.height)
                        NPC.frame.Y += NPC.height;
                    else
                        NPC.frame.Y = 1 * NPC.height;
                }
            }
            else if (AIState == Spit)
            {
                if (NPC.frame.Y < 5 * NPC.height)
                    NPC.frame.Y = 5 * NPC.height;
                NPC.frameCounter++;
                if (NPC.frameCounter % 5 == 0)
                {
                    if (NPC.frame.Y < 9 * NPC.height)
                        NPC.frame.Y += NPC.height;
                    else
                    {
                        AITimer = 0;
                        AIState = Idle;
                    }
                }
            }
        }
        else
        {
            NPC.frameCounter++;
            if (NPC.frameCounter % 5 == 0)
            {
                if (NPC.frame.Y < 4 * NPC.height)
                    NPC.frame.Y += NPC.height;
                else
                    NPC.frame.Y = 1 * NPC.height;
            }
        }
    }
    public override void HitEffect(NPC.HitInfo hit)
    {
        Helper.SpawnDust(NPC.position, NPC.Size, DustID.Grass, new Vector2(2 * hit.HitDirection, -1.5f), 2);
        Helper.SpawnDust(NPC.position, NPC.Size, ModContent.DustType<JunglePinkDust>(), Vector2.One * hit.HitDirection * 2, 2);
        if (NPC.life <= 0)
        {
            Helper.SpawnDust(NPC.position, NPC.Size, DustID.Grass, new Vector2(2 * hit.HitDirection, -1.5f), 8);
            Helper.SpawnGore(NPC, "EbonianMod/Vivine", 2, 1, Vector2.One * hit.HitDirection * 2);
            Helper.SpawnGore(NPC, "EbonianMod/Vivine", 1, 2, Vector2.One * hit.HitDirection * 2);
            Helper.SpawnGore(NPC, "EbonianMod/Vivine", 3, 3, Vector2.One * hit.HitDirection * 2);
            Helper.SpawnDust(NPC.position, NPC.Size, ModContent.DustType<JunglePinkDust>(), Vector2.One * hit.HitDirection * 2, 8);
        }
    }
    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        if (Main.invasionType > 0) return 0;
        return SpawnCondition.UndergroundJungle.Chance * 0.2f;
    }
    public override bool? CanFallThroughPlatforms()
    {
        Player player = Main.player[NPC.target];
        return player.Center.Y > NPC.Center.Y;
    }
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        var drawPos = NPC.Center - screenPos;
        var texture = Assets.NPCs.Jungle.Vivine_Glow.Value;
        var origTexture = TextureAssets.Npc[NPC.type].Value;
        var frame = new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height);
        var orig = frame.Size() / 2f - new Vector2(0, 3);
        Main.spriteBatch.Draw(origTexture, drawPos, frame, drawColor, NPC.rotation, orig, NPC.scale, NPC.direction < 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
        Main.spriteBatch.Draw(texture, drawPos, frame, Color.White, NPC.rotation, orig, NPC.scale, NPC.direction < 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
        return false;
    }
    public override void AI()
    {
        Lighting.AddLight(NPC.Center, new Vector3(.19f, .08f, .11f));
        NPC.TargetClosest();
        Player player = Main.player[NPC.target];
        NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;
        NPC.spriteDirection = NPC.direction;
        if (AIState == Idle)
        {
            NPC.velocity.X *= .9f;
            NPC.knockBackResist = 0.8f;
            AITimer++;
            if (AITimer >= 50)
            {
                AITimer = 0;
                AIState = Move;
                NPC.frame.Y = NPC.height * 2;
            }
        }
        else if (AIState == Move)
        {
            var Distance = Vector2.Distance(player.Center, NPC.Center);
            NPC.knockBackResist = 0.8f;
            AITimer++;
            NPC.GetGlobalNPC<FighterGlobalAI>().FighterAI(NPC, 6, 1, true, -1, 0);
            Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY);
            if (AITimer >= 200 && Distance < 500 && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
            {
                AITimer = 0;
                AIState = Spit;
                NPC.frame.Y = NPC.height * 5;
                NPC.velocity = Vector2.Zero;
            }
        }
        else if (AIState == Spit)
        {
            NPC.velocity.X *= 0;
            NPC.knockBackResist = 0f;
            if (NPC.frame.Y == 9 * NPC.height && AITimer == 0)
            {
                AITimer = 1;
                SoundEngine.PlaySound(SoundID.Item17);
                MPUtils.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(1, 14), Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * 20f, ProjectileType<VivineSpit>(), 15, 0);
            }
        }
    }
}
