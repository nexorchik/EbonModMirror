namespace EbonianMod.Common;

public class EbonianKeybinds : ModSystem
{
    public static ModKeybind ReiDash { get; private set; }

    public override void Load()
    {
        ReiDash = KeybindLoader.RegisterKeybind(Mod, "Time Slip", "W");
    }

    public override void Unload()
    {
        ReiDash = null;
    }
}
