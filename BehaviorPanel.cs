using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppInterop.Runtime;
using Il2CppNinjaKiwi.Common;
using Il2CppSystem.Reflection;
using System;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using System.Linq;
using UnityEngine.Events;
using System.Threading;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using Il2CppTMPro;
using Il2CppAssets.Scripts.Models;
using Object = UnityEngine.Object;
using Il2CppSystem;
using Il2Cpp;
using Boolean = Il2CppSystem.Boolean;
using Action = System.Action;
using Math = System.Math;
using MelonLoader;

namespace TowerCheatV2
{
    public static class BehaviorPanel
    {
        public static ModHelperPanel GenerateFieldPanel(FieldInfo[] info, PropertyInfo[] propertyinfo, string BehaviorName)
        {
            int height = 300 + (info.Length * 200) + (propertyinfo.Length * 200);
            var panel = ModHelperPanel.Create(new Info("Panel", 0, 0, 800, height), VanillaSprites.MainBGPanelBlue);

            var name = panel.AddText(new Info("BehaviorName", 0, (height / 2) - 100, 700, 200), BehaviorName, 70);
            name.transform.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;

            var tower = InGame.instance.inputManager.SelectedTower?.tower.towerModel;

            float ypos = (height - 300) / 2;

            for (int i = 0; info.Length > i; i++)
            {
                string value = "";
                FieldInfo field = info[i];
                ypos -= 200;

                if (field.FieldType.IsType<int>())
                {
                    value = FieldBehaviorExtentions.GetValue(field, BehaviorName).Unbox<int>().ToString();
                    var button = panel.AddButton(new Info("IntChangeButton", 250, ypos, 300, 125), VanillaSprites.BlueBtnLong, new Action(() =>
                    {
                        PopupScreen.instance.SafelyQueue(screen => screen.ShowSetNamePopup(field.Name, "Value to set this field to", new System.Action<string>(s =>
                        {
                            if (!string.IsNullOrEmpty(s))
                            {
                                FieldBehaviorExtentions.SetInt(field, s, BehaviorName);
                            }
                        }), value));
                        PopupScreen.instance.SafelyQueue(screen => screen.ModifyField(tmpInputField =>
                        {
                            tmpInputField.textComponent.font = Fonts.Btd6FontBody;
                            tmpInputField.characterLimit = 10;
                            tmpInputField.characterValidation = TMP_InputField.CharacterValidation.Integer;
                        }));
                    }));
                    button.AddText(new Info("EditText", 0, 0, 200, 100), "Edit", 60);
                }
                if (field.FieldType.IsType<bool>())
                {
                    value = FieldBehaviorExtentions.GetValue(field, BehaviorName).Unbox<bool>().ToString();
                    ModHelperButton Button = null;
                    Button = panel.AddButton(new Info("BoolChangeButton", 250, ypos, 150, 150), null, new Action(() =>
                    {
                        FieldBehaviorExtentions.ToggleBool(field, Button, BehaviorName);
                    }));
                    Button.Button.SetSprite(FieldBehaviorExtentions.GetToggleButtonSprite(Boolean.Parse(value)));

                }
                if (field.FieldType.IsType<float>())
                {
                    value = Math.Round(FieldBehaviorExtentions.GetValue(field, BehaviorName).Unbox<float>(), 2).ToString();
                    var button = panel.AddButton(new Info("IntChangeButton", 250, ypos, 300, 125), VanillaSprites.BlueBtnLong, new Action(() =>
                    {
                        PopupScreen.instance.SafelyQueue(screen => screen.ShowSetNamePopup(field.Name, "Value to set this field to", new System.Action<string>(s =>
                        {
                            if (!string.IsNullOrEmpty(s))
                            {
                                FieldBehaviorExtentions.SetFloat(field, s, BehaviorName);
                            }
                        }), value));
                        PopupScreen.instance.SafelyQueue(screen => screen.ModifyField(tmpInputField =>
                        {
                            tmpInputField.textComponent.font = Fonts.Btd6FontBody;
                            tmpInputField.characterLimit = 10;
                            tmpInputField.characterValidation = TMP_InputField.CharacterValidation.Decimal;
                        }));
                    }));
                    button.AddText(new Info("EditText", 0, 0, 200, 100), "Edit", 60);
                }
                if (field.FieldType.IsType<double>())
                {
                    value = Math.Round(FieldBehaviorExtentions.GetValue(field, BehaviorName).Unbox<double>(), 2).ToString();
                    var button = panel.AddButton(new Info("DoubleChangeButton", 250, ypos, 300, 125), VanillaSprites.BlueBtnLong, new Action(() =>
                    {
                        PopupScreen.instance.SafelyQueue(screen => screen.ShowSetNamePopup(field.Name, "Value to set this field to", new System.Action<string>(s =>
                        {
                            if (!string.IsNullOrEmpty(s))
                            {
                                FieldBehaviorExtentions.SetDouble(field, s, BehaviorName);
                            }
                        }), value));
                        PopupScreen.instance.SafelyQueue(screen => screen.ModifyField(tmpInputField =>
                        {
                            tmpInputField.textComponent.font = Fonts.Btd6FontBody;
                            tmpInputField.characterLimit = 10;
                            tmpInputField.characterValidation = TMP_InputField.CharacterValidation.Decimal;
                        }));
                    }));
                    button.AddText(new Info("EditText", 0, 0, 200, 100), "Edit", 60);
                }

                var text = panel.AddText(new Info("FieldName", -150, ypos, 450, 200), field.Name + ": " + value, 75);
                text.transform.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            }
            for (int i = 0; propertyinfo.Length > i; i++)
            {
                string value = "";
                PropertyInfo field = propertyinfo[i];
                ypos -= 200;

                if (field.PropertyType.IsType<int>())
                {
                    value = PropertyBehaviorExtentions.GetValue(field, BehaviorName).Unbox<int>().ToString();
                    var button = panel.AddButton(new Info("IntChangeButton", 250, ypos, 300, 125), VanillaSprites.BlueBtnLong, new Action(() =>
                    {
                        PopupScreen.instance.SafelyQueue(screen => screen.ShowSetNamePopup(field.Name, "Value to set this field to", new System.Action<string>(s =>
                        {
                            if (!string.IsNullOrEmpty(s))
                            {
                                PropertyBehaviorExtentions.SetInt(field, s, BehaviorName);
                            }
                        }), value));
                        PopupScreen.instance.SafelyQueue(screen => screen.ModifyField(tmpInputField =>
                        {
                            tmpInputField.textComponent.font = Fonts.Btd6FontBody;
                            tmpInputField.characterLimit = 10;
                            tmpInputField.characterValidation = TMP_InputField.CharacterValidation.Integer;
                        }));
                    }));
                    button.AddText(new Info("EditText", 0, 0, 200, 100), "Edit", 60);
                }
                if (field.PropertyType.IsType<bool>())
                {
                    value = PropertyBehaviorExtentions.GetValue(field, BehaviorName).Unbox<bool>().ToString();
                    ModHelperButton Button = null;
                    Button = panel.AddButton(new Info("BoolChangeButton", 250, ypos, 150, 150), null, new Action(() =>
                    {
                        PropertyBehaviorExtentions.ToggleBool(field, Button, BehaviorName);
                    }));
                    Button.Button.SetSprite(FieldBehaviorExtentions.GetToggleButtonSprite(Boolean.Parse(value)));

                }
                if (field.PropertyType.IsType<float>())
                {
                    value = Math.Round(PropertyBehaviorExtentions.GetValue(field, BehaviorName).Unbox<float>()).ToString();
                    var button = panel.AddButton(new Info("IntChangeButton", 250, ypos, 300, 125), VanillaSprites.BlueBtnLong, new Action(() =>
                    {
                        PopupScreen.instance.SafelyQueue(screen => screen.ShowSetNamePopup(field.Name, "Value to set this field to", new System.Action<string>(s =>
                        {
                            if (!string.IsNullOrEmpty(s))
                            {
                                PropertyBehaviorExtentions.SetFloat(field, s, BehaviorName);
                            }
                        }), value));
                        PopupScreen.instance.SafelyQueue(screen => screen.ModifyField(tmpInputField =>
                        {
                            tmpInputField.textComponent.font = Fonts.Btd6FontBody;
                            tmpInputField.characterLimit = 10;
                            tmpInputField.characterValidation = TMP_InputField.CharacterValidation.Decimal;
                        }));
                    }));
                    button.AddText(new Info("EditText", 0, 0, 200, 100), "Edit", 60);
                }
                if (field.PropertyType.IsType<double>())
                {
                    value = Math.Round(PropertyBehaviorExtentions.GetValue(field, BehaviorName).Unbox<double>()).ToString();
                    var button = panel.AddButton(new Info("DoubleChangeButton", 250, ypos, 300, 125), VanillaSprites.BlueBtnLong, new Action(() =>
                    {
                        PopupScreen.instance.SafelyQueue(screen => screen.ShowSetNamePopup(field.Name, "Value to set this field to", new System.Action<string>(s =>
                        {
                            if (!string.IsNullOrEmpty(s))
                            {
                                PropertyBehaviorExtentions.SetDouble(field, s, BehaviorName);
                            }
                        }), value));
                        PopupScreen.instance.SafelyQueue(screen => screen.ModifyField(tmpInputField =>
                        {
                            tmpInputField.textComponent.font = Fonts.Btd6FontBody;
                            tmpInputField.characterLimit = 10;
                            tmpInputField.characterValidation = TMP_InputField.CharacterValidation.Decimal;
                        }));
                    }));
                    button.AddText(new Info("EditText", 0, 0, 200, 100), "Edit", 60);
                }

                var text = panel.AddText(new Info("FieldName", -150, ypos, 450, 200), field.Name + ": " + value, 75);
                text.transform.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            }

            return panel;
        }
        public static class PropertyBehaviorExtentions
        {
            public static Il2CppSystem.Object GetValue(PropertyInfo info, string BehaviorName)
            {
                var tower = InGame.instance.inputManager.SelectedTower?.tower;
                var model = tower.towerModel.Duplicate().Cast<TowerModel>();

                var behaviors = model.GetDescendants<Model>().ToList();
                behaviors.Add(model);

                return info.GetValue(behaviors.First(b => b.name == BehaviorName));
            }
            public static void SetValue(PropertyInfo info, Il2CppSystem.Object value, string BehaviorName)
            {
                var tower = InGame.instance.inputManager.SelectedTower?.tower;
                var model = tower.towerModel.Duplicate().Cast<TowerModel>();    

                var behaviors = model.GetDescendants<Model>().ToList();
                behaviors.Add(model);

                var modifingbehavior = behaviors.First(b => b.name == BehaviorName);

                info.SetValue(modifingbehavior, value);
                tower.UpdateRootModel(model);

                TowerCheatV2.panel.ScrollContent.transform.DestroyAllChildren();
                TowerCheatV2.panel.AddPanels();
            }


            public static void ToggleBool(PropertyInfo info, ModHelperButton button, string BehaviorName)
            {
                var tower = InGame.instance.inputManager.SelectedTower?.tower.towerModel;
                switch (GetValue(info, BehaviorName).Unbox<bool>())
                {
                    case false:
                        SetValue(info, true, BehaviorName);
                        break;
                    case true:
                        SetValue(info, false, BehaviorName);
                        break;
                }


            }
            public static void SetInt(PropertyInfo info, string value, string BehaviorName)
            {
                int intvalue = int.Parse(value);
                SetValue(info, intvalue, BehaviorName);
            }
            public static void SetFloat(PropertyInfo info, string value, string BehaviorName)
            {
                float floatvalue = float.Parse(value);
                SetValue(info, floatvalue, BehaviorName);
            }
            public static void SetDouble(PropertyInfo info, string value, string BehaviorName)
            {
                double doublevalue = double.Parse(value);
                SetValue(info, doublevalue, BehaviorName);
            }
        }
        public static class FieldBehaviorExtentions
        {
            public static Sprite GetToggleButtonSprite(bool value)
            {
                switch (value)
                {
                    case true:
                        return ModContent.GetSprite<TowerCheatV2>("OnBtn");
                        break;
                    case false:
                        return ModContent.GetSprite<TowerCheatV2>("OffBtn");
                        break;
                }
            }
            public static void SetValue(FieldInfo info, Il2CppSystem.Object value, string BehaviorName)
            {
                var tower = InGame.instance.inputManager.SelectedTower?.tower;
                var model = tower.towerModel.Duplicate().Cast<TowerModel>();

                var behaviors = model.GetDescendants<Model>().ToList();
                behaviors.Add(model);

                var modifingbehavior = behaviors.First(b => b.name == BehaviorName);

                info.SetValue(modifingbehavior, value);
                tower.UpdateRootModel(model);

                TowerCheatV2.panel.ScrollContent.transform.DestroyAllChildren();
                TowerCheatV2.panel.AddPanels();
            }
            public static Il2CppSystem.Object GetValue(FieldInfo info, string BehaviorName)
            {
                var tower = InGame.instance.inputManager.SelectedTower?.tower;
                var model = tower.towerModel.Duplicate().Cast<TowerModel>();

                var behaviors = model.GetDescendants<Model>().ToList();
                behaviors.Add(model);
                return info.GetValue(behaviors.First(b => b.name == BehaviorName));
            }

            public static void ToggleBool(FieldInfo info, ModHelperButton button, string BehaviorName)
            {
                var tower = InGame.instance.inputManager.SelectedTower?.tower.towerModel;
                switch (FieldBehaviorExtentions.GetValue(info, BehaviorName).Unbox<bool>())
                {
                    case false:
                        SetValue(info, true, BehaviorName);
                        break;
                    case true:
                        SetValue(info, false, BehaviorName);
                        break;
                }


            }
            public static void SetInt(FieldInfo info, string value, string BehaviorName)
            {
                int intvalue = int.Parse(value);
                SetValue(info, intvalue, BehaviorName);
            }
            public static void SetFloat(FieldInfo info, string value, string BehaviorName)
            {
                float floatvalue = float.Parse(value);
                SetValue(info, floatvalue, BehaviorName);
            }
            public static void SetDouble(FieldInfo info, string value, string BehaviorName)
            {
                double doublevalue = double.Parse(value);
                SetValue(info, doublevalue, BehaviorName);
            }
        }
    }
}
