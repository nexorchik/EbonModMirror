using EbonianMod.Content.Projectiles.AsteroidShower;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Effects;

namespace EbonianMod.Core.Systems.Scenes;
public class RiverOfStarlight : ModSceneEffect
{
    public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/Meteor");
    public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
    public override bool IsSceneEffectActive(Player player)
    {
        return !Main.dayTime && Star.starfallBoost > 2f && (player.ZoneOverworldHeight || player.ZoneSkyHeight);
    }
    public override void SpecialVisuals(Player player, bool isActive)
    {
        if (Main.dedServ) return;
        if (IsSceneEffectActive(player))
        {
            if (!SkyManager.Instance["Asteroid"].IsActive())
            {
                SkyManager.Instance.Activate("Asteroid");
            }
            if (player.ZoneOverworldHeight || player.ZoneSkyHeight)
            {
                Filters.Scene["Asteroid"].GetShader().UseColor(Color.Blue).UseOpacity(0.1f);
            }
            player.ManageSpecialBiomeVisuals("Asteroid", isActive);
        }
        else
        {
            if (SkyManager.Instance["Asteroid"].IsActive())
            {
                SkyManager.Instance.Deactivate("Asteroid");
            }
            player.ManageSpecialBiomeVisuals("Asteroid", false);
        }
    }
}

public class RiverOfStarlightPlayer : ModPlayer
{
    public override void PostUpdate()
    {
        if (Star.starfallBoost >= 2 ? Main.rand.NextBool(8500) : false && !Main.dayTime)
        {
            MPUtils.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(1920 * Main.rand.NextFloat() - 960, -3000), new Vector2(Main.rand.NextFloat(-1, 1), 20f), ModContent.ProjectileType<FallingStarBig>(), 2000, 0);
        }
        if (Star.starfallBoost >= 2 ? Main.rand.NextBool(300) : false && !Main.dayTime)
        {
            MPUtils.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(1920 * Main.rand.NextFloat() - 960, -2500), new Vector2(Main.rand.NextFloat(-10, 10), 20f), ModContent.ProjectileType<FallingStarTiny>(), 10, 0);
        }
    }
}