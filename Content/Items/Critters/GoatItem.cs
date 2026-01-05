using EbonianMod.Content.NPCs.BloodMoon.Critters;

namespace EbonianMod.Content.Items.Critters;

public class GoatItem : ModItem
{
	public override string Texture => Helper.AssetPath + "Items/Critters/GoatItem";
	public override void SetDefaults()
	{
		Item.CloneDefaults(ItemID.Bunny);
		Item.makeNPC = NPCType<Goat>();
		Item.value = Item.buyPrice(0, 0, 10, 0);
	}
}