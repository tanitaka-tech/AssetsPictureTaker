using System;
using UnityEngine;

namespace TanitakaTech.AssetsPictureTaker.PrefabPictureTaker
{
    public interface IPrefabPictureTaker
    {
        Texture2D TakePicture(GameObject gameObject, Camera camera);
    }

    [Serializable]
    public class PrefabPictureTaker : IPrefabPictureTaker
    {
        [SerializeField] private int renderTextureWidth = 1024;
        [SerializeField] private int renderTextureHeight = 768;
        [SerializeField] private int renderTextureDepth = 24;
        [SerializeField] private float cameraDistanceAdjustValue = 1.5f;
        
        Texture2D IPrefabPictureTaker.TakePicture(GameObject gameObject, Camera camera)
        {
            // プレハブのBoundsを取得（プレハブの全Rendererから計算）
            Bounds bounds = GetPrefabBounds(gameObject);
            
            // カメラの視野角とアスペクト比を取得
            float verticalFOV = camera.fieldOfView;
            float aspectRatio = camera.aspect;

            // カメラとプレハブの中心との必要な距離を計算
            float distance = Mathf.Max(bounds.size.x / aspectRatio, bounds.size.y) / (2f * Mathf.Tan(verticalFOV * 0.5f * Mathf.Deg2Rad));
            distance *= cameraDistanceAdjustValue;

            // カメラの位置をプレハブの中心から後ろに距離分移動させる
            camera.transform.position = bounds.center - camera.transform.forward * distance;

            // Render and save the image
            
            RenderTexture renderTexture = new RenderTexture(renderTextureWidth, renderTextureHeight, renderTextureDepth);
            camera.targetTexture = renderTexture;
            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            camera.Render();
            RenderTexture.active = renderTexture;
            renderResult.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            renderResult.Apply();
            
            // Cleanup
            RenderTexture.active = null;
            camera.targetTexture = null;
            
            return renderResult;
        }
        

        private Bounds GetPrefabBounds(GameObject prefab)
        {
            // プレハブの全Rendererを取得
            Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();

            // プレハブの境界ボックスを計算
            Bounds bounds = new Bounds(renderers[0].bounds.center, Vector3.zero);
            foreach (Renderer renderer in renderers)
            {
                bounds.Encapsulate(renderer.bounds);
            }
            return bounds;
        }
    }
}