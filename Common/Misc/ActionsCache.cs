using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.Common.Misc;
public class ActionsCache : List<Action>, ILoadable
{
    public void InvokeAllAndClear()
    {
        foreach (Action action in this)
            action?.Invoke();
        Clear();
    }
    public void Load(Mod mod) { }
    public void Unload() => Clear();
}
