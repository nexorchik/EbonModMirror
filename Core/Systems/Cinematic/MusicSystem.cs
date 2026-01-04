namespace EbonianMod.Core.Systems.Cinematic;

public class MusicSystem : ModSystem
{
    public static float savedMusicVol, setMusicBackTimer, setMusicBackTimerMax;
    public override void UpdateUI(GameTime gameTime)
    {
        if (--setMusicBackTimer < 0)
        {
            savedMusicVol = Main.musicVolume;
        }
        else
            Main.musicVolume = Lerp(savedMusicVol, 0, setMusicBackTimer / setMusicBackTimerMax);
    }
    public static void TemporarilySetMusicTo0(float time)
    {
        savedMusicVol = Main.musicVolume;
        setMusicBackTimer = time;
        setMusicBackTimerMax = time;
    }
}