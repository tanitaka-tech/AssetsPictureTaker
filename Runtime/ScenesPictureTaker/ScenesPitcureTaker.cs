#if UNITY_EDITOR
using UnityEngine;

namespace TanitakaTech.AssetsPictureTaker.ScenesPictureTaker
{
    public class ScenesPictureTaker : MonoBehaviour
    {
        [SerializeField] private Transform instantiateParentTransform;
        [SerializeField] private Camera renderCamera;
        [SerializeField] private PrefabsPictureTakerSettingsScriptableObject prefabsPictureTakerSettingsScriptableObject;
        
        public Transform InstantiateParentTransform => instantiateParentTransform;
        public Camera RenderCamera => renderCamera;
        public PrefabsPictureTakerSettingsScriptableObject PrefabsPictureTakerSettingsScriptableObject => prefabsPictureTakerSettingsScriptableObject;
    }
}
#endif