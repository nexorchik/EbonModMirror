namespace EbonianMod.Common.Systems.CrossMod
{
    internal class ProjectTRUSupport : ModSystem
    {
        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("CalamityRuTranslate", out Mod tru))
            {
                var ebon = Mod;

                tru.Call("AddFeminineItems", ebon, new[]
                {
                    "TerrorFlail", "ToothToothbrush", "EbonianRocketLauncher", "CecitiorClawSummon",
                    "SpudCannon", "HotShield", "DoomsdayRemote", "BaseballBat", "GarbageFlail",
                    "ReiMask", "CursedDoll", "StaffofXWeapon", "EbonianScythe", "WardingStar", 
                    "BotanistHeadStaff"
                });

                tru.Call("AddNeuterItems", ebon, new[]
                {
                    "EbonianHeart", "ThawGauntlet", "XTentacleAcc", "RingOfFire",
                    "XareusPotion", "Bloodlash", "GoldenTip"
                });

                tru.Call("AddPluralItems", ebon, new[]
                {
                    "Shears", "CrimCannon", "Serration"
                });

                tru.Call("AddArmorSetBonusPreview",
                    ModContent.ItemType<Items.Armor.WoolArmor.WoolenUshanka>(),
                    () => Language.GetTextValue("Mods.EbonianMod.Items.WoolenUshanka.SetBonus", 1)
                );

                tru.Call("AddArmorSetBonusPreview",
                    ModContent.ItemType<Items.Armor.Cecitoma.Cecihead>(),
                    () => Language.GetTextValue("Mods.EbonianMod.Items.Cecihead.SetBonus")
                );

                tru.Call("AddArmorSetBonusPreview",
                    ModContent.ItemType<Items.Armor.Cecitoma.Terrorhead>(),
                    () => Language.GetTextValue("Mods.EbonianMod.Items.Terrorhead.SetBonus")
                );
            }
        }
    }
}

