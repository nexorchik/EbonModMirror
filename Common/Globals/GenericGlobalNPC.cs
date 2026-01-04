using EbonianMod.Content.Items.Vanity;
using EbonianMod.Content.NPCs.ArchmageX;
using EbonianMod.Content.NPCs.Cecitior;
using EbonianMod.Content.NPCs.Garbage;
using EbonianMod.Content.NPCs.Terrortoma;
using EbonianMod.Core.Systems.Boss;

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
                if (GetInstance<XareusSystem>().downedMartianXareus)
                    _chat.Add(Language.GetTextValue("Mods.EbonianMod.Dialogue.Wizard.WizardAboutX1"));
                else if (GetInstance<XareusSystem>().downedXareus)
                    _chat.Add(Language.GetTextValue("Mods.EbonianMod.Dialogue.Wizard.WizardAboutX2"));
                else if (GetInstance<XareusSystem>().timesDiedToXareus > 0)
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
                if (GetInstance<XareusSystem>().timesDiedToXareus > 0)
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
}