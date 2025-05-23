using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.Gores;

public abstract class GlowGore : ModGore
{
    public override Color? GetAlpha(Gore gore, Color lightColor) => Color.White * (1f - (float)gore.alpha / 255f);
}
public class StarG0 : GlowGore { }
public class StarG1 : GlowGore { }
public class StarG2 : GlowGore { }
public class StarG3 : GlowGore { }
public class StarG4 : GlowGore { }
public class StarG5 : GlowGore { }
public class StarG6 : GlowGore { }
