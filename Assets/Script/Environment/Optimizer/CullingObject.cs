using UnityEngine;

namespace Script.Environment.Optimizer
{
    public class CullingObject
    {
        public readonly Renderer Renderer;
        public readonly Vector3 Position;
        private readonly MeshFilter _meshFilter;
        private readonly float _maxRadius;

        public CullingObject(Renderer renderer, MeshFilter meshFilter, Transform transform, float maxRadius)
        {
            Renderer = renderer;
            Position = transform.position;
            _meshFilter = meshFilter;
            _maxRadius = maxRadius;
        }
        
        public float GetRadius()
        {
            var bounds = _meshFilter.mesh.bounds;
            var radius = bounds.extents.magnitude;

            if (radius > _maxRadius)
            {
                radius = _maxRadius;
                Debug.LogWarning($"Mesh {Renderer.gameObject.name} is too big!");
            }

            return radius;
        }

        public void SetActive(bool active)
        {
            var gameObj = Renderer.gameObject;

            if (gameObj != null)
            {
                Renderer.enabled = active;
                gameObj.SetActive(active);
            }
        }
    }
}
