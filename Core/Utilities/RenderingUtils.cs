using Terraria.Graphics.Shaders;

namespace EbonianMod.Core.Utilities;

public class RenderingUtils
{
	public static readonly BlendState Subtractive = new BlendState
	{
		ColorSourceBlend = Blend.SourceAlpha,
		ColorDestinationBlend = Blend.One,
		ColorBlendFunction = BlendFunction.ReverseSubtract,
		AlphaSourceBlend = Blend.SourceAlpha,
		AlphaDestinationBlend = Blend.One,
		AlphaBlendFunction = BlendFunction.ReverseSubtract
	};
	public static void DrawWithDye(SpriteBatch spriteBatch, DrawData data, int dye, Entity entity, bool Additive = false)
	{
		spriteBatch.End(out var sbParams);
		spriteBatch.Begin(SpriteSortMode.Immediate, Additive ? BlendState.Additive : BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
		GameShaders.Armor.GetShaderFromItemId(dye).Apply(null, data);
		data.Draw(Main.spriteBatch);
		spriteBatch.End();
		spriteBatch.Begin(sbParams);
	}
}