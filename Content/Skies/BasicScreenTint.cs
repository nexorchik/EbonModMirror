using Terraria.Graphics.Shaders;

namespace EbonianMod.Content.Skies;

public class BasicScreenTint : ScreenShaderData
{
    private int index;

    public BasicScreenTint(string passName)
        : base(passName)
    {
    }

    public override void Apply()
    {
        if (index != -1)
        {
            UseTargetPosition(Main.screenPosition);
        }
        base.Apply();
    }
}
