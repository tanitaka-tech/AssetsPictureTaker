using UnityEngine;

namespace TanitakaTech.AssetsPictureTaker.PrefabsPictureTaker
{
    public class PrefabsPictureTaker : MonoBehaviour
    {
        [SerializeField] private Transform instantiateParentTransform;
        [SerializeField] private Camera renderCamera;
        [SerializeField] private PrefabsPictureTakerSettingsScriptableObject prefabsPictureTakerSettingsScriptableObject;
        
        public Transform InstantiateParentTransform => instantiateParentTransform;
        public Camera RenderCamera => renderCamera;
        public PrefabsPictureTakerSettingsScriptableObject PrefabsPictureTakerSettingsScriptableObject => prefabsPictureTakerSettingsScriptableObject;
    }
}