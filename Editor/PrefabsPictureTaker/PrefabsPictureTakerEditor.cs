using System;
using UnityEditor;
using UnityEngine;

namespace TanitakaTech.AssetsPictureTaker.PrefabsPictureTaker
{
    [CustomEditor(typeof(PrefabsPictureTaker))]
    public class PrefabsPictureTakerEditor : Editor
    {
        private static bool isDuringTaking = false;

        public override async void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (!isDuringTaking && GUILayout.Button("Take"))
            {
                var prefabsPictureTaker = target as PrefabsPictureTaker;
                
                if (prefabsPictureTaker.InstantiateParentTransform == null)
                {
                    Debug.LogError("Parent Transform is null");
                    return;
                }
                if (prefabsPictureTaker.RenderCamera == null)
                {
                    Debug.LogError("Render Camera is null");
                    return;
                }

                isDuringTaking = true;
                try
                {
                    await prefabsPictureTaker.PrefabsPictureTakerSettingsScriptableObject.TakeCaptures(
                        prefabsPictureTaker.InstantiateParentTransform,
                        prefabsPictureTaker.RenderCamera);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                isDuringTaking = false;
            }
            else if (isDuringTaking)
            {
                GUILayout.Label("Taking...");
            }
        }
    }
}