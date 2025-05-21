using EbonianMod.Common.Misc;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace EbonianMod.Common.Graphics;
public class RTHandler : ModSystem
{
    public static PixelationTarget pixelationTarget => GetInstance<PixelationTarget>();
    public static InvisibleTarget invisTarget => GetInstance<InvisibleTarget>();
    public static GarbageTarget garbageTarget => GetInstance<GarbageTarget>();
    public static XareusTarget xareusTarget => GetInstance<XareusTarget>();
}
public abstract class CommonRenderTarget : ARenderTargetContentByRequest, INeedRenderTargetContent, ILoadable
{
    public RenderTarget2D _target2;
    public virtual ActionsCache[] Actions => null;
    public void InvokeActions(int index)
    {
        if (Actions == null)
            return;
        if (Actions.Length > index && index >= 0)
        {
            Actions[index].InvokeAllAndClear();
        }
    }
    public void InvokeActions(ActionsCache actions)
    {
        if (Actions == null)
            return;
        InvokeActions(Array.IndexOf(Actions, actions));
    }
    public void RequestAndPrepare()
    {
        Request();
        PrepareRenderTarget(Main.graphics.GraphicsDevice, Main.spriteBatch);
    }
    public void PrepareATarget(ref RenderTarget2D rt, GraphicsDevice gd, int? width = null, int? height = null, RenderTargetUsage usage = RenderTargetUsage.PlatformContents) =>
        PrepareARenderTarget_AndListenToEvents(ref rt, gd, width ?? Main.screenWidth, height ?? Main.screenHeight, usage);
    public void PrepareAndSet(ref RenderTarget2D rt, GraphicsDevice gd, int? width = null, int? height = null,
        RenderTargetUsage usage = RenderTargetUsage.PlatformContents, Color? clearColor = null)
    {
        PrepareATarget(ref rt, gd, width, height, usage);
        gd.SetRenderTarget(rt);
        gd.Clear(clearColor ?? Color.Transparent);
    }
    protected sealed override void HandleUseReqest(GraphicsDevice device, SpriteBatch spriteBatch)
    {
        var old = device.GetRenderTargets();

        if (Actions?.Any() ?? false)
        {
            foreach (ActionsCache action in Actions)
            {
                if (!action.Any())
                    return;
            }
        }
        HandleUseRequest(device, spriteBatch); // Advancement. Evolution. Progress.

        device.SetRenderTargets(old);
        _wasPrepared = true;
    }

    public abstract void HandleUseRequest(GraphicsDevice device, SpriteBatch spriteBatch);
    void ILoadable.Load(Mod mod) => Main.ContentThatNeedsRenderTargets.Add(this);
    void ILoadable.Unload() => Main.ContentThatNeedsRenderTargets.Remove(this);
    void INeedRenderTargetContent.Reset()
    {
        base.Reset();
        _target2 = null;
        OnReset();
    }
    public virtual void OnReset() { }
}
public class PixelationTarget : CommonRenderTarget
{
    public override ActionsCache[] Actions => [EbonianMod.pixelationDrawCache];
    public override void HandleUseRequest(GraphicsDevice gd, SpriteBatch sb)
    {
        PrepareAndSet(ref _target2, gd);
        sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        InvokeActions(0);
        sb.End();

        PrepareAndSet(ref _target, gd);
        sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, EbonianMod.colorQuant.Value, Matrix.Identity);
        EbonianMod.colorQuant.Value.Parameters["res"].SetValue(32);
        sb.Draw(_target2, new Rectangle(0, 0, (int)(Main.screenWidth / (1 + Main.GameZoomTarget)), (int)(Main.screenHeight / (1 + Main.GameZoomTarget))), Color.White);
        sb.End();
    }
}
public sealed class InvisibleTarget : CommonRenderTarget// if he's invisible how did he die
{
    public override ActionsCache[] Actions => [EbonianMod.affectedByInvisibleMaskCache, EbonianMod.invisibleMaskCache];
    public override void HandleUseRequest(GraphicsDevice gd, SpriteBatch sb)
    {
        PrepareAndSet(ref _target2, gd);
        sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        InvokeActions(0);
        sb.End();

        PrepareAndSet(ref _target, gd);
        sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        InvokeActions(1);
        sb.End();
    }
}
public sealed class GarbageTarget : CommonRenderTarget
{
    public override ActionsCache[] Actions => [EbonianMod.garbageFlameCache];
    public override void HandleUseRequest(GraphicsDevice gd, SpriteBatch sb)
    {
        PrepareAndSet(ref _target, gd);
        sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        InvokeActions(0);
        sb.End();
    }
}
public sealed class XareusTarget : CommonRenderTarget
{
    public override ActionsCache[] Actions => [EbonianMod.xareusGoopCache];
    public override void HandleUseRequest(GraphicsDevice gd, SpriteBatch sb)
    {
        PrepareAndSet(ref _target, gd);
        sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        InvokeActions(0);
        sb.End();
    }
}
