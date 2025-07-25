/*using Daybreak.Common.Features.ModPanel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;

namespace EbonianMod.Common.UI;
public class EbonianPanel : ModPanelStyle
{
    readonly Color main_color = new(232, 51, 118);
    readonly Color secondary_color = new(148, 15, 82);
    public override Color ModifyEnabledTextColor(bool enabled, Color color)
    {
        return color = enabled ? main_color : secondary_color;
    }
    public override void PostSetHoverColors(UIPanel element, bool hovered)
    {
        element.BackgroundColor = main_color;
        element.BorderColor = secondary_color;
    }
}
*/