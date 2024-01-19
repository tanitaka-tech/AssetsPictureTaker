using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TanitakaTech.AssetsPictureTaker
{
    public class PrefabsPictureTaker : MonoBehaviour
    {
        [SerializeField] private AddressableAssetGroup prefabContainAssetGroup;
        [SerializeField] private Transform instantiateParentTransform;
        
        // TODO: オプションも選択できるようにする
        [SerializeField] private DefaultAsset saveFolder;
        [SerializeField] private Camera renderCamera;
        [SerializeField] private bool isOverwriteSameName;
        string pattern = @"P_RoomItem_(\d+)";
        string replacement = "P_RoomItemIcon_$1";


        [MenuItem("Tools/Prefab Capture")]
        public static void ShowWindow()
        {
            //GetWindow<PrefabPictureTaker>("Prefab Capture");
        }

        private void OnGUI()
        {
            // AssetLabelReference のラベルをテキストフィールドで編集
            prefabContainAssetGroup =
                EditorGUILayout.ObjectField("Addressable Group", prefabContainAssetGroup, typeof(AddressableAssetGroup),
                    true) as AddressableAssetGroup;
            instantiateParentTransform =
                EditorGUILayout.ObjectField("Parent Transform", instantiateParentTransform, typeof(Transform), true) as Transform;
            saveFolder =
                EditorGUILayout.ObjectField("Storage Folder", saveFolder, typeof(DefaultAsset), false) as DefaultAsset;
            renderCamera = EditorGUILayout.ObjectField("Render Camera", renderCamera, typeof(Camera), true) as Camera;

            if (GUILayout.Button("Take"))
            {
                TakeCaptures().Forget();
            }
        }

        private async UniTaskVoid TakeCaptures()
        {
            await CapturePrefabs(prefabContainAssetGroup.entries
                .Select(_ => _.address as object)
                .ToList());
        }

        private async UniTask CapturePrefabs(List<object> keys)
        {
            foreach (var key in keys)
            {
                AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(key, instantiateParentTransform);
                await handle;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject prefabInstance = handle.Result;
                    CaptureAndSave(prefabInstance, key.ToString());
                    Addressables.ReleaseInstance(prefabInstance);
                }
            }
        }

        private void CaptureAndSave(GameObject prefab, string prefabName)
        {
            // Set up render camera here if needed

            // Render and save the image
            RenderTexture renderTexture = new RenderTexture(1024, 768, 24);
            renderCamera.targetTexture = renderTexture;
            Texture2D renderResult =
                new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            renderCamera.Render();
            RenderTexture.active = renderTexture;
            renderResult.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            renderResult.Apply();

            byte[] imageBytes = renderResult.EncodeToPNG();
            var path = AssetDatabase.GetAssetPath(saveFolder);
            var fileName = Regex.Replace(prefabName, pattern, replacement);
            
            string fileNamePath = Path.Combine(path, $"{fileName}.png");
            File.WriteAllBytes(fileNamePath, imageBytes);

            // Cleanup
            RenderTexture.active = null;
            renderCamera.targetTexture = null;
            DestroyImmediate(renderTexture);
            DestroyImmediate(renderResult);
        }
    }
}