namespace EbonianMod.Common.Players;

public class SummonPlayer : ModPlayer
{
    public bool doomMinion, spiritMinion, ClawMinion, titteringMinion, sudamaMinion, rolleg, ToxicGland;
    public override void ResetEffects()
    {
        rolleg = false;
        doomMinion = false;
        spiritMinion = false;
        sudamaMinion = false;
        titteringMinion = false;
        ClawMinion = false;
        ToxicGland = false;
    }
}