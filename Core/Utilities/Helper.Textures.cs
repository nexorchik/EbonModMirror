namespace EbonianMod.Core.Utilities;

public static partial class Helper
{
	public static string AssetPath = "EbonianMod/Assets/Images/";
	public static string BuffPlaceholder = Helper.AssetPath + "Buffs/ExolStun";
	public static string Empty = Helper.AssetPath + "Extras/Empty";
	public static string Placeholder = Helper.AssetPath + "Extras/Placeholder";
	public static Asset<Texture2D> GetTexture(string path, AssetRequestMode assetRequestMode = AssetRequestMode.AsyncLoad)
	{
		return path.StartsWith("EbonianMod/") ? Request<Texture2D>(path, assetRequestMode) : Request<Texture2D>("EbonianMod/" + path, assetRequestMode);
	}
	public static Asset<Texture2D> GetTextureAlt(string path, AssetRequestMode assetRequestMode = AssetRequestMode.AsyncLoad)
	{
		return EbonianMod.Instance.Assets.Request<Texture2D>(path, assetRequestMode);
	}
}