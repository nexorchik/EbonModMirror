using EbonianMod.Items.Accessories;
using EbonianMod.Items.Weapons.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Bestiary;

namespace EbonianMod.NPCs.Overworld.AsteroidH;
public class AsteroidHerder : ModNPC
{
    public override void SetDefaults()
    {
        NPC.Size = new Vector2(24, 30);
        NPC.noGravity = true;
        NPC.noTileCollide = true;
        NPC.damage = 0;
        NPC.defense = 5;
        NPC.lifeMax = 1000;
        NPC.value = Item.buyPrice(0, 1, 0, 0);
        NPC.aiStyle = -1;
        NPC.knockBackResist = 0f;
        NPC.HitSound = SoundID.NPCHit13;
    }
    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
        {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement(NPC.BestiaryKey())
        });
    }
    public override void HitEffect(NPC.HitInfo hit)
    {
        if (NPC.life <= 0)
        {
            SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
            Color newColor7 = Color.CornflowerBlue;
            for (int num613 = 0; num613 < 7; num613++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Enchanted_Pink, NPC.velocity.X * 0.1f, NPC.velocity.Y * 0.1f, 150, default, 0.8f);
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
            Helper.SpawnGore(NPC, "EbonianMod/Warden", vel: -Vector2.UnitY * 3);
        }
    }
    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return (Star.starfallBoost > 2 && !Main.dayTime && spawnInfo.Player.ZoneNormalSpace) ? 0.02f : 0;
    }
    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(ItemDropRule.Common(8, ItemType<StarBit>()));
    }
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D tex = TextureAssets.Npc[Type].Value;
        Texture2D glow = Assets.NPCs.Overworld.AsteroidH.AsteroidHerder_Glow.Value;
        Texture2D star = Assets.NPCs.Overworld.AsteroidH.AsteroidHerder_Star.Value;
        Texture2D star2 = Assets.NPCs.Overworld.AsteroidH.AsteroidHerder_StarSmall.Value;
        Main.spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, NPC.Size / 2, NPC.scale, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        Main.spriteBatch.Draw(glow, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.Size / 2, NPC.scale, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        return false;
    }
    public float AIState { get => NPC.ai[0]; set => NPC.ai[0] = value; }
    public float AITimer { get => NPC.ai[1]; set => NPC.ai[1] = value; }
    public float AITimer2 { get => NPC.ai[2]; set => NPC.ai[2] = value; }
    public override void AI()
    {
        Lighting.AddLight(NPC.Center, new Vector3(195, 169, 13) / 255 * 0.5f);
        Player player = Main.player[NPC.target];
        NPC.direction = player.Center.X < NPC.Center.X ? 1 : -1;
        NPC.spriteDirection = NPC.direction;
        switch (AIState)
        {
            case 0:
                {
                    NPC.TargetClosest(false);
                    if (!Main.dayTime && player.Center.Distance(NPC.Center) < 1050)
                        AITimer++;
                    else
                        NPC.velocity.Y -= 0.5f;
                }
                break;
        }
    }
}
