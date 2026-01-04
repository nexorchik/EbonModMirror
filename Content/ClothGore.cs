namespace EbonianMod.Content;

public class XCloth0 : ModGore
{
    public override string Texture => Helper.AssetPath + "Gores/" + Name;
    // Code from Spooky Mod
    public override void OnSpawn(Gore gore, IEntitySource source)
    {
        ChildSafety.SafeGore[gore.type] = true;
        gore.velocity = new Vector2(Main.rand.NextFloat() - 0.5f, Main.rand.NextFloat() * MathHelper.TwoPi);
        UpdateType = 910;
    }
    public override bool Update(Gore gore)
    {
        return base.Update(gore);
    }
}
public class XCloth1 : XCloth0 { 
    public override string Texture => Helper.AssetPath + "Gores/" + Name;}
public class XCloth2 : XCloth0 { 
    public override string Texture => Helper.AssetPath + "Gores/" + Name;}
public class XCloth3 : XCloth0 { 
    public override string Texture => Helper.AssetPath + "Gores/" + Name;}
public class XCloth4 : XCloth0 { 
    public override string Texture => Helper.AssetPath + "Gores/" + Name;}
