namespace EbonianMod.Core.Utilities;

public static partial class Helper
{
	public const string AssetPath = "EbonianMod/Assets/Images/";
	public const string BuffPlaceholder = AssetPath + "Buffs/ExolStun";
	public const string Empty = AssetPath + "Extras/Empty";
	public const string Placeholder = AssetPath + "Extras/Placeholder";
	public static Asset<Texture2D> GetTexture(string path, AssetRequestMode assetRequestMode = AssetRequestMode.AsyncLoad)
	{
		return path.StartsWith("EbonianMod/") ? Request<Texture2D>(path, assetRequestMode) : Request<Texture2D>("EbonianMod/" + path, assetRequestMode);
	}
	public static Asset<Texture2D> GetTextureAlt(string path, AssetRequestMode assetRequestMode = AssetRequestMode.AsyncLoad)
	{
		return EbonianMod.Instance.Assets.Request<Texture2D>(path, assetRequestMode);
	}
}