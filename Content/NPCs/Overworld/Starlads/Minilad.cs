using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Bestiary;

namespace EbonianMod.Content.NPCs.Overworld.Starlads;
public class Minilad : ModNPC
{
    public override string Texture => Helper.AssetPath + "NPCs/Overworld/Starlads/"+Name;
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = 8;
    }
    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        if (Main.invasionType > 0) return 0;
        return ((spawnInfo.Player.ZoneNormalSpace ? 0.2f : 0.15f) + Star.starfallBoost * 0.05f)
            * ((spawnInfo.Player.ZoneForest || spawnInfo.Player.ZoneNormalSpace) && !Main.dayTime).ToInt();
    }
    public override bool CheckDead()
    {
        Color newColor7 = Color.CornflowerBlue;
        for (int num613 = 0; num613 < 7; num613++)
        {
            Dust.NewDust(NPC.position, NPC.width, NPC.height, 58, NPC.velocity.X * 0.1f, NPC.velocity.Y * 0.1f, 150, default, 0.8f);
        }
        for (float num614 = 0f; num614 < 1f; num614 += 0.125f)
        {
            Dust.NewDustPerfect(NPC.Center, 278, Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, newColor7).noGravity = true;
        }
        for (float num615 = 0f; num615 < 1f; num615 += 0.25f)
        {
            Dust.NewDustPerfect(NPC.Center, 278, Vector2.UnitY.RotatedBy(num615 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (2f + Main.rand.NextFloat() * 3f), 150, Color.Gold).noGravity = true;
        }
        Vector2 vector52 = new Vector2(Main.screenWidth, Main.screenHeight);
        if (NPC.Hitbox.Intersects(Utils.CenteredRectangle(Main.screenPosition + vector52 / 2f, vector52 + new Vector2(400f))))
        {
            for (int num616 = 0; num616 < 7; num616++)
            {
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, Main.rand.NextVector2CircularEdge(0.5f, 0.5f) * NPC.velocity.Length(), Utils.SelectRandom(Main.rand, 16, 17, 17, 17, 17, 17, 17, 17));
            }
        }
        return true;
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
    public override void SetDefaults()
    {
        NPC.height = 32;
        NPC.width = 30;
        NPC.damage = 0;
        NPC.friendly = false;
        NPC.lifeMax = 12;
        NPC.defense = 0;
        NPC.aiStyle = 0;
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
        NPC.noGravity = false;
    }
    public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
    {
        SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        Vector2 origin = new Vector2(NPC.width / 2, NPC.height / 2);
        sb.Draw(Assets.NPCs.Overworld.Starlads.Minilad.Value, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, origin, NPC.scale, effects, 0f);

        return false;
    }
    public override void FindFrame(int frameHeight)
    {
        if (NPC.velocity.X != 0)
        {
            if (++NPC.frameCounter >= 3)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y = (NPC.frame.Y / 32 + 1) % 8 * 32;
            }
            return;
        }
        NPC.frame.Y = 0;
    }
    public override void AI()
    {
        NPC.TargetClosest(true);
        Lighting.AddLight(NPC.Center, new Color(241, 212, 62).ToVector3() * 0.5f);
        NPC.direction = NPC.velocity.X >= 0 ? -1 : 1;

        if (NPC.collideX)
        {
            NPC.velocity.Y = -10f;
        }

        if (!NPC.HasValidTarget) return;
        Player player = Main.player[NPC.target];
        Vector2 z = (NPC.Center - player.Center).SafeNormalize(Vector2.Zero);
        NPC.velocity.X = z.X * 4;
    }
}