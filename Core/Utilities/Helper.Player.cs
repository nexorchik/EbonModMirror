namespace EbonianMod.Core.Utilities;

public static partial class Helper
{
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
}