namespace EbonianMod.Common.Utilities;

public readonly struct SpritebatchParameters
{
    public SpriteSortMode sortMode { get; init; }
    public BlendState blendState { get; init; }
    public SamplerState samplerState { get; init; }
    public DepthStencilState depthStencilState { get; init; }
    public RasterizerState rasterizerState { get; init; }
    public Effect effect { get; init; }
    public Matrix matrix { get; init; }
    public SpritebatchParameters(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix matrix)
    {
        this.sortMode = sortMode;
        this.blendState = blendState;
        this.samplerState = samplerState;
        this.depthStencilState = depthStencilState;
        this.rasterizerState = rasterizerState;
        this.effect = effect;
        this.matrix = matrix;
    }
}
public static class SpritebatchExtensions
{
    public static SpritebatchParameters Snapshot(this SpriteBatch spriteBatch)
    {
        SpriteSortMode sortMode = (SpriteSortMode)ReflectionSystem.sbSortMode.GetValue(spriteBatch);
        BlendState blendState = (BlendState)ReflectionSystem.sbBlendState.GetValue(spriteBatch);
        SamplerState samplerState = (SamplerState)ReflectionSystem.sbSamplerState.GetValue(spriteBatch);
        DepthStencilState depthStencilState = (DepthStencilState)ReflectionSystem.sbDepthStencilState.GetValue(spriteBatch);
        RasterizerState rasterizerState = (RasterizerState)ReflectionSystem.sbRasterizerState.GetValue(spriteBatch);
        Effect effect = (Effect)ReflectionSystem.sbCustomEffect.GetValue(spriteBatch);
        Matrix matrix = (Matrix)ReflectionSystem.sbTransformMatrix.GetValue(spriteBatch);
        SpritebatchParameters sbParams = new(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
        return sbParams;
    }

    public static void ApplySaved(this SpriteBatch spriteBatch, SpritebatchParameters sbParams)
    {
        if ((bool)ReflectionSystem.sbBeginCalled.GetValue(spriteBatch))
        {
            spriteBatch.End();
        }
        SpriteSortMode sortMode = sbParams.sortMode;
        BlendState blendState = sbParams.blendState;
        SamplerState samplerState = sbParams.samplerState;
        DepthStencilState depthStencilState = sbParams.depthStencilState;
        RasterizerState rasterizerState = sbParams.rasterizerState;
        Effect effect = sbParams.effect;
        spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, sbParams.matrix);
    }
    public static void BeginDefault(this SpriteBatch spriteBatch) => spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
    public static bool BeginCalled(this SpriteBatch spriteBatch) => (bool)ReflectionSystem.sbBeginCalled.GetValue(spriteBatch);
    public static void Reload(this SpriteBatch spriteBatch, SamplerState _samplerState = default)
    {
        if ((bool)ReflectionSystem.sbBeginCalled.GetValue(spriteBatch))
        {
            spriteBatch.End();
        }
        SpriteSortMode sortMode = (SpriteSortMode)ReflectionSystem.sbSortMode.GetValue(spriteBatch);
        SamplerState samplerState = _samplerState;
        BlendState blendState = (BlendState)ReflectionSystem.sbBlendState.GetValue(spriteBatch);
        DepthStencilState depthStencilState = (DepthStencilState)ReflectionSystem.sbDepthStencilState.GetValue(spriteBatch);
        RasterizerState rasterizerState = (RasterizerState)ReflectionSystem.sbRasterizerState.GetValue(spriteBatch);
        Effect effect = (Effect)ReflectionSystem.sbCustomEffect.GetValue(spriteBatch);
        Matrix matrix = (Matrix)ReflectionSystem.sbTransformMatrix.GetValue(spriteBatch);
        spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
    }
    public static void Reload(this SpriteBatch spriteBatch, SpriteSortMode sortMode = SpriteSortMode.Deferred)
    {
        if ((bool)ReflectionSystem.sbBeginCalled.GetValue(spriteBatch))
        {
            spriteBatch.End();
        }
        BlendState blendState = (BlendState)ReflectionSystem.sbBlendState.GetValue(spriteBatch);
        SamplerState samplerState = (SamplerState)ReflectionSystem.sbSamplerState.GetValue(spriteBatch);
        DepthStencilState depthStencilState = (DepthStencilState)ReflectionSystem.sbDepthStencilState.GetValue(spriteBatch);
        RasterizerState rasterizerState = (RasterizerState)ReflectionSystem.sbRasterizerState.GetValue(spriteBatch);
        Effect effect = (Effect)ReflectionSystem.sbCustomEffect.GetValue(spriteBatch);
        Matrix matrix = (Matrix)ReflectionSystem.sbTransformMatrix.GetValue(spriteBatch);
        spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
    }
    public static void Reload(this SpriteBatch spriteBatch, BlendState blendState = default)
    {
        if ((bool)ReflectionSystem.sbBeginCalled.GetValue(spriteBatch))
        {
            spriteBatch.End();
        }
        SpriteSortMode sortMode = (SpriteSortMode)ReflectionSystem.sbSortMode.GetValue(spriteBatch);
        SamplerState samplerState = (SamplerState)ReflectionSystem.sbSamplerState.GetValue(spriteBatch);
        DepthStencilState depthStencilState = (DepthStencilState)ReflectionSystem.sbDepthStencilState.GetValue(spriteBatch);
        RasterizerState rasterizerState = (RasterizerState)ReflectionSystem.sbRasterizerState.GetValue(spriteBatch);
        Effect effect = (Effect)ReflectionSystem.sbCustomEffect.GetValue(spriteBatch);
        Matrix matrix = (Matrix)ReflectionSystem.sbTransformMatrix.GetValue(spriteBatch);
        spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
    }
    public static void Reload(this SpriteBatch spriteBatch, Effect effect = null)
    {
        if ((bool)ReflectionSystem.sbBeginCalled.GetValue(spriteBatch))
        {
            spriteBatch.End();
        }
        SpriteSortMode sortMode = (SpriteSortMode)ReflectionSystem.sbSortMode.GetValue(spriteBatch);
        BlendState blendState = (BlendState)ReflectionSystem.sbBlendState.GetValue(spriteBatch);
        SamplerState samplerState = (SamplerState)ReflectionSystem.sbSamplerState.GetValue(spriteBatch);
        DepthStencilState depthStencilState = (DepthStencilState)ReflectionSystem.sbDepthStencilState.GetValue(spriteBatch);
        RasterizerState rasterizerState = (RasterizerState)ReflectionSystem.sbRasterizerState.GetValue(spriteBatch);
        Matrix matrix = (Matrix)ReflectionSystem.sbTransformMatrix.GetValue(spriteBatch);
        spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
    }
}
