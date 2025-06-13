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
                    "SpudCannon", "HotShield", "DoomsdayRemote", "Bat", "GarbageFlail",
                    "ReiMask", "CursedToy", "StaffofXWeapon", "EbonianScythe", "WardingStar"
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
                    () => Language.GetTextValue("Mods.EbonianMod.Items.WoolenUshanka.SetBonus", 1));
            }
        }
    }
}
