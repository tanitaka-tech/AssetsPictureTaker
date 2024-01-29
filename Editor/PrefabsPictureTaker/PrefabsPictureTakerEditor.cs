using UnityEditor;
using UnityEngine;

namespace TanitakaTech.AssetsPictureTaker.PrefabsPictureTaker
{
    [CustomEditor(typeof(PrefabsPictureTaker))]
    public class PrefabsPictureTakerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (GUILayout.Button("Take"))
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

                prefabsPictureTaker.PrefabsPictureTakerSettingsScriptableObject.TakeCaptures(
                        prefabsPictureTaker.InstantiateParentTransform,
                        prefabsPictureTaker.RenderCamera)
                    .Forget();
            }
        }
    }
}