using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace EbonianMod.Common.Utilities;
public static class GenericExtensions
{
    public static void InvokeAllAndClear(this List<Action> list)
    {
        foreach (Action action in list)
            action?.Invoke();
        list.Clear();
    }
}
