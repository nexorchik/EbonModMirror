using EbonianMod.Items.Misc;
using EbonianMod.Items.Tiles;
using System.IO;
using Terraria.GameContent.Bestiary;

namespace EbonianMod.NPCs.Corruption.Ebonflies;

public class BloatedEbonfly : ModNPC
{
    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(ItemDropRule.Common(ItemType<WaspPaintingI>(), 200));
    }
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = 6;
    }
    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
            new FlavorTextBestiaryInfoElement("Mods.EbonianMod.Misc.Types.Evil"),
            new FlavorTextBestiaryInfoElement("Mods.EbonianMod.NPCs.BloatedEbonfly.Bestiary"),
        });
    }
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {

        Texture2D tex = Assets.ExtraSprites.Corruption.BloatedEbonfly_Glow.Value;
        Texture2D tex2 = TextureAssets.Npc[Type].Value;
        Texture2D tex3 = Assets.ExtraSprites.Corruption.BloatedEbonfly_Glow2.Value;
        SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        Main.EntitySpriteDraw(tex2, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
        Main.EntitySpriteDraw(tex, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
        Main.spriteBatch.Reload(BlendState.Additive);
        Main.EntitySpriteDraw(tex3, NPC.Center - screenPos, NPC.frame, Color.LawnGreen * glowAlpha, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
        Main.spriteBatch.Reload(BlendState.AlphaBlend);
        return false;
    }
    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        if (spawnInfo.Player.ZoneCorrupt)
        {
            return .14f;
        }
        else
        {
            return 0;
        }
    }

    public override void SetDefaults()
    {
        NPC.aiStyle = 5;
        AIType = 205;
        NPC.width = 40;
        NPC.height = 38;
        NPC.npcSlots = 0.1f;
        NPC.lifeMax = 200;
        NPC.damage = 0;
        NPC.lavaImmune = true;
        NPC.noGravity = true;
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
        NPC.buffImmune[24] = true;
        NPC.noTileCollide = false;
        NPC.defense = 0;
    }
    public override void FindFrame(int frameHeight)
    {
        NPC.frameCounter++;
        if (NPC.frameCounter % 5 == 0)
        {
            if (NPC.frame.Y < frameHeight * 5)
                NPC.frame.Y += frameHeight;
            else
                NPC.frame.Y = 0;
        }
    }
    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.Write(NPC.scale);
    }
    public override void ReceiveExtraAI(BinaryReader reader)
    {
        NPC.scale = reader.ReadSingle();
    }
    public override void OnSpawn(IEntitySource source)
    {
        NPC.scale = Main.rand.NextFloat(0.8f, 1.2f);
        NPC.Center += Main.rand.NextVector2CircularEdge(40, 40);
        NPC.velocity = Main.rand.NextVector2Unit();
        if (NPC.ai[3] < -2)
            NPC.dontTakeDamage = true;
        NPC.netUpdate = true; // TEST
    }
    float glowAlpha = 0;
    Vector2 lastPos;
    float AITimer;
    public override void PostAI()
    {
        foreach (NPC npc in Main.ActiveNPCs)
        {
            if (npc.active && npc.whoAmI != NPC.whoAmI)
            {
                if (npc.Center.Distance(NPC.Center) < npc.width * npc.scale)
                {
                    NPC.velocity += NPC.Center.FromAToB(npc.Center, true, true) * 0.5f;
                    NPC.netUpdate = true;
                }
                if (npc.Center == NPC.Center)
                {
                    NPC.velocity = Main.rand.NextVector2Unit() * 5;
                    NPC.netUpdate = true;
                }
            }
        }
        foreach (Player player in Main.ActivePlayers)
        {
            if (player.Center.Distance(NPC.Center) < 450 || (NPC.dontTakeDamage && ++AITimer > 60 * 8) || NPC.ai[3] > 0)
            {
                if (++NPC.ai[3] > 100 * NPC.scale)
                {
                    NPC.aiStyle = -1;
                    AIType = 0;
                    NPC.velocity *= 0.99f;
                    if (NPC.ai[3] >= 120)
                        NPC.Center = lastPos + Main.rand.NextVector2Circular(4 * glowAlpha, 4 * glowAlpha);
                    else
                        lastPos = NPC.Center;
                    glowAlpha += 0.03f;
                    if (NPC.ai[3] > 150)
                    {
                        Main.BestiaryTracker.Kills.RegisterKill(NPC);
                        MPUtils.NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ProjectileType<HostileCorruptExplosion>(), 50, 0);
                        NPC.dontTakeDamage = false;
                        NPC.StrikeInstantKill();
                        NPC.netUpdate = true;
                        break;
                    }
                    if ((int)NPC.ai[3] == 101)
                        NPC.netUpdate = true;
                }
            }
        }
        if (!NPC.dontTakeDamage && NPC.velocity.Length() < 6)
            NPC.velocity += NPC.Center.FromAToB(Main.LocalPlayer.Center, true) * .3f;
    }
    public override bool CheckDead()
    {
        if (Main.dedServ)
            return true;
        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Find<ModGore>("EbonianMod/EbonFlyGore").Type, NPC.scale);
        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Find<ModGore>("EbonianMod/EbonFlyGore2").Type, NPC.scale);
        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Find<ModGore>("EbonianMod/EbonFlyGore3").Type, NPC.scale);
        return true;
    }
}
