using EbonianMod.Dusts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Enums;
using Terraria.GameContent.Bestiary;

namespace EbonianMod.NPCs.Overworld.Starlads;
public class Obeselad : ModNPC
{
    public enum StateID
    {
        Spawn,
        Walking,
        Standing,
        Bounced,
        Tripped
    };

    StateID state = StateID.Spawn;

    float heightMod;
    float widthMod;

    int storedDirection;

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = 16;

        NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() { Velocity = 1f };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
    }

    public override void SetDefaults()
    {
        NPC.lifeMax = 5;
        NPC.damage = 0;
        NPC.defense = 0;
        NPC.knockBackResist = 1f;

        NPC.Size = new Vector2(72f, 36f);
        NPC.scale = 1f;

        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
        NPC.value = Item.sellPrice(0, 0, 0, 90);

        NPC.aiStyle = -1;
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Moon,
                new FlavorTextBestiaryInfoElement("Mods.EbonianMod.Misc.Types.Fungus"),
                new FlavorTextBestiaryInfoElement(NPC.BestiaryKey()),
            });
    }

    public override void AI()
    {
        Player player = Main.player[NPC.target];

        if (state == StateID.Spawn)
        {
            NPC.TargetClosest(true);
            storedDirection = NPC.direction;
            state = StateID.Walking;
        }

        NPC.spriteDirection = NPC.direction;
        Lighting.AddLight(NPC.Center, new Color(241, 212, 62).ToVector3() * 0.5f);

        if (NPC.Center.Distance(player.Center) <= 30f && player.velocity.Y > 0)
        {
            state = StateID.Bounced;
            SoundEngine.PlaySound(EbonianSounds.ObeseladBounce, NPC.Center);

            Color newColor7 = Color.CornflowerBlue;
            for (float num614 = 0f; num614 < 1f; num614 += 0.25f)
            {
                Dust.NewDustPerfect(NPC.Bottom, 278, -Vector2.UnitY.RotatedByRandom(PiOver2) * (4f + Main.rand.NextFloat() * 4f), 150, newColor7).noGravity = true;
            }
            for (float num615 = 0f; num615 < 1f; num615 += 0.25f)
            {
                Dust.NewDustPerfect(NPC.Bottom, 278, -Vector2.UnitY.RotatedByRandom(PiOver2) * (2f + Main.rand.NextFloat() * 3f), 150, Color.Gold).noGravity = true;
            }
            player.velocity.Y = -10f;
            player.SyncPlayerControls();
        }

        if (NPC.Center.Distance(player.Center) >= 100f)
        {
            if (state != StateID.Tripped && state != StateID.Bounced)
            {
                NPC.direction = storedDirection;
            }

            if (state == StateID.Walking)
            {
                NPC.velocity.X += NPC.direction * 0.05f;

                if (NPC.collideX)
                {
                    NPC.velocity.X = -NPC.oldVelocity.X;
                    storedDirection = -storedDirection;
                }
            }
            else
            {
                NPC.velocity.X *= 0.7f;
            }
        }
        else
        {
            if (state != StateID.Tripped && state != StateID.Bounced)
            {
                NPC.TargetClosest(true);
            }

            NPC.velocity.X *= 0.9f;
        }

        if (NPC.velocity.X >= 1f)
        {
            NPC.velocity.X = 1f;
        }

        if (NPC.velocity.X <= -1f)
        {
            NPC.velocity.X = -1f;
        }
    }

    public override bool CheckDead()
    {
        for (int k = 0; k < 20; k++)
        {
            Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<StarshroomDust>(), 0f, 0f, 0, default, 1f).noGravity = true;
        }

        for (int k = 0; k < 3; k++)
        {
            if (Main.netMode != NetmodeID.Server) { Gore.NewGore(NPC.GetSource_Death(), NPC.position, Main.rand.NextVector2CircularEdge(0.5f, 0.5f) * NPC.velocity.Length(), 16, 1f); }
        }

        if (Main.netMode != NetmodeID.Server) { Gore.NewGore(NPC.GetSource_Death(), NPC.position, Vector2.Zero, Mod.Find<ModGore>("ObeseladCap").Type, 1f); }
        return true;
    }

    public override void FindFrame(int frameHeight)
    {
        heightMod += (1 - heightMod) / 5f;
        widthMod += (1 - widthMod) / 5f;

        if (state == StateID.Bounced)
        {
            heightMod = 0.5f;
            widthMod = 2f;

            NPC.frame.Y = 8 * frameHeight;
            state = StateID.Tripped;
        }
        else if (state == StateID.Tripped)
        {
            NPC.frameCounter += 1f;

            if (NPC.frameCounter >= 5)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y >= 16 * frameHeight)
                {
                    heightMod = 1.5f;
                    widthMod = 0.5f;

                    NPC.frame.Y = 0 * frameHeight;
                    state = StateID.Walking;
                }
            }
        }
        else
        {
            NPC.frameCounter += (NPC.velocity.Length() * 1f);

            if (NPC.frameCounter >= 5)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y >= 8 * frameHeight) { NPC.frame.Y = 0 * frameHeight; }
            }
        }
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        if (Main.invasionType > 0) return 0;
        return ((spawnInfo.Player.ZoneNormalSpace ? 0.2f : 0.15f) + Star.starfallBoost * 0.05f)
            * ((spawnInfo.Player.ZoneForest || spawnInfo.Player.ZoneNormalSpace) && !Main.dayTime && Main.GetMoonPhase() == MoonPhase.Full).ToInt();
    }
    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        //npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Starspore>(), 3, 5, 35));
        npcLoot.Add(ItemDropRule.Common(ItemID.Mushroom, 5, 1, 4));
        npcLoot.Add(ItemDropRule.Common(ItemID.FallenStar, 10, 1, 3));
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {

        Texture2D texture = TextureAssets.Npc[Type].Value;

        Vector2 position = NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY) + new Vector2(0f, 20f);

        SpriteEffects spriteEffects = SpriteEffects.None;

        if (NPC.spriteDirection > 0)
        {
            spriteEffects = SpriteEffects.FlipHorizontally;
        }

        if (NPC.IsABestiaryIconDummy)
        {
            return true;
        }

        spriteBatch.Draw(texture, position, NPC.frame, Color.White, NPC.rotation, new Vector2(NPC.frame.Size().X / 2f, NPC.frame.Size().Y), new Vector2(widthMod, heightMod), spriteEffects, 0);

        return false;
    }
}
