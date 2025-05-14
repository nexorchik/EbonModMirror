using System;
using System.Linq;

namespace EbonianMod.Common.Graphics;

public class RTHandler : ModSystem
{
    public static PixelationTarget pixelationTarget;
    public static InvisibleTarget invisTarget;
    public static GarbageTarget garbageTarget;
    public static XareusTarget xareusTarget;
    public override void Load()
    {
        Main.ContentThatNeedsRenderTargets.Add(pixelationTarget = new());
        Main.ContentThatNeedsRenderTargets.Add(invisTarget = new());
        Main.ContentThatNeedsRenderTargets.Add(garbageTarget = new());
        Main.ContentThatNeedsRenderTargets.Add(xareusTarget = new());
    }
    public override void Unload()
    {
        Main.ContentThatNeedsRenderTargets.Remove(pixelationTarget);
        Main.ContentThatNeedsRenderTargets.Remove(invisTarget);
        Main.ContentThatNeedsRenderTargets.Remove(garbageTarget);
        Main.ContentThatNeedsRenderTargets.Remove(xareusTarget);
    }
}
public class PixelationTarget : ARenderTargetContentByRequest, INeedRenderTargetContent
{
    RenderTarget2D _target2;
    private void Reset()
    {
        base.Reset();
        _target2 = null;
    }
    protected override void HandleUseReqest(GraphicsDevice gd, SpriteBatch sb)
    {
        if (!EbonianMod.pixelationDrawCache.Any() && !EbonianMod.addPixelationDrawCache.Any())
            return;
        PrepareARenderTarget_AndListenToEvents(ref _target, gd, Main.screenWidth, Main.screenHeight, RenderTargetUsage.PlatformContents);
        PrepareARenderTarget_AndListenToEvents(ref _target2, gd, Main.screenWidth, Main.screenHeight, RenderTargetUsage.PlatformContents);
        var old = gd.GetRenderTargets();
        gd.SetRenderTargets(_target2);
        gd.Clear(Color.Transparent);

        sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null);

        foreach (Action draw in EbonianMod.pixelationDrawCache)
        {
            draw?.Invoke();
        }
        sb.End();

        sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null);
        foreach (Action draw in EbonianMod.addPixelationDrawCache)
        {
            draw?.Invoke();
        }
        sb.End();
        EbonianMod.addPixelationDrawCache.Clear();
        EbonianMod.pixelationDrawCache.Clear();

        gd.SetRenderTargets(_target);
        gd.Clear(Color.Transparent);
        sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, EbonianMod.colorQuant.Value, Matrix.Identity);
        EbonianMod.colorQuant.Value.Parameters["res"].SetValue(32);
        sb.Draw(_target2, new Rectangle(0, 0, (int)(Main.screenWidth / (1 + Main.GameZoomTarget)), (int)(Main.screenHeight / (1 + Main.GameZoomTarget))), Color.White);
        sb.End();

        gd.SetRenderTargets(old);
        _wasPrepared = true;
    }
}
public sealed class InvisibleTarget : ARenderTargetContentByRequest, INeedRenderTargetContent // if he's invisible how did he die
{
    public RenderTarget2D _target2;
    private void Reset()
    {
        base.Reset();
        _target2 = null;
    }
    protected override void HandleUseReqest(GraphicsDevice gd, SpriteBatch sb)
    {
        if (!EbonianMod.affectedByInvisibleMaskCache.Any() && !EbonianMod.invisibleMaskCache.Any())
            return;
        PrepareARenderTarget_AndListenToEvents(ref _target, gd, Main.screenWidth, Main.screenHeight, RenderTargetUsage.PlatformContents);
        PrepareARenderTarget_AndListenToEvents(ref _target2, gd, Main.screenWidth, Main.screenHeight, RenderTargetUsage.PlatformContents);
        var old = gd.GetRenderTargets();
        gd.SetRenderTargets(_target2);
        gd.Clear(Color.Transparent);

        sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
        foreach (Action draw in EbonianMod.affectedByInvisibleMaskCache)
        {
            draw?.Invoke();
        }
        EbonianMod.affectedByInvisibleMaskCache.Clear();
        sb.End();

        gd.SetRenderTargets(_target);
        gd.Clear(Color.Transparent);

        sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
        foreach (Action draw in EbonianMod.invisibleMaskCache)
        {
            draw?.Invoke();
        }
        EbonianMod.invisibleMaskCache.Clear();
        sb.End();

        gd.SetRenderTargets(old);
        _wasPrepared = true;
    }
}
public sealed class GarbageTarget : ARenderTargetContentByRequest
{
    protected override void HandleUseReqest(GraphicsDevice gd, SpriteBatch sb)
    {
        if (!EbonianMod.garbageFlameCache.Any()) return;
        PrepareARenderTarget_AndListenToEvents(ref _target, gd, Main.screenWidth, Main.screenHeight, RenderTargetUsage.PlatformContents);
        var old = gd.GetRenderTargets();
        gd.SetRenderTargets(_target);
        gd.Clear(Color.Transparent);

        sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        foreach (Action draw in EbonianMod.garbageFlameCache)
        {
            draw?.Invoke();
        }
        sb.End();
        EbonianMod.garbageFlameCache.Clear();

        gd.SetRenderTargets(old);
        _wasPrepared = true;
    }
}
public sealed class XareusTarget : ARenderTargetContentByRequest
{
    protected override void HandleUseReqest(GraphicsDevice gd, SpriteBatch sb)
    {
        if (!EbonianMod.xareusGoopCache.Any()) return;
        PrepareARenderTarget_AndListenToEvents(ref _target, gd, Main.screenWidth, Main.screenHeight, RenderTargetUsage.PlatformContents);
        var old = gd.GetRenderTargets();
        gd.SetRenderTargets(_target);
        gd.Clear(Color.Transparent);

        sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        foreach (Action draw in EbonianMod.xareusGoopCache)
        {
            draw?.Invoke();
        }
        sb.End();

        gd.SetRenderTargets(old);
        _wasPrepared = true;
    }
}
