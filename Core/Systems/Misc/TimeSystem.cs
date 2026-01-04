namespace EbonianMod.Core.Systems.Misc;

public class TimeSystem : ModSystem
{
    public int constantTimer;
    public static float deltaTime;
    public static int ConstantTimer => GetInstance<TimeSystem>().constantTimer;
    public override void PostUpdateEverything() => constantTimer++;
}