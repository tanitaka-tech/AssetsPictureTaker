#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace TanitakaTech.AssetsPictureTaker.PrefabsPictureTaker
{
    public class PrefabsPictureTaker : MonoBehaviour
    {
        [SerializeField] private Transform instantiateParentTransform;
        [SerializeField] private Camera renderCamera;
        [SerializeField] private PrefabsPictureTakerSettingsScriptableObject prefabsPictureTakerSettingsScriptableObject;
        [SerializeField] private List<GameObject> testPrefabs = null;
        
        public Transform InstantiateParentTransform => instantiateParentTransform;
        public Camera RenderCamera => renderCamera;
        public PrefabsPictureTakerSettingsScriptableObject PrefabsPictureTakerSettingsScriptableObject => prefabsPictureTakerSettingsScriptableObject;
        public List<GameObject> TestPrefabs => testPrefabs;
    }
}
#endif