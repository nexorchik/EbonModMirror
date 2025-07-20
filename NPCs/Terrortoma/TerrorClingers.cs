using System;
using System.IO;
using System.Linq;

namespace EbonianMod.NPCs.Terrortoma;
public abstract class TerrorClingerGeneric : ModNPC
{
    public void NetUpdateAtSpecificTime(params int[] times)
    {
        if (times.Contains(AITimer))
            NPC.netUpdate = true;
    }
    public int AITimer
    {
        get => (int)NPC.ai[1];
        set => NPC.ai[1] = value;
    }
    public float AITimer2
    {
        get => NPC.ai[2];
        set => NPC.ai[2] = value;
    }

    public float lerpSpeed;
    public float AITimer3;
    public Vector2 terrortomaCenter, savedP;
    public float bloomAlpha;
    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.Write(lerpSpeed);
        writer.Write(AITimer3);
        writer.WriteVector2(terrortomaCenter);
        writer.WriteVector2(savedP);
        writer.Write(bloomAlpha);
    }
    public override void ReceiveExtraAI(BinaryReader reader)
    {
        lerpSpeed = reader.Read();
        AITimer3 = reader.Read();
        terrortomaCenter = reader.ReadVector2();
        savedP = reader.ReadVector2();
        bloomAlpha = reader.Read();
    }
    public override void SetDefaults()
    {
        NPC.aiStyle = -1;
        NPC.buffImmune[BuffID.Confused] = true;
        NPC.lifeMax = 12323;
        NPC.dontTakeDamage = true;
        NPC.damage = 0;
        NPC.noTileCollide = true;
        NPC.defense = 10;
        NPC.knockBackResist = 0;
        NPC.npcSlots = 1f;
        NPC.lavaImmune = true;
        NPC.noGravity = true;
        NPC.buffImmune[24] = true;
        NPC.netAlways = true;
    }
    public override void PostAI()
    {
        NPC center = Main.npc[(int)NPC.ai[0]];
        if (!center.active || center.type != NPCType<Terrortoma>())
            return;
        if (center.netUpdate) NPC.netUpdate = true;
    }
}