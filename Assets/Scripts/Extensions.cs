using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    public static class Extensions
    {
        public static Color WithAlpha(this Color col, float alpha)
        {
            return new Color(col.r, col.g, col.b, alpha);
        }

        public static Texture2D ToTexture2D(this RenderTexture rTex)
        {
            Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
            // ReadPixels looks at the active RenderTexture.
            RenderTexture.active = rTex;
            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tex.Apply();
            return tex;
        }

        public static Transform FindAnyChild(this Transform t, string name, StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase, bool includeInactive = false)
        {
            var children = t.GetComponentsInChildren<Transform>(includeInactive).ToList();
            children.Remove(t);

            return children.FirstOrDefault(child => child.name.Equals(name, stringComparison));
        }

        public static List<Transform> FindAllChildren(this Transform t, bool includeInactive = false, bool includeSelf = false)
        {
            var children = t.GetComponentsInChildren<Transform>(includeInactive).ToList();
            if (!includeSelf)
            {
                children.Remove(t);
            }

            return children;
        }

        public static void DestroyAllChildren(this Transform t)
        {
            var children = new List<Transform>();
            for (int i = 0; i < t.childCount; i++)
            {
                children.Add(t.GetChild(i));
            }

            foreach (var child in children)
            {
                if (Application.isPlaying)
                {
                    GameObject.Destroy(child.gameObject);
                }
                else
                {
                    GameObject.DestroyImmediate(child.gameObject);
                }
            }
        }

        public static Vector2 GetCanvasScaleFactor(this CanvasScaler canvasScaler)
        {
            var width = Application.isEditor ? Screen.safeArea.width : Screen.width;
            var height = Application.isEditor ? Screen.safeArea.height : Screen.height;
            return new Vector2(width / canvasScaler.referenceResolution.x, height / canvasScaler.referenceResolution.y);
        }

        public static Vector3 WithX(this Vector3 v, float x)
        {
            return new Vector3(x, v.y, v.z);
        }
        
        public static Vector3 WithY(this Vector3 v, float y)
        {
            return new Vector3(v.x, y, v.z);
        }
        
        public static Vector3 WithZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }

        public static void CalculateBounds(this BoxCollider boxCollider, params GameObject[] targets)
        {
            List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
            if (!targets.Any())
                targets = new[] {boxCollider.gameObject};

            foreach (var target in targets)
                meshRenderers.AddRange(target.GetComponentsInChildren<MeshRenderer>());

            if (!meshRenderers.Any())
                return;

            var allBounds = meshRenderers.Select(r => r.bounds);
            boxCollider.size = 0.001f * Vector3.one;
            var totalBounds = allBounds.First();

            var minX = Mathf.Infinity;
            var maxX = Mathf.NegativeInfinity;
            var minY = Mathf.Infinity;
            var maxY = Mathf.NegativeInfinity;
            var minZ = Mathf.Infinity;
            var maxZ = Mathf.NegativeInfinity;

            foreach (var bounds in allBounds)
            {
                minX = Mathf.Min(minX, bounds.min.x);
                maxX = Mathf.Max(maxX, bounds.max.x);
                minY = Mathf.Min(minY, bounds.min.y);
                maxY = Mathf.Max(maxY, bounds.max.y);
                minZ = Mathf.Min(minZ, bounds.min.z);
                maxZ = Mathf.Max(maxZ, bounds.max.z);

                totalBounds.Encapsulate(bounds);
            }

            var avgX = (minX + maxX) / 2f;
            var avgY = (minY + maxY) / 2f;
            var avgZ = (minZ + maxZ) / 2f;

            var scale = boxCollider.transform.lossyScale;
            var scaleCorrection = new Vector3(1 / scale.x, 1 / scale.y, 1 / scale.z);
            var correctedScale = Vector3.Scale(totalBounds.size, scaleCorrection);
            boxCollider.size = correctedScale;

            var worldCenter = new Vector3(avgX, avgY, avgZ);
            boxCollider.center = boxCollider.transform.InverseTransformPoint(worldCenter);
        }
    }
    
}