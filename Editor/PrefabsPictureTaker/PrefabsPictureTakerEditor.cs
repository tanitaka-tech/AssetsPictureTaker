#if UNITY_EDITOR
using System;
using System.Linq;
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
            
            
            var prefabsPictureTaker = target as PrefabsPictureTaker;
            
            if (!isDuringTaking && GUILayout.Button("Take"))
            {
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
            
            // Test Prefab
            if (!isDuringTaking && prefabsPictureTaker.TestPrefabs.Any() && GUILayout.Button("Test Take"))
            {
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
                    foreach (var testPrefab in prefabsPictureTaker.TestPrefabs)
                    {
                        var instantiate = Instantiate(testPrefab, prefabsPictureTaker.InstantiateParentTransform);
                        prefabsPictureTaker.PrefabsPictureTakerSettingsScriptableObject.CaptureAndSave(instantiate, testPrefab.name, prefabsPictureTaker.RenderCamera);
                        DestroyImmediate(instantiate);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                isDuringTaking = false;
            }
        }
    }
}
#endif