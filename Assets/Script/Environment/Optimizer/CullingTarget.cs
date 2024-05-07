using System.Collections.Generic;
using UnityEngine;

namespace Script.Environment.Optimizer
{
    public class CullingTarget : MonoBehaviour
    {
        [SerializeField]
        private float maxBoundingRadius = 100f;
        
        public void GetCullingObjects(List<CullingObject> list)
        {
            var children = GetComponentsInChildren<MeshFilter>();
            
            foreach (var meshFilter in children)
            {
                if (!meshFilter.TryGetComponent(out Renderer rend))
                    continue;
                    
                list.Add(new CullingObject(rend, meshFilter, meshFilter.transform, maxBoundingRadius));
            }
        }
    }
}