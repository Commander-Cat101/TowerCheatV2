using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.ServerEvents;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppFacepunch.Steamworks;
using Il2CppInterop.Runtime;
using Il2CppSystem.IO;
using Il2CppSystem.Reflection;
using MelonLoader;
using System.Linq;

namespace TowerCheatV2
{
    public static class PanelLoading
    {
        public static ModHelperScrollPanel AddPanels(this ModHelperScrollPanel panel)
        {
            var tower = InGame.instance.inputManager.SelectedTower?.tower.rootModel.Cast<TowerModel>();
            var models = tower.GetDescendants<Model>();

            var towerfields = tower.GetIl2CppType().GetFields().Where(a => a.FieldType.IsValidType()).ToArray();
            var towerproperties = tower.GetIl2CppType().GetProperties().Where(a => a.PropertyType.IsValidType()).ToArray();

            var newtowerpanel = BehaviorPanel.GenerateFieldPanel(towerfields, towerproperties, tower.name);
            panel.AddScrollContent(newtowerpanel);

            foreach (var behavior in models.ToArray())
            {
                var fields = behavior.GetIl2CppType().GetFields().Where(a => a.FieldType.IsValidType()).ToArray();
                var properties = behavior.GetIl2CppType().GetProperties().Where(a => a.PropertyType.IsValidType()).ToArray();


                if (fields.Length <= 0 && properties.Length <= 0)
                {
                    continue;
                }

                var newpanel = BehaviorPanel.GenerateFieldPanel(fields, properties, behavior.name);
                panel.AddScrollContent(newpanel);
            }
            return panel;
        }
        public static void LoadPanels()
        {

        }
    }
}
