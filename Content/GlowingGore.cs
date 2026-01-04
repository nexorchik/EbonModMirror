using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.Content;

public abstract class GlowGore : ModGore
{
    public override Color? GetAlpha(Gore gore, Color lightColor) => Color.White * (1f - (float)gore.alpha / 255f);
}
public class StarG0 : GlowGore {
    public override string Texture => Helper.AssetPath + "Gores/" + Name;
}
public class StarG1 : GlowGore {
    public override string Texture => Helper.AssetPath + "Gores/" + Name;
}
public class StarG2 : GlowGore { 
    public override string Texture => Helper.AssetPath + "Gores/" + Name;
}
public class StarG3 : GlowGore { 
    public override string Texture => Helper.AssetPath + "Gores/" + Name;
}
public class StarG4 : GlowGore {
    public override string Texture => Helper.AssetPath + "Gores/" + Name;
}
public class StarG5 : GlowGore {
    public override string Texture => Helper.AssetPath + "Gores/" + Name;
}
public class StarG6 : GlowGore {
    public override string Texture => Helper.AssetPath + "Gores/" + Name;
}
public class HerderStarGore : GlowGore
{
    public override string Texture => Helper.AssetPath+"NPCs/Overworld/Asteroid/AsteroidHerder_Star";
}
public class HerderStarGore2 : GlowGore
{
    public override string Texture => Helper.AssetPath+"NPCs/Overworld/Asteroid/AsteroidHerder_StarSmall";
}
