#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using TanitakaTech.AssetsPictureTaker.ConvertResultHandler;
using TanitakaTech.AssetsPictureTaker.PictureConverter;
using TanitakaTech.AssetsPictureTaker.PictureEncoder;
using TanitakaTech.AssetsPictureTaker.PrefabPictureTaker;
using TanitakaTech.AssetsPictureTaker.Texture2DProcessor;
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
        [SerializeReference, SubclassSelector] private IPrefabPictureTaker prefabPictureTaker = new PrefabPictureTaker.PrefabPictureTaker();
        [SerializeReference, SubclassSelector] private ITexture2DProcessor[] texture2DProcessors;
        
        [Header("Save Settings")]
        [SerializeField] private DefaultAsset saveFolder;
        [SerializeField] private bool isOverwriteSameName;
        [SerializeReference, SubclassSelector] private IPictureConverter pictureConverter = new DefaultPictureConverter();
        [SerializeReference, SubclassSelector] private IConvertResultHandler convertResultHandler = new NothingConvertResultHandler();
        
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
        
        public async UniTask TakeCaptures(Transform instantiateParentTransform, Camera renderCamera)
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
            List<PictureConvertResult> pictureConvertResults = new List<PictureConvertResult>();
            foreach (var key in keys)
            {
                AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(key, instantiateParentTransform);
                await handle;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject prefabInstance = handle.Result;
                    var pictureConvertResult = CaptureAndSave(prefabInstance, key.ToString(), renderCamera);
                    pictureConvertResults.Add(pictureConvertResult);
                    Addressables.ReleaseInstance(prefabInstance);
                    
                    // NOTE: If the scene drawing is not updated, the drawing will stop, so call it manually.
                    EditorApplication.QueuePlayerLoopUpdate();
                }
            }
            AssetDatabase.Refresh();
            convertResultHandler.HandleConvertResult(pictureConvertResults);
        }

        private PictureConvertResult CaptureAndSave(GameObject prefabInstance, string prefabName, Camera renderCamera)
        {
            // Set up render camera here if needed

            // Render and save the image
            Texture2D renderResult = prefabPictureTaker.TakePicture(prefabInstance, renderCamera);
            foreach (var texture2DProcessor in texture2DProcessors)
            {
                renderResult = texture2DProcessor.Process(renderResult);
            }

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
            
            return pictureConvertResult;
        }
    }
}
#endif