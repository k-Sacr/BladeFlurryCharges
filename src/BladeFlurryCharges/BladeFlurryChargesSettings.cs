using PoeHUD.Hud.Settings;
using PoeHUD.Plugins;

namespace BladeFlurryCharges
{
    public class BladeFlurryChargesSettings : SettingsBase
    {
        public BladeFlurryChargesSettings()
        {
            LeftClick = false;
        }
        
        [Menu("Use Left Click", 1)]
        public ToggleNode LeftClick { get; set; }
    }
}
