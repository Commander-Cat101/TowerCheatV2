using MelonLoader;
using BTD_Mod_Helper;
using TowerCheatV2;
using UnityEngine;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using BTD_Mod_Helper.Extensions;
using static MelonLoader.MelonLogger;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Unity.Towers.Behaviors.Attack;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Unity.Towers.Upgrades;
using UnityEngine.UIElements;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Models.Towers.Mods;
using System.Linq;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppNinjaKiwi.Common;
using Il2CppAssets.Scripts.Simulation.Towers;

[assembly: MelonInfo(typeof(TowerCheatV2.TowerCheatV2), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace TowerCheatV2;

public class TowerCheatV2 : BloonsTD6Mod
{
    static bool MatchStarted = false;
    
    static bool IsMenuOpen = false;
    static GameObject upgrades;
    static GameObject tower;
    static GameObject editbutton;
    public static ModHelperScrollPanel panel;
    static TowerModel LastSelected;
    public override void OnApplicationStart()
    {
        ModHelper.Msg<TowerCheatV2>("TowerCheatV2 loaded!");
    }
    public override void OnMatchStart()
    {
        MatchStarted = true;

        tower = GameObject.Find("TowerElements");
        upgrades = GameObject.Find("SelectedTowerOptions");
        var themes = GameObject.Find("SelectedTowerInformation");
        panel = tower.AddModHelperScrollPanel(new Info("TowerEditPanel", 0, -333, 900, 1100), RectTransform.Axis.Vertical, VanillaSprites.BrownInsertPanel, 50, 100);
        panel.ScrollRect.verticalScrollbarSpacing = 100;
        panel.SetActive(false);

        var editpanel = themes.AddModHelperPanel(new Info("EditButton"));
        editbutton = editpanel.AddButton(new Info("EditButton", -372, -200, 120, 120), VanillaSprites.EditBtn, new System.Action(() =>
        {
            Open();
        }));
    }

    public override void OnMatchEnd()
    {
        MatchStarted = false;
    }

    public override void OnTowerSelected(Tower tower)
    {
        base.OnTowerSelected(tower);

        IsMenuOpen = false;
        upgrades.active = true;
        panel.SetActive(false);
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Input.GetKeyUp(KeyCode.P))
        {
            var tower = InGame.instance.inputManager.SelectedTower?.tower.towerModel;

            string log = "";

            var FieldInfo = Il2CppType.Of<TowerModel>().GetFields().Where(type => type.FieldType.IsValidType()).ToArray();



            foreach (var fieldInfo in FieldInfo)
            {
                switch (fieldInfo)
                {
                    case not null when fieldInfo.FieldType.IsType<float>():
                        log += fieldInfo.Name + " : " + fieldInfo.FieldType.FullName + " : " + fieldInfo.IsPrivate + " : " + fieldInfo.GetValue(tower).Unbox<float>() + "\n";
                        break;
                    case not null when fieldInfo.FieldType.IsType<int>():
                        log += fieldInfo.Name + " : " + fieldInfo.FieldType.FullName + " : " + fieldInfo.IsPrivate + " : " + fieldInfo.GetValue(tower).Unbox<int>() + "\n";
                        break;
                    case not null when fieldInfo.FieldType.IsType<bool>():
                        log += fieldInfo.Name + " : " + fieldInfo.FieldType.FullName + " : " + fieldInfo.IsPrivate + " : " + fieldInfo.GetValue(tower).Unbox<bool>() + "\n";
                        break;
                    case not null when fieldInfo.FieldType.IsType<long>():
                        log += fieldInfo.Name + " : " + fieldInfo.FieldType.FullName + " : " + fieldInfo.IsPrivate + " : " + fieldInfo.GetValue(tower).Unbox<long>() + "\n";
                        break;
                    case not null when fieldInfo.FieldType.IsType<uint>():
                        log += fieldInfo.Name + " : " + fieldInfo.FieldType.FullName + " : " + fieldInfo.IsPrivate + " : " + fieldInfo.GetValue(tower).Unbox<uint>() + "\n";
                        break;
                    case not null when fieldInfo.FieldType.IsType<ulong>():
                        log += fieldInfo.Name + " : " + fieldInfo.FieldType.FullName + " : " + fieldInfo.IsPrivate + " : " + fieldInfo.GetValue(tower).Unbox<ulong>() + "\n";
                        break;
                    case not null when fieldInfo.FieldType.IsType<short>():
                        log += fieldInfo.Name + " : " + fieldInfo.FieldType.FullName + " : " + fieldInfo.IsPrivate + " : " + fieldInfo.GetValue(tower).Unbox<short>() + "\n";
                        break;
                    case not null when fieldInfo.FieldType.IsType<ushort>():
                        log += fieldInfo.Name + " : " + fieldInfo.FieldType.FullName + " : " + fieldInfo.IsPrivate + " : " + fieldInfo.GetValue(tower).Unbox<ushort>() + "\n";
                        break;
                    case not null when fieldInfo.FieldType.IsType<double>():
                        log += fieldInfo.Name + " : " + fieldInfo.FieldType.FullName + " : " + fieldInfo.IsPrivate + " : " + fieldInfo.GetValue(tower).Unbox<double>() + "\n";
                        break;
                }
                //log += fieldInfo.Name + " : " + fieldInfo.FieldType.FullName + " : " + fieldInfo.IsPrivate + " : " + fieldInfo.GetValue(tower) + "\n";
            }
            Msg(log);
        }
        if (Input.GetKeyUp(KeyCode.F1))
        {
            Open();
        }
    }
    public void Open()
    {
        if (MatchStarted)
        {
            switch (IsMenuOpen)
            {
                case true:
                    IsMenuOpen = false;
                    upgrades.active = true;
                    panel.SetActive(false);
                    break;
                case false:
                    if (InGame.instance.inputManager.SelectedTower != null)
                    {
                        IsMenuOpen = true;
                        upgrades.active = false;
                        panel.SetActive(true);

                        panel.ScrollContent.transform.DestroyAllChildren();
                        TowerCheatV2.panel.AddPanels();
                    }
                    break;
            }
            LastSelected = InGame.instance.inputManager.SelectedTower.tower.rootModel.Cast<TowerModel>();
        }
    }
}
public static class Extentions
{
    public static bool IsType<T>(this Type typ)
    {
        var ty = Il2CppType.From(typeof(T));
        return ty.IsAssignableFrom(typ);
    }

    public static bool IsValidType(this Type type)
    {
        if (type.IsType<int>())
        {
            return true;
        }
        if (type.IsType<bool>())
        {
            return true;
        }
        if (type.IsType<float>())
        {
            return true;
        }
        if (type.IsType<double>())
        {
            return true;
        }
        return false;
    }
}