using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using EbonianMod.NPCs.Overworld.Critters;
using EbonianMod.NPCs.Ocean.Critters;

namespace EbonianMod.Items.Misc.Critters
{
    public class BoxFishItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Bunny);
            Item.makeNPC = NPCType<BoxFish>();
            Item.value = Item.buyPrice(0, 0, 10, 0);
        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ItemID.Sashimi)
               .AddIngredient(ModContent.ItemType<BoxFishItem>())
               .AddTile(TileID.WorkBenches)
               .Register();
        }
        public override bool? UseItem(Player player)
        {
            NPC.NewNPCDirect(null, Main.MouseWorld, Item.makeNPC);
            return true;
        }
    }
}