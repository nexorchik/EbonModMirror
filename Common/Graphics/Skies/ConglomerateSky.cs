using System;
using Terraria.Graphics.Effects;

namespace EbonianMod.Common.Graphics.Skies;

internal class ConglomerateSky : CustomSky
{
    private bool isActive;
    private float intensity;
    public int conglomerateIndex;
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
        GraphicsDevice gd = Main.graphics.GraphicsDevice;
        SpriteBatch sb = Main.spriteBatch;
        Rectangle rect2 = new Rectangle(-1000, -1000, 4000, 3000);
        SpritebatchParameters sbParams = sb.Snapshot();
        if (!Main.gameMenu)
        {
            if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f)
            {
                //sb.Draw(Helper.GetExtraTexture("black"), rect, Color.White * intensity);
                SpritebatchParameters _sbParams = Main.spriteBatch.Snapshot();
                Main.spriteBatch.End();
                sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);
                EbonianMod.waterEffect.Value.CurrentTechnique.Passes[0].Apply();
                gd.Textures[1] = ExtraTextures.swirlyNoise.Value;
                gd.Textures[2] = ExtraTextures.waterNoise.Value;
                gd.Textures[3] = ExtraTextures.vein.Value;
                EbonianMod.waterEffect.Value.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * 0.015f);
                EbonianMod.waterEffect.Value.Parameters["totalAlpha"].SetValue(0.5f * intensity * EbonianSystem.conglomerateSkyFlash);

                EbonianMod.waterEffect.Value.Parameters["offset"].SetValue(.05f + Clamp(EbonianSystem.conglomerateSkyFlash, 0, 10) * 0.01f);

                EbonianMod.waterEffect.Value.Parameters["mainScale"].SetValue(.7f);
                EbonianMod.waterEffect.Value.Parameters["secondaryScale"].SetValue(2f);
                EbonianMod.waterEffect.Value.Parameters["tertiaryScale"].SetValue(3.5f);

                EbonianMod.waterEffect.Value.Parameters["mainDirection"].SetValue(new Vector2(MathF.Sin(Main.GlobalTimeWrappedHourly * -0.0255f) * 0.4f, .45f));
                EbonianMod.waterEffect.Value.Parameters["secondaryDirection"].SetValue(new Vector2(MathF.Sin(Main.GlobalTimeWrappedHourly * 0.025f) * 0.4f, -.45f));
                EbonianMod.waterEffect.Value.Parameters["tertiaryDirection"].SetValue(new Vector2(-1.9f, 0));
                //sb.Draw(Helper.GetExtraTexture("conglomerateSky2"), npc.Center - Main.screenPosition, null, Color.White * intensity * 0.5f, npc.rotation, Helper.GetExtraTexture("conglomerateSky2").Size() / 2, 2 + MathF.Sin(Main.GlobalTimeWrappedHourly * 3) * 0.1f, SpriteEffects.None, 0);
                sb.Draw(ExtraTextures.conglomerateSky.Value, rect2, Color.White * intensity * 2);
                sb.ApplySaved(_sbParams);
            }
            SpritebatchParameters __sbParams = Main.spriteBatch.Snapshot();
            Main.spriteBatch.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone);
            EbonianMod.waterEffect.Value.CurrentTechnique.Passes[0].Apply();
            gd.Textures[1] = ExtraTextures.seamlessNoise.Value;
            gd.Textures[2] = ExtraTextures.waterNoise.Value;
            gd.Textures[3] = ExtraTextures.swirlyNoise.Value;
            EbonianMod.waterEffect.Value.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * 0.03f);
            EbonianMod.waterEffect.Value.Parameters["totalAlpha"].SetValue(intensity * (0.3f * EbonianSystem.conglomerateSkyFlash));

            EbonianMod.waterEffect.Value.Parameters["offset"].SetValue(.05f);
            EbonianMod.waterEffect.Value.Parameters["colOverride"].SetValue(GetInstance<EbonianSystem>().conglomerateSkyColorOverride.ToVector4());

            EbonianMod.waterEffect.Value.Parameters["mainScale"].SetValue(3);
            EbonianMod.waterEffect.Value.Parameters["secondaryScale"].SetValue(3f);
            EbonianMod.waterEffect.Value.Parameters["tertiaryScale"].SetValue(3);

            EbonianMod.waterEffect.Value.Parameters["mainDirection"].SetValue(new Vector2(MathF.Sin(Main.GlobalTimeWrappedHourly * -0.015f) * 0.1f, .45f));
            EbonianMod.waterEffect.Value.Parameters["secondaryDirection"].SetValue(new Vector2(MathF.Sin(Main.GlobalTimeWrappedHourly * 0.015f) * 0.1f, -.55f));
            EbonianMod.waterEffect.Value.Parameters["tertiaryDirection"].SetValue(new Vector2(2.9f, 0));

            //sb.Draw(Helper.GetExtraTexture("conglomerateSky2"), npc.Center - Main.screenPosition, null, Color.White * intensity * 0.5f, npc.rotation, Helper.GetExtraTexture("conglomerateSky2").Size() / 2, 2 + MathF.Sin(Main.GlobalTimeWrappedHourly * 3) * 0.1f, SpriteEffects.None, 0);
            sb.Draw(ExtraTextures.conglomerateSky.Value, rect2, Color.White * intensity * 2);
            sb.ApplySaved(__sbParams);
        }
        sb.ApplySaved(sbParams);
    }

    public override float GetCloudAlpha()
    {
        return 1f;
    }
    public override Color OnTileColor(Color inColor)
    {
        return inColor;
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
