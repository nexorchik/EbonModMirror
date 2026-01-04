using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.Graphics.CameraModifiers;
using Terraria.Graphics.Shaders;
using EbonianMod.GeneratedAssets.DataStructures;

namespace EbonianMod;


public static class Helper
{
    public static Rectangle ScreenRect => new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
    public static Vector2 HalfScreen => new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f;

    public static string BestiaryKey(this NPC npc) => "Mods.EbonianMod.NPCs." + npc.ModNPC.Name + ".Bestiary";

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
    public static void AddCameraModifier(ICameraModifier modifier)
    {
        if (!Main.dedServ)
            Main.instance.CameraModifiers.Add(modifier);
    }

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
    public static Color HunterPotionColor(this NPC npc, Color defaultColor)
    {
        return Main.LocalPlayer.HasBuff(BuffID.Hunter) && !npc.IsABestiaryIconDummy ? HunterPotionColor(npc) : defaultColor;
    }
    public static Color HunterPotionColor(this NPC npc)
    {
        return Color.Lerp(Color.OrangeRed * 0.5f, Color.Transparent, Clamp(Utils.GetLerpValue(npc.Size.Length(), 0, Main.LocalPlayer.Distance(npc.Center)), 0, 1));
    }
    public static float ClosestTo(this IEnumerable<float> collection, float target)
    {
        // NB Method will return int.MaxValue for a sequence containing no elements.
        // Apply any defensive coding here as necessary.
        var closest = float.MaxValue;
        var minDifference = float.MaxValue;
        foreach (var element in collection)
        {
            var difference = Math.Abs(element - target);
            if (minDifference > difference)
            {
                minDifference = difference;
                closest = element;
            }
        }

        return closest;
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
    public static float Closer(float a, float b, float compareValue)
    {

        float calcA = Math.Abs(a - compareValue);
        float calcB = Math.Abs(b - compareValue);

        if (calcA == calcB)
        {
            return 0;
        }

        if (calcA < calcB)
        {
            return a;
        }

        return b;
    }
    /// <summary>
    /// Laggy grounded check, only use this for stuff like death animations where you absolutely dont want the npc to not be able to detect the ground
    /// </summary>
    public static bool Grounded(this Entity entity, float offset = .5f, float offsetX = 1f)
    {
        if (entity is NPC)
            if ((entity as NPC).collideY)
                return true;
        if ((!Collision.CanHitLine(new Vector2(entity.Center.X, entity.Center.Y + entity.height / 2), 1, 1
            , new Vector2(entity.Center.X, entity.Center.Y + (entity.height * offset) / 2), 1, 1)
            || Collision.FindCollisionDirection(out int dir, entity.Center, 1, entity.height / 2)))
            return true;
        for (int i = 0; i < entity.width * offsetX; i += (int)(1 / (offsetX == 0 ? 1 : offsetX))) //full sprite check
        {
            bool a = TRay.CastLength(entity.BottomLeft + Vector2.UnitX * i, Vector2.UnitY, entity.height * offset * 2) < entity.height * offset;
            if (!a)
                continue;
            return a;
        }
        return false; //give up
    }
    public static string RgbToHex(Color color)
    {
        int r = color.R;
        int g = color.G;
        int b = color.B;
        return string.Format("{0:X2}{1:X2}{2:X2}", r, g, b);
    }
    public static VertexPositionColorTexture AsVertex(Vector2 position, Color color, Vector2 texCoord)
    {
        return new VertexPositionColorTexture(new Vector3(position, 50), color, texCoord);
    }
    public static VertexPositionColorTexture AsVertex(Vector2 position, Vector2 texCoord, Color color)
    {
        return new VertexPositionColorTexture(new Vector3(position, 50), color, texCoord);
    }
    public static VertexPositionColorTexture AsVertex(Vector3 position, Color color, Vector2 texCoord)
    {
        return new VertexPositionColorTexture(position, color, texCoord);
    }
    public static VertexPositionColorTexture AsVertex(Vector3 position, Vector2 texCoord, Color color)
    {
        return new VertexPositionColorTexture(position, color, texCoord);
    }
    private static Matrix view;
    private static Matrix projection;

    /// <summary>
    /// used for uuhhhhh evenly distrubuted velocity or sum shit i forgor
    /// </summary>
    public static float CircleDividedEqually(float i, float max)
    {
        return 2f * (float)Math.PI / max * i;
    }
    public static Matrix GetMatrix()
    {
        var device = Main.graphics.GraphicsDevice;
        int width = device.Viewport.Width;
        int height = device.Viewport.Height;
        Vector2 zoom = Main.GameViewMatrix.Zoom;
        view =
            Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up)
            * Matrix.CreateTranslation(width / 2, height / -2, 0)
            * Matrix.CreateRotationZ(Pi)
            * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
        projection = Matrix.CreateOrthographic(width, height * Main.LocalPlayer.gravDir, 0, 1000);

        return view * projection;
    }

    private static int GetPrimitiveCount(int vertexCount, PrimitiveType type)
    {
        switch (type)
        {
            case PrimitiveType.LineList:
                return vertexCount / 2;
            case PrimitiveType.LineStrip:
                return vertexCount - 1;
            case PrimitiveType.TriangleList:
                return vertexCount / 3;
            case PrimitiveType.TriangleStrip:
                return vertexCount - 2;
            default: return 0;
        }
    }
    public static void DrawPrimitives(VertexPositionColorTexture[] vertices, PrimitiveType type, bool drawBacksides = true)
    {
        if (vertices.Length < 6) return;
        GraphicsDevice device = Main.graphics.GraphicsDevice;
        Effect effect = Effects.TrailShader.Value;
        effect.Parameters["WorldViewProjection"].SetValue(GetMatrix());
        effect.CurrentTechnique.Passes["Default"].Apply();
        if (drawBacksides)
        {
            short[] indices = new short[vertices.Length * 2];
            for (int i = 0; i < vertices.Length; i += 3)
            {
                indices[i * 2] = (short)i;
                indices[i * 2 + 1] = (short)(i + 1);
                indices[i * 2 + 2] = (short)(i + 2);

                indices[i * 2 + 5] = (short)i;
                indices[i * 2 + 4] = (short)(i + 1);
                indices[i * 2 + 3] = (short)(i + 2);
            }

            device.DrawUserIndexedPrimitives(type, vertices, 0, vertices.Length, indices, 0,
                GetPrimitiveCount(vertices.Length, type) * 2);
        }
        else
        {
            device.DrawUserPrimitives(type, vertices, 0, GetPrimitiveCount(vertices.Length, type));
        }
    }
    public static void DrawTexturedPrimitives(VertexPositionColorTexture[] vertices, PrimitiveType type, Asset<Texture2D> texture, bool drawBacksides = true, bool actualColor = false) =>
        DrawTexturedPrimitives(vertices, type, texture.Value, drawBacksides, actualColor);
    public static void DrawTexturedPrimitives(VertexPositionColorTexture[] vertices, PrimitiveType type, LazyAsset<Texture2D> texture, bool drawBacksides = true, bool actualColor = false) =>
        DrawTexturedPrimitives(vertices, type, texture.Value, drawBacksides, actualColor);
    public static void DrawTexturedPrimitives(VertexPositionColorTexture[] vertices, PrimitiveType type, Texture2D texture, bool drawBacksides = true, bool actualColor = false)
    {
        GraphicsDevice device = Main.graphics.GraphicsDevice;
        Effect effect = Effects.TrailShader.Value;
        effect.Parameters["WorldViewProjection"].SetValue(GetMatrix());
        effect.Parameters["tex"].SetValue(texture);
        effect.Parameters["useActualCol"].SetValue(actualColor);
        effect.CurrentTechnique.Passes["Texture"].Apply();
        if (drawBacksides)
        {
            short[] indices = new short[vertices.Length * 2];
            for (int i = 0; i < vertices.Length; i += 3)
            {
                indices[i * 2] = (short)i;
                indices[i * 2 + 1] = (short)(i + 1);
                indices[i * 2 + 2] = (short)(i + 2);

                indices[i * 2 + 5] = (short)i;
                indices[i * 2 + 4] = (short)(i + 1);
                indices[i * 2 + 3] = (short)(i + 2);
            }

            device.DrawUserIndexedPrimitives(type, vertices, 0, vertices.Length, indices, 0,
                GetPrimitiveCount(vertices.Length, type) * 2);
        }
        else
        {
            device.DrawUserPrimitives(type, vertices, 0, GetPrimitiveCount(vertices.Length, type));
        }
    }

    public static Vector2 GetRotation(List<Vector2> oldPos, int index)
    {
        if (oldPos.Count == 1)
            return oldPos[0];

        if (index == 0)
        {
            return Vector2.Normalize(oldPos[1] - oldPos[0]).RotatedBy(MathHelper.Pi / 2);
        }

        return (index == oldPos.Count - 1
            ? Vector2.Normalize(oldPos[index] - oldPos[index - 1])
            : Vector2.Normalize(oldPos[index + 1] - oldPos[index - 1])).RotatedBy(MathHelper.Pi / 2);
    }
    public static string BuffPlaceholder = "EbonianMod/Buffs/ExolStun";
    public static string Empty = "EbonianMod/Assets/Extras/Empty";
    public static string Placeholder = "EbonianMod/Assets/Extras/Placeholder";
    public static class TRay
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
    public static Asset<Texture2D> GetExtraTexture(string tex, bool altMethod = false, AssetRequestMode assetRequestMode = AssetRequestMode.AsyncLoad)
    {
        if (altMethod)
            return GetTextureAlt("Extras/" + tex, assetRequestMode);
        return GetTexture("Extras/" + tex, assetRequestMode);
    }
    public static Asset<Texture2D> GetTexture(string path, AssetRequestMode assetRequestMode = AssetRequestMode.AsyncLoad)
    {
        return path.StartsWith("EbonianMod/") ? Request<Texture2D>(path, assetRequestMode) : Request<Texture2D>("EbonianMod/" + path, assetRequestMode);
    }
    public static Asset<Texture2D> GetTextureAlt(string path, AssetRequestMode assetRequestMode = AssetRequestMode.AsyncLoad)
    {
        return EbonianMod.Instance.Assets.Request<Texture2D>(path, assetRequestMode);
    }
    public static void SineMovement(this Projectile projectile, Vector2 initialCenter, Vector2 initialVel, float frequencyMultiplier, float amplitude)
    {
        projectile.ai[1]++;
        float wave = (float)Math.Sin(projectile.ai[1] * frequencyMultiplier);
        Vector2 vector = new Vector2(initialVel.X, initialVel.Y).RotatedBy(MathHelper.ToRadians(90));
        vector.Normalize();
        wave *= projectile.ai[0];
        wave *= amplitude;
        Vector2 offset = vector * wave;
        projectile.Center = initialCenter + (projectile.velocity * projectile.ai[1]);
        projectile.Center = projectile.Center + offset;
    }
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
    public static void SpawnDust(Vector2 position, Vector2 size, int type, Vector2 velocity = default, int amount = 1, Action<Dust> dustModification = null)
    {
        for (int i = 0; i < amount; i++)
        {
            var dust = Main.dust[Dust.NewDust(position, (int)size.X, (int)size.Y, type, velocity.X, velocity.Y)];
            dustModification?.Invoke(dust);
        }
    }
    public static void SpawnGore(this NPC NPC, string gore, int amount = 1, int type = -1, Vector2 vel = default, float scale = 1f)
    {
        if (Main.dedServ) return;

        var position = NPC.Center;
        if (type != -1)
        {
            gore += type;
        }
        for (int i = 0; i < amount; i++)
        {
            Gore.NewGore(NPC.GetSource_OnHit(NPC), position + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), vel, Find<ModGore>(gore).Type, scale);
        }
    }
    public static void SpawnGore(this NPC NPC, int gore, int amount = 1, Vector2 vel = default, float scale = 1f)
    {
        var position = NPC.Center;
        for (int i = 0; i < amount; i++)
        {
            Gore.NewGore(NPC.GetSource_OnHit(NPC), position + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), vel, gore, scale);
        }
    }
    public static bool InRange(this float f, float target, float range = 1f) => f > target - range && f < target + range;
    public static bool InRange(this int f, int target, int range = 1) => f > target - range && f < target + range;
    public static bool InRange(this double f, double target, double range = 1.0) => f > target - range && f < target + range;
    public static void DustExplosion(Vector2 pos, Vector2 size = default, int type = 0, Color color = default, bool sound = true, bool smoke = true, float scaleFactor = 1, float increment = 0.125f, Vector2 _vel = default, float MinMulti = 1, float MaxMulti = 1)
    {
        int dustType = DustType<Dusts.ColoredFireDust>();
        switch (type)
        {
            case 0:
                dustType = DustType<Dusts.ColoredFireDust>();
                break;
            case 1:
                dustType = DustType<Dusts.FireDust>();
                break;
            case 2:
                dustType = DustType<Dusts.SmokeDustAkaFireDustButNoGlow>();
                break;
        }
        if (sound)
            SoundEngine.PlaySound(SoundID.Item62.WithPitchOffset(Main.rand.NextFloat(-0.2f, 0.4f)), pos);
        for (float num614 = 0f; num614 < 1f; num614 += increment)
        {
            Vector2 velocity = Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f) * Main.rand.NextFloat(MinMulti, MaxMulti);
            if (increment == 1 || type == 2)
                velocity = _vel;
            Dust dust = Dust.NewDustPerfect(pos, dustType, velocity, 150, color, scaleFactor);
            dust.noGravity = true;
            dust.color = color;

        }
        if (smoke)
            for (int num905 = 0; num905 < 10; num905++)
            {
                int num906 = Dust.NewDust(new Vector2(pos.X, pos.Y), (int)size.X, (int)size.Y, 31, 0f, 0f, 0, default(Color), 2.5f * scaleFactor);
                Main.dust[num906].position = pos + Vector2.UnitX.RotatedByRandom(3.1415927410125732) * size.X / 2f;
                Main.dust[num906].noGravity = true;
                Dust dust2 = Main.dust[num906];
                dust2.velocity *= 3f;
            }
        if (smoke)
            for (int num899 = 0; num899 < 4; num899++)
            {
                int num900 = Dust.NewDust(new Vector2(pos.X, pos.Y), (int)size.X, (int)size.Y, 31, 0f, 0f, 100, default(Color), 1.5f * scaleFactor);
                Main.dust[num900].position = pos + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * size.X / 2f;
            }
    }
    public static void Log(this Projectile obj)
    {
        Main.NewText("Friendly?" + obj.friendly);
        Main.NewText("Hostile?" + obj.hostile);
        Main.NewText("Object:" + obj.Name);
        Main.NewText("Timeleft:" + obj.timeLeft);
        Main.NewText("Damage:" + obj.damage);
        Main.NewText("AI: [" + obj.ai[0] + ", " + obj.ai[1] + "]");
        Main.NewText("Direction:" + obj.direction);
        Main.NewText("LocalAI: [" + obj.localAI[0] + ", " + obj.localAI[1] + "]");
        Main.NewText("Velocity:" + obj.velocity);
        Main.NewText("Owner:" + obj.owner);
    }
    public static void Log(this NPC obj)
    {
        Main.NewText("Friendly?" + obj.friendly);
        Main.NewText("Object:" + obj.TypeName);
        Main.NewText("Timeleft:" + obj.timeLeft);
        Main.NewText("Damage:" + obj.damage);
        Main.NewText("AI: [" + obj.ai[0] + ", " + obj.ai[1] + ", " + obj.ai[2] + ", " + obj.ai[3] + "]");
        Main.NewText("Direction:" + obj.direction);
        Main.NewText("LocalAI: [" + obj.localAI[0] + ", " + obj.localAI[1] + ", " + obj.localAI[2] + ", " + obj.localAI[3] + "]");
        Main.NewText("Velocity:" + obj.velocity);
    }
    public static void CollisionTPNoDust(Vector2 targetPosition, Player player)
    {
        int num = 150;
        Vector2 vector = player.position;
        Vector2 vector2 = player.velocity;
        for (int i = 0; i < num; i++)
        {
            vector2 = (vector + player.Size / 2f).DirectionTo(targetPosition).SafeNormalize(Vector2.Zero) * 12f;
            Vector2 vector3 = Collision.TileCollision(vector, vector2, player.width, player.height, fallThrough: true, fall2: true, (int)player.gravDir);
            vector += vector3;
        }
        _ = vector - player.position;
        TPNoDust(vector, player);
        NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, player.whoAmI, vector.X, vector.Y, 0);
    }
    public static void TPNoDust(Vector2 newPos, Player player)
    {
        try
        {
            player._funkytownAchievementCheckCooldown = 100;
            player.environmentBuffImmunityTimer = 4;
            player.RemoveAllGrapplingHooks();
            player.StopVanityActions();
            float num = MathHelper.Clamp(1f - player.teleportTime * 0.99f, 0.01f, 1f);
            Vector2 otherPosition = player.position;
            float num2 = Vector2.Distance(player.position, newPos);
            PressurePlateHelper.UpdatePlayerPosition(player);
            player.position = newPos;
            player.fallStart = (int)(player.position.Y / 16f);
            if (player.whoAmI == Main.myPlayer)
            {
                bool flag = false;
                if (num2 < new Vector2(Main.screenWidth, Main.screenHeight).Length() / 2f + 100f)
                {
                    int time = 0;
                    Main.SetCameraLerp(0.1f, time);
                    flag = true;
                }
                else
                {
                    NPC.ResetNetOffsets();
                    Main.BlackFadeIn = 255;
                    Lighting.Clear();
                    Main.screenLastPosition = Main.screenPosition;
                    Main.screenPosition.X = player.position.X + (float)(player.width / 2) - (float)(Main.screenWidth / 2);
                    Main.screenPosition.Y = player.position.Y + (float)(player.height / 2) - (float)(Main.screenHeight / 2);
                    Main.instantBGTransitionCounter = 10;
                    player.ForceUpdateBiomes();
                }
            }
            PressurePlateHelper.UpdatePlayerPosition(player);
            player.ResetAdvancedShadows();
            for (int i = 0; i < 3; i++)
            {
                player.UpdateSocialShadow();
            }
            player.oldPosition = player.position + player.BlehOldPositionFixer;
        }
        catch
        {
        }
    }
    public static void QuickDustLine(Vector2 start, Vector2 end, float splits, Color color)
    {
        Dust.QuickDust(start, color).scale = 1f;
        Dust.QuickDust(end, color).scale = 1f;
        float num = 1f / splits;
        for (float amount = 0.0f; (double)amount < 1.0; amount += num)
            Dust.QuickDustSmall(Vector2.Lerp(start, end, amount), color).scale = 1f;
    }
    public static void QuickDustLine(this Dust dust, Vector2 start, Vector2 end, float splits, Color color1, Color color2)
    {
        Dust.QuickDust(start, color1).scale = 1f;
        Dust.QuickDust(end, color2).scale = 1f;
        float num = 1f / splits;
        for (float amount = 0.0f; (double)amount < 1.0; amount += num)
        {
            Color color = Color.Lerp(color1, color2, amount);
            Dust.QuickDustSmall(Vector2.Lerp(start, end, amount), color).scale = 1f;
        }
    }
}
public class MiscDrawingMethods
{
    public static readonly BlendState Subtractive = new BlendState
    {
        ColorSourceBlend = Blend.SourceAlpha,
        ColorDestinationBlend = Blend.One,
        ColorBlendFunction = BlendFunction.ReverseSubtract,
        AlphaSourceBlend = Blend.SourceAlpha,
        AlphaDestinationBlend = Blend.One,
        AlphaBlendFunction = BlendFunction.ReverseSubtract
    };
    public readonly static BlendState AlphaSubtractive = new BlendState
    {
        ColorSourceBlend = Blend.SourceAlpha,
        AlphaSourceBlend = Blend.SourceAlpha,
        ColorDestinationBlend = Blend.One,
        AlphaDestinationBlend = Blend.One,
        ColorBlendFunction = BlendFunction.ReverseSubtract,
        AlphaBlendFunction = BlendFunction.ReverseSubtract
    };
    public static void DrawWithDye(SpriteBatch spriteBatch, DrawData data, int dye, Entity entity, bool Additive = false)
    {
        spriteBatch.End(out var sbParams);
        spriteBatch.Begin(SpriteSortMode.Immediate, Additive ? BlendState.Additive : BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
        GameShaders.Armor.GetShaderFromItemId(dye).Apply(null, data);
        data.Draw(Main.spriteBatch);
        spriteBatch.End();
        spriteBatch.Begin(sbParams);
    }
    public static void DrawShinyText(DrawableTooltipLine line)
    {
        var font = FontAssets.MouseText.Value;

        DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, font, line.Text, new Vector2(line.X, line.Y), line.Color * 0.5f);
        MiscDrawingMethods.LocalDrawShinyText(line, 0.5f);
    }
    public static void LocalDrawShinyText(DrawableTooltipLine line, float opacity = 1)
    {
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, Effects.CrystalShine.Value, Main.UIScaleMatrix);
        var font = FontAssets.MouseText.Value;
        Effects.CrystalShine.Value.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);
        Effects.CrystalShine.Value.Parameters["uOpacity"].SetValue(opacity);
        DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, font, line.Text, new Vector2(line.X, line.Y), Color.White);
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
    }
    public static void LocalDrawShinyText(string text, Vector2 pos, float opacity = 1)
    {
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, Effects.CrystalShine.Value, Main.UIScaleMatrix);
        var font = FontAssets.MouseText.Value;
        Effects.CrystalShine.Value.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);
        Effects.CrystalShine.Value.Parameters["uOpacity"].SetValue(opacity);
        DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, font, text, pos, Color.White);
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
    }
}