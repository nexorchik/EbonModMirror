using System;
using System.Collections.Generic;
using System.Linq;

namespace EbonianMod.Core.Utilities;

public static partial class Helper
{
	/// <summary>
	/// Clamps the value between 0-1
	/// </summary>
	public static float Saturate(this float f) => Clamp(f, 0, 1);
	/// <summary>
	/// Avoids division by zero 
	/// </summary>
	public static float Safe(this float f, float x = 1)
	{
		return f + (f == 0 ? x : 0);
	}
	public static Rectangle ToRectangle(this System.Drawing.RectangleF rect)
	{
		return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
	}
	public static int IndexOfClosestTo(this IEnumerable<float> collection, float target)
	{
		// NB Method will return int.MaxValue for a sequence containing no elements.
		// Apply any defensive coding here as necessary.
		int closest = 0;
		var minDifference = float.MaxValue;
		foreach (float element in collection)
		{
			var difference = Math.Abs(element - target);
			if (minDifference > difference)
			{
				minDifference = difference;
				closest = Array.IndexOf(collection.ToArray(), element);
			}
		}

		return closest;
	}
	public static float CircleDividedEqually(float i, float max)
	{
		return 2f * MathF.PI / max * i;
	}
	public static bool InRange(this float f, float target, float range = 1f) => f > target - range && f < target + range;
	public static bool InRange(this int f, int target, int range = 1) => f > target - range && f < target + range;
	public static bool InRange(this double f, double target, double range = 1.0) => f > target - range && f < target + range;
	public static Vector2 FromAToB(this Vector2 a, Vector2 b, bool normalize = true, bool reverse = false)
	{
		Vector2 baseVel = b - a;
		if (normalize && baseVel.LengthSquared() > 0f)
			baseVel.Normalize();
		if (reverse)
		{
			Vector2 baseVelReverse = a - b;
			if (normalize)
				baseVelReverse.Normalize();
			return baseVelReverse;
		}
		return baseVel;
	}
	public static Vector2 FromAToB(this Entity a, Entity b, bool normalize = true, bool reverse = false) => FromAToB(a.Center, b.Center, normalize, reverse);
	public static Vector2 FromAToB(this Vector2 a, Entity b, bool normalize = true, bool reverse = false) => FromAToB(a, b.Center, normalize, reverse);
	public static Vector2 FromAToB(this Entity a, Vector2 b, bool normalize = true, bool reverse = false) => FromAToB(a.Center, b, normalize, reverse);
	
	public static class TileRaycast
	{
		public static Vector2 Cast(Vector2 start, Vector2 direction, float length, bool platformCheck = false)
		{
			direction = direction.SafeNormalize(Vector2.UnitY);
			Vector2 output = start;

			for (int i = 0; i < length; i++)
			{
				if ((Collision.CanHitLine(output, 0, 0, output + direction, 0, 0) && (platformCheck ? !Collision.SolidTiles(output, 1, 1, platformCheck) && Main.tile[(int)output.X / 16, (int)output.Y / 16].TileType != TileID.Platforms : true)))
				{
					output += direction;
				}
				else
				{
					break;
				}
			}

			return output;
		}
		public static float CastLength(Vector2 start, Vector2 direction, float length, bool platformCheck = false)
		{
			Vector2 end = Cast(start, direction, length, platformCheck);
			return (end - start).Length();
		}
	}
}