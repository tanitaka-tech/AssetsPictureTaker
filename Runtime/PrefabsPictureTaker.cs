using UnityEngine;

namespace TanitakaTech.AssetsPictureTaker
{
    public class PrefabsPictureTaker : MonoBehaviour
    {
        [SerializeField] private Transform instantiateParentTransform;
        [SerializeField] private Camera renderCamera;
        [SerializeField] private PrefabsPictureTakerSettingsScriptableObject prefabsPictureTakerSettingsScriptableObject;
        
        public Transform InstantiateParentTransform => instantiateParentTransform;
        public Camera RenderCamera => renderCamera;
        public PrefabsPictureTakerSettingsScriptableObject PrefabsPictureTakerSettingsScriptableObject => prefabsPictureTakerSettingsScriptableObject;
        
        private void OnGUI()
        {
            if (GUILayout.Button("Take"))
            {
                if (instantiateParentTransform == null)
                {
                    Debug.LogError("Parent Transform is null");
                    return;
                }
                if (renderCamera == null)
                {
                    Debug.LogError("Render Camera is null");
                    return;
                }
                
                prefabsPictureTakerSettingsScriptableObject.TakeCaptures(instantiateParentTransform, renderCamera).Forget();
            }
        }
    }
}