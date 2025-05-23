using EbonianMod.Items.Accessories;
using EbonianMod.Items.Consumables.BossItems;
using EbonianMod.Items.Consumables.Food;
using EbonianMod.Items.Pets;
using EbonianMod.Items.Pets.Blinkroot;
using EbonianMod.Items.Tiles;
using EbonianMod.Items.Weapons.Magic;
using EbonianMod.Items.Weapons.Melee;
using EbonianMod.Items.Weapons.Ranged;
using System;

namespace EbonianMod.Common.Systems.Worldgen;

public class ChestSpawnSystem : ModSystem
{
    void FillChests()
    {
        WeightedRandom<int> goldChestMainLoot = new();
        goldChestMainLoot.Add(ItemType<GarbageRemote>(), 3);
        goldChestMainLoot.Add(ItemType<SpudCannon>());
        goldChestMainLoot.Add(ItemType<GoldenTip>());

        WeightedRandom<int> goldChestSecondaryLoot = new();
        goldChestSecondaryLoot.Add(ItemType<WaspPaintingI>(), 0.5f);
        goldChestSecondaryLoot.Add(ItemType<DjungelskogI>(), 0.5f);
        goldChestSecondaryLoot.Add(ItemType<BlinkrootI>());
        goldChestSecondaryLoot.Add(ItemType<Potato>());

        WeightedRandom<int> shadowChestMainLoot = new();
        shadowChestMainLoot.Add(ItemType<Corebreaker>(), 2);
        shadowChestMainLoot.Add(ItemType<RingOfFire>());
        shadowChestMainLoot.Add(ItemType<Exolsaw>());

        for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
        {
            Chest chest = Main.chest[chestIndex];
            if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers)
            {
                if (Main.tile[chest.x, chest.y].TileFrameX == 1 * 36)
                {
                    if (WorldGen.genRand.NextBool(5)) //secondary
                    {
                        int type = goldChestSecondaryLoot;
                        chest.item[1].SetDefaults(type);
                        if (chest.item[1].type == ItemType<Potato>())
                            chest.item[1].stack = Main.rand.Next(2, 20);
                    }
                    if (WorldGen.genRand.NextBool(5)) //primary
                    {
                        int type = goldChestMainLoot;
                        chest.item[0].SetDefaults(type);
                        if (chest.item[0].type == ItemType<SpudCannon>())
                        {
                            chest.item[1].SetDefaults(ItemType<Potato>());
                            chest.item[1].stack = Main.rand.Next(50, 200);
                        }

                    }
                }
                if (Main.tile[chest.x, chest.y].TileFrameX == 3 * 36 || Main.tile[chest.x, chest.y].TileFrameX == 4 * 36)
                {
                    if (WorldGen.genRand.NextBool(3))
                        chest.item[0].SetDefaults(shadowChestMainLoot);
                }
            }
        }
    }
    public override void PostWorldGen()
    {
        FillChests();
    }
}
