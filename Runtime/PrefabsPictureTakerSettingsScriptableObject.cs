using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using TanitakaTech.AssetsPictureTaker.PictureConverter;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TanitakaTech.AssetsPictureTaker
{
    [Serializable]
    public class PrefabsPictureTakerSettingsScriptableObject : ScriptableObject
    {
        [Header("Reference Settings")]
        [SerializeField] private AddressableAssetGroup prefabContainAssetGroup;
        
        [Header("Capture Settings")]
        [SerializeField] private int renderTextureWidth = 1024;
        [SerializeField] private int renderTextureHeight = 768;
        [SerializeField] private int renderTextureDepth = 24;
        
        [Header("Save Settings")]
        [SerializeField] private DefaultAsset saveFolder;
        [SerializeField] private bool isOverwriteSameName;
        [SerializeReference, SubclassSelector] private IPictureConverter pictureConverter = new DefaultPictureConverter();
        
        [MenuItem("Assets/Create/ScriptableObjects/PrefabsPictureTakerSettingsPreset")]
        public static void CreatePreset()
        {
            PrefabsPictureTakerSettingsScriptableObject roomTemplateScriptableObjectSo = CreateInstance<PrefabsPictureTakerSettingsScriptableObject>();
            
            string filePath = EditorUtility.SaveFilePanelInProject("Create PrefabsPictureTakerSettingsPreset", "PrefabsPictureTakerSettingsPreset", "", "Creating PrefabsPictureTakerSettingsPreset", GetActiveFolderPath());
            if (string.IsNullOrEmpty(filePath)) return;
            filePath = filePath.Replace(Application.dataPath, "Assets");
            AssetDatabase.CreateAsset(roomTemplateScriptableObjectSo, filePath + ".asset");
            AssetDatabase.SaveAssets();
            
            string GetActiveFolderPath()
            {
                MethodInfo method = EditorWindow.focusedWindow.GetType().GetMethod("GetActiveFolderPath", BindingFlags.Instance | BindingFlags.NonPublic);
                if (method == null) return "";
                return (string)method.Invoke(EditorWindow.focusedWindow, new object[0]);
            }
        }

        private bool SerializeFieldValidation()
        {
            if (prefabContainAssetGroup == null)
            {
                Debug.LogError("Addressable Group is null");
                return false;
            }
            if (saveFolder == null)
            {
                Debug.LogError("Storage Folder is null");
                return false;
            }
            if (pictureConverter == null)
            {
                Debug.LogError("Picture Converter is null");
                return false;
            }

            return true;
        }
        
        public async UniTaskVoid TakeCaptures(Transform instantiateParentTransform, Camera renderCamera)
        {
            if (!SerializeFieldValidation())
            {
                return;
            }

            await CapturePrefabs(prefabContainAssetGroup.entries
                .Select(_ => _.address as object)
                .ToList(), instantiateParentTransform, renderCamera);
        }

        private async UniTask CapturePrefabs(List<object> keys, Transform instantiateParentTransform, Camera renderCamera)
        {
            foreach (var key in keys)
            {
                AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(key, instantiateParentTransform);
                await handle;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject prefabInstance = handle.Result;
                    CaptureAndSave(key.ToString(), renderCamera);
                    Addressables.ReleaseInstance(prefabInstance);
                }
            }
            AssetDatabase.Refresh();
        }

        private void CaptureAndSave(string prefabName, Camera renderCamera)
        {
            // Set up render camera here if needed

            // Render and save the image
            RenderTexture renderTexture = new RenderTexture(renderTextureWidth, renderTextureHeight, renderTextureDepth);
            renderCamera.targetTexture = renderTexture;
            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            renderCamera.Render();
            RenderTexture.active = renderTexture;
            renderResult.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            renderResult.Apply();

            // Save the image
            var path = AssetDatabase.GetAssetPath(saveFolder);
            var pictureConvertResult = pictureConverter.ConvertPicture(
                saveDirectory: path,
                renderResult: renderResult,
                prefabName: prefabName);
            bool isExists = File.Exists(pictureConvertResult.FileNamePath);
            if (!isExists || isOverwriteSameName)
            {
                File.WriteAllBytes(pictureConvertResult.FileNamePath, pictureConvertResult.PictureBytes);
                string type = isExists ? "Overwritten" : "Saved";
                Debug.Log($"{type}: <a href=\"{pictureConvertResult.FileNamePath}\">{pictureConvertResult.FileNamePath}</a>", 
                    AssetDatabase.LoadAssetAtPath(pictureConvertResult.FileNamePath, typeof(UnityEngine.Object))
                    );
            }
            else
            {
                Debug.LogWarning($"File already exists: {{<a href=\"{pictureConvertResult.FileNamePath}\">{pictureConvertResult.FileNamePath}</a>",
                    AssetDatabase.LoadAssetAtPath(pictureConvertResult.FileNamePath, typeof(UnityEngine.Object))
                    );
            }

            // Cleanup
            RenderTexture.active = null;
            renderCamera.targetTexture = null;
            DestroyImmediate(renderTexture);
            DestroyImmediate(renderResult);
        }
    }
}