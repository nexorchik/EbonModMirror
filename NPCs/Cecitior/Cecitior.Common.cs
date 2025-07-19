using EbonianMod.Bossbars;
using EbonianMod.Common.Systems.Verlets;
using EbonianMod.Items.Armor.Vanity;
using EbonianMod.Items.BossTreasure;
using EbonianMod.Items.Materials;
using EbonianMod.Items.Misc;
using EbonianMod.Items.Pets;
using EbonianMod.Items.Tiles;
using EbonianMod.Items.Tiles.Trophies;
using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Bestiary;

namespace EbonianMod.NPCs.Cecitior;
public partial class Cecitior : ModNPC
{

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
            new FlavorTextBestiaryInfoElement("Mods.EbonianMod.Misc.Types.Cecitoma"),
            new FlavorTextBestiaryInfoElement(NPC.BestiaryKey()),
        });
    }
    public override void SetStaticDefaults()
    {
        NPCID.Sets.BossBestiaryPriority.Add(Type);
        NPCID.Sets.ShouldBeCountedAsBoss[Type] = true;
        Main.npcFrameCount[NPC.type] = 7;
    }
    public override void SetDefaults()
    {
        NPC.aiStyle = -1;
        NPC.lifeMax = 8000;
        NPC.damage = 40;
        NPC.noTileCollide = true;
        NPC.dontTakeDamage = true;
        NPC.defense = 27;
        NPC.knockBackResist = 0;
        NPC.width = 118;
        NPC.height = 100;
        NPC.rarity = 999;
        NPC.npcSlots = 1f;
        NPC.lavaImmune = true;
        NPC.noGravity = true;
        NPC.boss = true;
        SoundStyle death = EbonianSounds.cecitiorDie;
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = death;
        NPC.buffImmune[24] = true;
        NPC.buffImmune[BuffID.Ichor] = true;
        NPC.buffImmune[BuffID.Confused] = true;
        NPC.value = Item.buyPrice(0, 10);
        NPC.netAlways = true;
        NPC.hide = true;
        NPC.BossBar = GetInstance<CecitiorBar>();
    }
    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
        notExpertRule.OnSuccess(new CommonDrop(ItemType<CecitiorMaterial>(), 1, 40, 60));
        notExpertRule.OnSuccess(new CommonDrop(ItemType<SelfStab>(), 1));
        npcLoot.Add(notExpertRule);

        npcLoot.Add(ItemDropRule.Common(ItemType<CeciMask>(), 4));
        npcLoot.Add(ItemDropRule.Common(ItemType<CecitiorTrophy>(), 4));
        npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ItemType<CecitiorPet>(), 4));
        npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ItemType<CecitiorRelic>()));
        npcLoot.Add(ItemDropRule.Common(ItemType<DapperCecitiorI>(), 5));

        npcLoot.Add(ItemDropRule.BossBag(ItemType<CecitiorBag>()));
    }
    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.WriteVector2(openOffset);
        writer.Write(open);
        writer.Write(NPC.localAI[0]);
        writer.Write(NPC.localAI[1]);
        writer.Write((short)OldState);
        for (int i = 0; i < claw.Length; i++)
            writer.WriteVector2(claw[i].position);
        writer.WriteVector2(savedPos);
        writer.WriteVector2(savedClawPos);
        writer.Write(oldHP);
        writer.Write(AITimer2);
        writer.Write(AITimer3);
        writer.Write((Half)shakeVal);
    }
    public override void ReceiveExtraAI(BinaryReader reader)
    {
        openOffset = reader.ReadVector2();
        open = reader.ReadBoolean();
        NPC.localAI[0] = reader.ReadSingle();
        NPC.localAI[1] = reader.ReadSingle();
        OldState = reader.ReadInt16();
        for (int i = 0; i < claw.Length; i++)
            claw[i].position = reader.ReadVector2();
        savedPos = reader.ReadVector2();
        savedClawPos = reader.ReadVector2();
        oldHP = reader.Read();
        AITimer2 = reader.ReadSingle();
        AITimer3 = reader.ReadSingle();
        shakeVal = (float)reader.ReadHalf();
    }
    public int AIState
    {
        get => (int)NPC.ai[0];
        set => NPC.ai[0] = value;
    }
    public int AITimer
    {
        get => (int)NPC.ai[1];
        set => NPC.ai[1] = value;
    }
    Verlet[] verlet = new Verlet[10];
    float shakeVal;
    public float AITimer2 = 0;
    public float AITimer3 = 0;
    Projectile tongue = null;

    const int PhaseTransition = -4, PrePreDeath = -3, Death = -2, PreDeath = -1, Intro = 0, Idle = 1, EyeBehaviour = 2, Chomp = 3, Teeth = 4, EyeBehaviour2 = 5,
        SpitTeeth = 6, ThrowUpBlood = 7, Tongue = 8, Phase2ThrowUpEyes = 9, Phase2Claw = 10, Phase2ClawGrab = 11, Phase2ClawMultiple = 12,
        Phase2GrabBomb = 13, Phase2ClawBodySlam = 14;

    public SlotId openSound, cachedSound;
    Vector2 savedPos, savedClawPos;
    const int attackNum = 14;
    public int OldState;
    public bool open;
    public Vector2 openOffset;
    float rotation, openRotation;
    public CecitiorClaw[] claw;
    public bool deathAnim;
    public bool halfEyesPhase2;
    public int oldHP;
    public bool phase2;
    public Player player => Main.player[NPC.target];
}
