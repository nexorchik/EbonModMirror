namespace EbonianMod.Core.Systems.Boss;

public class ConglomerateSystem : ModSystem
{
    public static float conglomerateSkyFlash;
    public Color conglomerateSkyColorOverride;
    
    public override void PostUpdateEverything()
    {
        conglomerateSkyFlash = Lerp(conglomerateSkyFlash, 0, 0.07f);
        conglomerateSkyColorOverride = Color.Lerp(conglomerateSkyColorOverride, Color.White, 0.03f);
        if (conglomerateSkyFlash < 0.05f)
        {
            conglomerateSkyFlash = 0;
            conglomerateSkyColorOverride = Color.White;
        }
    }
}