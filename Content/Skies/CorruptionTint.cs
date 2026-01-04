using Terraria.Graphics.Effects;

namespace EbonianMod.Content.Skies;

public class BasicTint : CustomSky
{
    private bool isActive;
    private float intensity;

    public override void Update(GameTime gameTime)
    {
        if (isActive && intensity < 1f)
        {
            intensity += 0.01f;
        }
        else if (!isActive && intensity > 0)
        {
            intensity -= 0.01f;
        }
    }
    public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
    {
    }

    public override float GetCloudAlpha()
    {
        return 1f;
    }

    public override void Activate(Vector2 position, params object[] args)
    {
        isActive = true;
    }

    public override void Deactivate(params object[] args)
    {
        isActive = false;
    }

    public override void Reset()
    {
        isActive = false;
    }

    public override bool IsActive()
    {
        return isActive || intensity > 0;
    }
}
