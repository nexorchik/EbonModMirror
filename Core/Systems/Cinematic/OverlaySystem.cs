namespace EbonianMod.Core.Systems.Cinematic;

public class OverlaySystem : ModSystem
{
    public static float FlashAlpha, DarkAlpha;

    public override void PostUpdateEverything()
    {
        if (FlashAlpha > 0)
            FlashAlpha -= 0.01f;

        if (!Main.gameInactive)
            DarkAlpha = Lerp(DarkAlpha, 0, 0.1f);
        if (DarkAlpha < .05f)
            DarkAlpha = 0;
    }
}