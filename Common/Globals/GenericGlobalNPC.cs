using EbonianMod.Items.Armor.Vanity;
using EbonianMod.NPCs.ArchmageX;
using EbonianMod.NPCs.Cecitior;
using EbonianMod.NPCs.Garbage;
using EbonianMod.NPCs.Terrortoma;

namespace EbonianMod.Common.Globals;
public class GenericGlobalNPC : GlobalNPC
{
    public override void GetChat(NPC npc, ref string chat)
    {
        if (npc.type == NPCID.Wizard)
        {
            if (Main.rand.NextBool(10))
            {
                WeightedRandom<string> _chat = new();
                if (GetInstance<EbonianSystem>().xareusFuckingDies)
                    _chat.Add(Language.GetTextValue("Mods.EbonianMod.Dialogue.Wizard.WizardAboutX1"));
                else if (GetInstance<EbonianSystem>().downedXareus)
                    _chat.Add(Language.GetTextValue("Mods.EbonianMod.Dialogue.Wizard.WizardAboutX2"));
                else if (GetInstance<EbonianSystem>().timesDiedToXareus > 0)
                {
                    _chat.Add(Language.GetTextValue("Mods.EbonianMod.Dialogue.Wizard.WizardAboutX3"));
                    _chat.Add(Language.GetTextValue("Mods.EbonianMod.Dialogue.Wizard.WizardAboutX4"));
                    _chat.Add(Language.GetTextValue("Mods.EbonianMod.Dialogue.Wizard.WizardAboutX5"));
                }
                else if (NPC.AnyNPCs(NPCType<ArchmageStaffNPC>()))
                {
                    _chat.Add(Language.GetTextValue("Mods.EbonianMod.Dialogue.Wizard.WizardAboutX6"));
                    _chat.Add(Language.GetTextValue("Mods.EbonianMod.Dialogue.Wizard.WizardAboutX7"));
                }
                if (GetInstance<EbonianSystem>().timesDiedToXareus > 0)
                    _chat.Add(Language.GetTextValue("Mods.EbonianMod.Dialogue.Wizard.WizardAboutX8"));
                chat = _chat;
            }
        }
    }
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type == NPCID.Zombie)
            npcLoot.Add(ItemDropRule.Common(ItemType<ClementinesCap>(), 300));
    }
    public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
    {
        if (NPC.AnyDanger())
        {
            maxSpawns = 0;
            spawnRate = 0;
        }
    }
}