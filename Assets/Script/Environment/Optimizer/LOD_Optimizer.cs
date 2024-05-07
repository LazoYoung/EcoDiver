using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Script.Environment.Optimizer
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class LOD_Optimizer : MonoBehaviour
    {
        [Range(0, 1)] [SerializeField] private float cullingRatio = 0.2f;

        [Range(0, 1)] [SerializeField] private float fadeTransitionWidth = 0.3f;

        [SerializeField] private Camera playerCamera;

        [SerializeField] private List<LODGroup> groups;

        private void Start()
        {
            if (playerCamera == null)
            {
                Debug.LogWarning("Attempting to find a camera in the scene...");
                playerCamera = FindObjectOfType<Camera>();
            }

            if (playerCamera == null)
            {
                Debug.LogError("Unable to find a camera!");
                enabled = false;
                return;
            }
            
            foreach (var group in groups)
            {
                SupplyRenderers(group);
            }
        }

        private void LateUpdate()
        {
            foreach (var group in groups)
            {
                group.localReferencePoint = playerCamera.transform.position;
            }
        }

        private void SupplyRenderers(LODGroup group)
        {
            var lod0 = new LOD
            {
                renderers = group.GetComponentsInChildren<Renderer>(),
                screenRelativeTransitionHeight = 1f,
                fadeTransitionWidth = fadeTransitionWidth
            };
            var culled = new LOD
            {
                renderers = Array.Empty<Renderer>(),
                screenRelativeTransitionHeight = cullingRatio
            };

            group.fadeMode = LODFadeMode.CrossFade;
            group.SetLODs(new[] { lod0, culled });
        }
    }
}