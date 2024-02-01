#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.SettingsManagement;
using UnityEditorInternal;
using UnityEngine;

namespace TanitakaTech.AssetsPictureTaker
{
    [PublicAPI]
    public static class AssetsPictureTakerSettingsManager
    {
        public class ProjectSetting<T> : UserSetting<T>
        {
            internal ProjectSetting(string key, T value) : base(Settings, key, value, SettingsScope.Project)
            {
            }
        }

        private const string MenuPrefixBase = "TanitakaTech/Assets Picture Taker";
        private const string SettingsMenuPath = "Project/" + MenuPrefixBase;
        internal const string SettingsMenuPathForDisplay = "Project Settings/" + MenuPrefixBase;
        private static readonly Assembly ContainingAssembly = typeof(AssetsPictureTakerSettingsManager).Assembly;
        private static readonly Settings Settings = new Settings("com.tanitaka-tech.assets-picture-taker");
        
        [SettingsProvider]
        private static SettingsProvider CreateUserSettingsProvider() => new UserSettingsProvider(SettingsMenuPath, Settings, new[] { ContainingAssembly }, SettingsScope.Project);
        
        [PublicAPI]
        public static class PrefabsPictureTaker
        {
            private const string CategoryName = "0_PrefabsPictureTaker";

            // [field: UserSetting(
            //     category: CategoryName, 
            //     title: "PrefabPictureTakerSettingsPreset",
            //     tooltip: "")]
            // public static ProjectSetting<PrefabsPictureTakerSettingsScriptableObject> PrefabPictureTakerSettingsPreset { get; }
            //     = new("PrefabPictureTakerSettingsPreset", null);
            
            // [UserSettingBlock(CategoryName)]
            // private static void Draw(string searchContext)
            // {
            //     PrefabPictureTakerSettingsPreset.value = (PrefabsPictureTakerSettingsScriptableObject)EditorGUILayout.ObjectField("PrefabPictureTakerSettingsPreset", PrefabPictureTakerSettingsPreset.value, typeof(PrefabsPictureTakerSettingsScriptableObject), false);
            // }
        }
        
        private static ReorderableList DrawReorderableList<T>(List<T> displayList, Action<Rect, int> onDrawElementCallback, Action onModifyListCallback)
        {
            ReorderableList ret = null;
            ret = new ReorderableList(displayList, typeof(T), true, false, true, true)
            {
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    onDrawElementCallback?.Invoke(rect, index);
                    onModifyListCallback?.Invoke();
                },
                onAddCallback = (list) =>
                {
                    displayList.Add(default);
                    onModifyListCallback?.Invoke();
                    ret.DoLayoutList();
                },
                onRemoveCallback = (list) =>
                {
                    displayList.RemoveAt(list.index);
                    onModifyListCallback?.Invoke();
                    ret.DoLayoutList();
                },
                onSelectCallback = (list) =>
                {
                    list.Select(list.index);
                }, 
                onReorderCallbackWithDetails	 = (list, oldIndex, newIndex) =>
                {
                    (displayList[newIndex], displayList[oldIndex]) = (displayList[oldIndex], displayList[newIndex]);
                    onModifyListCallback?.Invoke();
                    ret.DoLayoutList();
                },
                    
            };
            ret.DoLayoutList();
            return ret;
        }
    }
}
#endif