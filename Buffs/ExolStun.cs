namespace EbonianMod.Buffs;

public class ExolStun : ModBuff
{
    public override void SetStaticDefaults()
    {

        Main.buffNoTimeDisplay[Type] = false;
    }
    public override void Update(NPC npc, ref int buffIndex)
    {
        npc.GetGlobalNPC<EbonianMod.Common.Globals.EbonGlobalNPC>().stunned = true;
    }
}