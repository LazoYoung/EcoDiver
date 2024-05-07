using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Environment.Optimizer
{
    public class CullingOptimizer : MonoBehaviour
    {
        [Header("Requirements")]
        [SerializeField]
        [Tooltip("A camera that represents the player's POV.")]
        private new Camera camera;
        
        [SerializeField]
        [Tooltip("Culling targets that contain objects to be optimized.")]
        private List<CullingTarget> targets;

        [Header("Experimental options")]
        [SerializeField]
        [Tooltip("Markers indicate rendering state of each object. (Red marker = in sight, White marker = culled)")]
        private bool displayMarkers;
        
        private CullingGroup _group;
        private BoundingSphere[] _boundingSpheres;
        private CullingObject[] _objects;
        private MarkerObject[] _markers;
        private bool _markerMode;

        private void OnEnable()
        {
            ValidateCamera();

            _group = new CullingGroup();
            _objects = GetTargetObjects();
            _boundingSpheres = GetBoundingSpheres(_objects);
            _markerMode = displayMarkers;
            _markers = GetMarkers(_objects);
            var count = _boundingSpheres.Length;
            
            _group.SetBoundingSpheres(_boundingSpheres);
            _group.SetBoundingSphereCount(count);
            Debug.Log($"CullingOptimizer is tracking {count} objects.");
            
            _group.onStateChanged = OnCullingStateChanged;
            _group.targetCamera = camera;
            _group.enabled = true;
            
            UpdateObjects(!_markerMode);

        }

        private void LateUpdate()
        {
            if (_markerMode != displayMarkers)
            {
                _markerMode = displayMarkers;
                UpdateObjects(!_markerMode);
                ResetMarkers(_markerMode);
            }
        }
        
        private void ResetMarkers(bool display)
        {
            if (display)
            {
                _markers = GetMarkers(_objects);
                return;
            }
            
            gameObject.transform.DetachChildren();

            foreach (var marker in _markers)
            {
                Destroy(marker.GameObject);
            }
            
            _markers = Array.Empty<MarkerObject>();
        }
        
        private void UpdateObjects(bool display)
        {
            foreach (var cullingObj in _objects)
            {
                cullingObj.Renderer.enabled = display;
            }
        }

        private CullingObject[] GetTargetObjects()
        {
            var list = new List<CullingObject>();
            
            foreach (var target in targets)
            {
                target?.GetCullingObjects(list);
            }

            return list.ToArray();
        }

        private BoundingSphere[] GetBoundingSpheres(CullingObject[] objects)
        {
            var array = new BoundingSphere[objects.Length];
            
            for (var i = 0; i < objects.Length; ++i)
            {
                var obj = objects[i];
                array[i] = new BoundingSphere(obj.Position, obj.GetRadius());
            }

            return array;
        }
        
        private MarkerObject[] GetMarkers(CullingObject[] objects)
        {
            if (!displayMarkers)
                return Array.Empty<MarkerObject>();
            
            var list = new List<MarkerObject>();
            var selfTransform = gameObject.transform;
            selfTransform.localScale.Set(1f, 1f, 1f);

            foreach (var cullingObj in objects)
            {
                var rad = cullingObj.GetRadius();
                var gameObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                var rend = gameObj.GetComponent<Renderer>();
                rend.enabled = true;
                gameObj.transform.parent = selfTransform;
                gameObj.transform.position = cullingObj.Position;
                gameObj.transform.localScale = new Vector3(rad, rad, rad);
                list.Add(new MarkerObject(gameObj, rend));
            }

            return list.ToArray();
        }
        
        private void OnCullingStateChanged(CullingGroupEvent sphere)
        {
            var cullingObj = _objects[sphere.index];

            if (_markerMode)
            {
                var color = sphere.isVisible ? Color.red : Color.gray;
                var rend = _markers[sphere.index].Renderer;
                rend.material.color = color;
            }
            else
            {
                cullingObj.SetActive(sphere.isVisible);
            }
        }

        private void OnDisable()
        {
            foreach (var cullingObj in _objects)
            {
                cullingObj.SetActive(true);
            }
            
            _group.enabled = false;
            _group.Dispose();
            _group = null;
        }
        
        private void ValidateCamera()
        {
            camera = camera != null ? camera : FindObjectOfType<Camera>();
            
            if (camera == null)
            {
                throw new UnityException("CullingOptimizer requires a camera in the scene!");
            }
        }
    }
}