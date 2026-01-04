using EbonianMod.Content.Items.Consumables.Food;
using Terraria.GameContent.Bestiary;

namespace EbonianMod.Content.NPCs.Corruption.Rotling;

public class RotlingHead : WormHead
{
    public override string Texture => Helper.AssetPath + "NPCs/Corruption/Rotling/"+Name;
    public override void SetStaticDefaults()
    {

        var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
        {
            CustomTexturePath = Helper.AssetPath+"NPCs/Corruption/Rotling/Rotling",
            Position = new Vector2(0, 24f),
            PortraitPositionXOverride = 0f,
            PortraitPositionYOverride = 12f
        };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
    }
    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(ItemDropRule.Common(ItemType<VileNoodleBox>(), 50, 1, 3));
        npcLoot.Add(ItemDropRule.Common(ItemID.RottenChunk, 12, 1, 3));
    }
    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        if (Main.invasionType > 0) return 0;
        return spawnInfo.Player.ZoneCorrupt ? 0.065f : 0;
    }
    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
            new FlavorTextBestiaryInfoElement("Mods.EbonianMod.Misc.Types.Evil"),
            new FlavorTextBestiaryInfoElement("Mods.EbonianMod.NPCs.RotlingHead.Bestiary"),
        });
    }

    public override bool CheckDead()
    {
        if (Main.dedServ)
            return base.CheckDead();
        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Find<ModGore>("EbonianMod/EbonFlyGore2").Type, NPC.scale);
        return base.CheckDead();
    }
    public override int BodyType => NPCType<RotlingBody>();
    public override int TailType => NPCType<RotlingTail>();
    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.DiggerHead);
        NPC.Size = new Vector2(10, 14);
        NPC.damage = 1;
        quiet = true;
        NPC.aiStyle = -1;
        NPC.lifeMax = 15;
        NPC.value = Item.buyPrice(0, 0, 0, 1);
    }
    int soundTimer;
    public override void ExtraAI()
    {
    }
    public override void Init()
    {
        MinSegmentLength = 5;
        MaxSegmentLength = 10;

        MoveSpeed = 11.5f;
        Acceleration = 0.1f;
    }
}
public class RotlingBody : WormBody
{
    public override string Texture => Helper.AssetPath + "NPCs/Corruption/Rotling/"+Name;
    public override void HitEffect(NPC.HitInfo hitinfo)
    {
        if (Main.dedServ)
            return;
        if (hitinfo.Damage > NPC.life && NPC.life <= 0)
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Find<ModGore>("EbonianMod/EbonFlyGore2").Type, NPC.scale);
    }
    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.DiggerHead);
        NPC.Size = new Vector2(10, 8);
        NPC.damage = 1;
        NPC.aiStyle = -1;
        NPC.lifeMax = 15;
    }
    public override void ExtraAI()
    {
        NPC.soundDelay = 10;
    }
    public override void Init()
    {
        MoveSpeed = 13.5f;
        Acceleration = 0.1f;
    }
}
public class RotlingTail : WormTail
{
    public override string Texture => Helper.AssetPath + "NPCs/Corruption/Rotling/"+Name;
    public override void HitEffect(NPC.HitInfo hitinfo)
    {
        if (Main.dedServ)
            return;
        if (hitinfo.Damage > NPC.life && NPC.life <= 0)
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Find<ModGore>("EbonianMod/EbonFlyGore2").Type, NPC.scale);
    }
    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.DiggerHead);
        NPC.Size = new Vector2(10, 8);
        NPC.damage = 1;
        NPC.aiStyle = -1;
        NPC.lifeMax = 15;
    }
    public override void ExtraAI()
    {
        NPC.soundDelay = 10;
    }
    public override void Init()
    {
        MoveSpeed = 13.5f;
        Acceleration = 0.1f;
    }
}
