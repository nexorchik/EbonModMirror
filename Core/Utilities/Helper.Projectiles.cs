using System;

namespace EbonianMod.Core.Utilities;

public static partial class Helper
{
	public static int HostileProjDmg(int normal, int expert, int master) => Main.masterMode ? master / 6 : (Main.expertMode ? expert / 4 : normal / 2);

	public static bool UseAmmo(this Projectile projectile, int AmmoType)
	{
		bool UsedAnything = false;
		Player player = Main.player[projectile.owner];
		for (int j = 0; j < 58; j++)
		{
			if (player.inventory[j].ammo == AmmoType && player.inventory[j].stack > 0)
			{
				if (player.inventory[j].maxStack > 1)
				{
					player.inventory[j].stack--;
					UsedAnything = true;
				}
				break;
			}
		}
		return UsedAnything;
	}
	public static void SineMovement(this Projectile projectile, Vector2 initialCenter, Vector2 initialVel, float frequencyMultiplier, float amplitude)
	{
		projectile.ai[1]++;
		float wave = MathF.Sin(projectile.ai[1] * frequencyMultiplier);
		Vector2 vector = new Vector2(initialVel.X, initialVel.Y).RotatedBy(MathHelper.ToRadians(90));
		vector.Normalize();
		wave *= projectile.ai[0];
		wave *= amplitude;
		Vector2 offset = vector * wave;
		projectile.Center = initialCenter + (projectile.velocity * projectile.ai[1]);
		projectile.Center = projectile.Center + offset;
	}
}