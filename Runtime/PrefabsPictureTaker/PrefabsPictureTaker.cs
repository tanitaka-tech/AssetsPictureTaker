#if UNITY_EDITOR
using UnityEngine;

namespace TanitakaTech.AssetsPictureTaker.PrefabsPictureTaker
{
    public class PrefabsPictureTaker : MonoBehaviour
    {
        [SerializeField] private Transform instantiateParentTransform;
        [SerializeField] private Camera renderCamera;
        [SerializeField] private PrefabsPictureTakerSettingsScriptableObject prefabsPictureTakerSettingsScriptableObject;
        [SerializeField] private GameObject testPrefab = null;
        
        public Transform InstantiateParentTransform => instantiateParentTransform;
        public Camera RenderCamera => renderCamera;
        public PrefabsPictureTakerSettingsScriptableObject PrefabsPictureTakerSettingsScriptableObject => prefabsPictureTakerSettingsScriptableObject;
        public GameObject TestPrefab => testPrefab;
    }
}
#endif